using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LiveCoaching.Models.DTO;
using LiveCoaching.Models.LCU;
using LiveCoaching.Models.Mappings;
using LiveCoaching.Util;
using Polly;

namespace LiveCoaching.Services;

public static class LeagueUiClientManager
{
    private static readonly PlatformID SystemOS = Environment.OSVersion.Platform;
    private static HttpClient? _sharedClient;
    private static bool _isClientOpen;

    private static readonly AsyncPolicy RetryPolicy = Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

    public static void SetClientStatus()
    {
        ProcessStartInfo startInfo;

        if (SystemOS == PlatformID.Win32NT)
            startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/cwmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
        else if (SystemOS == PlatformID.Unix)
            startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash/",
                Arguments = "-c \"ps -A | grep LeagueClientUx\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
        else
            throw new Exception("Not Supported OS");

        using (var terminalOrCmd = Process.Start(startInfo))
        {
            if (terminalOrCmd == null) return;
            using (var reader = terminalOrCmd.StandardOutput)
            {
                var commandLine = reader.ReadToEnd();

                if (Regex.Match(commandLine, "is not recognized as an internal or external command").Success &&
                    SystemOS == PlatformID.Win32NT) throw new Exception("System is not supported");
                // In Windows 11, wmic is deprecated use: (Get-CimInstance Win32_Process -Filter "Name='LeagueClientUx.exe'").CommandLine
                // TODO: Implement with Get-CimInstance
                var appPortMatch = Regex.Match(commandLine, @"--app-port=([0-9]*)");
                var authTokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]*)");

                if (!appPortMatch.Success)
                {
                    _isClientOpen = false;
                    return;
                }

                var authByte = Encoding.ASCII.GetBytes("riot:" + authTokenMatch.Groups[1].Value);
                var auth = Convert.ToBase64String(authByte);

                HttpClientHandler handler = new()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                _sharedClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://127.0.0.1:" + appPortMatch.Groups[1].Value),
                    Timeout = TimeSpan.FromSeconds(5),
                    DefaultRequestHeaders =
                    {
                        { "Authorization", "Basic " + auth }
                    }
                };

                _isClientOpen = true;
            }
        }
    }

    public static async Task<Summoner?> GetLeagueSummonerAsync()
    {
        if (_sharedClient == null || _isClientOpen == false) return null;
        try
        {
            var response = await RetryPolicy.ExecuteAsync(() =>
                _sharedClient.GetFromJsonAsync<Summoner>("lol-summoner/v1/current-summoner"));

            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public static async Task<List<GameDto>?> GetLeagueSummonerMatchHistoryAsync()
    {
        if (_sharedClient == null || _isClientOpen == false) return null;
        try
        {
            var response = await RetryPolicy.ExecuteAsync(() =>
                _sharedClient.GetFromJsonAsync<MatchHistory>(
                    "lol-match-history/v1/products/lol/current-summoner/matches"));

            List<GameDto> games = new();
            response?.games.games.ForEach(game =>
            {
                if (GameModeMapping.Modes.TryGetValue(game?.gameMode!, out var mappedGameMode))
                {
                    var gameCreationTimeStamp = DateTime.Parse(game?.gameCreationDate!);
                    var gameTimeAgo = TimeConversion.CompareTimestampToCurrentTime(gameCreationTimeStamp);
                    games.Add(new GameDto(game?.gameId, mappedGameMode, gameTimeAgo));
                }
            });


            return games;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    // TODO: Implement
    public static bool IsInChampionSelect()
    {
        throw new Exception("Not Implemented");
    }

    public static bool GetIsClientOpen()
    {
        return _isClientOpen;
    }
}
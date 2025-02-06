using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using LiveCoaching.Models.DTO;
using LiveCoaching.Models.LCU;
using LiveCoaching.Models.Mappings;
using LiveCoaching.Util;
using Polly;

namespace LiveCoaching.Services.Api;

public class LeagueClientApiService
{
    private readonly PlatformID _systemOS = Environment.OSVersion.Platform;
    private readonly HttpClient? _sharedClient; 
    private bool _isClientOpen;
    private Timer _timer;

    private readonly AsyncPolicy _retryPolicy = Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

    public LeagueClientApiService(HttpClient sharedClient)
    {
       _sharedClient = sharedClient;
       _timer = new Timer(async void (_) =>
       {
           try
           {
               SetClientStatus();
           }
           catch (Exception e)
           {
               Debug.WriteLine(e);
           }
       }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }
    
    public void SetClientStatus()
    {
        ProcessStartInfo startInfo;

        if (_systemOS == PlatformID.Win32NT)
        {
            startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/cwmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
        }
        else if (_systemOS == PlatformID.Unix)
        {
            startInfo = new ProcessStartInfo();
            startInfo.FileName = "/bin/bash/";
            startInfo.Arguments = "-c \"ps -A | grep LeagueClientUx\"";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
        }
        else
            throw new Exception("Not Supported OS");

        using (var terminalOrCmd = Process.Start(startInfo))
        {
            if (terminalOrCmd == null) return;
            using (var reader = terminalOrCmd.StandardOutput)
            {
                var commandLine = reader.ReadToEnd();

                if (Regex.Match(commandLine, "is not recognized as an internal or external command").Success &&
                    _systemOS == PlatformID.Win32NT) throw new Exception("System is not supported");
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
                
                _sharedClient.BaseAddress = new Uri("https://127.0.0.1:" + appPortMatch.Groups[1].Value);
                _sharedClient.Timeout = TimeSpan.FromSeconds(5);
                _sharedClient.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

                _isClientOpen = true;
            }
        }
    }

    public async Task<Summoner?> GetLeagueSummonerAsync()
    {
        if (_sharedClient == null || _isClientOpen == false) return null;
        try
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _sharedClient.GetFromJsonAsync<Summoner>("lol-summoner/v1/current-summoner"));

            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<List<GameDto>?> GetLeagueSummonerMatchHistoryAsync()
    {
        if (_sharedClient == null || _isClientOpen == false) return null;
        try
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _sharedClient.GetFromJsonAsync<MatchHistory>(
                    "lol-match-history/v1/products/lol/current-summoner/matches"));

            List<GameDto> games = new();
            response?.games.games.ForEach(game =>
            {
                if (!GameModeMapping.Modes.TryGetValue(game.gameMode, out var mappedGameMode)) return;

                // Get time lapsed since game creation
                var gameCreationTimeStamp = DateTime.Parse(game.gameCreationDate);
                var gameTimeAgo = TimeConversion.CompareTimestampToCurrentTime(gameCreationTimeStamp);

                // Get win/lose status and covert to color
                var participant = game.participants.Find(matchParticipant =>
                    matchParticipant.participantId == game.participantIdentities[0].participantId
                );

                var headerColorGradiant = participant?.stats?.win == true
                    ? new ExpanderHeaderColorGradient(Color.Parse("#37D5D6"), Color.Parse("#35096D"))
                    : new ExpanderHeaderColorGradient(Color.Parse("#dd1818"), Color.Parse("#333333"));

                games.Add(new GameDto(game.gameId, mappedGameMode, gameTimeAgo, headerColorGradiant));
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

    public bool GetIsClientOpen()
    {
        return _isClientOpen;
    }
}
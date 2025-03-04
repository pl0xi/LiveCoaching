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
using LiveCoaching.Utils;
using Polly;

namespace LiveCoaching.Services.Api;

// TODO: Create class to handle interactions between web API and LCU API
public class LeagueClientApiService
{
    private readonly LeagueWebApiService _leagueWebApiService;

    private readonly AsyncPolicy _retryPolicy = Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

    private readonly HttpClient? _sharedClient;
    private readonly PlatformID _systemOS = Environment.OSVersion.Platform;
    private bool _isClientOpen;
    private Timer _timer;

    public LeagueClientApiService(HttpClient sharedClient, LeagueWebApiService leagueWebApiService)
    {
        _sharedClient = sharedClient;

        // Check client status periodically
        _timer = new Timer(void (_) =>
        {
            try
            {
                // TODO: Refactor to make this task, skip process check, if already established a connection (Check with API call instead of checking if client is open with process)
                // Make functions return status, if request is bad 
                SetClientStatus();
                ClientStatusChanged?.Invoke();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        _leagueWebApiService = leagueWebApiService;
    }

    public event Action? ClientStatusChanged;


    private void SetClientStatus()
    {
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true
        };

        switch (_systemOS)
        {
            case PlatformID.Win32NT:
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/cwmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline";
                break;
            case PlatformID.Unix:
                startInfo.FileName = "/bin/bash/";
                startInfo.Arguments = "-c \"ps -A | grep LeagueClientUx\"";
                break;
            default:
                throw new Exception("Not Supported OS");
        }

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

                // TODO: Add this in the Program.cs instead
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
            response?.games.games.ForEach(async void (game) =>
            {
                try
                {
                    if (!GameModeMapping.Modes.TryGetValue(game.gameMode, out var mappedGameMode)) return;

                    // Get time lapsed since game creation
                    var gameCreationTimeStamp = DateTime.Parse(game.gameCreationDate);
                    var gameTimeAgo = TimeConversion.CompareTimestampToCurrentTime(gameCreationTimeStamp);

                    // Get win/lose status and convert to color
                    var participant = game.participants.Find(matchParticipant =>
                        matchParticipant.participantId == game.participantIdentities[0].participantId
                    );

                    // Get items
                    var items = new List<ItemDto>();
                    for (var i = 0; i <= 6; i++)
                    {
                        var item = participant?.stats.GetType().GetProperty($"item{i}")
                            ?.GetValue(participant.stats, null);
                        if (item != null && (int)item != 0)
                            items.Add(new ItemDto(
                                $"https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/{item}.png"));
                        else
                            items.Add(new ItemDto(
                                "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"));
                    }

                    var headerColorGradiant = participant?.stats.win == true
                        ? new ExpanderHeaderColorGradient(Color.Parse("#37D5D6"), Color.Parse("#35096D"))
                        : new ExpanderHeaderColorGradient(Color.Parse("#dd1818"), Color.Parse("#333333"));

                    var spell1IconUrl = _leagueWebApiService.GetSummonerSpellFileName(participant?.spell1Id ?? 4);
                    var spell2IconUrl = _leagueWebApiService.GetSummonerSpellFileName(participant?.spell2Id ?? 4);

                    var calculatedKda = participant.stats.deaths == 0
                        ? participant.stats.kills + participant.stats.assists
                        : Math.Round(
                            (float)(participant.stats.kills + participant.stats.assists) / participant.stats.deaths, 1);

                    var gameDto = new GameDto(game.gameId, mappedGameMode, gameTimeAgo, headerColorGradiant,
                        $"https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/{participant?.championId}.png",
                        $"Level {participant.stats.champLevel}",
                        items, participant.stats.goldEarned,
                        $"https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/{spell1IconUrl}",
                        $"https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/{spell2IconUrl}",
                        participant.stats.kills, participant.stats.deaths, participant.stats.assists,
                        calculatedKda, participant.stats.visionScore, participant.stats.totalDamageTaken,
                        participant.stats.totalDamageDealt, participant.stats.damageDealtToObjectives);
                    games.Add(gameDto);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
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

    public bool GetIsClientOpen()
    {
        return _isClientOpen;
    }
}
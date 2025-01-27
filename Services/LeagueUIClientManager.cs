using LiveCoaching.Types;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiveCoaching.Services;
public static class LeagueUiClientManager
{
    private static readonly PlatformID systemOS = Environment.OSVersion.Platform;
    private static HttpClient? sharedClient;
    private static bool isClientOpen = false;

    public static void SetClientStatus()
    {
        ProcessStartInfo startInfo;

        if (systemOS == PlatformID.Win32NT)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/cwmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
        }
        else if (systemOS == PlatformID.Unix)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash/",
                Arguments = "-c \"ps -A | grep LeagueClientUx\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

        }
        else
        {
            throw new Exception("Not Supported OS");
        }

        using (var terminalOrCMD = Process.Start(startInfo))
        {
            if (terminalOrCMD == null) return;
            using (var reader = terminalOrCMD.StandardOutput)
            {
                string commandLine = reader.ReadToEnd();

                if (Regex.Match(commandLine, "is not recognized as an internal or external command").Success && systemOS == PlatformID.Win32NT)
                {
                    throw new Exception("System is not supported");
                    // In Windows 11, wmic is deprecated use: (Get-CimInstance Win32_Process -Filter "Name='LeagueClientUx.exe'").CommandLine
                    // TODO: Implement with Get-CimInstance
                }

                // TODO: Change so app-port and * dosent get used
                var appPortMatch = Regex.Match(commandLine, @"--app-port=([0-9]*)");
                var authTokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]*)");

                if (!appPortMatch.Success) return;

                byte[] authByte = Encoding.ASCII.GetBytes("riot:" + authTokenMatch.Groups[1].Value);
                string auth = Convert.ToBase64String(authByte);

                HttpClientHandler handler = new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                sharedClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://127.0.0.1:" + appPortMatch.Groups[1].Value),
                    Timeout = TimeSpan.FromSeconds(5),
                    DefaultRequestHeaders =
                    {
                        { "Authorization", "Basic " + auth },
                    }
                };

                isClientOpen = true;
            }
        }
    }

    public static async Task<string?> GetLeagueName()
    {
        if (sharedClient == null) return "Failed to get username";
        var response = await sharedClient.GetFromJsonAsync<Summoner>("lol-summoner/v1/current-summoner");

        return response?.gameName;
    }

    // TODO: Implement
    public static bool IsInChampionSelect()
    {
        throw new Exception("Not Implemented");
    }

    public static bool GetIsClientOpen()
    {
        return isClientOpen;
    }
}
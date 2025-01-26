using System.Text.RegularExpressions;
using System;
using System.Diagnostics;
using System.Text;
using System.Net.Http;

public class LeagueUiClientManager
{
    private PlatformID systemOS = Environment.OSVersion.Platform;
    private static HttpClient? sharedClient;
    private static bool isClientOpen = false;

    public void setClientStatus()
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

                var appPortMatch = Regex.Match(commandLine, @"--app-port=([0-9]*)");
                var authTokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]*)");

                if (!appPortMatch.Success) return;

                byte[] authByte = Encoding.ASCII.GetBytes("riot:" + authTokenMatch);
                string auth = Convert.ToBase64String(authByte);

                sharedClient = new HttpClient()
                {
                    BaseAddress = new Uri("https://127.0.0.1:" + appPortMatch),
                };

                sharedClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);

                isClientOpen = true;
            }
        }
    }

    // TODO: Implement
    public string? getLeagueName()
    {
        throw new Exception("Not Implemented");
    }

    // TODO: Implement
    public bool isInChampionSelect()
    {
        throw new Exception("Not Implemented");
    }

    public bool getIsClientOpen()
    {
        return isClientOpen;
    }
}
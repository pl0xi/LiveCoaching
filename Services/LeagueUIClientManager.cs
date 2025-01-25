using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Diagnostics;
using System.Text;

public class LeagueUiClientManager
{
    private string? baseClientUrl = null;
    private PlatformID systemOS = Environment.OSVersion.Platform;
    private string auth = string.Empty;

    private bool isClientOpen()
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
            if (terminalOrCMD == null) return false;
            using (var reader = terminalOrCMD.StandardOutput)
            {
                string commandLine = reader.ReadToEnd();

                var appPortMatch = Regex.Match(commandLine, @"--app-port=([0-9]*)");
                var authTokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]*)");

                if (!appPortMatch.Success) return false;

                baseClientUrl = "https://127.0.0.1:" + appPortMatch;

                byte[] authByte = Encoding.ASCII.GetBytes("riot:" + authTokenMatch);
                auth = Convert.ToBase64String(authByte);
            }
            ;
        }

        return true;
    }

    public bool isInChampionSelect()
    {
        if (!isClientOpen()) return false;

        throw new Exception("Not Implemented");
    }
}
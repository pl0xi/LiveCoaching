using System.Text.RegularExpressions;
using System;
using System.Diagnostics;

public class UIClientManager {

    private string? baseClientUrl = null;

    private PlatformID systemOS = Environment.OSVersion.Platform;
    private string auth = string.Empty;

    public Boolean IsClientOpen() {
        if (systemOS == PlatformID.Win32NT) {
            var startInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/cwmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true    
            };

            using (var cmdProcess = Process.Start(startInfo)) {
                using (var reader = cmdProcess.StandardOutput) {
                    string commandLine = reader.ReadToEnd();

                    var appPortMatch = Regex.Match(commandLine, @"--app-port=([0-9]*)");
                    var authTokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]*)");

                    baseClientUrl = "https://127.0.0.1:" + appPortMatch;

                    // TODO: Converto to base64 for auth
                };
            }
            
            return true;

        } else if (systemOS == PlatformID.Unix) {
            // TODO: Support MacOS 
            throw new Exception("Not Supported OS");
        } else {
            throw new Exception("Not Supported OS");
        } 
    }
}
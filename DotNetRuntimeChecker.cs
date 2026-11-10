using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;

namespace WinForm
{
    internal static class DotNetRuntimeChecker
    {
        public static List<string> GetDotNetFrameworkVersions()
        {
            var results = new List<string>();

            using (var ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
            {
                if (ndpKey?.GetValue("Release") is int release)
                {
                    results.Add($".NET Framework {ReleaseToVersion(release)} (Release {release})");
                }
            }

            using (var ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP"))
            {
                if (ndpKey != null)
                {
                    foreach (var legacyName in new[] { "v3.5", "v3.0", "v2.0.50727" })
                    {
                        using (var legacyKey = ndpKey.OpenSubKey(legacyName))
                        {
                            if (legacyKey?.GetValue("Install") is int installed && installed == 1)
                            {
                                results.Add($".NET Framework {legacyName}");
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static string ReleaseToVersion(int release)
        {
            if (release >= 533320) return "4.8.1";
            if (release >= 528040) return "4.8";
            if (release >= 461808) return "4.7.2";
            if (release >= 461308) return "4.7.1";
            if (release >= 460798) return "4.7";
            if (release >= 394802) return "4.6.2";
            if (release >= 394254) return "4.6.1";
            if (release >= 393295) return "4.6";
            if (release >= 379893) return "4.5.2";
            if (release >= 378675) return "4.5.1";
            if (release >= 378389) return "4.5";
            return $"不明 (Release {release})";
        }

        public static List<string> GetDotNetCoreRuntimes()
        {
            var results = new List<string>();

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-runtimes",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    foreach (var line in output.Split('\n'))
                    {
                        var trimmed = line.Trim();
                        if (trimmed.Length > 0)
                        {
                            results.Add(trimmed);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // dotnet コマンドが見つからない場合は .NET (Core/5+) 未インストールとみなす
            }

            return results;
        }
    }
}

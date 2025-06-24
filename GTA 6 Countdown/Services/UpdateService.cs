using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace GTA_6_Countdown.Services
{
    public class UpdateService
    {
        private const string GITHUB_API_URL = "https://api.github.com/repos/KrazzyElf/gta6-countdown/releases/latest";
        private static readonly HttpClient httpClient = new HttpClient();

        static UpdateService()
        {
            // GitHub API requires a User-Agent header
            httpClient.DefaultRequestHeaders.Add("User-Agent", "GTA6Countdown-Updater");
        }

        public class GitHubRelease
        {
            public string tag_name { get; set; } = "";
            public string name { get; set; } = "";
            public string body { get; set; } = "";
            public bool prerelease { get; set; }
            public GitHubAsset[] assets { get; set; } = Array.Empty<GitHubAsset>();
            public DateTime published_at { get; set; }
        }

        public class GitHubAsset
        {
            public string name { get; set; } = "";
            public string browser_download_url { get; set; } = "";
            public long size { get; set; }
        }

        /// <summary>
        /// Gets the current version of the application
        /// </summary>
        public static string GetCurrentVersion()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version?.ToString(3) ?? "1.0.0"; // Major.Minor.Patch
            }
            catch
            {
                return "1.0.0";
            }
        }

        /// <summary>
        /// Checks if an update is available
        /// </summary>
        public static async Task<(bool hasUpdate, GitHubRelease? release)> CheckForUpdateAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(GITHUB_API_URL);
                var release = JsonSerializer.Deserialize<GitHubRelease>(response);

                if (release == null || string.IsNullOrEmpty(release.tag_name))
                    return (false, null);

                var currentVersion = GetCurrentVersion();
                var latestVersion = release.tag_name.TrimStart('v'); // Remove 'v' prefix if present

                if (IsNewerVersion(latestVersion, currentVersion))
                {
                    return (true, release);
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for updates: {ex.Message}");
                return (false, null);
            }
        }

        /// <summary>
        /// Compares two version strings
        /// </summary>
        private static bool IsNewerVersion(string newVersion, string currentVersion)
        {
            try
            {
                var newVer = new Version(newVersion);
                var currentVer = new Version(currentVersion);
                return newVer > currentVer;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Shows an update notification dialog
        /// </summary>
        public static void ShowUpdateDialog(GitHubRelease release)
        {
            var result = MessageBox.Show(
                $"A new version is available!\n\n" +
                $"Current Version: {GetCurrentVersion()}\n" +
                $"Latest Version: {release.tag_name}\n\n" +
                $"Release Notes:\n{release.body}\n\n" +
                $"Would you like to download the update?",
                "Update Available",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                DownloadUpdate(release);
            }
        }

        /// <summary>
        /// Downloads and installs the update
        /// </summary>
        private static async void DownloadUpdate(GitHubRelease release)
        {
            try
            {
                // Find the executable asset
                GitHubAsset? exeAsset = null;
                foreach (var asset in release.assets)
                {
                    if (asset.name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        exeAsset = asset;
                        break;
                    }
                }

                if (exeAsset == null)
                {
                    MessageBox.Show("No executable found in the latest release.", "Update Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Show progress dialog
                var progressWindow = new UpdateProgressWindow();
                progressWindow.Show();

                // Download the file
                var tempPath = Path.Combine(Path.GetTempPath(), exeAsset.name);

                using (var response = await httpClient.GetAsync(exeAsset.browser_download_url))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fileStream = File.Create(tempPath))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                }

                progressWindow.Close();

                // Create update script
                var currentExePath = Process.GetCurrentProcess().MainModule?.FileName;
                if (currentExePath == null)
                {
                    MessageBox.Show("Could not determine current executable path.", "Update Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var updateScript = CreateUpdateScript(tempPath, currentExePath);

                // Run update script and exit current app
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{updateScript}\"",
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading update: {ex.Message}", "Update Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Creates a batch script to replace the current executable
        /// </summary>
        private static string CreateUpdateScript(string newExePath, string currentExePath)
        {
            var scriptPath = Path.Combine(Path.GetTempPath(), "update_gta6countdown.bat");
            var scriptContent = $@"
@echo off
echo Updating GTA 6 Countdown...
timeout /t 2 /nobreak > nul
taskkill /f /im ""{Path.GetFileName(currentExePath)}"" > nul 2>&1
timeout /t 1 /nobreak > nul
copy /y ""{newExePath}"" ""{currentExePath}""
if %errorlevel% equ 0 (
    echo Update completed successfully!
    start """" ""{currentExePath}""
) else (
    echo Update failed!
    pause
)
del ""{newExePath}""
del ""{scriptPath}""
";
            File.WriteAllText(scriptPath, scriptContent);
            return scriptPath;
        }
    }

    /// <summary>
    /// Simple progress window for updates
    /// </summary>
    public class UpdateProgressWindow : Window
    {
        public UpdateProgressWindow()
        {
            Title = "Downloading Update...";
            Width = 300;
            Height = 100;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.ToolWindow;
            ResizeMode = ResizeMode.NoResize;

            var textBlock = new System.Windows.Controls.TextBlock
            {
                Text = "Downloading update, please wait...",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Content = textBlock;
        }
    }
}

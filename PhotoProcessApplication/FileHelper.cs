using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProcessApplication
{
    class FileHelper
    {

        public static void CopyImages(string sourceDir, string targetDir, string filePrefix = null,  Action<string> logMessage = null)
        {
            EnsureTargetDirs(sourceDir, targetDir);
            foreach (string sourceFilePath in Directory.GetFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                var targetFilePath = sourceFilePath.Replace(sourceDir, targetDir);
                if (filePrefix != null)
                {
                    targetFilePath = Path.Combine(Path.GetDirectoryName(targetFilePath), $"{filePrefix}-{Path.GetFileName(targetFilePath)}");
                }
                File.Copy(sourceFilePath, targetFilePath, false);
                logMessage?.Invoke($"{targetFilePath} copied.");
            }
        }

        public static void OpenFileExplorer(string dir)
        {
            System.Diagnostics.Process.Start("explorer", dir);
        }

        public static string BrowseDir()
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                return dialog.SelectedPath;
            return null;
        }

        public static Result ConvertImages(string sourceDir, string targetDir, bool highRes)
        {
            EnsureTargetDirs(sourceDir, targetDir);
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "ConvertImages.bat",
                WorkingDirectory = Directory.GetCurrentDirectory(),
                Arguments = $"{sourceDir} {targetDir} {(highRes ? "1" : "0")}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };
            var errMsg = new StringBuilder();
            var msg = new StringBuilder();
            process.Start();
            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                msg.AppendLine(e.Data);
            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                errMsg.AppendLine(e.Data);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(errMsg.ToString()))
                return Result.WithError($"Image conversion failed. \r\n {errMsg} \r\n {msg}");

            return Result.WithSuccess(msg.ToString());

        }


        private static void EnsureTargetDirs(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir)) { Directory.CreateDirectory(targetDir); }
            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, targetDir));
            }
        }
    }
}

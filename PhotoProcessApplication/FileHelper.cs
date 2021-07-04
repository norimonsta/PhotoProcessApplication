using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProcessApplication
{
    class FileHelper
    {

        public static void CopyImages(string sourceDir, string targetDir, Action<string> logMessage)
        {
            EnsureTargetDirs(sourceDir, targetDir);
            foreach (string sourceFilePath in Directory.GetFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                var targetFilePath = sourceFilePath.Replace(sourceDir, targetDir);
                File.Copy(sourceFilePath, targetFilePath, true);
                logMessage($"{targetFilePath} copied.");
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

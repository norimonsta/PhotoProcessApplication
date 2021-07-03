using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoProcessApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportSourceButton_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                ImportSourceDir.Text = dialog.SelectedPath;
        }

        private void ImportTargetButton_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                ImportTargetDir.Text = dialog.SelectedPath;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sourceDir = ImportSourceDir.Text;
                string targetDir = System.IO.Path.Combine(ImportTargetDir.Text, SessionId.Text);
                ResetImportStatus();
                if (string.IsNullOrWhiteSpace(sourceDir)) { LogImportError("SD Card directory required."); return; }
                if (!Directory.Exists(sourceDir)) { LogImportError("Invalid SD Card directory."); return; }
                if (string.IsNullOrWhiteSpace(targetDir)) { LogImportError("Picture Folder directory required."); return; }
                EnsureImportTargets(sourceDir, targetDir);
                ImportFiles(sourceDir, targetDir);
                OpenFileExplorer(targetDir);
            }
            catch(Exception ex)
            {
                LogImportError(ex.Message);
            }
        }

        private void ResetImportStatus()
        {
            ImportStatus.Text = string.Empty;
        }

        private void LogImportMessage(string message)
        {
            ImportStatus.Foreground = new SolidColorBrush(Colors.Black);
            ImportStatus.Text += $"{Environment.NewLine}{message}";
        }

        private void LogImportError(string message)
        {
            ImportStatus.Foreground = new SolidColorBrush(Colors.Red);
            ImportStatus.Text += $"{Environment.NewLine}{message}";
        }

        private void EnsureImportTargets(string importSourceDir, string importTargetDir)
        {
            if (!Directory.Exists(ImportTargetDir.Text)) { Directory.CreateDirectory(ImportTargetDir.Text); }
            foreach (string dirPath in Directory.GetDirectories(importSourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(importSourceDir, importTargetDir));
            }
        }

        private void ImportFiles(string importSourceDir, string ImportTargetDir)
        {
            foreach (string sourceFilePath in Directory.GetFiles(importSourceDir, "*.*", SearchOption.AllDirectories))
            {
                var targetFilePath = sourceFilePath.Replace(importSourceDir, ImportTargetDir);
                File.Copy(sourceFilePath, targetFilePath, true);
                LogImportMessage($"{targetFilePath} copied.");
            }
        }

        private void OpenFileExplorer(string dir)
        {
            System.Diagnostics.Process.Start("explorer", dir);
        }
    }
}

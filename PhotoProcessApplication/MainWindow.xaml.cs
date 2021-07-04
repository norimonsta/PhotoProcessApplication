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


        #region Import
        private void ImportSourceButton_Click(object sender, RoutedEventArgs e)
        {
            ImportSourceDir.Text = FileHelper.BrowseDir();
        }

        private void ImportTargetButton_Click(object sender, RoutedEventArgs e)
        {
            ImportTargetDir.Text = FileHelper.BrowseDir();
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
                FileHelper.CopyImages(sourceDir, targetDir, LogImportMessage);
                LogImportMessage("Copy completed.");
                ExportSourceDir.Text = targetDir;
                FileHelper.OpenFileExplorer(targetDir);
            }
            catch (Exception ex)
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
        #endregion

        #region Export
        private void ExportSourceButton_Click(object sender, RoutedEventArgs e)
        {
            ExportSourceDir.Text = FileHelper.BrowseDir();
        }

        private void ExportTargetButton_Click(object sender, RoutedEventArgs e)
        {
            ExportTargetDir.Text = FileHelper.BrowseDir();
        }

        private void ExportHighButton_Click(object sender, RoutedEventArgs e)
        {
            ExportPictures();
        }

        private void ExportLowButton_Click(object sender, RoutedEventArgs e)
        {
            ExportPictures();
        }

        private void ExportPictures()
        {
            try
            {
                string sourceDir = ExportSourceDir.Text;
                string targetDir = ExportTargetDir.Text;
                ResetExportStatus();
                if (string.IsNullOrWhiteSpace(sourceDir)) { LogExportError("Picture Folder directory required."); return; }
                if (!Directory.Exists(sourceDir)) { LogExportError("Invalid Picture Folder directory."); return; }
                if (string.IsNullOrWhiteSpace(targetDir)) { LogExportError("USB Flash Drive directory required."); return; }
                FileHelper.CopyImages(sourceDir, targetDir, LogExportMessage);
                LogExportMessage("Copy completed.");
                MessageBox.Show("Copy completed");
            }
            catch (Exception ex)
            {
                LogExportError(ex.Message);
            }
        }

        private void ResetExportStatus()
        {
            ExportStatus.Text = string.Empty;
        }

        private void LogExportMessage(string message)
        {
            ExportStatus.Foreground = new SolidColorBrush(Colors.Black);
            ExportStatus.Text += $"{Environment.NewLine}{message}";
        }

        private void LogExportError(string message)
        {
            ExportStatus.Foreground = new SolidColorBrush(Colors.Red);
            ExportStatus.Text += $"{Environment.NewLine}{message}";
        }
        #endregion

    }
}

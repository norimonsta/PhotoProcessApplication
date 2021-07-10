using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProcessApplication
{
    public class ConvertImage
    {
        public static Result StartProcess(string inputDir, string outputDir, bool highRes)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ConvertImage.bat",
                Arguments = $"{inputDir} {outputDir} {(highRes ? "1" : "0")}",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };
            process.Start();
            var errorMsg = process.StandardError.ReadToEnd();
            var msg = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            if (process.ExitCode != 0 || !string.IsNullOrEmpty(errorMsg))
                return Result.WithError($"Image conversion failed. {errorMsg}");

            return Result.WithSuccess(msg);

        }
    }
}

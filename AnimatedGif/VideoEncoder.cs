using System.Diagnostics;
using System.IO;

namespace AnimatedGif {
    public class VideoEncoder {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Args { get; set; }
        public string OutputExtension { get; set; }

        public VideoEncoder() {
            Name = "x264 encoder to MP4";
            Path = "x264.exe";
            Args = "--output %output %input";
            OutputExtension = "mp4";
        }

        /// <param name="sourceFilePath">AVI file path</param>
        /// <param name="targetFilePath">Target file path without extension</param>
        public void Encode(string sourceFilePath, string targetFilePath) {
            if (IsValid() && !string.IsNullOrEmpty(sourceFilePath) && !string.IsNullOrEmpty(targetFilePath)) {
                if (!targetFilePath.EndsWith(OutputExtension)) {
                    targetFilePath += "." + OutputExtension.TrimStart('.');
                }

                Helper.CreateDirectoryFromFilePath(targetFilePath);

                using (Process process = new Process()) {
                    ProcessStartInfo psi = new ProcessStartInfo(Path) {
                        Arguments = Args.Replace("%input", "\"" + sourceFilePath + "\"").Replace("%output", "\"" + targetFilePath + "\""),
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    process.StartInfo = psi;
                    process.Start();
                    process.WaitForExit();
                }
            }
        }

        public bool IsValid() {
            return !string.IsNullOrEmpty(Path) && File.Exists(Path) && !string.IsNullOrEmpty(OutputExtension);
        }

        public override string ToString() {
            return Name;
        }
    }
}

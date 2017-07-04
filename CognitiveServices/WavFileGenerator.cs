/// WavFileGenerator.cs provides functions to utilise ffmpeg to create
/// wav audio files by extracting the audio from mp4 files.
/// 
/// Copyright(C) <2017>  <Robert Palmer>
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program.If not, see<http://www.gnu.org/licenses/>.

namespace CognitiveServices
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// This class is used to extract the audio from mp4 files and create corresponding wav files for use with the APIs.
    /// </summary>
    internal class WavFileGenerator
    {
        const string GEN_WAV_COMMAND = @"ffmpeg -i {0}.mp4 {0}.wav";

        public static void GenerateMissingWav(string inputFolder, string fileName)
        {
            var cdCmd = "cd " + inputFolder;
            var genCmd = string.Format(GEN_WAV_COMMAND, fileName);
            var concatCmd = "/C " + cdCmd + "&" + genCmd;

            var process = new Process(); //Process.Start("CMD.exe", concatCmd);

            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = concatCmd;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        public static void GenerateMissingWavs(HashSet<string> mp4Files, HashSet<string> wavFiles, string inputFolder)
        {
            foreach (var videoName in mp4Files)
            {
                // If there are mp4 files without corresponding wav files, generate the wav files
                if (!wavFiles.Contains(videoName))
                {
                    GenerateMissingWav(videoName, inputFolder);
                }
            }
        }
    }
}

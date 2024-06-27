using System;
using System.IO;
using UnityEditor;

namespace MonkeyJam.Assets.Editor
{
    public class BuildScript
    {
        public static void PerformBuild()
        {
            string[] scenes = { "Assets/Scenes/SampleScene.unity" };
            string buildPath = "Builds/Windows/MonkeyJam/MonkeyJam.exe";

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(buildPath) ?? throw new InvalidOperationException());

            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows, BuildOptions.None);
        }
    }
}
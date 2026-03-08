using System.Diagnostics;
using System.IO;
using System.Security;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build Bundle and Assembly #b")]
        static void BuildAllAssetBundles()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            BuildPipeline.BuildAssetBundles("Assets/Bundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

            var buildTime = stopwatch.Elapsed.TotalSeconds;

            const string bundlePath = "Assets/Bundles/planettweaks2ui";
            const string dllPath = "Library/ScriptAssemblies/PlanetTweaks2.UI.dll";
            const string localPath = "Assets/Editor/localpath.json";

            var targets = JsonUtility.FromJson<string[]>(localPath);

            foreach (string target in targets)
            {
                CopyTo(bundlePath, target);
                CopyTo(dllPath, target);
            }

            var totalTime = stopwatch.Elapsed.TotalSeconds;
            var copyTime = totalTime - buildTime;

            UnityEngine.Debug.Log($"Build Done in {totalTime:f2}s! (build: {buildTime:f2}s, copy: {copyTime:f2}s)");
        }

        private static void CopyTo(string file, string dir, bool overwrite = true)
        {
            File.Copy(file, Path.Combine(dir, Path.GetFileName(file)), overwrite);
        }
    }
}

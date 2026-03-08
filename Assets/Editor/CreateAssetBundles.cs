using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            var builds = new AssetBundleBuild[]
            {
                new()
                {
                    assetBundleName = "planettweaks2ui_withoutasset",
                    assetNames = GetFilteredDependencies("Assets/Prefabs/PlanetTweaks2UI.prefab")
                }
            };

            BuildPipeline.BuildAssetBundles("Assets/Bundles", builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

            var buildTime = stopwatch.Elapsed.TotalSeconds;

            const string bundlePath = "Assets/Bundles/planettweaks2ui";
            const string dllPath = "Library/ScriptAssemblies/PlanetTweaks2.UI.dll";
            const string localPath = "Assets/Editor/localpath.json";

            var targets = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(localPath));

            foreach (string target in targets)
            {
                CopyTo(bundlePath, target);
                CopyTo(dllPath, target);
            }

            var totalTime = stopwatch.Elapsed.TotalSeconds;
            var copyTime = totalTime - buildTime;

            UnityEngine.Debug.Log($"Build Done in {totalTime:f2}s! (build: {buildTime:f2}s, copy: {copyTime:f2}s)");
        }

        private static string[] GetFilteredDependencies(string assetPath)
        {
            var deps = AssetDatabase.GetDependencies(assetPath, true);

            return deps.Where(p =>
            {
                if (p.StartsWith("Assets/Images")) return true;
                if (p.StartsWith("Assets/Prefabs")) return true;

                return false;
            }).ToArray();
        }

        private static void CopyTo(string file, string dir, bool overwrite = true)
        {
            File.Copy(file, Path.Combine(dir, Path.GetFileName(file)), overwrite);
        }
    }
}

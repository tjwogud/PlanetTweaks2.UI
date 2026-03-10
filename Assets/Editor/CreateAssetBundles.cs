using log4net.Core;
using Newtonsoft.Json;
using PlanetTweaks2.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Assets.Editor
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Test")]
        static void Test()
        {
            var prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/PlanetTweaks2UI.prefab");

            Recursive(prefab);

            PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/Prefabs/PlanetTweaks2UI.prefab");

            PrefabUtility.UnloadPrefabContents(prefab);
        }

        private static void Recursive(GameObject obj, int level = 0)
        {
            if (obj.GetComponent<TMP_Text>() || obj.GetComponent<TMP_InputField>())
                obj.AddComponent<PTText>();
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                Recursive(obj.transform.GetChild(i).gameObject, level + 1);
            }
        }

        private static IEnumerable<T> GetComps<T>(GameObject obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp) yield return comp;
            if (obj.transform.childCount > 0)
            {
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    foreach (var text in GetComps<T>(obj.transform.GetChild(i).gameObject))
                        yield return text;
                }
            }
        }

        [MenuItem("Assets/Build Bundle and Assembly #b")]
        static void BuildAllAssetBundles()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/PlanetTweaks2UI.prefab");

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/godoMaum SDF.asset");

            foreach (var text in GetComps<TMP_Text>(prefab))
            {
                text.font = null;
            }

            foreach (var text in GetComps<TMP_InputField>(prefab))
            {
                text.fontAsset = null;
            }

            PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/Prefabs/PlanetTweaks2UI.prefab");

            PrefabUtility.UnloadPrefabContents(prefab);

            BuildPipeline.BuildAssetBundles("Assets/Bundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

            prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/PlanetTweaks2UI.prefab");

            foreach (var text in GetComps<TMP_Text>(prefab))
            {
                text.font = fontAsset;
            }

            foreach (var text in GetComps<TMP_InputField>(prefab))
            {
                text.fontAsset = null;
            }

            PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/Prefabs/PlanetTweaks2UI.prefab");

            PrefabUtility.UnloadPrefabContents(prefab);

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

        private static Object[] GetDependencies(string path)
        {
            return AssetDatabase.GetDependencies(path).Where(p =>
            {
                if (p.StartsWith("Assets/Images")) return true;
                if (p.StartsWith("Assets/Prefabs")) return true;

                return false;
            }).Select(p =>
            {
                return AssetDatabase.LoadAssetAtPath<Object>(p);
            }).ToArray();
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

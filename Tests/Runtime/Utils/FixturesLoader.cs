using UnityEngine;
using System.IO;
using UnityEditor;

namespace LnxArch.TestTools
{
    public sealed class FixturesLoader
    {
        private const string PackagePrefix = "Sensen.LnxArch";
        public static FixturesLoader RuntimeAutofetch { get; } = new(testsGroup: "Autofetch");
        private readonly string _testsGroup;
        private readonly bool _isEditor;
        private string _testsFolderPath;

        private string TestTypeName => _isEditor ? "Editor" : "Runtime";
        private string TestsAssemblyGUID => AssetDatabase.FindAssets($"t:AssemblyDefinitionAsset {PackagePrefix}.{TestTypeName}.Tests")[0];
        private string TestsFolderPath => _testsFolderPath ??= Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(TestsAssemblyGUID));
        private string FixturesPath => Path.Combine(TestsFolderPath, "Fixtures");
        private string PrefabsPath => Path.Combine(FixturesPath, "Prefabs");
        private FixturesLoader(string testsGroup, bool isEditor = false)
        {
            _testsGroup = testsGroup;
            _isEditor = isEditor;
        }

        public T LoadPrefab<T>(string name)
        where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(PrefabsPath, _testsGroup, $"{name}.prefab"));
        }
        public GameObject LoadPrefab(string name) => LoadPrefab<GameObject>(name);

        public LnxEntity InstantiateEntityPrefab(string name)
        {
            var prefab = LoadPrefab<LnxEntity>(name);
            return GameObject.Instantiate<LnxEntity>(prefab);
        }
    }
}

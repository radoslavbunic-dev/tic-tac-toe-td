using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAutomation
{
    private const string WebGlBuildFolderName = "build";
    private const string MacBuildFolderName = "Client";
    private const string MacAppName = "TicTacToeRB.app";

    [MenuItem("Tools/Build/Auto Build WebGL (Switch Back)")]
    public static void AutoBuildWebGlAndRestoreTarget()
    {
        BuildTarget originalTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup originalGroup = BuildPipeline.GetBuildTargetGroup(originalTarget);
        bool switched = false;
        List<BoolSettingSnapshot> addressablesSnapshots = null;

        try
        {
            if (originalTarget != BuildTarget.WebGL)
            {
                bool switchOk = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
                if (!switchOk)
                {
                    Debug.LogError("Failed to switch active target to WebGL.");
                    return;
                }

                switched = true;
            }

            addressablesSnapshots = ForceCompatibleAddressablesBundleSettings();

            if (!BuildAddressablesContent())
            {
                Debug.LogError("Addressables build failed. Aborting player build.");
                return;
            }

            string outputPath = Path.Combine(GetProjectRootPath(), WebGlBuildFolderName);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.WebGL,
                locationPathName = outputPath,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"WebGL build succeeded: {report.summary.totalSize} bytes, path: {outputPath}");
            }
            else
            {
                Debug.LogError($"WebGL build failed: {report.summary.result}");
            }
        }
        finally
        {
            RestoreSnapshots(addressablesSnapshots);

            if (switched && originalTarget != BuildTarget.WebGL)
            {
                bool restoreOk = EditorUserBuildSettings.SwitchActiveBuildTarget(originalGroup, originalTarget);
                if (!restoreOk)
                {
                    Debug.LogError($"Failed to restore build target to {originalTarget}.");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private static bool BuildAddressablesContent()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("Addressables settings asset was not found.");
            return false;
        }

        AddressableAssetSettings.CleanPlayerContent();
        try
        {
            AddressableAssetSettings.BuildPlayerContent();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Addressables build exception: " + ex.Message);
            return false;
        }
    }

    private static List<BoolSettingSnapshot> ForceCompatibleAddressablesBundleSettings()
    {
        var snapshots = new List<BoolSettingSnapshot>();

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            return snapshots;
        }

        // Global Addressables settings asset.
        TryForceBool(settings, "m_UseUWRForLocalBundles", false, snapshots);

        // Per-group bundled schema settings.
        if (settings.groups != null)
        {
            for (int groupIndex = 0; groupIndex < settings.groups.Count; groupIndex++)
            {
                AddressableAssetGroup group = settings.groups[groupIndex];
                if (group == null || group.Schemas == null)
                {
                    continue;
                }

                for (int schemaIndex = 0; schemaIndex < group.Schemas.Count; schemaIndex++)
                {
                    if (group.Schemas[schemaIndex] is BundledAssetGroupSchema bundledSchema)
                    {
                        TryForceBool(bundledSchema, "m_UseUWRForLocalBundles", false, snapshots);
                        TryForceBool(bundledSchema, "m_StripDownloadOptions", false, snapshots);
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
        return snapshots;
    }

    [MenuItem("Tools/Build/Auto Build Mac (Switch Back)")]
    public static void AutoBuildMacAndRestoreTarget()
    {
        BuildTarget originalTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup originalGroup = BuildPipeline.GetBuildTargetGroup(originalTarget);
        bool switched = false;

        try
        {
            if (originalTarget != BuildTarget.StandaloneOSX)
            {
                bool switchOk = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
                if (!switchOk)
                {
                    Debug.LogError("Failed to switch active target to StandaloneOSX.");
                    return;
                }

                switched = true;
            }

            string outputPath = Path.Combine(GetBuildRootPath(), MacBuildFolderName, MacAppName);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.StandaloneOSX,
                locationPathName = outputPath,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Mac build succeeded: {report.summary.totalSize} bytes, path: {outputPath}");
            }
            else
            {
                Debug.LogError($"Mac build failed: {report.summary.result}");
            }
        }
        finally
        {
            if (switched && originalTarget != BuildTarget.StandaloneOSX)
            {
                bool restoreOk = EditorUserBuildSettings.SwitchActiveBuildTarget(originalGroup, originalTarget);
                if (!restoreOk)
                {
                    Debug.LogError($"Failed to restore build target to {originalTarget}.");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private static void TryForceBool(UnityEngine.Object target, string propertyName, bool forcedValue, List<BoolSettingSnapshot> snapshots)
    {
        if (target == null)
        {
            return;
        }

        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);
        if (property == null || property.propertyType != SerializedPropertyType.Boolean)
        {
            return;
        }

        bool currentValue = property.boolValue;
        snapshots.Add(new BoolSettingSnapshot(target, propertyName, currentValue));
        if (currentValue == forcedValue)
        {
            return;
        }

        property.boolValue = forcedValue;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }

    private static void RestoreSnapshots(List<BoolSettingSnapshot> snapshots)
    {
        if (snapshots == null || snapshots.Count == 0)
        {
            return;
        }

        for (int i = snapshots.Count - 1; i >= 0; i--)
        {
            BoolSettingSnapshot snapshot = snapshots[i];
            if (snapshot.Target == null)
            {
                continue;
            }

            SerializedObject serializedObject = new SerializedObject(snapshot.Target);
            SerializedProperty property = serializedObject.FindProperty(snapshot.PropertyName);
            if (property == null || property.propertyType != SerializedPropertyType.Boolean)
            {
                continue;
            }

            if (property.boolValue == snapshot.OriginalValue)
            {
                continue;
            }

            property.boolValue = snapshot.OriginalValue;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(snapshot.Target);
        }
    }

    private static string[] GetEnabledScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        int enabledCount = 0;
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].enabled)
            {
                enabledCount++;
            }
        }

        string[] output = new string[enabledCount];
        int index = 0;
        for (int i = 0; i < scenes.Length; i++)
        {
            if (!scenes[i].enabled)
            {
                continue;
            }

            output[index] = scenes[i].path;
            index++;
        }

        return output;
    }

    private static string GetBuildRootPath()
    {
        return GetProjectRootPath();
    }

    private static string GetProjectRootPath()
    {
        return Path.GetDirectoryName(Application.dataPath);
    }

    private readonly struct BoolSettingSnapshot
    {
        public readonly UnityEngine.Object Target;
        public readonly string PropertyName;
        public readonly bool OriginalValue;

        public BoolSettingSnapshot(UnityEngine.Object target, string propertyName, bool originalValue)
        {
            Target = target;
            PropertyName = propertyName;
            OriginalValue = originalValue;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
#endif


[Serializable]
public class Template : ScriptableObject
{
    [SerializeField] int id;
    public int Id { get { return id; } }

#if UNITY_EDITOR
    public void GenerateId()
    {
        id = NameTo10DigitId(name);
        EditorUtility.SetDirty(this);
    }

    public void SetId(int newId)
    {
        id = newId;
    }

#endif

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Id > 0)
        {
            return;
        }

        int newId = NameTo10DigitId(name);
        if (id == newId)
        {
            return;
        }

        id = newId;
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(this);
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
        AddressableAssetEntry entry = settings.FindAssetEntry(guid);

        if (entry != null)
        {
            entry.address = id.ToString();
            EditorUtility.SetDirty(this);
        }
    }
    
    int NameTo10DigitId(string name)
    {
        uint h = FNV1a32(name);

        const int min = 1_000_000_000;
        const int range = int.MaxValue - min + 1;

        int id = min + (int)(h % (uint)range);

        return id;
    }

    uint FNV1a32(string input)
    {
        const uint offset = 2166136261;
        const uint prime = 16777619;

        uint hash = offset;

        for (int i = 0; i < input.Length; i++)
        {
            hash ^= input[i];
            hash *= prime;
        }

        return hash;
    }
#endif
}

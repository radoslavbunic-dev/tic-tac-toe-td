using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkinTemplate", menuName = "Templates/Skin")]
[Serializable]
public class SkinTemplate : Template
{
    [SerializeField] int order;
    [SerializeField] GameObject boardPrefab;
    [SerializeField] Sprite boardSprite;
    [SerializeField] MarkSpriteKeyPair[] markSprites;
    [SerializeField] MarkGameObjectKeyPair[] markPrefabs;

    public int Order { get { return order; } }
    public GameObject BoardPrefab { get { return boardPrefab; } }
    public Sprite BoardSprite { get { return boardSprite; } }
    public MarkSpriteKeyPair[] MarkSprites { get { return markSprites; } }
    public MarkGameObjectKeyPair[] MarkPrefabs { get { return markPrefabs; } }

    public Sprite GetMarkSprite(TicTacToeMark mark)
    {
        for (int i = 0; i < markSprites.Length; i++)
        {
            if (markSprites[i].Key == mark)
            {
                return markSprites[i].Value;
            }
        }
        Debug.LogError($"Missing sprite for key: {mark} for skin: {Id}");
        return null;
    }

    public GameObject GetMarkPrefab(TicTacToeMark mark)
    {
        for (int i = 0; i < markPrefabs.Length; i++)
        {
            if (markPrefabs[i].Key == mark)
            {
                return markPrefabs[i].Value;
            }
        }
        Debug.LogError($"Missing prefab for key: {mark} for skin: {Id}");
        return null;
    }
}
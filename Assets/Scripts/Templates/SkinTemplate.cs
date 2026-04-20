using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkinTemplate", menuName = "Templates/Skin")]
[Serializable]
public class SkinTemplate : Template
{
    [SerializeField] GameObject boardPrefab;
    [SerializeField] Sprite boardSprite;
    [SerializeField] Sprite xSprite;
    [SerializeField] GameObject xPrefab;
    [SerializeField] Sprite oSprite;
    [SerializeField] GameObject oPrefab;
    [SerializeField] int order;

    public GameObject BoardPrefab { get { return boardPrefab; } }
    public Sprite BoardSprite { get { return boardSprite; } }
    public Sprite XSprite { get { return xSprite; } }
    public GameObject XPrefab { get { return xPrefab; } }
    public Sprite OSprite { get { return oSprite; } }
    public GameObject OPrefab { get { return oPrefab; } }
    public int Order { get { return order; } }
}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkinTemplate", menuName = "Templates/Skin")]
[Serializable]
public class SkinTemplate : Template
{
    [SerializeField] Sprite boardSprite;
    [SerializeField] Sprite xSprite;
    [SerializeField] Sprite oSprite;
    [SerializeField] int order;


    public Sprite BoardSprite { get { return boardSprite; } }
    public Sprite XSprite { get { return xSprite; } }
    public Sprite OSprite { get { return oSprite; } }
    public int Order { get { return order; } }
}
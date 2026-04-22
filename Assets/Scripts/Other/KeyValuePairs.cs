using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SoundKeyPair
{
    public string Key;
    public AudioClip Value;
}

[Serializable]
public class MarkGameObjectKeyPair
{
    public TicTacToeMark Key;
    public GameObject Value;
}

[Serializable]
public class MarkSpriteKeyPair
{
    public TicTacToeMark Key;
    public Sprite Value;
}

[Serializable]
public class MarkTextKeyPair
{
    public TicTacToeMark Key;
    public TextMeshProUGUI Value;
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSlot : MonoBehaviour
{
    public Action<UISkinSlot> OnSelected;

    [SerializeField] Image tableImage;
    [SerializeField] Image xImage;
    [SerializeField] Image oImage;
    [SerializeField] Toggle selectionToggle;

    public SkinTemplate Template { get; private set; }

    public void Init(SkinTemplate skin, bool isSelected = false)
    {
        Template = skin;
        tableImage.sprite = skin.BoardSprite;
        xImage.sprite = skin.XSprite;
        oImage.sprite = skin.OSprite;
        SetSelection(isSelected);
        selectionToggle.onValueChanged.SetListener((value) =>
        {
            if (value)
            {
                OnSelected?.Invoke(this);
            }
        });
    }

    public void SetSelection(bool isSelected)
    {
        selectionToggle.isOn = isSelected;
    }

    public void SetSelectionWithoutNotify(bool isSelected)
    {
        selectionToggle.SetIsOnWithoutNotify(isSelected);
    }
}

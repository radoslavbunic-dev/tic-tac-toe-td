using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIPopups : MonoBehaviour
{
    [SerializeField] RectTransform popupContent;
    public static Action<PopupData> ShowPopup;

    void OnEnable()
    {
        ShowPopup += OnPopupShown;
    }

    void OnDisable()
    {
        ShowPopup -= OnPopupShown;
    }

    public virtual void OnPopupShown(PopupData data)
    {
        if (!data.AllowMultiple)
        {
            var popups = popupContent.GetComponentsInChildren<UIPopup>();
            foreach (var popup in popups)
            {
                if (popup.Data.Type == data.Type)
                {
                    popup.Close();
                    return;
                }
            }
        }
        if (!data.SkipCleanUp)
        {
            CleanUp();
        }
        Addressables.LoadAssetAsync<GameObject>(data.Type.ToString()).Completed += operation => OnPopupLoaded(operation, data);
    }

    void OnPopupLoaded(AsyncOperationHandle<GameObject> operation, PopupData data)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            return;
        }
        var popup = Instantiate(operation.Result, popupContent).GetComponent<UIPopup>();
        popup.Init(data);
        popup.OnWindowClosed += (UIWindow window) =>
        {
            if (operation.IsValid())
            {
                Addressables.Release(operation);
            }
            Destroy(popup.gameObject);
        };
        
        GameEvents.PlaySFX(AudioClipsId.Popup);
        data.Callback?.Invoke(popup);
    }

    protected virtual void CleanUp()
    {
        var popups = popupContent.GetComponentsInChildren<UIPopup>();
        foreach (var item in popups)
        {
            if (!item.Data.IsPersistent)
            {
                item.Close();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class UIPreGamePopup : UIPopup
{
    [SerializeField] protected Button startButton;
    [SerializeField] protected GameObject skinSlotPrefab;
    [SerializeField] protected Transform skinSlotsContainer;
    [SerializeField] ScrollRect skinsScrollRect;
    AsyncOperationHandle<IList<SkinTemplate>> skinsLoadingOperation;
    Dictionary<int, SkinTemplate> allSkins = new Dictionary<int, SkinTemplate>();
    readonly List<UISkinSlot> skinSlots = new List<UISkinSlot>();
    public int CurrentSkinId { get; private set; }

    protected override void Start()
    {
        skinsLoadingOperation = Addressables.LoadAssetsAsync<SkinTemplate>("skin", null);
        StartCoroutine(LoadSkins());
    }

    IEnumerator LoadSkins()
    {
        while (!skinsLoadingOperation.IsDone)
        {
            yield return null;
        }
        
        if (skinsLoadingOperation.Status != AsyncOperationStatus.Succeeded)
        {
            Close();
            yield break;
        }

        CurrentSkinId = PlayerPrefs.GetInt("CurrentSkin", 0);
        if (CurrentSkinId <= 0)
        {
            CurrentSkinId = skinsLoadingOperation.Result[0].Id;
        }

        allSkins.Clear();
        skinSlots.Clear();
        var skins = skinsLoadingOperation.Result.OrderBy(skin => skin.Order).ToList();
        UISkinSlot selectedSkinSlot = null;

        foreach (var skin in skins)
        {
            var skinSlot = Instantiate(skinSlotPrefab, skinSlotsContainer).GetComponent<UISkinSlot>();
            skinSlot.OnSelected += OnSkinSlotClicked;
            bool isSelected = skin.Id == CurrentSkinId;
            skinSlot.Init(skin, isSelected);
            if (isSelected)
            {
                selectedSkinSlot = skinSlot;
            }
            allSkins.Add(skin.Id, skin);
            skinSlots.Add(skinSlot);
        }

        startButton.onClick.SetListener(() =>
        {
            if (!allSkins.TryGetValue(CurrentSkinId, out var skin))
            {
                return;
            }
            
            new LoadingGameSceneState(skin, 3, 3).Enter(Data.SourceState);
        });

        base.Start();

        if (skinsScrollRect != null)
        {
            if (selectedSkinSlot == null && skinSlots.Count > 0)
            {
                selectedSkinSlot = skinSlots[0];
            }

            if (selectedSkinSlot != null)
            {
                StartCoroutine(FocusSelectedSkinAfterPopupOpen(selectedSkinSlot));
            }
        }
    }

    void OnSkinSlotClicked(UISkinSlot skinSlot)
    {
        for (int i = 0; i < skinSlots.Count; i++)
        {
            bool isSelected = skinSlots[i] == skinSlot;
            skinSlots[i].SetSelectionWithoutNotify(isSelected);
        }

        ScrollToSlot(skinSlot.transform as RectTransform);
        CurrentSkinId = skinSlot.Template.Id;
        PlayerPrefs.SetInt("CurrentSkin", skinSlot.Template.Id);
        PlayerPrefs.Save();
    }

    IEnumerator FocusSelectedSkinAfterPopupOpen(UISkinSlot selectedSkinSlot)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = skinSlotsContainer as RectTransform;
        if (contentRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }

        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();

        ScrollToSlot(selectedSkinSlot.transform as RectTransform);
    }

    void ScrollToSlot(RectTransform slotRect)
    {
        if (slotRect == null || skinsScrollRect == null || skinsScrollRect.content == null)
        {
            return;
        }

        RectTransform viewportRect = skinsScrollRect.viewport;
        if (viewportRect == null)
        {
            viewportRect = skinsScrollRect.GetComponent<RectTransform>();
        }
        if (viewportRect == null)
        {
            return;
        }

        skinsScrollRect.StopMovement();
        skinsScrollRect.velocity = Vector2.zero;

        Bounds viewportBounds = new Bounds(viewportRect.rect.center, viewportRect.rect.size);
        Bounds contentBounds = GetBoundsInViewSpace(viewportRect, skinsScrollRect.content);
        Bounds slotBounds = GetBoundsInViewSpace(viewportRect, slotRect);

        if (skinsScrollRect.horizontal)
        {
            float hiddenWidth = contentBounds.size.x - viewportBounds.size.x;
            if (hiddenWidth > 0f)
            {
                float offset = viewportBounds.center.x - slotBounds.center.x;
                float normalizedOffset = offset / hiddenWidth;
                float target = skinsScrollRect.horizontalNormalizedPosition - normalizedOffset;
                skinsScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(target);
            }
        }

        if (skinsScrollRect.vertical)
        {
            float hiddenHeight = contentBounds.size.y - viewportBounds.size.y;
            if (hiddenHeight > 0f)
            {
                float offset = viewportBounds.center.y - slotBounds.center.y;
                float normalizedOffset = offset / hiddenHeight;
                float target = skinsScrollRect.verticalNormalizedPosition + normalizedOffset;
                skinsScrollRect.verticalNormalizedPosition = Mathf.Clamp01(target);
            }
        }
    }

    Bounds GetBoundsInViewSpace(RectTransform viewRect, RectTransform targetRect)
    {
        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners);
        Matrix4x4 worldToLocal = viewRect.worldToLocalMatrix;

        Vector3 firstCorner = worldToLocal.MultiplyPoint3x4(corners[0]);
        Bounds bounds = new Bounds(firstCorner, Vector3.zero);
        for (int i = 1; i < corners.Length; i++)
        {
            Vector3 corner = worldToLocal.MultiplyPoint3x4(corners[i]);
            bounds.Encapsulate(corner);
        }

        return bounds;
    }
}
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform boardRoot;
    [SerializeField] SpriteRenderer backgroundRenderer;
    [SerializeField] float gameplayPlaneZ = 0f;
    [SerializeField] [Range(0.2f, 1.5f)] float boardViewportFill = 1;
    [SerializeField] float boardScaleMultiplier = 1f;

    GameObject activeBoard;

    void OnEnable()
    {
        UIManager.OnOrientationChanged += OnViewportChanged;
    }

    void OnDisable()
    {
        UIManager.OnOrientationChanged -= OnViewportChanged;
    }

    void Start()
    {
        LoadBoardFromSelectedSkin();
        FitLayout();
    }

    void OnViewportChanged(ScreenOrientation orientation)
    {
        FitLayout();
    }

    void LoadBoardFromSelectedSkin()
    {
        SkinTemplate selectedSkin = GameSession.SelectedSkin;
        if (selectedSkin == null)
        {
            Debug.LogWarning("BoardManager: Selected skin is null.");
            return;
        }

        if (selectedSkin.BoardPrefab == null)
        {
            Debug.LogWarning($"BoardManager: Skin '{selectedSkin.name}' has no board prefab assigned.");
            return;
        }

        Transform parent = boardRoot != null ? boardRoot : transform;
        activeBoard = Instantiate(selectedSkin.BoardPrefab, parent);
        activeBoard.transform.localPosition = Vector3.zero;
    }

    void FitLayout()
    {
        Camera cam = ViewController.Instance != null ? ViewController.Instance.MainCamera : Camera.main;
        if (cam == null)
        {
            return;
        }

        PositionBoardRootAtViewportCenter(cam);
        FitBackground(cam);
        if (activeBoard != null)
        {
            FitBoardSquare(cam, activeBoard);
        }
    }

    void PositionBoardRootAtViewportCenter(Camera cam)
    {
        if (boardRoot == null)
        {
            return;
        }

        Vector3 center = ViewController.GetViewportWorldCenter(cam, gameplayPlaneZ);
        boardRoot.position = new Vector3(center.x, center.y, gameplayPlaneZ);
    }

    void FitBackground(Camera cam)
    {
        if (backgroundRenderer == null || backgroundRenderer.sprite == null)
        {
            return;
        }

        Vector2 worldSize = ViewController.GetVisibleWorldSize(cam);
        if (worldSize.x <= 0f || worldSize.y <= 0f)
        {
            return;
        }

        Vector3 spriteLocalSize = backgroundRenderer.sprite.bounds.size;
        float coverScale = Mathf.Max(worldSize.x / spriteLocalSize.x, worldSize.y / spriteLocalSize.y);

        Transform t = backgroundRenderer.transform;
        t.localScale = new Vector3(coverScale, coverScale, 1f);

        Vector3 center = ViewController.GetViewportWorldCenter(cam, gameplayPlaneZ);
        t.position = new Vector3(center.x, center.y, gameplayPlaneZ);
    }

    void FitBoardSquare(Camera cam, GameObject board)
    {
        Vector2 worldSize = ViewController.GetVisibleWorldSize(cam);
        float minDimension = Mathf.Min(worldSize.x, worldSize.y);
        float targetSide = minDimension * boardViewportFill;

        board.transform.localScale = Vector3.one;

        Renderer[] renderers = board.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return;
        }

        Bounds combined = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combined.Encapsulate(renderers[i].bounds);
        }

        float maxSide = Mathf.Max(combined.size.x, combined.size.y, combined.size.z);
        if (maxSide <= Mathf.Epsilon)
        {
            return;
        }

        float uniformScale = (targetSide / maxSide) * boardScaleMultiplier;
        board.transform.localScale = Vector3.one * uniformScale;

        if (boardRoot != null)
        {
            board.transform.localPosition = Vector3.zero;
        }
        else
        {
            Vector3 center = ViewController.GetViewportWorldCenter(cam, gameplayPlaneZ);
            board.transform.position = new Vector3(center.x, center.y, gameplayPlaneZ);
        }
    }
}

using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform boardRoot;
    [SerializeField] SpriteRenderer backgroundRenderer;
    [SerializeField] float gameplayPlaneZ = 0f;
    [SerializeField] [Range(0.2f, 1.5f)] float boardViewportFill = 1;
    [SerializeField] float boardScaleMultiplier = 1f;

    Board board;
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        ViewController.OnOrientationChanged += OnViewportChanged;
        GameState.OnStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        ViewController.OnOrientationChanged -= OnViewportChanged;
        GameState.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(IState fromState, IState toState)
    {
        if (toState is StartGameState startGameState)
        {
            CreateBoard(startGameState.GameData.Skin);
            FitLayout();
        }
    }

    void CreateBoard(SkinTemplate skin)
    {
        boardRoot.DestroyChildren();
        board = Instantiate(skin.BoardPrefab, boardRoot).GetComponent<Board>();
        board.transform.localPosition = Vector3.zero;
        board.Init(skin);
    }

    void OnViewportChanged(ScreenOrientation orientation)
    {
        if (!board)
        {
            return;
        }
        FitLayout();
    }

    void FitLayout()
    {
        PositionBoardRootAtViewportCenter();
        FitBackground();
        FitBoardSquare();
    }

    Vector2 GetVisibleWorldSize()
    {
        if (mainCamera == null || !mainCamera.orthographic)
        {
            return Vector2.zero;
        }

        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }

    void PositionBoardRootAtViewportCenter()
    {
        if (boardRoot == null)
        {
            return;
        }

        Vector3 center = GetViewportWorldCenter(gameplayPlaneZ);
        boardRoot.position = new Vector3(center.x, center.y, gameplayPlaneZ);
    }

    Vector3 GetViewportWorldCenter(float planeZ)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Mathf.Abs(ray.direction.z) < 0.001)
        {
            return new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, planeZ);
        }

        float distance = (planeZ - ray.origin.z) / ray.direction.z;
        return ray.GetPoint(distance);
    }

    void FitBackground()
    {
        if (backgroundRenderer == null || backgroundRenderer.sprite == null)
        {
            return;
        }

        Vector2 worldSize = GetVisibleWorldSize();
        if (worldSize.x <= 0f || worldSize.y <= 0f)
        {
            return;
        }

        Vector3 spriteLocalSize = backgroundRenderer.sprite.bounds.size;
        float coverScale = Mathf.Max(worldSize.x / spriteLocalSize.x, worldSize.y / spriteLocalSize.y);

        Transform rendTransform = backgroundRenderer.transform;
        rendTransform.localScale = new Vector3(coverScale, coverScale, 1f);

        Vector3 center = GetViewportWorldCenter(gameplayPlaneZ);
        rendTransform.position = new Vector3(center.x, center.y, gameplayPlaneZ);
    }

    void FitBoardSquare()
    {
        Vector2 worldSize = GetVisibleWorldSize();
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

        float uniformScale = targetSide / maxSide * boardScaleMultiplier;
        board.transform.localScale = Vector3.one * uniformScale;
        board.transform.localPosition = Vector3.zero;
    }
}

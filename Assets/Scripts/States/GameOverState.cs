using UnityEngine;

public class GameOverState : GameState
{
    public GameOverData GameOverData { get; private set; }
    public UIGameOver Window { get; private set; }

    public GameOverState(GameOverData gameOverData)
    {
        GameOverData = gameOverData;
    }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        UIWindows.ShowWindow(new()
        {
            Type = WindowType.GameOver,
            Callback = OnWindowLoaded
        });
    }

    void OnWindowLoaded(UIWindow window)
    {
        Window = (UIGameOver)window;
        Window.OnPlayAgainButtonClicked += OnPlayAgainButtonClicked;
        Window.OnCloseButtonClicked += OnCloseButtonClicked;
        Window.Init(GameOverData);
    }

    void OnPlayAgainButtonClicked()
    {
        var gameData = GameOverData.GameData;
        new LoadingGameSceneState(gameData.Skin, gameData.GridSizeX, gameData.GridSizeY).Enter(this);
    }
    
    void OnCloseButtonClicked()
    {
        new LoadingPlaySceneState().Enter(this);
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            new MainMenuState().Enter(this);
        }
    }

    public override void Exit()
    {
        if (Window)
        {
            Window.OnPlayAgainButtonClicked -= OnPlayAgainButtonClicked;
            Window.OnCloseButtonClicked -= OnCloseButtonClicked;
            Window = null;
        }
        base.Exit();
    }
}

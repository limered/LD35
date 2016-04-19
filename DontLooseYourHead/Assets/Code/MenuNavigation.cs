using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public const int MainMenuScene = 0;
    public const int LeaderboardScene = 1;
    public const int GameScene = 2;

    public MenuNavigation()
    {
        IoC.RegisterSingleton<MenuNavigation>(this);
    }

    public void NavigateToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    public void NavigateToLeaderboard()
    {
        SceneManager.LoadScene(LeaderboardScene);
    }

    public void NavigateToGame()
    {
        SceneManager.LoadScene(GameScene);
    }
}

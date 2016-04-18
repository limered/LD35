using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public MenuNavigation()
    {
        IoC.RegisterSingleton<MenuNavigation>(this);
    }

    void Start()
    {

    }

    void Update()
    {

    }


    public void NavigateToMainMenu()
    {
        SceneManager.LoadScene("main_menu");
    }

    public void NavigateToLeaderboard()
    {
        SceneManager.LoadScene("leaderboard");
    }

    public void NavigateToGame()
    {
        SceneManager.LoadScene("base");
    }
}

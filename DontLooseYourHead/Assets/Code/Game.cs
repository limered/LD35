using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private Leaderboard leaderboard;

    [SerializeField]
    private Texture2D startShape;

    public Texture2D StartShape { get { return startShape; } }

    [SerializeField]
    private Texture2D[] validShapes;
    public Texture2D[] ValidShapes { get { return validShapes; } }

    public Game()
    {
        IoC.RegisterSingleton<Game>(this);
    }

    void Start()
    {
        leaderboard = IoC.Resolve<Leaderboard>();

        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("game start");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}

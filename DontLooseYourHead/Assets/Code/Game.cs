using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private Leaderboard leaderboard;

    [SerializeField]
    private Texture2D[] _validShapes;
    public Texture2D[] ValidShapes { get { return _validShapes; } }

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

        var shape = IoC.Resolve<ShapeCreator>().GenerateNextShape();
        
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

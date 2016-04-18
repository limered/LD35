using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private Leaderboard leaderboard;

    private AudioSource audio;

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
        audio = gameObject.GetComponent<AudioSource>();
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

    public void SlowdownMusic()
    {
        audio.pitch = 0.1f;
    }

    public void SpeedupMusic()
    {
        audio.pitch = 1f;
    }
}

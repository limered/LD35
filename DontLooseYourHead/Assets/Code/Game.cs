using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] _validShapes;
    public Texture2D[] ValidShapes { get { return _validShapes; } }

    public Game()
    {
        IoC.RegisterSingleton<Game>(this);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("start");

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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField]
    private InputField name;

    private Leaderboard leaderboard;
    private bool initilized = false;

    void Start()
    {
        leaderboard = IoC.Resolve<Leaderboard>();
        name.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!initilized && !leaderboard.IsInitializing)
        {
            initilized = true;
            name.text = leaderboard.PlayerName;
            name.gameObject.SetActive(true);
        }
    }


    public void SetPlayerName()
    {
        IoC.Resolve<Leaderboard>().CreateNewPlayer(name.text);
    }
}

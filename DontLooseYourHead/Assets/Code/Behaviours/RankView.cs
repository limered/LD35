using UnityEngine;
using UnityEngine.UI;

public class RankView : MonoBehaviour
{
    private Leaderboard leaderboard;

    public Text name;

    public Text score;

    void Start()
    {
        leaderboard = IoC.Resolve<Leaderboard>();
    }

    void Update()
    {

    }
}

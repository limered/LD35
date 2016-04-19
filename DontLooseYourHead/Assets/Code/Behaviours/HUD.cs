using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text nextScoreText;

    [SerializeField]
    private Text endScoreText;

    [SerializeField]
    private Text bloodText;

    [SerializeField]
    private GameObject gameOverPanel;

    private float score;
    private float nextScore;

    public float Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = score.ToString(CultureInfo.InvariantCulture);
            endScoreText.text = score.ToString(CultureInfo.InvariantCulture);
        }
    }

    public float NextScore
    {
        get
        {
            return nextScore;
        }
        set
        {
            nextScore = value;
            nextScoreText.text = "+ "+nextScore.ToString(CultureInfo.InvariantCulture);
        }
    }

    private float blood;
    public float Blood
    {
        get
        {
            return blood;
        }
        set
        {
            blood = value;
            bloodText.text = ((int)blood).ToString(CultureInfo.InvariantCulture)+ " ml";
        }
    }

    public HUD()
    {
        IoC.RegisterSingleton(this);
    }


    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }


    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        var player = IoC.Resolve<Player>();
        NextScore = player.tempPoints;
        Blood = player.Blood;
    }
}

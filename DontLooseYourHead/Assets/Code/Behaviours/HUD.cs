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

    private float test;
    void Update()
    {
        test += Time.deltaTime;
        if (test>5)
        {
            ShowGameOverScreen();
        }
    }
}

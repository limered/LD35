using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{

    private Leaderboard leaderboard;

    [SerializeField]
    private List<RankView> ranks = new List<RankView>();

    [SerializeField]
    private Text highscoreText;

    [SerializeField]
    private GameObject loadingPanel;

    public bool Initialized = false;

    void Start()
    {
        Initialized = false;
        if (loadingPanel != null) loadingPanel.SetActive(true);
        leaderboard = IoC.Resolve<Leaderboard>();
        leaderboard.Prepare();

        ranks.Clear();
        var rankViews = gameObject.GetComponentsInChildren<RankView>();
        foreach (var rankView in rankViews)
        {
            rankView.name.text = "";
            rankView.score.text = "";
            ranks.Add(rankView);
        }
    }

    void Update()
    {
        if (!Initialized)
        {
            if (!leaderboard.IsInitializing)
            {
                Initialized = true;
                if (loadingPanel != null) loadingPanel.SetActive(false);
                ReadData();
            }
        }
    }

    private void ReadData()
    {
        var board = leaderboard.board;
        for (int i = 0; i < board.Count; i++)
        {
            if (i < ranks.Count)
            {
                ranks[i].name.text = board[i].name.Contains("#") 
                    && board[i].name.LastIndexOf("#", StringComparison.InvariantCulture) < board[i].name.Length 
                        ? board[i].name.Substring(0, board[i].name.LastIndexOf("#", StringComparison.InvariantCulture))
                        : board[i].name;
                ranks[i].score.text = board[i].highscore.ToString();
            }
        }

        highscoreText.text = leaderboard.PlayerName + ": " + leaderboard.Highscore;
    }
}

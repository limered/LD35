using System;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;
using UnityEngine;

public class Leaderboard
{
    public struct Ranking
    {
        public string name;
        public float highscore;
        public string rank;
    }

    public List<Ranking> board = new List<Ranking>();

    private GameService gameService;
    private ScoreBoardService scoreBoardService;
    private string playerName;
    private string playerSuffix;
    public string PlayerNameLong { get { return playerName+"#"+playerSuffix; } }

    public bool IsInitializing { get; set; }
    private float highscore;

    public float LastScore { get; set; }

    public float Highscore
    {
        get { return highscore; }
        set
        {
            highscore = value; 
            PlayerPrefs.SetFloat("highscore_"+PlayerName, highscore);
            PlayerPrefs.Save();
        }
    }

    public void NewScore(float hs)
    {
        LastScore = hs;
        if (hs > Highscore)
        {
            Highscore = hs;
            SaveScore(hs);
        }
    }

    private string rank;

    public string Rank
    {
        get { return rank; }
        set
        {
            rank = value; 
            PlayerPrefs.GetString("rank_"+PlayerName, rank);
            PlayerPrefs.Save();
        }
    }

    public string PlayerName
    {
        get { return playerName; }
    }

    public Leaderboard()
    {
        Prepare();
    }

    public void Prepare()
    {
        try
        {
            IsInitializing = true;

            RegisterGame(Init);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void RegisterGame(Action whenDone)
    {
        App42API.Initialize(
            //API Key
            "083addb87189cffe6604aa3036dfedf29369ebbfa0bae8edc91e7b0c9c727003"
            ,
            //Secret
            "fd0cf72bb1850414b9624eaa3d679d91e6de7fc51ce5dd064088652923fcbafe"
            );

        gameService = App42API.BuildGameService();
        scoreBoardService = App42API.BuildScoreBoardService();

        gameService.GetGameByName(GameConstants.GameName,
            new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, App42NotFoundException>(
                game =>
                {
                    Debug.Log("game exists");
                    whenDone();
                },
                notFound =>
                {
                    gameService.CreateGame(GameConstants.GameName, GameConstants.GameDescription,
                        new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, App42NotFoundException>(
                            success =>
                            {
                                Debug.Log("game created");
                                whenDone();
                            },
                            noInternet => {}));
                }
                ));
    }

    private void Init()
    {
        ReadPlayerData(()=> GetTopRankings(10, (list, err) =>
        {
            if (list != null)
            {
                board.Clear();

                foreach (var score in list)
                {
                    board.Add(new Ranking
                    {
                        name = score.userName,
                        highscore = (float)score.value,
                        rank = score.rank
                    });
                }

                IsInitializing = false;
            }
            else Debug.LogError(err);
        }));
    }

    private void ReadPlayerData(Action whenDone)
    {
        ReadSettings(out playerName, out playerSuffix, out highscore, out rank);

        GetHighscore((hs, err)=>
        {
            if (hs != null)
            {
                highscore = (float) hs.value;
                rank = hs.rank;
            }
            else
            {
                highscore = 0;
                rank = "0";
            }

            whenDone();
        });
    }

    private void ReadSettings(out string name, out string suffix, out float high, out string ranking)
    {
        name = PlayerPrefs.GetString("playerName", "LD35");
        suffix = PlayerPrefs.GetString(name, DateTime.Now.Ticks.ToString());
        high = PlayerPrefs.GetFloat("highscore_" + name, 0f);
        ranking = PlayerPrefs.GetString("rank_" + name, "0");
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.SetString(name, suffix);
        PlayerPrefs.SetFloat("highscore_" + name, high);
        PlayerPrefs.GetString("rank_" + name, ranking);
        PlayerPrefs.Save();
    }

    public void CreateNewPlayer(string name=null)
    {
        name = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()) ? "LD35" : name;
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.Save();

        ReadSettings(out playerName, out playerSuffix, out highscore, out rank);
    }

    private void SaveScore(double score, Action<bool, Exception> whenDone = null)
    {
        scoreBoardService.SaveUserScore(GameConstants.GameName, PlayerNameLong, score, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, Exception>(
            game => { if (whenDone != null) whenDone(true, null); },
            error => { if (whenDone != null) whenDone(false, error); }
        ));
    }

    private void GetScores(Action<IList<com.shephertz.app42.paas.sdk.csharp.game.Game.Score>, Exception> whenDone = null)
    {
        scoreBoardService.GetScoresByUser(GameConstants.GameName, PlayerNameLong, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, Exception>(
            game => { if (whenDone != null) whenDone(game.GetScoreList(), null); },
            error => { if (whenDone != null) whenDone(null, error); }
        ));
    }

    private void GetHighscore(Action<com.shephertz.app42.paas.sdk.csharp.game.Game.Score, Exception> whenDone = null)
    {
        scoreBoardService.GetHighestScoreByUser(GameConstants.GameName, PlayerNameLong, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, Exception>(
            game => { if (whenDone != null) whenDone(game.scoreList.Count > 0 ? game.scoreList[0] : null, null); },
            error => { if (whenDone != null) whenDone(null, error); }
        ));
    }

    private void HasScore(Action<bool, Exception> whenDone)
    {
        GetHighscore((score, exception) => whenDone(score != null, exception));
    }

    private void GetTopRankings(int limit, Action<IList<com.shephertz.app42.paas.sdk.csharp.game.Game.Score>, Exception> whenDone)
    {
        scoreBoardService.GetTopNRankings(GameConstants.GameName, limit, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, Exception>(
            game => { if (whenDone != null) whenDone(game.scoreList, null); },
            error => { if (whenDone != null) whenDone(null, error); }
        ));
    }

    private class LeaderboardCallback<TResponse, TException> : App42CallBack
        where TResponse : App42Response
        where TException : Exception
    {
        private readonly Action<TResponse> onSuccess;
        private readonly Action<TException> onError;

        public LeaderboardCallback(Action<TResponse> onSuccess = null, Action<TException> onError = null)
        {
            this.onSuccess = onSuccess;
            this.onError = onError;
        }

        public void OnSuccess(object response)
        {
            if (onSuccess != null) onSuccess((TResponse)response);
            else Debug.Log(response);
        }

        public void OnException(Exception ex)
        {
            if (onError != null) onError(ex as TException);
            else Debug.LogError(ex.GetType().FullName + ": " + ex.Message);
        }
    }
}

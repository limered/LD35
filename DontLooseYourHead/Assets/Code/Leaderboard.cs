//using System;
//using com.shephertz.app42.paas.sdk.csharp;
//using com.shephertz.app42.paas.sdk.csharp.game;
//using UnityEngine;

//public class Leaderboard
//{
//    private com.shephertz.app42.paas.sdk.csharp.game.Game app42Game;
//    private GameService gameService;
//    private ScoreBoardService scoreBoardService;

//    public Leaderboard()
//    {
//        Init();
        
//        gameService.GetGameByName(GameConstants.GameName, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, App42NotFoundException>(game =>
//        {
//            Debug.Log("game exists");
//        },
//            notFound =>
//            {
//                gameService.CreateGame(GameConstants.GameName, GameConstants.GameDescription, new LeaderboardCallback<com.shephertz.app42.paas.sdk.csharp.game.Game, App42NotFoundException>(
//                    success =>
//                    {
//                        Debug.Log("game created");
//                    }));
//            }
//        ));
        
//    }

//    private void Init()
//    {
//        App42API.Initialize(
//            //API Key
//            "083addb87189cffe6604aa3036dfedf29369ebbfa0bae8edc91e7b0c9c727003"
//            ,
//            //Secret
//            "fd0cf72bb1850414b9624eaa3d679d91e6de7fc51ce5dd064088652923fcbafe"
//            );


//        gameService = App42API.BuildGameService();
//        scoreBoardService = App42API.BuildScoreBoardService();
//    }

//    public class LeaderboardCallback<TResponse,TException> : App42CallBack 
//        where TResponse : App42Response
//        where TException : Exception
//    {
//        private readonly Action<TResponse> onSuccess;
//        private readonly Action<TException> onError;

//        public LeaderboardCallback(Action<TResponse> onSuccess =null, Action<TException> onError=null)
//        {
//            this.onSuccess = onSuccess;
//            this.onError = onError;
//        }

//        public void OnSuccess(object response)
//        {
//            if (onSuccess != null) onSuccess((TResponse)response);
//            else Debug.Log(response);
//        }

//        public void OnException(Exception ex)
//        {
//            if (onError != null) onError(ex as TException);
//            else Debug.LogError(ex.GetType().FullName + ": " + ex.Message);
//        }
//    }
//}

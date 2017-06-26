using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class ScoreManager : MonoBehaviour {
    //public Text timerText;          //The timer text below the player sprite
    public Text pointText;          //The timer text below the player sprite
    public Text scoreText;          //The score text shown when game is lost
    public Text highscoreText;      //The highscore textshown when not playing
    public Button leaderboardButton;

    //private string leaderboardID = "";
    //private Coroutine _timerRoutine;
    private static ScoreManager _instance;
    private int _curScore = 0;
    private const string HIGHSCORE_PREFS_PATH = "CMI2_Highscore_";
    private const string LASTSCORE_PREFS_PATH = "CMI2_Lastscore_";
    //private const string LEADERBOARD_PATH = "Fsdf";

    public static ScoreManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject scoreManagerObject = new GameObject("ScoreManager");
                _instance = scoreManagerObject.AddComponent<ScoreManager>();
                DontDestroyOnLoad(scoreManagerObject);
            }
            return _instance;
        }
    }



    private void Awake()
    {
        if (pointText == null)
            pointText = GameObject.Find("PointText").GetComponent<Text>();
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (highscoreText == null)
            highscoreText = GameObject.Find("BestScore").GetComponent<Text>();
        if (leaderboardButton == null)
            leaderboardButton = GameObject.Find("LeaderboardButton").GetComponent<Button>();

        EventManager.Start += GameStart;
        EventManager.Lost += GameLost;
        EventManager.Correct += AddPoint;

        if (leaderboardButton)
            leaderboardButton.onClick.AddListener(() => OpenLeaderboard());
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!GameManager.isEditor)
        {

        }

        if (PlayerPrefs.HasKey(GetHighscorePath()))
        {
            highscoreText.text = "Best:\n" + PlayerPrefs.GetInt(GetHighscorePath());
        }
        else
        {
            PlayerPrefs.SetInt(GetHighscorePath(), 0);
        }

        if (PlayerPrefs.HasKey(GetLastscorePath()))
        {
            scoreText.text = "Last:\n" + PlayerPrefs.GetInt(GetLastscorePath());
        }
        else
        {
            scoreText.text = "Last:\n" + "0";
            PlayerPrefs.SetInt(GetLastscorePath(), 0);
        }
    }


    
    private void OpenLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else{
            //TODO: Show error window to player
        }
    }



    private void AddPoint()
    {
        SetScore(_curScore + 1);
        
    }


    public void GameStart()
    {
        //_timerRoutine = StartCoroutine(GameTimer());
        SetScore(0);
    }



    public void SetScore(int score)
    {
        _curScore = score;
        pointText.text = _curScore.ToString();
    }




    public void GameLost()
    {
        //StopCoroutine(_timerRoutine);
        ReportScore(_curScore);
        UpdateScores();
    }



    public void UpdateScores()
    {
        scoreText.text = "Last:\n" + PlayerPrefs.GetInt(GetLastscorePath());
        highscoreText.text = "Best:\n" + PlayerPrefs.GetInt(GetHighscorePath());
    }


    //private IEnumerator GameTimer()
    //{
    //    timerText.text = "00:00:00";
    //    float timeAtStart = Time.time;

    //    while (true)
    //    {
    //        _curScore = Time.time - timeAtStart;
    //        timerText.text = GetScoreString(_curScore);

    //        yield return new WaitForFixedUpdate();
    //    }
    //}



    //private string GetScoreString(float score)
    //{
    //    if (score == 0)
    //        return "--:--:--";

    //    float minutes = score / 60;
    //    float seconds = score % 60;
    //    float fraction = score * 100;
    //    fraction = fraction % 100;
        
    //    return string.Format("{0:00}:{1:00}:{2:00}", (int)minutes, (int)seconds, (int)fraction);
    //}



    public void ReportScore(int score)
    {
        if (Social.localUser.authenticated)
        {
            string leaderboard = "";
            switch (GameManager.currentDifficulty)
            {
                case E_Difficulty.Easy:
                    leaderboard = CMI_Resources.leaderboard_easy;
                    break;
                case E_Difficulty.Medium:
                    leaderboard = CMI_Resources.leaderboard_medium;
                    break;
                case E_Difficulty.Hard:
                    leaderboard = CMI_Resources.leaderboard_hard;
                    break;
            }
            Social.ReportScore(score, leaderboard, (bool success) => {
                // handle success or failure
            });
        }

        if(score > PlayerPrefs.GetInt(GetHighscorePath()))
        {
            //Debug.Log("New highscore of " + score);
            PlayerPrefs.SetInt(GetHighscorePath(), score);
        }
        else
        {
           // Debug.Log("No new highscore. Score: " + score);
        }

        PlayerPrefs.SetInt(GetLastscorePath(), score);

        PlayerPrefs.Save();
        UpdateScores();
    }


    private string GetHighscorePath()
    {
        return HIGHSCORE_PREFS_PATH + GameManager.currentDifficulty.ToString();
    }



    private string GetLastscorePath()
    {
        return LASTSCORE_PREFS_PATH + GameManager.currentDifficulty.ToString();
    }
}

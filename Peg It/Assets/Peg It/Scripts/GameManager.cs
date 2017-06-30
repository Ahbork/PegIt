using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GameManager : MonoBehaviour {

    public static int availableMods = 2;       //How many colours/shapes are enabled
    public Button startBtn;
    public Canvas menuCanvas;
    public Button easy, med, hard;
    public static E_Difficulty currentDifficulty;
    public static bool isEditor = true;
    public static bool isConnectedToGoogleServices = false;

    private const string DIFFICULTY_PREFS_PATH = "PegIt_Difficulty";
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject gameManagerObject = new GameObject("GameManager");
                _instance = gameManagerObject.AddComponent<GameManager>();
                DontDestroyOnLoad(gameManagerObject);
            }
            return _instance;
        }
    }



    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        

#if UNITY_EDITOR
        isEditor = true;
#else
        isEditor = false;
        PlayGamesPlatform.Activate();
        isConnectedToGoogleServices = ConnectToPlayServices();
#endif
    }



    private bool ConnectToPlayServices()
    {
        if (!isConnectedToGoogleServices)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                isConnectedToGoogleServices = success;

                if(success == false)
                {
                    ScoreManager.Instance.leaderboardButton.interactable = false;
                }
            });
        }

        return isConnectedToGoogleServices;
    }



    private void Start()
    {
        _instance = this;
        Init();
    }



    private void Init()
    {
        //Initialise text on UI, such as last known highscore, remember which difficulty the user used last, etc.
        EventManager.Lost += GameLost;
        EventManager.Start += GameStart;
        EventManager.ChangeDifficulty += OnDifficultyChange;
        EventManager.Countdown += DisableDifficultyButtons;

        menuCanvas = GameObject.Find("CanvasMenu").GetComponent<Canvas>();

        easy.onClick.AddListener(() => EventManager.Instance.SetDifficulty(E_Difficulty.Easy));
        med.onClick.AddListener(() => EventManager.Instance.SetDifficulty(E_Difficulty.Medium));
        hard.onClick.AddListener(() => EventManager.Instance.SetDifficulty(E_Difficulty.Hard));

        InitializeDifficulty();
    }



    private void GameStart()
    {
        menuCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void GameLost()
    {
        EnableDifficultyButtons();
        menuCanvas.enabled = true;
    }



    public void DisableDifficultyButtons()
    {
        easy.interactable = false;
        med.interactable = false;
        hard.interactable = false;
    }


    private void EnableDifficultyButtons()
    {
        switch (currentDifficulty)
        {
            case E_Difficulty.Easy:
                easy.interactable = false;
                med.interactable = true;
                hard.interactable = true;
                break;
            case E_Difficulty.Medium:
                easy.interactable = true;
                med.interactable = false;
                hard.interactable = true;
                break;
            case E_Difficulty.Hard:
                easy.interactable = true;
                med.interactable = true;
                hard.interactable = false;
                break;
            default:
                easy.interactable = false;
                med.interactable = true;
                hard.interactable = true;
                break;
        }
    }



    private void InitializeDifficulty()
    {
        E_Difficulty lastDiff = E_Difficulty.Easy;

        if (PlayerPrefs.HasKey(DIFFICULTY_PREFS_PATH))
        {
            switch (PlayerPrefs.GetString(DIFFICULTY_PREFS_PATH))
            {
                case "Easy":
                    lastDiff = E_Difficulty.Easy;
                    break;
                case "Medium":
                    lastDiff = E_Difficulty.Medium;
                    break;
                case "Hard":
                    lastDiff = E_Difficulty.Hard;
                    break;
                default:
                    Debug.LogWarning("LastDifficulty has defaulted. Nothing has been coded for this event.");
                    lastDiff = E_Difficulty.Easy;
                    break;
            }

        }

        EventManager.Instance.SetDifficulty(lastDiff);

    }


    public void OnDifficultyChange(E_Difficulty difficulty)
    {
        currentDifficulty = difficulty;

        switch (difficulty)
        {
            case E_Difficulty.Easy:
                easy.interactable = false;
                med.interactable = true;
                hard.interactable = true;
                PlayerPrefs.SetString(DIFFICULTY_PREFS_PATH, "Easy");
                break;
            case E_Difficulty.Medium:
                easy.interactable = true;
                med.interactable = false;
                hard.interactable = true;
                PlayerPrefs.SetString(DIFFICULTY_PREFS_PATH, "Medium");

                break;
            case E_Difficulty.Hard:
                easy.interactable = true;
                med.interactable = true;
                hard.interactable = false;
                PlayerPrefs.SetString(DIFFICULTY_PREFS_PATH, "Hard");
                break;
            default:
                Debug.LogWarning("LastDifficulty has defaulted. Nothing has been coded for this event.");
                easy.interactable = false;
                med.interactable = true;
                hard.interactable = true;
                PlayerPrefs.SetString(DIFFICULTY_PREFS_PATH, "Easy");
                break;
        }

        PlayerPrefs.Save();

        ScoreManager.Instance.UpdateScores();
    }
}

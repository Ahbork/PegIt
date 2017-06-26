using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void DifficultyChange(E_Difficulty difficulty);
    public static event DifficultyChange ChangeDifficulty;

    public delegate void PlayerLook(int shapeID, int colourID);
    public static event PlayerLook UpdatePlayer;

    public delegate void MenuMove(int moveStage);
    public static event MenuMove MoveMenues;

    public delegate void GameState();
    public static event GameState Countdown;
    public static event GameState Start;
    public static event GameState Lost;
    public static event GameState Correct;
    //public static event GameState Poin;




    private static EventManager _instance = null;
    public static EventManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject eventManagerObject = new GameObject("EventManager");
                _instance = eventManagerObject.AddComponent<EventManager>();
                DontDestroyOnLoad(eventManagerObject);
            }
            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void SetDifficulty(E_Difficulty dif)
    {
        if (ChangeDifficulty != null)
        {
            ChangeDifficulty(dif);
        }
    }



    public void ChangePlayer()
    {

        if (UpdatePlayer != null)
        {
            int shapeID = (int)SliderRefs.Instance.sliders[0].value;
            int colourID = (int)SliderRefs.Instance.sliders[1].value;

            UpdatePlayer(shapeID, colourID);

        }
    }



    public void MoveMenuElements(int stage)
    {
        if(MoveMenues != null)
        {
            MoveMenues(stage);
        }
    }



    public void CorrectColor()
    {
        if(Correct != null)
        {
            Correct();
        }
    }



    public void StartCountdown()
    {
        if(Countdown != null)
        {
            Countdown();
        }
    }


    public void StartGame()
    {
        if(Start != null)
        {
            Start();
        }
    }


    public void GameLost()
    {
        if(Lost != null)
        {
            Lost();
        }
    }
}


public enum E_Difficulty
{
    Easy,
    Medium,
    Hard,
    Unending //Eternal, infinite
}
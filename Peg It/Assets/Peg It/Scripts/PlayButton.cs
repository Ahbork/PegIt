using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    public Sprite defaultSprite;
    public Sprite[] countdownSprites;

    private Coroutine _countdownRoutine;
    private const float START_COUNTDOWN = 3f;
    private Image img;
    private Button btn;


    private void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();

        btn.onClick.AddListener(() => EventManager.Instance.StartCountdown());
        
        EventManager.Countdown += StartCountdown;
        EventManager.Lost += GameLost;
    }



    private void GameLost()
    {
        btn.interactable = true;
        img.sprite = defaultSprite;
    }



    private void StartCountdown()
    {
        btn.interactable = false;
        if (!Options.skipCountdown)
        {
            _countdownRoutine = StartCoroutine(CountdownToStart());

        }
        else
        {
            StartInstantly();
        }
    }

    private void StartInstantly()
    {
        //EventManager.Instance.MoveMenuElements(1);
        //EventManager.Instance.MoveMenuElements(2);
        //EventManager.Instance.MoveMenuElements(3);
        EventManager.Instance.StartGame();
    }

    private IEnumerator CountdownToStart()
    {
        float startTime = Time.time;
        float endTime = startTime + START_COUNTDOWN;
        bool stage2Called = false;
        bool stage3Called = false;

        EventManager.Instance.MoveMenuElements(1);

        while (Time.time < endTime)
        {
            int timeLeft = Mathf.FloorToInt(endTime - Time.time);
            if(timeLeft < countdownSprites.Length)
                img.sprite = countdownSprites[timeLeft];

            if (Time.time - startTime >= START_COUNTDOWN * 0.33f && stage2Called == false)
            {
                EventManager.Instance.MoveMenuElements(2);
                stage2Called = true;
            }

            if (Time.time - startTime >= START_COUNTDOWN * 0.66f && stage3Called == false)
            {
                EventManager.Instance.MoveMenuElements(3);
                stage3Called = true;
            }

            yield return new WaitForFixedUpdate();
        }

        _countdownRoutine = null;
        EventManager.Instance.StartGame();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuElementMovement : MonoBehaviour {

    public Vector2 startPosition;
    public Vector2 endPosition;
    public int moveStage;           //When elements are meant to move, an event will be called with 1-2: 1 being immediately, 2 being after half the countdown
    private RectTransform rect;
    private float moveTime = 0.6f;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPosition = rect.anchoredPosition;

        EventManager.MoveMenues += BeginMoving;
        EventManager.Lost += ResetPosition;

    }


    public void BeginMoving(int stage)
    {
        if(stage == moveStage)
        {
            StartCoroutine(MoveElement());
        }
    }


    public void ResetPosition()
    {
        rect.anchoredPosition = startPosition;
    }


    private IEnumerator MoveElement()
    {
        Vector2 start = rect.anchoredPosition;
        Vector2 end = start == startPosition ? endPosition : startPosition;

        float startTime = Time.time;
        float currentMoveTime = 0;


        while(Time.time < startTime + moveTime)
        {
            //increment timer once per frame
            currentMoveTime += Time.deltaTime;
            if (currentMoveTime > moveTime)
            {
                currentMoveTime = moveTime;
            }

            //lerp!
            float perc = currentMoveTime / moveTime;
            //perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
            perc = perc * perc * 2;
            rect.anchoredPosition = Vector2.Lerp(start, end, perc);
            yield return new WaitForEndOfFrame();
        }

        rect.anchoredPosition = end;
    }
}

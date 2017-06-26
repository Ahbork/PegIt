using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Image img;
    private Image backgroundImg;
    private int _curShapeIndex = 0;
    private int _curColourIndex = 0;
    private const float TOUCH_DIST_THRESHOLD = 90f;
    private Color defColor = Color.white;
    private const float COLOR_FADE_TIME = 0.4f;

    public Image mask;
   

    private void Start()
    {
        img = GetComponent<Image>();
        backgroundImg = GetComponent<Image>();
        EventManager.UpdatePlayer += UpdatePlayer;
        //EventManager.Correct += CorrectColor;
        //EventManager.Lost += WrongColor;

    }



    private void UpdatePlayer(int shapeID, int colourID)
    {
        img.sprite = SliderRefs.Instance.holes[shapeID].colour[colourID];
        mask.sprite = SliderRefs.Instance.masks[shapeID].colour[colourID];
        _curShapeIndex = shapeID;
        _curColourIndex = colourID;
    }



    public void CorrectColor()
    {
        Color color = Color.green;

        StartCoroutine(ColorFade(color));
    }



    public void WrongColor()
    {
        Color color = Color.red;

        StartCoroutine(ColorFade(color));
    }



    private IEnumerator ColorFade(Color startColor)
    {
        backgroundImg.color = startColor;
        float startTime = Time.time;

        while(Time.time < startTime + COLOR_FADE_TIME)
        {
            float percent = (Time.time - startTime) / COLOR_FADE_TIME;
            backgroundImg.color = Color.Lerp(startColor, defColor, percent);
            yield return new WaitForFixedUpdate();
        }
        backgroundImg.color = defColor;
        yield return null;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Peg")
        {
            if(collision.transform.position.y < transform.position.y - TOUCH_DIST_THRESHOLD)
            {
                Peg peg = collision.GetComponent<Peg>();

                if (_curShapeIndex == peg.ShapeIndex && _curColourIndex == peg.ColourIndex)
                {
                    //Debug.Log("Correct!");
                    EventManager.Instance.CorrectColor();
                    peg.DestroyPeg();

                }
                else
                {
                    //Debug.Log("False!");
                    EventManager.Instance.GameLost();
                    peg.DestroyPeg();

                }
            }  
        }
    }
}

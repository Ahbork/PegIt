using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Image img;
    private Image backgroundImg;
    private int _curShapeIndex = 0;
    private int _curColourIndex = 0;
    private const float TOUCH_DIST_THRESHOLD = 200f;
    private Color defColor = Color.white;
    private const float COLOR_FADE_TIME = 0.4f;

    public Image mask;
    public GameObject pegCorrect, pegWrong;

    private void Start()
    {
        img = GetComponent<Image>();
        backgroundImg = GetComponent<Image>();
        EventManager.UpdatePlayer += UpdatePlayer;
        EventManager.Correct += CorrectColor;
        EventManager.Wrong += WrongColor;

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
        Image correct = GameObject.Instantiate(pegCorrect, mask.transform.GetChild(0)).GetComponent<Image>();

        StartCoroutine(CorrectPegMove(correct));
    }



    public void WrongColor()
    {
        Image wrong = GameObject.Instantiate(pegWrong, mask.transform.GetChild(1)).GetComponent<Image>();

        StartCoroutine(WrongPegFade(wrong));
    }

    private IEnumerator CorrectPegMove(Image img)
    {
        float duration = 0.75f;
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector2 startPos = img.rectTransform.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(10,75);
        Color startColor = img.color;
        Color endColor = new Color(img.color.r, img.color.g, img.color.b, 0);


        while(Time.time < endTime)
        {
            
            float percent = (Time.time - startTime) / duration;
            float fadePercent = percent * percent * percent;
            img.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, percent);
            img.color = Color.Lerp(startColor, endColor, fadePercent);
            yield return new WaitForEndOfFrame();
        }

        Destroy(img.gameObject);
    }

    private IEnumerator WrongPegFade(Image img)
    {

        float duration = 1f;
        float startTime = Time.time;
        float endTime = startTime + duration;
        Color startColor = img.color;
        Color endColor = new Color(img.color.r, img.color.g, img.color.b, 0);
        Vector2 startPos = img.rectTransform.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(-10, 30);

        while (Time.time < endTime)
        {
            float percent = (Time.time - startTime) / duration;
            float fadePercent = percent * percent * percent;
            img.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, percent);
            img.color = Color.Lerp(startColor, endColor, fadePercent);

            yield return new WaitForEndOfFrame();
        }

        Destroy(img.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Peg")
        {
            Peg peg = collision.GetComponent<Peg>();

            if (_curShapeIndex != peg.ShapeIndex || _curColourIndex != peg.ColourIndex)
            {
                //Debug.Log("False!");
                //EventManager.Instance.GameLost();
                EventManager.Instance.WrongPeg();
                //peg.DestroyPeg();
            }
            else
            {
                EventManager.Instance.DisableControl();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Peg")
        {

            Peg peg = collision.GetComponent<Peg>();

            if (_curShapeIndex == peg.ShapeIndex && _curColourIndex == peg.ColourIndex)
            {
                //Debug.Log("Correct!");
                EventManager.Instance.CorrectPeg();
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

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if(collision.tag == "Peg")
    //    {
    //        if(collision.transform.position.y < transform.position.y - TOUCH_DIST_THRESHOLD)
    //        {
    //            Peg peg = collision.GetComponent<Peg>();

    //            if (_curShapeIndex == peg.ShapeIndex && _curColourIndex == peg.ColourIndex)
    //            {
    //                //Debug.Log("Correct!");
    //                EventManager.Instance.CorrectColor();
    //                peg.DestroyPeg();

    //            }
    //            else
    //            {
    //                //Debug.Log("False!");
    //                EventManager.Instance.GameLost();
    //                peg.DestroyPeg();

    //            }
    //        }  
    //    }
    //}
}

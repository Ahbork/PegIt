using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Peg : MonoBehaviour {

    private float lifetime;
    private Image img;
    private int _shapeIndex = 0;
    private int _colourIndex = 0;
    private RectTransform _rect;
    private Coroutine _moveRoutine;
    private const float PEG_SPEED_MULT_START = 0.95f;
    private const float PEG_SPEED_MULT_END = 0.985f;


    private void Awake()
    {
        img = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        Random.seed = System.Environment.TickCount;

        EventManager.Wrong += WrongPeg;

        RandomizePeg();

        //if(PegSpawner.pegLifeTime > 0.5f)
        //{
        //    PegSpawner.pegLifeTime *= PegSpawner.pegLifeTime > 1.5f ? PEG_SPEED_MULT_START : PEG_SPEED_MULT_END;
        //}

        lifetime = PegSpawner.Instance.GetLifetime();

        //Debug.Log("Peg lifetime: " + lifetime);
    }

    private void Start()
    {
        _moveRoutine = StartCoroutine(Move());
    }


    private void RandomizePeg()
    {
        ShapeIndex = Random.Range(0, GameManager.availableMods);
        ColourIndex = Random.Range(0, GameManager.availableMods);

        img.sprite = SliderRefs.Instance.shapes[_shapeIndex].colour[_colourIndex];

        int offset = _shapeIndex == 2 ? 45 : 20;    //If star, reduce size of sprite, to avoid masking issues
       
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width / 2 - offset, img.sprite.rect.height / 2 - offset);

    }


    IEnumerator Move()
    {
        float startTime = Time.time;
        float endTime = startTime + lifetime;
        float timeToMove = lifetime;
        Vector3 startPos = _rect.position;
        Vector3 endPos = new Vector3(_rect.position.x, -100, _rect.position.z);


        while(Time.time < endTime)
        {
            float percentComplete = (Time.time - startTime) / timeToMove;

            transform.position = Vector3.Lerp(startPos, endPos, percentComplete);
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine(_moveRoutine);
    }


    public void WrongPeg()
    {
        StopCoroutine(_moveRoutine);
        StartCoroutine(DelayLoss());
    }


    private IEnumerator DelayLoss()
    {
        yield return new WaitForSeconds(1);
        
        EventManager.Instance.GameLost();
        DestroyPeg();
    }

    public void DestroyPeg()
    {
        EventManager.Wrong -= WrongPeg;
        Destroy(gameObject);
    }

  

    public int ShapeIndex
    {
        get
        {
            return _shapeIndex;
        }

        set
        {
            _shapeIndex = value;
        }
    }

    public int ColourIndex
    {
        get
        {
            return _colourIndex;
        }

        set
        {
            _colourIndex = value;
        }
    }
}

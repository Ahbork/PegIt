using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Peg : MonoBehaviour {

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

        RandomizePeg();

        if(PegSpawner.pegLifeTime > 0.5f)
        {
            PegSpawner.pegLifeTime *= PegSpawner.pegLifeTime > 1.5f ? PEG_SPEED_MULT_START : PEG_SPEED_MULT_END;
        }

        Debug.Log("Peg Speed: " + PegSpawner.pegLifeTime);
        EventManager.Lost += DestroyPeg;
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
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width / 2 - 5, img.sprite.rect.height / 2 - 5);

    }


    IEnumerator Move()
    {
        float startTime = Time.time;
        float endTime = startTime + PegSpawner.pegLifeTime;
        float timeToMove = PegSpawner.pegLifeTime;
        Vector3 startPos = _rect.position;
        Vector3 endPos = new Vector3(_rect.position.x, 0, _rect.position.z);


        while(Time.time < endTime)
        {
            float percentComplete = (Time.time - startTime) / timeToMove;

            transform.position = Vector3.Lerp(startPos, endPos, percentComplete);
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine(_moveRoutine);
    }


    public void DestroyPeg()
    {
        EventManager.Lost -= DestroyPeg;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderRefs : MonoBehaviour {

    public static SliderRefs Instance;
    public Slider[] sliders;
    public Image[] shapeImages;
    public Image[] colourImages;
    public List<Shape> shapes;
    public List<Shape> holes;
    public List<Shape> masks;

    private void Awake()
    {
        Instance = this;
    }
}

[System.Serializable]
public struct Shape
{
    public Sprite[] colour;
}
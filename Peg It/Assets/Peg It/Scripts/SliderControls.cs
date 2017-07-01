using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControls : MonoBehaviour {

    public float alphaDisabled = 0.15f;
    public float alphaEnabled = 1f;


    private void Start()
    {
        EventManager.ChangeDifficulty += ChangeMaxSliderValue;
        foreach (Slider slider in SliderRefs.Instance.sliders)
        {
            slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(slider); });
        }
    }


    public void OnSliderValueChanged(Slider slider)
    {
        if(slider.value > GameManager.availableMods - 1)
        {
            slider.value = GameManager.availableMods - 1;
            return;
        }

        EventManager.Instance.ChangePlayer();
    }




    private void ChangeMaxSliderValue(E_Difficulty dif)
    {
        int newMax = 2;
        switch (dif)
        {
            case E_Difficulty.Easy:
                newMax = 2;
                break;
            case E_Difficulty.Medium:
                newMax = 3;
                break;
            case E_Difficulty.Hard:
                newMax = 4;
                break;
            case E_Difficulty.Unending:
                newMax = 4;
                break;

        }

        GameManager.availableMods = newMax;
        UpdateSliders();
    }
	

    private void UpdateSliders()
    {
        for (int i = 0; i < SliderRefs.Instance.shapeImages.Length; i++)
        {
            Color newColorColours = SliderRefs.Instance.shapeImages[i].color;
            newColorColours.a = i < GameManager.availableMods ? alphaEnabled : alphaDisabled;
            SliderRefs.Instance.shapeImages[i].color = newColorColours;

            Color newColorShapes = SliderRefs.Instance.colourImages[i].color;
            newColorShapes.a = i < GameManager.availableMods ? alphaEnabled : alphaDisabled;
            SliderRefs.Instance.colourImages[i].color = newColorShapes;
        }
        foreach(Slider slider in SliderRefs.Instance.sliders)
        {
            OnSliderValueChanged(slider);
        }
    }
}

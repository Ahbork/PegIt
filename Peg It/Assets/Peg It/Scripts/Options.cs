﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    private Image skipCountdownImg, useGoogleImg;
    private GameObject optionsWindow;
    private const string PREFS_SKIP_COUNTDOWN = "PegIt_SkipCountdown";
    private const string PREFS_USE_GOOGLE = "PegIt_UseGoogleServices";
    private bool showOptions = false;
    private bool hasInitializedToggles = false;

    public static bool useGoogleServices = true;
    public static bool skipCountdown = false;
    public Sprite[] countdownSprites, googleSprites;
    public static Button optionsBtn, skipCountdownBtn, useGoogleBtn;


    // Use this for initialization
    void Start () {
        FindVariables();
        Init();
    }


    private void FindVariables()
    {
        if (optionsBtn == null)
            optionsBtn = GetComponent<Button>();
        if (optionsWindow == null)
            optionsWindow = transform.GetChild(0).gameObject;
        if (skipCountdownBtn == null)
            skipCountdownBtn = optionsWindow.transform.GetChild(0).GetComponent<Button>();
        if (useGoogleBtn == null)
            useGoogleBtn = optionsWindow.transform.GetChild(1).GetComponent<Button>();
        if (skipCountdownImg == null)
            skipCountdownImg = skipCountdownBtn.GetComponent<Image>();
        if (useGoogleImg == null)
            useGoogleImg = useGoogleBtn.GetComponent<Image>();

        EventManager.Countdown += ToggleOptionsButton;
        EventManager.Countdown += HideOptionsWindow;
        EventManager.Lost += ToggleOptionsButton;
    }

    private void Init()
    {
        optionsBtn.onClick.AddListener(() => ToggleOptionsWindow());
        skipCountdownBtn.onClick.AddListener(() => ToggleSkipCountdown());
        useGoogleBtn.onClick.AddListener(() => ToggleUseGoogle());

        //Initialize use Google Play Services
        if (PlayerPrefs.HasKey(PREFS_USE_GOOGLE))
        {
            bool google = PlayerPrefs.GetInt(PREFS_USE_GOOGLE) == 1 ? true : false;
            ToggleUseGoogle(google);
        }

        //Initialize skip countdown
        if (PlayerPrefs.HasKey(PREFS_SKIP_COUNTDOWN))
        {
            bool skipping = PlayerPrefs.GetInt(PREFS_SKIP_COUNTDOWN) == 1 ? true : false;
            ToggleSkipCountdown(skipping);
        }

       
        
    }

    public void SaveSettingsCountdown()
    {

        int skipping = skipCountdown ? 1 : 0;   //1 = true, 0 = false
        PlayerPrefs.SetInt(PREFS_SKIP_COUNTDOWN, skipping);
        //print("Prefs saved! SkipCountdown: " + skipping);

        PlayerPrefs.Save();
    }

    public void SaveSettingsGoogle()
    {
        int google = useGoogleServices ? 1 : 0;   //1 = true, 0 = false
        PlayerPrefs.SetInt(PREFS_USE_GOOGLE, google);
        //print("Prefs saved! UseGoogle: " + google);

        PlayerPrefs.Save();
    }


    private void ToggleOptionsButton()
    {
        optionsBtn.interactable = !optionsBtn.interactable;
    }


    public void ToggleOptionsWindow()
    {
        showOptions = !showOptions;
        optionsWindow.SetActive(showOptions);
    }


    private void HideOptionsWindow()
    {
        showOptions = false;
        optionsWindow.SetActive(showOptions);
    }


    public void ToggleSkipCountdown(bool skip)
    {
        skipCountdown = skip;
        skipCountdownImg.sprite = skip ? countdownSprites[0] : countdownSprites[1];

        SaveSettingsCountdown();

    }

    public void ToggleSkipCountdown()
    {
        skipCountdown = !skipCountdown;
        skipCountdownImg.sprite = skipCountdown ? countdownSprites[0] : countdownSprites[1];
        //print("Use Countdown: " + skipCountdown);

        SaveSettingsCountdown();

    }


    public void ToggleUseGoogle()
    {
        useGoogleServices = !useGoogleServices;
        useGoogleImg.sprite = useGoogleServices ? googleSprites[0] : googleSprites[1];
        //print("Use Google Services: " + useGoogleServices);

        SaveSettingsGoogle();

        //Log out of google play
        if(hasInitializedToggles)
            GameManager.Instance.ToggleConnectToGoogleServices(useGoogleServices);
        else
        {
            hasInitializedToggles = true;
        }
    }


    public void ToggleUseGoogle(bool use)
    {
        useGoogleServices = use;
        useGoogleImg.sprite = use ? googleSprites[0] : googleSprites[1];

        SaveSettingsGoogle();

        //Log out of google play
        if (hasInitializedToggles)
            GameManager.Instance.ToggleConnectToGoogleServices(useGoogleServices);
        else
        {
            hasInitializedToggles = true;
        }

    }
}

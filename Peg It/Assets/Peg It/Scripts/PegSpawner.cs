using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegSpawner : MonoBehaviour {

    public GameObject pegPrefab;
    public static float pegLifeTime = 4.0f;
    public PegStats pegEasy, pegMedium, pegHard;
    public PegStats pegCurrent;


    private static PegSpawner _instance = null;
    public static PegSpawner Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject pegSpawnerObject = new GameObject("PegSpawner");
                _instance = pegSpawnerObject.AddComponent<PegSpawner>();
                DontDestroyOnLoad(pegSpawnerObject);
            }
            return _instance;
        }
    }



    private void Awake()
    {
        _instance = this;
        EventManager.Start += SingleSpawn;
        EventManager.Correct += SingleSpawn;
        EventManager.Lost += ResetPegLifeTime;
        EventManager.ChangeDifficulty += OnDifficultyChange;
    }


    public void OnDifficultyChange(E_Difficulty dif)
    {
        switch (dif)
        {
            case E_Difficulty.Easy:
                pegCurrent = pegEasy;
                break;
            case E_Difficulty.Medium:
                pegCurrent = pegMedium;
                break;
            case E_Difficulty.Hard:
                pegCurrent = pegHard;
                break;
        }

        ResetPegLifeTime();
    }


    public float GetLifetime()
    {
        float curLifetime = pegLifeTime;

        if(pegLifeTime > pegCurrent.lifetimeEnd) //Update lifetime for next peg that spawns
        {
            pegLifeTime -= (pegCurrent.lifetimeStart - pegCurrent.lifetimeEnd) / pegCurrent.spawnsBeforeEndLifetime;
            if (pegLifeTime < pegCurrent.lifetimeEnd)
                pegLifeTime = pegCurrent.lifetimeEnd;
        }

        return curLifetime;
    }


    public void ResetPegLifeTime()
    {
        pegLifeTime = pegCurrent.lifetimeStart;
    }


    public void SingleSpawn()
    {
        Instantiate(pegPrefab, transform, false);
    }
    
}

[System.Serializable]
public struct PegStats
{
    public float lifetimeStart;
    public float lifetimeEnd;
    public int spawnsBeforeEndLifetime;
}
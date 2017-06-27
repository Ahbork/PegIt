using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegSpawner : MonoBehaviour {

    public GameObject pegPrefab;
    public static float spawnInterval = 3.0f;
    public static float pegLifeTime = 4.0f;


    private const float SPAWN_INTERVAL_START = 3.0f;
    private const float SPAWN_INTERVAL_END = 1.0f;
    private const float PEG_LIFETIME_DEFAULT = 4.0f;
    private const int SPAWNS_BEFORE_END_INTERVAL = 7;

    //Spawn vars for EASY
    private const float EASY_PEG_LIFETIME_START = 4.0f;
    private const float EASY_PEG_LIFETIME_END = 1.5f;
    private const int EASY_SPAWNS_BEFORE_END_LIFETIME = 15;

    //Spawn vars for EASY
    private const float MEDIUM_PEG_LIFETIME_START = 4.0f;
    private const float MEDIUM_PEG_LIFETIME_END = 1.5f;
    private const int MEDIUM_SPAWNS_BEFORE_END_LIFETIME = 15;

    //Spawn vars for EASY
    private const float HARD_PEG_LIFETIME_START = 4.0f;
    private const float HARD_PEG_LIFETIME_END = 1.5f;
    private const int HARD_SPAWNS_BEFORE_END_LIFETIME = 15;


    private Coroutine _spawnRoutine;
    private float _intervalReduction;
    private bool _spawning = false;
    private bool _spawnOnCorrect = true;

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
        _intervalReduction = (SPAWN_INTERVAL_START - SPAWN_INTERVAL_END) / SPAWNS_BEFORE_END_INTERVAL;
        
        _instance = this;
        if (_spawnOnCorrect)
        {
            EventManager.Start += SingleSpawn;
            EventManager.Correct += SingleSpawn;
            EventManager.Lost += ResetPegLifeTime;
        }
        else
        {
            EventManager.Start += StartSpawning;
            EventManager.Lost += StopSpawning;

        }
    }


    public void ResetPegLifeTime()
    {
        pegLifeTime = PEG_LIFETIME_DEFAULT;
    }



    public void StartSpawning()
    {
        if(_spawnRoutine == null)
        {
            spawnInterval = SPAWN_INTERVAL_START;
            _spawning = true;
            _spawnRoutine = StartCoroutine(Spawn());
            //GameObject.Find("CanvasMenu").GetComponent<Canvas>().enabled = false;
        }
    }



    public void StopSpawning()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }


    //Used if new wall peg when other peg is correct
    public void SingleSpawn()
    {
        Instantiate(pegPrefab, transform, false);
    }


    private IEnumerator Spawn()
    {
        while (_spawning)
        {
            SingleSpawn();
            yield return new WaitForSeconds(spawnInterval);
            if (spawnInterval > SPAWN_INTERVAL_END)
            {
                spawnInterval -= _intervalReduction;
                
            }
            else if (spawnInterval != SPAWN_INTERVAL_END)
                spawnInterval = SPAWN_INTERVAL_END;

            Debug.Log(spawnInterval);
        }
        _spawning = false;

        StopCoroutine(_spawnRoutine);
        _spawnRoutine = null;

    }
}

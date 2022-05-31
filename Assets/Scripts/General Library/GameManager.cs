using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region SingleTone
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager)
                {
                    _instance = gameManager;
                }
                else
                { 
                    var gm = new GameObject("GameManager");
                    _instance = gm.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != FindObjectOfType<GameManager>())
            { 
                Destroy(gameObject);
                return;
            }
        }
        _instance = this;
    }
    #endregion SingleTone



    #region Propertys
    private bool isGameOver = false;
    private List<GameObject> enemys = new List<GameObject>();
    #endregion Propertys

    #region Callback Events
    [Header("Level Progression Responses")]
    public UnityEvent OnLevelStart;
    public UnityEvent OnLevelCompleted;
    public UnityEvent OnLevelFailed;

    [Header("Pauses / Resume Responses")]
    public UnityEvent OnPaused;
    public UnityEvent OnResume;
    public UnityEvent OnExit;

    [Header("Audio Responses")]
    public UnityEvent onMute;
    public UnityEvent onUnmute;

    [Header("Enemy Counter Responses")]
    public UnityEvent OnEnemyAdded;
    public UnityEvent OnEnemyRemoved;
    public UnityEvent OnEnemyFinished;

    #endregion Callback Events


    #region Public Member Functions
    public void Pause()
    {
        OnPaused.Invoke();
        Time.timeScale = 0;
    }
    public void Resume()
    {
        OnResume.Invoke();
        Time.timeScale = 1;
    }
    public void StartLevel()
    {
        isGameOver = false;
        OnLevelStart.Invoke();
    }
    public void LevelCompleted()
    {
        OnLevelCompleted.Invoke();
    }
    public void LevelFailed()
    {
        isGameOver = true;
        OnLevelFailed.Invoke();
    }

    public void Exit()
    {
        OnExit.Invoke();
        Application.Quit();
    }

    public void SetAudioStatus(bool mute)
    {
        if (mute)
            onMute.Invoke();
        else
            onUnmute.Invoke();
    }


    public void AddEnemy(GameObject enemy)
    {
        enemys.Add(enemy);
        OnEnemyAdded.Invoke();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemys.Remove(enemy);
        OnEnemyRemoved.Invoke();

        if (enemys.Count <= 0)
        { 
            OnEnemyFinished.Invoke();
            LevelCompleted();
        }
    }
    #endregion Public Member Functions
}

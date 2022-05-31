using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public delegate void ChangeScore(int newScore);
    public ChangeScore OnChangeScore;

    [Tooltip("Coin Saving Scriptable Object")]
    public PlayerData playerData;

    [Header("Text Message Setup")]
    [SerializeField] private TextMeshProUGUI coin_text;
    [SerializeField] private string preMessage = string.Empty;
    [SerializeField] private string postMessage = string.Empty;

#if UNITY_EDITOR
    [Header("Debugger")]
    [SerializeField] private bool useDebug = false;
#endif

    [Header("Callbacks")]
    [SerializeField] private UnityEvent OnScoreUpdate;


    private void OnEnable()
    {
        if (hasPlayerData)
            UpdateScoreUI(playerData.score);

        if (hasPlayerData)
            playerData.OnScoreChanged += UpdateScoreUI;

    }
    private void OnDisable()
    {
        if (hasPlayerData)
            playerData.OnScoreChanged -= UpdateScoreUI;
    }

    /// <summary>
    /// Update score to Scriptable and UI (+) amount will increase score and (-) amount will decrease socre
    /// </summary>
    /// <param name="amount"></param>

    public void UpdateScore(int amount)
    {
#if UNITY_EDITOR
        if (useDebug)
            Debug.Log("<color=cyan>"+ "Has Player Data =  " + hasPlayerData  + " Also updateScore delta amount = " + amount + " of "+ name + " Game Object </color>");
#endif

        if (!hasPlayerData)
            return;

        playerData.AddScore(amount);
        if(OnChangeScore != null)
            OnChangeScore.Invoke(playerData.score);
        OnScoreUpdate.Invoke();
    }

    public void AddScoreAsync(int amount)
    {
        if (!hasPlayerData)
            return;
        StartCoroutine(addscoreScync(amount));
        IEnumerator addscoreScync(int amount)
        {
            float delta = 1 / (float)30;
            float fraction = amount * delta; 
            float targetFreamCount = 30;

            int totalSum = playerData.score + amount;

            while (targetFreamCount > 0)
            {
                targetFreamCount--;
                playerData.AddScore(Mathf.CeilToInt(fraction));
                if (OnChangeScore != null)
                    OnChangeScore.Invoke(playerData.score);
                OnScoreUpdate.Invoke();
                yield return null;
            }

            playerData.SetScore(totalSum);
            if (OnChangeScore != null)
                OnChangeScore.Invoke(playerData.score);
            OnScoreUpdate.Invoke();
        }
    }

    private void UpdateScoreUI(int newScore)
    {
        if (coin_text == null)
        {
#if UNITY_EDITOR
            if (useDebug)
                Debug.Log("<color=cyan>Coin Text is Null of " + name + "Game Object </color>");
#endif
            return;
        }
        if (!hasPlayerData)
            return;
        coin_text.text = preMessage + newScore + postMessage;
    }

    private bool hasPlayerData
    {
        get
        {
            if (playerData == null)
            {
#if UNITY_EDITOR
                if(useDebug)
                    Debug.Log("<color=cyan>Player Data is Null of " + name + "Game Object </color>");
#endif
                return false;
            }
            return true;
        }
    
    }

}

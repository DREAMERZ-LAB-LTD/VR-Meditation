using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PauseResumer : MonoBehaviour
{
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public void Pause()
    {
        Time.timeScale = 0;
        transform.DOScale(Vector3.one, .5f).SetUpdate(true);
        OnPause.Invoke();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        transform.DOScale(Vector3.zero, .5f).SetUpdate(true);
        OnResume.Invoke();
    }
}

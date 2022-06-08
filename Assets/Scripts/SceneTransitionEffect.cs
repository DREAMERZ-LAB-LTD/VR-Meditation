using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransitionEffect : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] protected string parameter = string.Empty;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private GameObject fadeOut;

    private static bool initialized = false;

    private void Awake()
    {
        if (initialized)
            FadeOut();
        else
            initialized = true;
    }

    public void LoadSceneWithFadeIn(string sceneName)
    {
        StopAllCoroutines();
        fadeIn.SetActive(true);
        fadeOut.SetActive(false);
        StartCoroutine(FadingRoutine(fadeIn, 0, 1, ()=> SceneManager.LoadScene(sceneName)));
    }

    public void FadeIn()
    {
        StopAllCoroutines();

        fadeIn.SetActive(true);
        fadeOut.SetActive(false);
        StartCoroutine(FadingRoutine(fadeIn, 0, 1));
    }

    public void FadeOut()
    {
        StopAllCoroutines();

        fadeIn.SetActive(false);
        fadeOut.SetActive(true);
        StartCoroutine(FadingRoutine(fadeOut, 1, 0));
    }

    private IEnumerator FadingRoutine(GameObject fadeObj, float start, float end, UnityAction action = null)
    {
        var rend = fadeObj.GetComponent<Renderer>();
        if (rend == null)
        {
            yield break;
        }

        var fadeMat = rend.material;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            t = Mathf.Clamp01(t);
            float fade = Mathf.Lerp(start, end, t);

            fadeMat.SetFloat(parameter, fade);
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        if (action != null)
            action.Invoke();
    }
}

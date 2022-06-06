using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransitionEffect : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] protected string parameter = string.Empty;
    private Material[] materials = null;

    private static bool initialized = true;
    private void Awake()
    {
        var rend = GetComponent<Renderer>();
        if (rend)
            materials = rend.materials;


        if (initialized)
            FadeOut();
        else
            initialized = true;
    }

    public void LoadSceneWithFadeIn(string sceneName)
    {
        StopAllCoroutines();
        StartCoroutine(FadingRoutine(0, 1, ()=> SceneManager.LoadScene(sceneName)));
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadingRoutine(0, 1));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadingRoutine(1, 0));
    }

    private IEnumerator FadingRoutine(float start, float end, UnityAction action = null)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            t = Mathf.Clamp01(t);
            float fade = Mathf.Lerp(start, end, t);

            foreach(var mat in materials)
                if(mat)
                    mat.SetFloat(parameter, fade);
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        if (action != null)
            action.Invoke();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine currentActiveFade = null;

    // Used to Fade in and out of the different scenes

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // fades out immediately by setting rootCanvas alpha to 1
    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1;
    }

    // fades out after a delay
    public Coroutine FadeOut(float time)
    {
        return Fade(1, time);
    }

    // fades in after a delay
    public Coroutine FadeIn(float time)
    {
        return Fade(0, time);
    }

    // Stops the currently active coroutine and calls FadesRoutine.
    // Sets FadeRoutine as the new active fade
    public Coroutine Fade(float target, float time)
    {
        if (currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }
        currentActiveFade = StartCoroutine(FadeRoutine(target, time));
        return currentActiveFade;
    }

    // Used to fade in or out with respect to time. by moving canvas alpha to target (0, 1)
    // according to time
    private IEnumerator FadeRoutine(float target, float time)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
            yield return null;
        }
    }
}

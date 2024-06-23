using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FadeIn : MonoBehaviour
{
    private bool isFading;
    private bool isClicked;
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private InputHandler inputHandler;

    private void Awake()
    {
        inputHandler.onPointerLeftClick += InputHandler_onPointerLeftClick;
    }

    private void InputHandler_onPointerLeftClick()
    {
        isClicked = true;
    }

    public IEnumerator FadeInText(string text, float duration)
    {
        isClicked = false;

        float startTime = Time.time;

        // Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
        isFading = true;

        textMesh.text = text;

        faderCanvasGroup.alpha = 0;

        // Make sure the CanvasGroup blocks raycasts into the scene so no more input can be accepted.
        faderCanvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - 1) / fadeDuration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(faderCanvasGroup.alpha, 1))
        {
            if (isClicked)
            {
                yield return (StartCoroutine(FadeOut()));
                yield break;
            }
            else
            {
                // ... move the alpha towards it's target alpha.
                faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, 1,
                    fadeSpeed * Time.deltaTime);

                // Wait for a frame then continue.
                yield return null;
            }
        }

        while (Time.time <= duration + startTime)
        {
            if (isClicked)
            {
                yield return (StartCoroutine(FadeOut()));
                yield break;
            }
            else
            {
                yield return null;
            }
        }

        yield return (StartCoroutine(FadeOut()));
    }

    private IEnumerator FadeOut()
    {
        // Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - 0) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, 0))
        {
            // ... move the alpha towards it's target alpha.
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, 0,
                fadeSpeed * Time.deltaTime);

            // Wait for a frame then continue.
            yield return null;
        }

        faderCanvasGroup.alpha = 0;

        // Set the flag to false since the fade has finished.
        isFading = false;

        // Stop the CanvasGroup from blocking raycasts so input is no longer ignored.
        faderCanvasGroup.blocksRaycasts = false;

        yield break;
    }
}

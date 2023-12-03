/*
 * File: HeartUIController.cs
 * Author: Amal Chaudry
 * Created: 11/05/2023
 * Modified: 11/30/2023
 * Description: This script fades in the death screen.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeToBlack : MonoBehaviour
{
    public TextMeshProUGUI deathScreenText;
    public Button[] buttons;
    public float fadeInDuration = 1.5f; // Duration for fading in

    private void Start()
    {
        // Ensure the death screen text and buttons are initially hidden
        SetAlpha(deathScreenText, 0f);

        foreach (Button button in buttons)
        {
            foreach (Graphic graphic in button.GetComponentsInChildren<Graphic>())
            {
                SetAlpha(graphic, 0f);
            }
        }
    }

    // Call this method to start the fade-in animation for the text and buttons
    public void FadeInTextAndButtons()
    {
        StartCoroutine(FadeInSequence());
    }

    // Coroutine for fading in the text and buttons sequentially
    IEnumerator FadeInSequence()
    {
        // Fade in the death screen text first
        yield return StartCoroutine(FadeInElement(deathScreenText));

        // Then fade in the buttons
        foreach (Button button in buttons)
        {
            foreach (Graphic graphic in button.GetComponentsInChildren<Graphic>())
            {
                Debug.Log("fade in buttons");
                yield return StartCoroutine(FadeInElement(graphic));
            }
        }
    }

    IEnumerator FadeInElement(Graphic element)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime = Time.time - startTime;
            float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            SetAlpha(element, alphaValue);
            yield return null;
        }

        // Ensure the alpha is precisely at the target value when the animation ends
        SetAlpha(element, 1f);
    }

    // Helper method to set the alpha of the UI element
    void SetAlpha(Graphic element, float alpha)
    {
        Color newColor = element.color;
        newColor.a = alpha;
        element.color = newColor;
    }
}

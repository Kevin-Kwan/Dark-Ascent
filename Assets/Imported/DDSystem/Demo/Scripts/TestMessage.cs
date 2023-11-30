/*
 * File: TestMessage.cs
 * Authors: Mehar Johal
 * Created: 10/28/2023
 * Modified: 10/29/2023
 * Description: This script is used to display the story dialogue
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;

    public GameObject[] Example;

    public Image fadeImage;
    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("You have shunned me, the warden of hell, for the last time", "Enemy"));

        dialogTexts.Add(new DialogData("You have already passed away, and must spend the rest of your life as a ghost", "Enemy"));
        
        dialogTexts.Add(new DialogData("To stay here in hell is your only choice, so get comfortable", "Enemy"));

        dialogTexts.Add(new DialogData("I won't stand here idle and let you do whatever you want.", "MC"));

        dialogTexts.Add(new DialogData("I will escape hell, just you watch", "MC"));

        dialogTexts.Add(new DialogData("You? Escape? With all of the enemies and obstacles that no human has every passed, I doubt you'll make it very far", "Enemy"));

        dialogTexts.Add(new DialogData("Nothing can stand in my way of freedom", "MC"));

        DialogData final = new DialogData("We'll see about that... /click//sound:evillaugh/", "Enemy", null, false);

        final.Callback = () => StartCoroutine(FadeAndLoadScene("Level1"));
        dialogTexts.Add(final);

        DialogManager.Show(dialogTexts);        
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        // Start the fade and scene transition
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        Debug.Log("Fading");

        // Fade out the image over a duration of 2 seconds
        float duration = 2f;
        Color originalColor = fadeImage.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - progress);
            yield return null;
        }

        // Ensure the image is completely transparent at the end
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
}




/*
 * File: BossIntroCutscene.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the cutscene transition between the boss fight introduction and the boss fight
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossIntroCutscene : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string sceneToLoad = "Boss Fight"; // The name of the scene you want to load
    public float delayBeforeFade = 7f; // The delay before the fade starts
    public GameObject player;
    public AudioClip cutsceneAudio;
    public GameObject warden;

    private void Start()
    {
        //ThirdPController controller = player.GetComponent<ThirdPController>();
        //controller.enabled = false;
        StartCoroutine(DelayedSceneTransition());
    }

    private IEnumerator DelayedSceneTransition()
    {
        //player.GetComponentInChildren<Animator>().SetBool("isDead", true);
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeFade);

        // Start the fade and scene transition
        FadeAndLoadScene(sceneToLoad);
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        Debug.Log("Fading");
        // Fade to black
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
}

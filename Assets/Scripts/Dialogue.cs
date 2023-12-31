/*
 * File: Dialogue.cs
 * Authors: Mehar Johal
 * Created: 10/27/2023
 * Modified: 10/27/2023
 * Description: This script handles the dialogue of the game
 * 
 *  * Contributions:
 *   Mehar Johal:
 *    - Did script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // Added tutorial and dialogue UI canvas https://www.youtube.com/watch?v=8oTYabhj248
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public static int index = 0;
    public static bool playAgain = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (index == lines.Length && playAgain)
        {
            StartDialogue();
        }
        //if (index == 1)
        //{
        //    ThirdPersonCamera.followPlayer = true;
        //}
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))//PauseMenuScript.pause
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // type each character 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index++;
            textComponent.text = string.Empty;
            gameObject.SetActive(false);
        }
        Debug.Log(index);
    }
}

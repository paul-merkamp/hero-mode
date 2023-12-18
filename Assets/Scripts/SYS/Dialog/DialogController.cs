using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public Animator animator;
    public AudioSource sfx;
    public AudioClip letterBlipFX;
    public AudioClip vineBoomFX;

    public TMP_Text text;
    public float animationDelay = 0.3f;
    public float letterDelay = 0.05f;
    public float lineDelay = 1f;

    public bool isDialogActive = false;
    
    // Triggers the dialog with the given list of dialog lines
    public void TriggerDialog(List<string> dialogLines)
    {
        if (!isDialogActive)
        {
            isDialogActive = true;
            animator.SetBool("Shown", true);
            StartCoroutine(DialogCoroutine(dialogLines));
        }
    }

    // Triggers a test dialog from the editor
    [ContextMenu("Trigger Test Dialog From Editor")]
    public void TestDialog()
    {
        List<string> dummyLines = new()
        {
            "This is a dummy line 1",
            "This is a dummy line 2",
            "This is a dummy line 3"
        };

        TriggerDialog(dummyLines);
    }

    // Triggers a gag dialog
    [ContextMenu("Trigger Gag Dialog")]
    public void GagDialog()
    {
        List<string> gagLines = new()
        {
            "You're a <color=\"yellow\"><b>baby</b></color>."
        };

        TriggerDialog(gagLines);
    }

    // Handles different event types
    void HandleEvent(char eventType)
    {
        switch (eventType)
        {
            case 'a':
                sfx.PlayOneShot(vineBoomFX);
                break;
            default:
                // Handle unknown event type
                break;
        }
    }

    // Coroutine for displaying the dialog lines
    IEnumerator DialogCoroutine(List<string> dialogLines)
    {
        yield return new WaitForSeconds(animationDelay);

        foreach (string line in dialogLines)
        {
            text.text = "";
            bool isTag = false;
            string tagText = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '|')
                {
                    // Handle event
                    if (i + 1 < line.Length)
                    {
                        HandleEvent(line[i + 1]);
                        i++;
                    }
                }
                else if (line[i] == '<')
                {
                    // Start of a tag
                    isTag = true;
                    tagText += line[i];
                }
                else if (line[i] == '>')
                {
                    // End of a tag
                    isTag = false;
                    tagText += line[i];
                    text.text += tagText;
                    tagText = "";
                }
                else
                {
                    if (isTag)
                    {
                        // Inside a tag
                        tagText += line[i];
                    }
                    else
                    {
                        // Displaying a letter
                        text.text += line[i];
                        sfx.PlayOneShot(letterBlipFX);
                        yield return new WaitForSeconds(letterDelay);
                    }
                }
            }
            yield return new WaitForSeconds(lineDelay);
        }
        text.text = "";

        animator.SetBool("Shown", false);
        isDialogActive = false;
    }
}
using System.Collections;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public AudioClip selectAudio;

    private AudioSource sfx;
    private Animator animator;
    private DialogController dialogController;

    private bool wasSelected = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sfx = FindObjectOfType<AudioSource>();
        dialogController = FindObjectOfType<DialogController>();
    }

    private void OnMouseEnter()
    {
        if (!wasSelected)
            animator.SetBool("Raised", true);
    }

    private void OnMouseExit()
    {
        if (!wasSelected)
            animator.SetBool("Raised", false);
    }

    private void OnMouseDown()
    {
        if (!wasSelected && MainMenuController.selectedMode == MainMenuController.GameMode.Normal)
        {
            dialogController.TriggerDialog(new()
            {
                "You're a |a<color=\"yellow\"><b>baby</b></color>."
            });

            animator.SetBool("Raised", false);
            wasSelected = true;

            StartCoroutine(gagDelay());
        }

        else if (!wasSelected)
        {
            if (selectAudio != null)
                sfx.PlayOneShot(selectAudio);

            animator.SetBool("Raised", false);
            wasSelected = true;
        }
    }

    IEnumerator gagDelay()
    {
        yield return new WaitForSeconds(3f);
        
        animator.SetBool("Raised", true);
        wasSelected = false;
    }
}

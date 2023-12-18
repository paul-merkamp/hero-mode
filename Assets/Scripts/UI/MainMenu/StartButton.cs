using System.Collections;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public AudioClip selectAudio;

    private AudioSource sfx;
    private Animator animator;
    private DialogController dialogController;

    public ModePanel babyPanel;
    public ModePanel knightPanel;

    private bool wasSelected = false;
    private bool busy = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sfx = FindObjectOfType<AudioSource>();
        dialogController = FindObjectOfType<DialogController>();
    }

    public void TryStartGame()
    {
        if (!busy)
        {
            if (MainMenuController.selectedMode == MainMenuController.GameMode.Normal)
            {
                PlayGag();
            }
            else if (MainMenuController.selectedMode == MainMenuController.GameMode.Baby)
            {
                StartGame();
            }
        }
    }

    public Animator startGameAnimator;

    public void StartGame()
    {
        startGameAnimator.SetTrigger("Fade");

        StartCoroutine(StartGameDelay());
    }

    public IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(3f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public int gagCounter = 0;

    public void PlayGag()
    {
        if (gagCounter == 0)
        {
            dialogController.TriggerDialog(new()
            {
                "I think you misclicked. Here, let me fix that."
            });

            StartCoroutine(Gag());

            gagCounter++;
        }

        else if (gagCounter == 1)
        {
            dialogController.TriggerDialog(new()
            {
                "No, no. You're a |a<color=\"yellow\"><b>baby</b></color>."
            });

            StartCoroutine(Gag());

            gagCounter++;
        }

        else if (gagCounter == 2)
        {
            knightPanel.Disable();
        }
    }

    IEnumerator Gag()
    {
        busy = true;
        yield return new WaitForSeconds(4f);
        busy = false;

        babyPanel.Select();
    }
}

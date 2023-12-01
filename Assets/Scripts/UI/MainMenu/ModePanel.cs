using UnityEngine;

public class ModePanel : MonoBehaviour
{
    public ModePanel pairedPanel;
    public MainMenuController.GameMode mode = MainMenuController.GameMode.None;
    public AudioClip selectAudio;

    private AudioSource sfx;
    private MainMenuController mainMenu;
    private Animator animator;
    private bool isSelected = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainMenu = FindObjectOfType<MainMenuController>();
        sfx = FindObjectOfType<AudioSource>();
    }

    private void OnMouseEnter()
    {
        if (!isSelected)
        {
            animator.SetBool("Raised", true);
        }
    }

    private void OnMouseExit()
    {
        if (!isSelected)
        {
            animator.SetBool("Raised", false);
        }
    }

    private void OnMouseDown()
    {
        if (selectAudio != null && isSelected == false)
            sfx.PlayOneShot(selectAudio);

        isSelected = true;
        if (mode != MainMenuController.GameMode.None)
            MainMenuController.selectedMode = mode;

        if (pairedPanel != null) pairedPanel.Deselect();
    }

    public void Deselect()
    {
        isSelected = false;
        animator.SetBool("Raised", false);
    }
}

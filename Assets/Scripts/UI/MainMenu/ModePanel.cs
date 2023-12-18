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

    private bool disabled = false;
    public GameObject disabledIndicator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainMenu = FindObjectOfType<MainMenuController>();
        sfx = FindObjectOfType<AudioSource>();
    }

    public void Select()
    {
        if (!disabled)
        {
            if (selectAudio != null && isSelected == false)
                sfx.PlayOneShot(selectAudio);

            isSelected = true;
            animator.SetBool("Raised", true);

            if (mode != MainMenuController.GameMode.None)
                MainMenuController.selectedMode = mode;
            
            if (pairedPanel != null) pairedPanel.Deselect();
        }
    }

    public void Disable()
    {
        disabled = true;
        disabledIndicator.SetActive(true);
        animator.SetBool("Raised", false);
    }
    
    private void OnMouseEnter()
    {
        if (!isSelected && !disabled)
        {
            animator.SetBool("Raised", true);
        }
        else if (disabled)
        {
            animator.SetBool("Raised", false);
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
        Select();
    }

    public void Deselect()
    {
        isSelected = false;
        animator.SetBool("Raised", false);
    }
}

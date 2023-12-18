using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        Normal,
        Locked,
        Boss
    }

    public DoorType doorType = DoorType.Normal;
    public bool locked = false;

    private GameObject col;

    private SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;

    private AudioSource sfx;
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip unlockSFX;

    private bool open;

    public void Start()
    {
        col = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Environment").GetComponent<AudioSource>();

        if (doorType == DoorType.Locked)
        {
            locked = true;
        }
        else if (doorType == DoorType.Boss)
        {
            locked = !PlayerData.bossDoorUnlocked;
        }
    }

    public void ToggleOpen()
    {
        if (open)
        {
            Close();
        }
        else
        {
            Open();
        }
        open = !open;
    }

    public void Open()
    {
        col.SetActive(false);
        spriteRenderer.sprite = openSprite;
        sfx.PlayOneShot(openSFX);
    }

    public void Close()
    {
        col.SetActive(true);
        spriteRenderer.sprite = closedSprite;
        sfx.PlayOneShot(closeSFX);
    }
}

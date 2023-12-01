using TMPro;
using UnityEngine;

public class SimpleDestructible : MonoBehaviour
{
    private AudioSource sfx;
    private Animator animator;

    public Sprite destroyedSprite;
    public AudioClip hitSound;
    public GameObject destroyedComponentsGO;
    
    public void Start()
    {
        sfx = GameObject.Find("SFX/SFX_Environment").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void GetDestroyed()
    {
        GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        GetComponent<Collider2D>().enabled = false;

        sfx.PlayOneShot(hitSound);

        if (destroyedComponentsGO != null)
            Destroy(destroyedComponentsGO);

        if (animator != null)
            Destroy(animator);

        Destroy(this);
    }
}

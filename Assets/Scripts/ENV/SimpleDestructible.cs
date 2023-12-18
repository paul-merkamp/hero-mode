using TMPro;
using UnityEngine;

public class SimpleDestructible : MonoBehaviour
{
    private AudioSource sfx;
    private Animator animator;

    public Sprite destroyedSprite;
    public AudioClip hitSound;
    public GameObject destroyedComponentsGO;

    public bool destroyed = false;

    public GameObject heartPrefab;
    public float heartDropChance = 0.5f;

    private Door door;

    public enum DestructibleType
    {
        Any,
        BigOnly,
        None
    }

    public DestructibleType type = DestructibleType.Any;
    
    public void Start()
    {
        sfx = GameObject.Find("SFX/SFX_Environment").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        door = GetComponent<Door>();
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

        if (door != null)
        {
            Destroy(door);
        }

        if (heartPrefab != null && heartDropChance > 0 && Random.Range(0f, 1f) <= heartDropChance)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

        destroyed = true;
    }
}

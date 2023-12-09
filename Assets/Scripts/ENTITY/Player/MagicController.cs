using System.Collections;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public GameObject magicCursor;
    public Camera cam;
    public float moveSpeed = 5f;

    private bool isItemLocked = false;
    private SimpleDestructible lockedItem;
    private ParticleSystem particles;

    private AudioSource sfx;
    public float maxVolume = 1f;
    public float audioTransitionTime = 2f;

    private bool isMagicEnabled = false;

    public void Start()
    {
        particles = magicCursor.transform.GetChild(0).GetComponent<ParticleSystem>();
        sfx = GameObject.Find("SFX/SFX_Magic").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isMagicEnabled) return;

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 worldPosition = cam.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Set the z-coordinate to 0 to ensure the cursor is on the same plane as the game objects

        // Move the magicCursor gameObject to the mouse position
        magicCursor.transform.position = worldPosition;

        // Check if the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            if (isItemLocked)
            {
                // Release the locked item
                lockedItem = null;
                isItemLocked = false;

                particles.Stop();
                StartCoroutine(ChangeVolume(0.0f, audioTransitionTime));
            }
            else
            {
                // Cast a ray from the mouse position to detect SimpleDestructible objects
                RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    // Check if the hit object is a SimpleDestructible
                    SimpleDestructible destructible = hit.collider.GetComponent<SimpleDestructible>();
                    if (destructible != null && destructible.type == SimpleDestructible.DestructibleType.Any)
                    {
                        // Lock the item to the cursor
                        lockedItem = destructible;
                        isItemLocked = true;

                        particles.Play();
                        StartCoroutine(ChangeVolume(maxVolume, audioTransitionTime));
                    }
                }
            }
        }

        // Check if an item is locked
        if (isItemLocked && lockedItem != null)
        {
            // Hold the position of the locked item to the cursor
            Rigidbody2D lockedItemRigidbody = lockedItem.GetComponent<Rigidbody2D>();
            if (lockedItemRigidbody != null)
            {
                lockedItemRigidbody.MovePosition(Vector2.MoveTowards(lockedItemRigidbody.position, worldPosition, Time.deltaTime * moveSpeed));
            }
        }
    }

    private IEnumerator ChangeVolume(float targetVolume, float duration)
    {
        float elapsedTime = 0.0f;
        float startVolume = sfx.volume;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            sfx.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        sfx.volume = targetVolume;
    }

    public void Enable()
    {
        magicCursor.SetActive(true);
        isMagicEnabled = true;
        
        Cursor.visible = false;
    }

    public void Disable()
    {
        magicCursor.SetActive(false);
        isMagicEnabled = false;

        Cursor.visible = true;
    }
}

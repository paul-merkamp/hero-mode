using UnityEngine;

public class MagicController : MonoBehaviour
{
    public GameObject magicCursor;
    public Camera cam;
    public float moveSpeed = 5f;

    private bool isItemLocked = false;
    private SimpleDestructible lockedItem;
    private ParticleSystem particles;

    private PlayerModeController playerModeController;

    public void Start()
    {
        particles = magicCursor.transform.GetChild(0).GetComponent<ParticleSystem>();
        playerModeController = FindAnyObjectByType<PlayerModeController>();
    }

    void Update()
    {
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
            }
            else
            {
                // Cast a ray from the mouse position to detect SimpleDestructible objects
                RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    // Check if the hit object is a SimpleDestructible
                    SimpleDestructible destructible = hit.collider.GetComponent<SimpleDestructible>();
                    if (destructible != null)
                    {
                        // Lock the item to the cursor
                        lockedItem = destructible;
                        isItemLocked = true;

                        particles.Play();
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

    public void Enable() {
        magicCursor.SetActive(true);
    }

    public void Disable() {
        magicCursor.SetActive(false);
    }
}

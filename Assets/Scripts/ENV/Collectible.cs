using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType {
        Coin,
        BossKey,
        ModeUnlock,
        Health,
        MaxHealth
    }

    public AudioClip collectSound;

    public CollectibleType type;
    public PlayerModeController.PlayerMode modeUnlock;

    public GameEvent pickupEvent;

    public int healthAmount = 1;

    public int lifetime = 0;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        pickupEvent = GetComponent<GameEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (lifetime > 0)
        {
            StartCoroutine(AnimateLifetime());
        }
    }

    private System.Collections.IEnumerator AnimateLifetime()
    {
        float seconds = lifetime;

        while (seconds > 0)
        {
            float alpha = 1f;
            if (seconds <= lifetime * 0.75f)
            {
                float pingPongTime = (lifetime - seconds + 1) / (lifetime * 0.75f);
                alpha = Mathf.PingPong(Time.time * pingPongTime, 1f);
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            yield return null;
            seconds -= Time.deltaTime;
        }

        Destroy(gameObject);
    }
}

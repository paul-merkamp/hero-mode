using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType {
        Coin,
        BossKey,
        ModeUnlock
    }

    public AudioClip collectSound;

    public CollectibleType type;
    public PlayerModeController.PlayerMode modeUnlock;

    public GameEvent pickupEvent;

    public void Start()
    {
        pickupEvent = GetComponent<GameEvent>();
    }
}

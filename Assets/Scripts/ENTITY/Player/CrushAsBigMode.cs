using UnityEngine;

public class CrushAsBigMode : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("destructible") ||
            collision.CompareTag("door") ||
            collision.CompareTag("sign"))
        {
            SimpleDestructible destructible = collision.GetComponent<SimpleDestructible>();
            if (destructible.type <= SimpleDestructible.DestructibleType.BigOnly)
            {
                destructible.GetDestroyed();
            }
        }
    }
}

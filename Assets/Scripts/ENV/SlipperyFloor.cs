using UnityEngine;

public class SlipperyFloor : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerForm>().MakeSlippery();
        }
        else if (collision.gameObject.CompareTag("entity"))
        {
            collision.gameObject.GetComponent<Entity>().MakeSlippery();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerForm>().MakeNotSlippery();
        }
        else if (collision.gameObject.CompareTag("entity"))
        {
            collision.gameObject.GetComponent<Entity>().MakeNotSlippery();
        }
    }
}

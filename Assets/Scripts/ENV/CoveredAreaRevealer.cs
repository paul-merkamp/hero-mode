using UnityEngine;

public class CoveredAreaRevealer : MonoBehaviour
{
    public GameObject coveredArea;

    public Animator bannerAnimator;
    public bool animatorTriggered = false;

    public void Start()
    {
        coveredArea.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_follower"))
        {
            coveredArea.SetActive(false);
            
            if (bannerAnimator != null && !animatorTriggered)
            {
                bannerAnimator.SetTrigger("Reveal");
                animatorTriggered = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player_follower"))
        {
            coveredArea.SetActive(true);
        }
    }
}

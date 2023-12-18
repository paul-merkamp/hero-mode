using UnityEngine;

public class CoveredAreaRevealer : MonoBehaviour
{
    public GameObject coveredArea;

    public Animator bannerAnimator;
    public bool animatorTriggered = false;

    private MusicController music;

    public AudioClip musicToPlayOnEnter;
    public AudioClip musicToPlayOnEnterSecondPass;
    public AudioClip musicToPlayOnExit;
    public AudioClip musicToPlayOnExitSecondPass;

    public void Start()
    {
        coveredArea.SetActive(true);
        music = GameObject.Find("SYS/SYS_MusicController").GetComponent<MusicController>();
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

            if (musicToPlayOnEnter != null)
            {
                music.PlaySequentially(musicToPlayOnEnter, musicToPlayOnEnterSecondPass);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player_follower"))
        {
            coveredArea.SetActive(true);
            
            if (musicToPlayOnExit != null)
            {
                music.PlaySequentially(musicToPlayOnExit, musicToPlayOnExitSecondPass);
            }
        }
    }
}

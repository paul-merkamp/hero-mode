using System.Collections;
using UnityEngine;

public class Stovetop : MonoBehaviour
{
    private GameObject timedObj;
    public Animator fireAnimator;

    public float delay;
    public float duration;
    public float offset;

    public void Start()
    {
        timedObj = transform.GetChild(0).gameObject;
        StartCoroutine(PlayAnimationRepeatedly());
    }

    private IEnumerator PlayAnimationRepeatedly()
    {
        yield return new WaitForSeconds(offset); // Add initial delay

        while (true)
        {
            timedObj.SetActive(true);
            fireAnimator.SetBool("Active", true);
            yield return new WaitForSeconds(duration);

            timedObj.SetActive(false);
            fireAnimator.SetBool("Active", false);
            yield return new WaitForSeconds(delay);
        }
    }
}
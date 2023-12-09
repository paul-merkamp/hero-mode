using UnityEngine;

public class BlackBarController : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GameObject.Find("UI/UI_EventBlackBars/BlackBarsCanvas").GetComponent<Animator>();
    }

    [ContextMenu("Toggle Black Bars")]
    public void ToggleBlackBars()
    {
        animator.SetBool("Shown", !animator.GetBool("Shown"));
    }
}

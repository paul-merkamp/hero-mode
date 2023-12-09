using TMPro;
using UnityEngine;

public class ResourceUIController : MonoBehaviour
{
    public TMP_Text frogCoinCountText;
    public Animator frogCoinAnimator;
    public GameObject bossKeyDisplay;

    public void UpdateFrogCoinCount(int count)
    {
        frogCoinCountText.text = "X " + count.ToString();
        frogCoinAnimator.SetTrigger("Display");
    }

    public void ShowBossKey(bool hasKey)
    {
        bossKeyDisplay.SetActive(hasKey);
        frogCoinAnimator.SetTrigger("Display");
    }
}

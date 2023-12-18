using System.Collections;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject target;
    private int ppu = 100;

    private PlayerModeController playerModeController;

    public bool pixelAdjusted = true;

    private void Start()
    {
        target = GameObject.Find("Player/Forms/Sword");

        playerModeController = FindObjectOfType<PlayerModeController>();
    }

    private void LateUpdate()
    {
        if (pixelAdjusted)
        {
            transform.position = new Vector3(Mathf.Round(target.transform.position.x *  ppu) / ppu, Mathf.Round(target.transform.position.y * ppu) / ppu, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            transform.rotation = target.transform.rotation;
        }
    }
}

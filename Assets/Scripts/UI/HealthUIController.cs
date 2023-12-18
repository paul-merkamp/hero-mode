using System.Collections.Generic;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public GameObject layoutGroup;
    public GameObject heartSlotPrefab;

    private PlayerController player;

    private List<GameObject> heartSlots = new List<GameObject>();

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        for (int i=0; i<layoutGroup.transform.childCount; i++)
        {
            Destroy(layoutGroup.transform.GetChild(i).gameObject);
        }

        UpdateHeartsUI();
    }

    public void UpdateHeartsUI()
    {
        if (heartSlots.Count < player.maxHealth)
        {
            int difference = player.maxHealth - heartSlots.Count;

            for (int i=0; i<difference; i++)
            {
                GameObject heartSlot = Instantiate(heartSlotPrefab, layoutGroup.transform);
                heartSlot.name = "HeartSlot" + (heartSlots.Count + i);
                heartSlots.Add(heartSlot);
            }
        }

        for (int i=0; i<heartSlots.Count; i++)
        {
            if (i < player.health)
            {
                heartSlots[i].GetComponent<Animator>().SetBool("isFull", true);
            }
            else
            {
                heartSlots[i].GetComponent<Animator>().SetBool("isFull", false);
            }
        }
    }
}

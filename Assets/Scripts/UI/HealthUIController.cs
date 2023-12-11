using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public GameObject layoutGroup;
    public GameObject heartSlotPrefab;

    private PlayerController player;

    private List<GameObject> heartSlots = new List<GameObject>();

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        for (int i=0; i<layoutGroup.transform.childCount; i++)
        {
            Destroy(layoutGroup.transform.GetChild(i).gameObject);
        }
        for (int i=0; i<player.maxHealth; i++)
        {
            GameObject heartSlot = Instantiate(heartSlotPrefab, layoutGroup.transform);
            heartSlot.name = "HeartSlot" + i;
            heartSlots.Add(heartSlot);
        }
    }

    public void UpdateHeartsUI()
    {
        if (heartSlots.Count < player.maxHealth)
        {
            for (int i=0; i<player.maxHealth - heartSlots.Count; i++)
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

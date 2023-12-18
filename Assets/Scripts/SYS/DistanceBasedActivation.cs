using System.Linq;
using UnityEngine;

public class DistanceBasedActivation : MonoBehaviour
{
    public GameObject playerFollower;
    public float activationDistance = 1.0f;

    private GameObject[] childObjects;

    void Start()
    {
        playerFollower = GameObject.Find("ENTITY/PlayerFollower");

        // Cache all child objects at start
        childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
    }


    void Update()
    {
        for (int i = childObjects.Length - 1; i >= 0; i--)
        {
            var obj = childObjects[i];
            if (obj == null)
            {
                // Remove null object from the list
                childObjects = childObjects.Where((val, idx) => idx != i).ToArray();
                continue;
            }

            // Check the distance from the player to each child
            if (Vector2.Distance(playerFollower.transform.position, obj.transform.position) <= activationDistance)
            {
                // Activate the object if it's within the distance
                if (!obj.activeSelf) obj.SetActive(true);
            }
            else
            {
                // Deactivate the object if it's outside the distance
                if (obj.activeSelf) obj.SetActive(false);
            }
        }
    }

}
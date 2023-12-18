using System.Collections;
using UnityEngine;

public class BakoFireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab;

    public void Start()
    {
        StartCoroutine(SpawnFireballs());
    }

    public IEnumerator SpawnFireballs()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float randomX = transform.position.x + Random.Range(-0.2f, 0.2f);
            Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        }
    }
}

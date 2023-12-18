using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject objectToSpawn;

    public List<GameObject> spawnedObjects;

    public float initialDelay = 0f;
    public int spawnCount = 10;
    public float spawnDelay = 5f;

    public void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Spawn()
    {
        spawnedObjects.Add(Instantiate(objectToSpawn, transform.position, Quaternion.identity));
    }

    public void StartSpawning()
    {
        particles.Play();
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
            yield return new WaitForSeconds(spawnDelay);
        }

        StopSpawning();
    }

    private void StopSpawning()
    {
        particles.Stop();
    }

    public void DestroyAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
    }
}

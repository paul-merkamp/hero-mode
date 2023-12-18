using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombShockwavePrefab;
    public float delayTime = 2f;

    private void Start()
    {
        StartCoroutine(SpawnBombShockwaveCoroutine());
    }

    private IEnumerator SpawnBombShockwaveCoroutine()
    {
        yield return new WaitForSeconds(delayTime);
        Instantiate(bombShockwavePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

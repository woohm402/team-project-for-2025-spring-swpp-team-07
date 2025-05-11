using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject spawnPosObj;
    [SerializeField]
    private Vector3 spawnRange = new Vector3(2, 0, 2);
    [SerializeField]
    private float spawnInterval = 2f;
    [SerializeField]
    private float spawnProbability = 0.2f;

    private Vector3 spawnPos;

    private void Start()
    {
        spawnPos = spawnPosObj.transform.position;
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            if (Random.Range(0, 1f) > spawnProbability)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            float offsetX = Random.Range(-spawnRange.x, spawnRange.x);
            float offsetZ = Random.Range(-spawnRange.z, spawnRange.z);
            GameObject obj = Instantiate(prefab);
            obj.transform.position = spawnPos + new Vector3(offsetX, 0, offsetZ);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

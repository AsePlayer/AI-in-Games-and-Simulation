using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemysToSpawn;
    private bool spawnedEnemy;

    Vector3 pos;
    Quaternion rotation;

    private int count = 1;

    // Start is called before the first frame update
    void Start()
    {
        spawnedEnemy = true;

        pos = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedEnemy)
        {
            StartCoroutine(spawnEnemy());
            spawnedEnemy = false;
        }
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(10 * count);
        // Spawn random enemy. Have multiple of the same enemy for increased chances.
        Instantiate(enemysToSpawn[Random.Range(0, enemysToSpawn.Count)], pos, Quaternion.identity);
        spawnedEnemy = true;
        count++;
    }
}

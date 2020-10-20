using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    public GameObject playerRef;
    public GameObject enemyRef;

    public GameObject spawnPointsParent;
    public GameObject[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new GameObject[spawnPointsParent.transform.childCount];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnPointsParent.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enemyRef.GetComponent<MonsterAI>().SpawnEnemyNearby();
        }
    }
}

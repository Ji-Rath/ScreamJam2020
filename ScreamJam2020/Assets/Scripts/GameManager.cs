using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    public float spawnRadiusMin;
    public float spawnRadiusMax;
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
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateEnemy();
        }
    }

    public void ActivateEnemy()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(Vector3.Distance(playerRef.transform.position, spawnPoints[i].transform.position) < spawnRadiusMax &&
                Vector3.Distance(playerRef.transform.position, spawnPoints[i].transform.position) > spawnRadiusMin)
            {
                enemyRef.SetActive(true);
                enemyRef.transform.position = spawnPoints[i].transform.position;
                i = spawnPoints.Length;
            }
        }
    }
}

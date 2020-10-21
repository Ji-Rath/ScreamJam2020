using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonobehaviourSingleton<GameManager>
{
    public float textTime;
    public string text;
    public GameObject playerRef;
    public GameObject enemyRef;
    private MonsterAI enemy;

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

        EquipSystem.OnPlayerDropItem += SpawnEnemyNearby;
        enemy = enemyRef.GetComponent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Test
        /*if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DialogueBox.Get().SetText(text);
            DialogueBox.Get().TriggerText(textTime);
        }*/
    }

    //Stimulates the monster to appear at an available spawn point
    public void SpawnEnemyNearby(int chance)
    {
        int randomChance = Random.Range(0, 101);

        if(randomChance <= chance)
        {
            //GameManager gameManager = GameManager.Get();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Vector3 spawnPos = spawnPoints[i].transform.position;
                float distanceBetween = Vector3.Distance(playerRef.transform.position, spawnPos);
                Ray ray = new Ray(spawnPos, (playerRef.transform.position - spawnPos).normalized);


                if (distanceBetween < enemy.spawnRadiusMax && distanceBetween > enemy.spawnRadiusMin
                    /*&& Physics.Raycast(ray, distanceBetween, LayerMask.GetMask("Default"))*/)
                {
                    enemyRef.SetActive(true);
                    enemyRef.transform.position = spawnPos;
                    break;
                }
            }

            DialogueBox.Get().SetText("I think the enemy heard that..");
            DialogueBox.Get().TriggerText(2);
        }

        
    }

    private void OnDestroy()
    {
        EquipSystem.OnPlayerDropItem -= SpawnEnemyNearby;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InventorySystem.EquipSystem;


public class GameManager : MonobehaviourSingleton<GameManager>
{
    public float textTime;
    public string text;
    public GameObject playerRef;
    public GameObject enemyRef;
    public KeyDoor vodooDollDoor;
    public KeyDoor mainEntrance;
    public DeathScreen deathScreen;
    private MonsterAI enemy;
    public Transform PlayerSpawnLocation;

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

        EquipManager.OnPlayerDropItem += SpawnEnemyNearby;
        enemy = enemyRef.GetComponent<MonsterAI>();

        if(vodooDollDoor)
        {
            vodooDollDoor.OnKeyDoorUnlocked += UnlockMainEntrance;
        }

        if (mainEntrance)
        {
            mainEntrance.OnDoorOpened += PlayerVictoryEvent;
        }

        // Create instance of player
        playerRef = Instantiate(playerRef, PlayerSpawnLocation.position, PlayerSpawnLocation.rotation);
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

    public void UnlockMainEntrance()
    {
        if(mainEntrance)
        {
            mainEntrance.isLocked = false;
            //mainEntrance.InteractDoor();

            DialogueBox.Get().SetText("The main entrance is open..");
            DialogueBox.Get().TriggerText(3);
        }
    }

    public void PlayerVictoryEvent()
    {
        Debug.Log("PLAYER WON!");
        if (deathScreen)
        {
            deathScreen.StartWinScreen();
        }
    }

    private void OnDestroy()
    {
        EquipManager.OnPlayerDropItem -= SpawnEnemyNearby;
        if (vodooDollDoor)
        {
            vodooDollDoor.OnKeyDoorUnlocked -= UnlockMainEntrance;
        }

        if (mainEntrance)
        {
            mainEntrance.OnDoorOpened -= PlayerVictoryEvent;
        }
    }
}

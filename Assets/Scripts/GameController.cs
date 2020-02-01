using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private readonly int TotalPopulation = 500;
    private readonly string SpawnAreaTag = "SpawnArea";
    private readonly string DecorTag = "Decor";

    public GameObject Person;
    public GameObject[] People;
    public int NumberOfPeople;
    public GameObject[] SpawnAreas;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAreas = GameObject.FindGameObjectsWithTag(SpawnAreaTag);
        People =  new GameObject[NumberOfPeople];
        
        for(int i = 0; i < NumberOfPeople; i++) {
            int spawnAreaIndex = Random.Range(0, SpawnAreas.Length);
            Vector2 areaCenter = SpawnAreas[spawnAreaIndex].transform.position;
            float radius =  SpawnAreas[spawnAreaIndex].GetComponent<CircleCollider2D>().radius;
            bool collidingSpawn = true;
            Vector2 spawnPosition = Vector2.zero;
            while (collidingSpawn) {
                spawnPosition = areaCenter + Random.insideUnitCircle * radius;
                RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
                collidingSpawn = hit.transform != null && hit.transform.tag == DecorTag;
            }
            
            People[i] = Instantiate(Person, spawnPosition, Quaternion.identity);
        }
    }
}

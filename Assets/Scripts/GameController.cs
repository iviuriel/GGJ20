using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private readonly int TotalPopulation = 500;
    private readonly string SpawnAreaTag = "SpawnArea";

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
            People[i] = Instantiate(Person, areaCenter + Random.insideUnitCircle * radius, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private int CalculateScore(int[] attributes1, int[] attributes2){
        int result = 0;
        for(int i = 0; i < 3; i++) {
            result += attributes1[i] + attributes2[i];
        }
        return result;
    }
}

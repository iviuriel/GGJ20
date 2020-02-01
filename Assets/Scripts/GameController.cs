using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private readonly int TotalPopulation = 500;
    private readonly string SpawnAreaTag = "SpawnArea";
    private readonly string DecorTag = "Decor";

    public GameObject Person;
    public List<GameObject> People;
    public int NumberOfPeople;
    public GameObject[] SpawnAreas;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAreas = GameObject.FindGameObjectsWithTag(SpawnAreaTag);
        People =  new List<GameObject>();
        SpawnPeople(NumberOfPeople);
        
    }

    public void FinishRound(){
        //All information is picked by gamecontroller
        int TotalAffinity = 0;
        int i = 0;
        List<GameObject> TempList = new List<GameObject>();
        while(i < People.Count){
            GameObject p = People[i];
            if(p.GetComponent<Person>().HasCouple()){                
                if(TempList.Contains(p.GetComponent<Person>().GetCouple())){
                    People.Remove(p);
                    TempList.Add(p);
                    i--;
                }else{
                    People.Remove(p);
                    TotalAffinity += p.GetComponent<Person>().GetAffinity();
                    TempList.Add(p);
                    i--;
                }
            }
            i++;
        }
        

        int CP = PlayerPrefs.GetInt(Constants.CoupledPopulationKey);
        int TA = PlayerPrefs.GetInt(Constants.HappinessKey);

        PlayerPrefs.SetInt(Constants.CoupledPopulationKey, CP + TempList.Count);
        PlayerPrefs.SetInt(Constants.HappinessKey, TA + TotalAffinity);

        //Now the people left is instantiate with new ones
        SpawnPeople(TempList.Count);

        foreach(GameObject c in TempList){
            Destroy(c);
        }
    }

    void SpawnPeople(int Amount){
        for(int i = 0; i < Amount; i++) {
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
            
            People.Add(Instantiate(Person, spawnPosition, Quaternion.identity));
        }
    }
}

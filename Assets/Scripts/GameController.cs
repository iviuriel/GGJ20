using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private readonly int TotalPopulation = 500;
    private readonly string SpawnAreaTag = "SpawnArea";
    private readonly string DecorTag = "Decor";
    private readonly string LastDay = "LAST DAY";


    public GameObject Person;
    public List<GameObject> People;
    public Animator EndGamePopUp;
    public Text HapinessPercentage;
    public Text Unpaired;
    public Text NewDay;
    public Slider Heart;
    public Slider HeartMini;
    public Text HapinessMini;
    public Animator HeartAnimator;
    public int NumberOfPeople;
    public GameObject[] SpawnAreas;
    public Color[] Colours;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAreas = GameObject.FindGameObjectsWithTag(SpawnAreaTag);
        People =  new List<GameObject>();

        int GP = PlayerPrefs.GetInt(Constants.GlobalPopulationKey);
        int TA = PlayerPrefs.GetInt(Constants.HappinessKey);
        int CP = PlayerPrefs.GetInt(Constants.CoupledPopulationKey);

        int MaxHapiness = (GP * Constants.MaxAffinity) / 2;
        int HappinessPercentageValue = (TA * 100) / MaxHapiness;
        HapinessMini.text = HappinessPercentageValue + "%";
        HeartMini.value = HappinessPercentageValue;

        int nonCoupledPopulation = GP - CP;

        Unpaired.text = Constants.UnpairedText + nonCoupledPopulation;

        if (PlayerPrefs.GetInt(Constants.EndGameKey) == 1) {
            HapinessPercentage.text = HappinessPercentageValue + "%";
            Heart.value = HappinessPercentageValue;
            EndGamePopUp.SetTrigger(Constants.HideShowPopUp);
        }else if (nonCoupledPopulation < NumberOfPeople) {
            NewDay.text = LastDay;
            SpawnPeople(nonCoupledPopulation, false);
            HeartAnimator.Play("HeartAnimation");
        }else{
            SpawnPeople(NumberOfPeople, true);
            HeartAnimator.Play("HeartAnimation");
        }
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
        
        int GP = PlayerPrefs.GetInt(Constants.GlobalPopulationKey);
        int CP = PlayerPrefs.GetInt(Constants.CoupledPopulationKey);
        int TA = PlayerPrefs.GetInt(Constants.HappinessKey);

        CP = CP + TempList.Count;
        TA = TA + TotalAffinity;

        PlayerPrefs.SetInt(Constants.CoupledPopulationKey, CP);
        PlayerPrefs.SetInt(Constants.HappinessKey, TA);


        int nonCoupledPopulation = GP - CP - CountCurrentUncoupled();

        int peopleToSpawn = TempList.Count > nonCoupledPopulation ? nonCoupledPopulation: TempList.Count;

        int MaxHapiness = (GP * Constants.MaxAffinity) / 2;
        int HappinessPercentageValue = (TA * 100) / MaxHapiness;
        HapinessPercentage.text = HappinessPercentageValue + "%";
        HapinessMini.text = HappinessPercentageValue + "%";
        Heart.value = HappinessPercentageValue;
        HeartMini.value = HappinessPercentageValue;

        if (peopleToSpawn == 0 && CountCurrentUncoupled() == 0){
            EndGamePopUp.SetTrigger(Constants.HideShowPopUp);
            PlayerPrefs.SetInt(Constants.EndGameKey, 1);
        }else if (peopleToSpawn == 0) {
            NewDay.text = LastDay;
            HeartAnimator.Play("HeartReset");
        }else{
            HeartAnimator.Play("HeartReset");
        }
        //Now the people left is instantiate with new ones
        SpawnPeople(peopleToSpawn, false);

        foreach(GameObject c in TempList){
            Destroy(c);
        }

        
    }

    public void ResetPopulation() {
        int globalPopulation = PlayerPrefs.GetInt(Constants.GlobalPopulationKey);
        PlayerPrefs.SetInt(Constants.GlobalPopulationKey, globalPopulation);
        PlayerPrefs.SetInt(Constants.HappinessKey, 0);
        PlayerPrefs.SetInt(Constants.CoupledPopulationKey, 0);
        PlayerPrefs.SetInt(Constants.EndGameKey, 0);
        GoToMainMenu();
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("Main_Menu");
    }

    void SpawnPeople(int Amount, bool InitialSpawn){
        if(InitialSpawn){
            for(int i = 0; i < Colours.Length; i++) {
                GameObject p = InstantiatePerson();
                p.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colours[i];
                p.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Colours[i];
                People.Add(p);
            }
            for(int i = 0; i < (Amount - Colours.Length); i++) {
                GameObject p = InstantiatePerson();
                int ColorIndex = Random.Range(0, Colours.Length);
                p.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colours[ColorIndex];
                p.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Colours[ColorIndex];
                People.Add(p);
            }
        }else{
             for(int i = 0; i < Amount; i++) {
                GameObject p = InstantiatePerson();
                int ColorIndex = Random.Range(0, Colours.Length);
                p.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colours[ColorIndex];
                p.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Colours[ColorIndex];
                People.Add(p);
            }
        }
    }

    private GameObject InstantiatePerson(){
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
        GameObject p = Instantiate(Person, spawnPosition, Quaternion.identity);
        return p;
    }

    private int CountCurrentUncoupled(){
        return People.FindAll((person) => !person.GetComponent<Person>().HasCouple()).Count;
    }
}

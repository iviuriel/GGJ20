using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private readonly int TotalPopulation = 500;

    public GameObject Person;
    public GameObject[] People;
    public int NumberOfPeople;
    public int Test;

    // Start is called before the first frame update
    void Start()
    {
        People =  new GameObject[NumberOfPeople];
        for(int i = 0; i < NumberOfPeople; i++) {
            People[i] = Instantiate(Person, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Test = CalculateScore(People[3].GetComponent<PersonAttributes>().Attributes, People[2].GetComponent<PersonAttributes>().Attributes);
    }

    private int CalculateScore(int[] attributes1, int[] attributes2){
        int result = 0;
        for(int i = 0; i < 3; i++) {
            result += attributes1[i] + attributes2[i];
        }
        return result;
    }
}

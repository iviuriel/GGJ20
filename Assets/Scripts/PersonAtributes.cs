using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAtributes : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly int UpperRange = 5;
    private readonly int LowerRange = -5;

    public int CatDogs;
    public int HamburgerPizza;
    public int MountainBeach;

    public int[] Attributes;

    // Start is called before the first frame update
    void Start()
    {
        CatDogs = Random.Range(UpperRange, LowerRange);
        HamburgerPizza = Random.Range(UpperRange, LowerRange);
        MountainBeach = Random.Range(UpperRange, LowerRange);

        Attributes = new int[] {CatDogs, HamburgerPizza, MountainBeach};
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public GameObject Couple;
    private GameObject OverCouple;
    private int Affinity = 0;
    private bool Clicked = false;
    void Awake(){
        Couple = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)){            
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);            
            if (hit) {
                if (hit.transform == this.transform){
                    Clicked = true;
                }
            }         
        }
        if(Input.GetMouseButton(0) && Clicked){
            this.transform.position = new Vector3(pos.x, pos.y, pos.y);
        }

        if (Input.GetMouseButtonUp(0) && Clicked){
            Clicked = false;
            CheckCouple();
        }
    }

    private void CheckCouple(){
        if(!Couple && OverCouple){ //Single
            if(!OverCouple.GetComponent<Person>().HasCouple()){
                int a = CalculateScore(this.GetComponent<PersonAttributes>().GetAttributes(), OverCouple.GetComponent<PersonAttributes>().GetAttributes());
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
            }
        }else if(Couple && OverCouple){ //Cheat
            if(Couple == OverCouple){ //if same just returns to original pos
                this.transform.position = new Vector3(Couple.transform.position.x -0.8f, Couple.transform.position.y, Couple.transform.position.z);
            }else{ //other person
                Couple.GetComponent<Person>().DoBreakUp();
                int a = CalculateScore(this.GetComponent<PersonAttributes>().GetAttributes(), OverCouple.GetComponent<PersonAttributes>().GetAttributes());
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
            }
        }else if(Couple && !OverCouple){ //Divorce
            Couple.GetComponent<Person>().DoBreakUp();
            DoBreakUp();
        }
    }

    public void DoCouple(GameObject c, bool clicked, int a){
        Couple = c;
        Affinity = a;
        if(clicked){
            this.transform.position = new Vector3(Couple.transform.position.x -0.8f, Couple.transform.position.y, Couple.transform.position.z);
        }
        //Do Animation
    }

    public void DoBreakUp(){
        Couple = null;
        Affinity = 0;
    }

    
    public bool HasCouple(){
        return Couple != null;
    }

    private int CalculateScore(int[] attributes1, int[] attributes2){
        int result = 0;
        for(int i = 0; i < 3; i++) {
            result += attributes1[i] + attributes2[i];
        }
        return result;
    }

    //********************
    //COLLIISION
    //********************
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && Clicked){
            OverCouple = other.transform.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.gameObject == OverCouple){
            OverCouple = null;
        }
    }
}

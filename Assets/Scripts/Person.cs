using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    public GameObject Couple;
    private GameObject OverCouple;
    private int Affinity = 0;
    private bool Clicked = false;
    private Animator Animator;
    private int Layers; //layer of people
    private Transform UIMask;
    void Awake(){
        Couple = null;
        Animator = this.GetComponent<Animator>();
        Layers = LayerMask.GetMask("Person");
        UIMask = this.transform.GetChild(2).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)){            
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, Layers);            
            if (hit) {
                if (hit.transform == this.transform){
                    Clicked = true;
                    HideAttributes();
                    if(Couple){
                        Couple.GetComponent<Person>().HideAttributes();
                    }
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
                Debug.Log("Affinity: "+a);
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
                ShowAttributes();
            }
        }else if(Couple && OverCouple){ //Cheat
            if(Couple == OverCouple){ //if same just returns to original pos
                this.transform.position = new Vector3(Couple.transform.position.x -0.8f, Couple.transform.position.y, Couple.transform.position.z);
            }else{ //other person
                Couple.GetComponent<Person>().DoBreakUp();
                int a = CalculateScore(this.GetComponent<PersonAttributes>().GetAttributes(), OverCouple.GetComponent<PersonAttributes>().GetAttributes());
                Debug.Log("Affinity: "+a);
                DoCouple(OverCouple, true, a);
                OverCouple.GetComponent<Person>().DoCouple(this.gameObject, false, a);
                ShowAttributes();
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

    public int GetAffinity(){ return Affinity;}
    public GameObject GetCouple(){return Couple;}

    private int CalculateScore(int[] attributes1, int[] attributes2){
        int result = 0;
        for(int i = 0; i < 3; i++) {
            result += attributes1[i] + attributes2[i];
        }
        result = Mathf.Abs(result);
        return result;
    }

    public void ShowAttributes(){
        int[] attr1 = this.GetComponent<PersonAttributes>().GetAttributes();
        int[] attr2 = Couple.GetComponent<PersonAttributes>().GetAttributes();        

        UIMask.GetChild(0).GetComponent<Image>().sprite = attr1[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoPareja") : Resources.Load<Sprite>("Sprites/PerroPareja");
        UIMask.GetChild(1).GetComponent<Image>().sprite = attr1[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaPareja")  : Resources.Load<Sprite>("Sprites/PizzaPareja");
        UIMask.GetChild(2).GetComponent<Image>().sprite = attr1[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaPareja") : Resources.Load<Sprite>("Sprites/PlayaPareja");
        UIMask.GetChild(3).GetComponent<Image>().sprite = attr2[0] < 0 ? Resources.Load<Sprite>("Sprites/GatoPareja") : Resources.Load<Sprite>("Sprites/PerroPareja");
        UIMask.GetChild(4).GetComponent<Image>().sprite = attr2[1] < 0 ? Resources.Load<Sprite>("Sprites/HamburguesaPareja") : Resources.Load<Sprite>("Sprites/PizzaPareja");
        UIMask.GetChild(5).GetComponent<Image>().sprite = attr2[2] < 0 ? Resources.Load<Sprite>("Sprites/MontañaPareja") : Resources.Load<Sprite>("Sprites/PlayaPareja");
        UIMask.GetChild(6).GetComponent<Slider>().value = Affinity;
        Animator.Play("ShowAttributes");
    }

    public void HideAttributes(){
        Animator.Play("HideAttributes");
    }

    //********************
    //COLLIISION
    //********************
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && other.transform.tag != "Decor" && other.transform.gameObject.layer != 2 && Clicked){
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public GameObject couple;
    private GameObject overCouple;
    private bool clicked = false;
    void Awake(){
        couple = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)){            
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);            
            if (hit) {
                if (hit.transform == this.transform ){
                    clicked = true;
                }
            }         
        }
        if(Input.GetMouseButton(0) && clicked){
            this.transform.position = new Vector3(pos.x, pos.y, pos.y);
        }

        if (Input.GetMouseButtonUp(0) && clicked){
            clicked = false;
            CheckCouple();
        }
    }

    private void CheckCouple(){
        if(!couple && overCouple){ //Single
            if(!overCouple.GetComponent<Person>().HasCouple()){
                DoCouple(overCouple, true);
                overCouple.GetComponent<Person>().DoCouple(this.gameObject, false);
            }
        }else if(couple && overCouple){ //Cheat
            if(couple == overCouple){ //if same just returns to original pos
                this.transform.position = new Vector3(couple.transform.position.x -0.8f, couple.transform.position.y, couple.transform.position.z);
            }else{ //other person
                couple.GetComponent<Person>().DoBreakUp();
                DoCouple(overCouple, true);
                overCouple.GetComponent<Person>().DoCouple(this.gameObject, false);
            }

        }else if(couple && !overCouple){ //Divorce
            couple.GetComponent<Person>().DoBreakUp();
            DoBreakUp();
        }
    }

    public void DoCouple(GameObject c, bool clicked){
        couple = c;
        if(clicked){
            this.transform.position = new Vector3(couple.transform.position.x -0.8f, couple.transform.position.y, couple.transform.position.z);
        }
    }

    public void DoBreakUp(){
        couple = null;
    }

    
    public bool HasCouple(){
        return couple != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && clicked){
            overCouple = other.transform.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.gameObject == overCouple){
            overCouple = null;
        }
    }
}

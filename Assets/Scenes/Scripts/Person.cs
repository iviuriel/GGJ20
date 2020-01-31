using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private GameObject couple;
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
            if (hit != null) {
                if (hit.transform == this.transform ){
                    clicked = true;
                }
            }         
        }
        if(Input.GetMouseButton(0) && clicked){
            this.transform.position = new Vector3(pos.x, pos.y, 0);
        }

        if (Input.GetMouseButtonUp(0) && clicked){
            clicked = false;
            CheckCouple();
        }
    }

    private void CheckCouple(){
        if(overCouple != null){
            if(!overCouple.GetComponent<Person>().HasCouple()){
                DoCouple(overCouple);
                overCouple.GetComponent<Person>().DoCouple(this.gameObject);
            }
        }
    }

    public void DoCouple(GameObject c){
        couple = c;
        Debug.Log(couple);
    }

    
    public bool HasCouple(){
        return couple != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && clicked){
            Debug.Log("ya me canse de ser buena onda");
            overCouple = other.transform.gameObject;
        }
    }
}

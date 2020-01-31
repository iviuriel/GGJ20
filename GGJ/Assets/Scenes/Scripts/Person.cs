using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{

    void Awake(){

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(pos.x, pos.y, pos.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{
    // Start is called before the first frame update

    CharacterController player;
    


    Vector3 groundPosition;
    string groundName;

    void Start(){

        player = this.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update(){

        if(player.isGrounded){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            /*
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0f)) print("Found an object - distance: " + hit.distance);
            */

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null)
                {
                    GameObject groundedIn = hit.collider.gameObject;
            
                    groundName = groundedIn.name;

                    if (groundName == "Snow"){
                        Debug.Log("Tocó nieve");
                        //player.transform.position(0,0,0);
                        player.transform.position = new Vector3(0, 0, 0);
                    }
                    
                }
            }
        }

            
            
            
        
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRigidBody : MonoBehaviour{
    // Start is called before the first frame update
    [Range(0,10)]public float pushPower;
    private float targetMass;
    
    private void onControllerColliderHit(ControllerColliderHit hit){

        // Cuando colisionemos con un cuerpo, detectamos si es rigidbody y lo almacenamos
        Rigidbody body = hit.collider.attachedRigidbody;
        
        // si choca con algo que no es rigidbody, no hace nada
        if(body == null || body.isKinematic){
            return;
        }
        // Si chocamos con un objeto en y es menor que -0.3
        if(hit.moveDirection.y < -0.3){
            return;
        }
        // almacenamos la masa del rigidbody
        targetMass = body.mass;

        // almacena el vector que señala la direccion a la que empujamos
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = (pushDir * pushPower) / targetMass;
    } 
    
}

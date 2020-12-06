using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    // Start is called before the first frame update
    
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;

    public float playerSpeed;
    private  Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    
    void Start(){
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);


        camDirection();
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        
        movePlayer = movePlayer * playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);

        setGravity();
        player.Move(movePlayer *Time.deltaTime);

        Debug.Log(player.velocity.magnitude);
    }
    void camDirection() {


        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        
        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
    void setGravity(){

        if(player.isGrounded){
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

        }else{
            fallVelocity -= gravity * Time.deltaTime; 
            movePlayer.y = fallVelocity;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    // Variables de movimiento
    public float horizontalMove;
    public float verticalMove;

    private Vector3 playerInput;
    //public GameManager gameManager;

    public CharacterController player;
    public float playerSpeed;
    public float gravity;
    public float fallVelocity;
    public float jumpForce;

    // Variables movimiento relativo a camara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    // Variables deslizamiento en pendientes
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;


    // Variables Animación

    public Animator playerAnimatorController;



    // Carga de componente CharacterController en la
    // variable player, al iniciar el script
    void Start(){
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();
    
    }

    //  bucle de juego que se ejecuta en cada frame
    void Update(){

        // Guarda el valor de entrada horizontal y vertical
        // para el movimiento
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");


        // Se almacena en un Vector3
        playerInput = new Vector3(horizontalMove, 0, verticalMove);

        // Se limita su magnitud a 1 para evitar aceleraciones en
        // movimientos diagonales
        playerInput = Vector3.ClampMagnitude(playerInput, 1);



        playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);



        camDirection();

        // Almacenamos en movePlayer el vector de movimiento corregido con 
        // respecto a la posicion de la camara.
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        
        // Se multiplica por la velocidad del jugador "playerSpeed"
        movePlayer = movePlayer * playerSpeed;
        // Hacemos que nuestro personaje mire siempre en la direccion en
        // la que nos estamos moviendo
        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity(); // aplicar la gravedad
        PlayerSkills(); // invoca las habilidades del personaje

        player.Move(movePlayer *Time.deltaTime);

        //Debug.Log(player.velocity.magnitude);
    }
    // Funcion para determinar la direccion a la que mira la camara. 
    void camDirection() {

        // Guarda los vectores correspondientes a la posicion/rotacion
        // de la carama tanto hacia delante como hacia la derecha.
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        // Asignamos los valores de "y" a 0 para no crear conflictos con
        // otras operaciones de movimiento.
        camForward.y = 0;
        camRight.y = 0;
        // Y normalizamos sus valores.
        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }    
    
    // Funcion para las habilidades de nuestro jugador.
    public void PlayerSkills(){
        // Si estamos tocanto el suelo y pulsamos el boton "Jump"
        if(player.isGrounded && Input.GetButtonDown("Jump")){
            // La velocidad de caida pasa a ser igual a la velocidad
            // de salto
            fallVelocity = jumpForce;
            // Y pasamos el valor a movePlayer.y
            movePlayer.y = fallVelocity;

            playerAnimatorController.SetTrigger("PlayerJump");
        }
    }

    // Funcion para la gravedad.
    public void SetGravity(){
        // Si estamos tocando el suelo
        if(player.isGrounded){
            // La velocidad de caida es igual a la gravedad 
            // en valor negativo * Time.deltaTime
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

        }else{
            // sino aceleramos la caida cada frame restando el 
            // valor de la gravedad * Time.deltaTime.
            fallVelocity -= gravity * Time.deltaTime; 
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }
        // Llamamos a la funcion SlideDown() 
        // para comprobar si estamos en una pendiente
        playerAnimatorController.SetBool("IsGrounded", player.isGrounded);
        SlideDown();
    }

    // Esta funcion detecta si estamos en una pendiente muy
    // pronunciada y nos desliza hacia abajo
    public void SlideDown(){
        // si el angulo de la pendiente en la que nos encontramos 
        // es mayor o igual al asignado en player.slopeLimit,
        // isOnSlope es VERDADERO
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if(isOnSlope){
            // movemos a nuestro jugador en los ejes "x" y "z" 
            // mas o menos deprisa en proporcion al angulo de la pendiente.
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;
            // y aplicamos una fuerza extra hacia abajo para evitar
            // saltos al caer por la pendiente.
            movePlayer.y += slopeForceDown;
        }
    }
    // Esta funcion detecta cuando colisinamos con otro objeto mientras nos movemos
    private void OnControllerColliderHit(ControllerColliderHit hit){
        //Almacenamos la normal del plano contra el que hemos chocado en hitNormal.
        hitNormal = hit.normal;
    }
    private void onTriggerStay(Collider other){
        if(other.tag == "MovingPlatform"){
            Debug.Log("UNA PLATAFORMA!");
            player.transform.SetParent(other.transform);
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.tag == "MovingPlatform"){
            player.transform.SetParent(null);
        }
    }
    private void OnAnimatorMove(){

    }
     

}

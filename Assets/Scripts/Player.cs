using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform respawnPoint;
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;
    //Jump
    private bool jumpKeyWayPressed;
    
    //walk/sprint
    private float horizontalInput;
    private Rigidbody rigitBodyComponent;
    private int superJumpsRemaining=0;


    // Start is called before the first frame update
    void Start()
    {
        rigitBodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if space key is pressed down
        //Note we won't apply any physics in the update loop
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWayPressed = true;
        }
        
        //Sprinting
        if (Input.GetKey(KeyCode.LeftShift)){
            horizontalInput = Input.GetAxis("Horizontal")*2;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        //If the player has fallen below  -50 in y axis, respawn player
        if(transform.position.y < -5)
        {
            Respawn();
        }
        //Grabbed from input 
    }
   
    //Fixed update is called once every physics update. Any physics applications should be done here
    private void FixedUpdate()
    {
        //We copy the getcomponent to not fight the y, since we apply gravity on y, then we say y velocity is 
        rigitBodyComponent.velocity = new Vector3(horizontalInput, rigitBodyComponent.velocity.y, 0);

        //How many collisions are happening with the players feet. 
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (jumpKeyWayPressed)
        {
            float jumpPower = 5f;
            if (superJumpsRemaining > 0)
            {
                jumpPower *= 2;
                superJumpsRemaining--;
            }
            rigitBodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWayPressed = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }

    //Respawn player
    private void Respawn()
    {
        //(0,0,0) velocity, standing still
        rigitBodyComponent.velocity = Vector3.zero;
        rigitBodyComponent.Sleep();
        transform.position = respawnPoint.position;
    }
}

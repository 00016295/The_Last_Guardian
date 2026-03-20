//using UnityEngine;
//using System.Collections;

//public class Player_Defence : MonoBehaviour
//{
//    public float dodge_force = 5f; // Force applied to the player when dodging
//    public float dodge_duration = 0.35f; // Duration of the dodge in seconds
//    private bool facingRight; // Flag to check the direction the player is facing
//    public Rigidbody2D rb;// Reference to the Rigidbody2D component
//    public bool isDodging; // Flag to check if the player is currently dodging
//    private float dodgeDirection; // Variable to store the direction of the dodge
//    private Animator animator; // Reference to the Animator component
//    private bool isBlocking;
//   private float movement; // Reference to the player's movement variable

//    void Start()
//    {
//        isDodging = false; // Initialize the dodging flag to false
//        facingRight = true; // Assuming the player starts facing right
//        animator = this.gameObject.GetComponent<Animator>();
//        rb = this.gameObject.GetComponent<Rigidbody2D>();
//        isBlocking = false;
//        movement = this.gameObject.GetComponent<Player>().movement;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        float move = Input.GetAxisRaw("Horizontal");
//        if (move > 0f)
//        {
//            facingRight = true;
//        }
//        else if (move < 0f)
//        {
//            facingRight = false;
//        }

//        if (Input.GetKeyDown(KeyCode.LeftShift) && isDodging == false)
//        {
//            if (Input.GetAxisRaw("Horizontal") != 0f) // Check if the player is moving horizontally

//            {
//                StartCoroutine(Dodge()); // Start the dodge coroutine when the left shift key is pressed and the player is not already dodging
//                animator.SetTrigger("Roll"); // Trigger the dodge animation
//            }
//        }

        
//    }

//    IEnumerator Dodge()
//    {
//        isDodging = true; // Set the dodging flag to true

//        if (facingRight == true)
//        {
//            dodgeDirection = 1f; // Set the dodge direction to the right
//        }
//        else if (facingRight == false)
//        {
//            dodgeDirection = -1f; // Set the dodge direction to the left
//        }
//        rb.gravityScale = 0f; // Disable gravity during the dodge
//        rb.linearVelocity = new Vector2(dodgeDirection * dodge_force, 0f); // Apply the dodge force to the player's Rigidbody2D

//        yield return new WaitForSeconds(dodge_duration); // Short delay before applying the dodge force

//        rb.linearVelocity = Vector2.zero; // Reset the player's velocity after the dodge duration

//        rb.gravityScale = 1f; // Enable gravity during the dodge
//        isDodging = false; // Set the dodging flag back to false after the dodge duration

//    }

//    void Block()
//    {
//        if(isBlocking == true)
//        {
//            animator.SetBool("is_blocking",true);
//        }
//    }

//}

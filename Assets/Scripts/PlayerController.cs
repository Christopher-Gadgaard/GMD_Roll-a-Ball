using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpStrength = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool jump = false;
    
    private Vector3 startPosition; 
    public float deathHeight = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        startPosition = transform.position;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jump = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            jump = false;
        }
        
        if (transform.position.y < deathHeight)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
        
        else if (other.gameObject.CompareTag("DeathWalls")) 
        {
            Respawn(); 
        }
    }
    
    void Respawn()
    {
        count = 0;
        SetCountText();
        rb.velocity = Vector3.zero; 
        transform.position = startPosition; 
    }
}
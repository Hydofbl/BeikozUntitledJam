using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb; //
    private PlayerManager playerManager;
    
    // Launcher
    private AudioSource audioSource;
    [SerializeField] private float launchForce = 1f;
    [SerializeField] private float launchForceLimit = 5f;
    [SerializeField] private float timeSlowAmount = 0.3f;
    [SerializeField] private float directionFactor = 1f;
    [SerializeField] private GameObject pointsParent;
    [SerializeField] private GameObject point;
    private GameObject[] points;
    [SerializeField] private float gravithFactor = 0.5f;
    [SerializeField] private int numberOfPoints;
    [SerializeField] private float spaceBetweenPoints;
    private Vector2 direction;

        // Ground Check
    [SerializeField] 
    private GameObject groundChecker;
    [SerializeField] 
    private float groundCheckDistance = 0.05f;
    [SerializeField]
    LayerMask groundLayer; //
    
    // Movement
    [SerializeField] 
    private float movementMultiplier = 10f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingBasic = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenTurning = 0.5f;

    public bool onAir, movable, facingRight;
    
    
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
        movable = true;
        onAir = false;
        facingRight = true;
        
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, transform.position, Quaternion.identity).gameObject;
            points[i].transform.SetParent(pointsParent.transform);
        }
    }
	
	void Update ()
    {
        if (rb.velocity.x < -0.01f && facingRight)
        {
            FlipFace();
        }
        else if (rb.velocity.x > 0.01f && !facingRight)
        {
            FlipFace();
        }
        
        if (playerManager.isBoxModeOn)
        {
            
            if (!pointsParent.activeInHierarchy)
                pointsParent.SetActive(true);

            if (Input.GetMouseButtonDown(0) && !onAir && movable)
            {
                if (Time.timeScale != timeSlowAmount)
                {
                    Time.timeScale = timeSlowAmount;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
                
                movable = false;
                // Zamanı yavaşlar, yolu göster, hareket etmeyi kapat
            }
        
            if (Input.GetMouseButtonUp(0) && !onAir)
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }

                Shooting();
            }
            
            if (!onAir)
            {
                transform.rotation = Quaternion.identity;
                movable = true;
            }
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
        }
        else
        {
            if (pointsParent.activeInHierarchy)
                pointsParent.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (playerManager.isBoxModeOn)
        {
            onAir = !Physics2D.OverlapBox(groundChecker.transform.position, new Vector2(0.03f, groundCheckDistance), 0, groundLayer);
            
            if (onAir)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            
            float fHorizontalVelocity = rb.velocity.x;
            fHorizontalVelocity += Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * movementMultiplier);
            else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(fHorizontalVelocity))
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * movementMultiplier);
            else
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingBasic, Time.deltaTime * movementMultiplier);

            rb.velocity = new Vector2(fHorizontalVelocity, rb.velocity.y);
        }
    }
    
    void Shooting()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        audioSource.Play();
        // Normalize etmek yerine sınırlanabilir.
        // Normilize etmeden kullanırsak yay germeye benzer bir mekanik oluyor...
        // GetComponent<Rigidbody2D>().velocity = direction * launchForce;
        
        float multiply = launchForce * direction.magnitude * directionFactor;

        
        if (multiply > launchForceLimit)
            multiply = launchForce;
        
        print(multiply);
        GetComponent<Rigidbody2D>().velocity = direction.normalized * multiply;
    }

    Vector2 PointPosition(float t)
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float multiply = launchForce * direction.magnitude * directionFactor;

        if (multiply > launchForceLimit)
            multiply = launchForce;
        
        Vector2 position = (Vector2) transform.position + (direction.normalized * t * multiply) + gravithFactor * Physics2D.gravity * (t*t);
        return position;
    }
    
    public void FlipFace()
    {
        facingRight = !facingRight;
        Vector3 tempLocalScale = transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }
}

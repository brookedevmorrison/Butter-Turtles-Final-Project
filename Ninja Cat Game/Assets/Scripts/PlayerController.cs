using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Morrison, Brooke & Melendrez, Servando
/// 10/31/23
/// This script controls the full movment of the player. Also controls collisions
/// Our Camera Follow Script is our own element
/// </summary>
public class playerController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rigidbodyRef;
    public float jumpForce = 10f;
    public float totalHealth = 99f;
    public float maxHealth = 99f;
    public float healthVal = 50f;
    private Vector3 startPos;
    private bool jetpackCollected = false;
    private bool doubleJump = false;
    private bool canTakeDamage = false;
    private Renderer[] renderers;
    private bool isGroundedtoFloor;
    private bool isFacingLeft = false;
    //Shooting Variables
    public GameObject Bullet;
    public GameObject HeavyBullet;
    private bool heavybulletOn = false;
    private bool shootRight = true;
    private bool canShoot = true;

    //waypoint gameobject for hard enemy script
    public GameObject waypoint;
    public float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigidbody component off of this object and stores a reference to it
        rigidbodyRef = GetComponent<Rigidbody>();

        //set the statrting position
        startPos = transform.position;
        // Get the Renderer components of the player and its children
        renderers = GetComponentsInChildren<Renderer>();
        // Ensure that all renderers are initially visible
        SetRenderersVisibility(true);
        // Set canTakeDamage to true after initializing renderers
        canTakeDamage = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            //The position of the waypoint will update to the player's position
            UpdatePosition();
            timer = 0.5f;
        }
        //side to side player movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!isFacingLeft) // Check if the player is not already facing left
            {
                isFacingLeft = true;
                RotatePlayerModel(-90f); // Rotate the player model 180 degrees on the X-axis

            }
            shootRight = false;
            // Handle player movement to the left
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (isFacingLeft) // Check if the player is not already facing right
            {
                isFacingLeft = false;
                RotatePlayerModel(90f); // Rotate the player model back to its original rotation

            }
            shootRight = true;
            // Handle player movement to the right
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
        if (Input.GetKeyDown(KeyCode.Return) && canShoot)
        {
            StartCoroutine(ShootWithDelay());
        }
        Die();
    }
    /// <summary>
    /// Helps the Boss Enemys ai detect player
    /// </summary>
    private void UpdatePosition()
    {
        //The wayPoint's position will now be the player's current position.
        waypoint.transform.position = transform.position;
    }

    /// <summary>
    /// Spawns bullet directly from the player and destroys it after a certain amount of seconds has passed
    /// </summary>
    private void ShootBullet()
    {
        if (heavybulletOn == true)
        {
            GameObject bulletInstance = Instantiate(HeavyBullet, transform.position, Quaternion.Euler(0, 0, 0));
            //bulletInstance.GetComponent<Bullet>().goingRight = shootRight;
        }
        else
        {
            GameObject bulletInstance = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 0));
            //bulletInstance.GetComponent<Bullet>().goingRight = shootRight;
        }

    }
    /// <summary>
    /// Adds a delay to shooting
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootWithDelay()
    {
        canShoot = false; // Disable shooting
        ShootBullet();

        yield return new WaitForSeconds(0.5f); // 0.5-second delay

        canShoot = true; // Enable shooting after the delay
    }
    /// <summary>
    /// Game Over when Health Drops to 0
    /// </summary>
    private void Die()
    {
        if (totalHealth <= 0f)
        {
            //add code to end the game by loading the game over scene
            SceneManager.LoadScene(2);  //change scene number later
            Debug.Log("Game Ends");
        }
    }
    /// <summary>
    /// Makes character blink 
    /// </summary>
    private void Blink()
    {
        if (canTakeDamage)
        {
            StartCoroutine(SetInvincibility());
        }
    }
    // Rotate the player model on the X-axis
    private void RotatePlayerModel(float angle)
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y = angle;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// makes sure the player is touching the ground before they are allowed to jump
    /// </summary>
    private void HandleJump()
    {
        RaycastHit Hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Hit, 1.5f))
        {
            isGroundedtoFloor = true;
            Debug.Log("single jumping");
        }
        else
        {
            isGroundedtoFloor = false;
            Debug.Log("not grounded");
        }

        if ((Input.GetKeyDown(KeyCode.Space) && isGroundedtoFloor == true) && !jetpackCollected)
        {
            rigidbodyRef.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (isGroundedtoFloor || doubleJump) && jetpackCollected)
        {
            rigidbodyRef.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jetpack jump " + doubleJump);
            doubleJump = !doubleJump;

        }
    }
    /// <summary>
    /// makes stuff happen when you hit certain tagged objects
    /// </summary>
    /// <param name="other">The object that is being collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "portal")
        {
            // move the player to the teleport point's position stored on the portal object
           // transform.position = other.gameObject.GetComponent<portal>().teleportPoint.transform.position;
            startPos = transform.position;
        }
        if (other.gameObject.tag == "jetpack")
        {
            jetpackCollected = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "enemy" && canTakeDamage)
        {
            totalHealth -= 15f;
            Blink();

        }
        if (other.gameObject.tag == "bossenemy" && canTakeDamage)
        {
            totalHealth -= 35f;
            Blink();
        }
        if (other.gameObject.tag == "Win")
        {
            SceneManager.LoadScene(3);
        }
        if (other.gameObject.tag == "bulletpickup")
        {
            heavybulletOn = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Healthpack")
        {
            totalHealth += healthVal;
            if (totalHealth >= maxHealth)
            {
                totalHealth = maxHealth;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Extrahealth")
        {
            maxHealth = 199f;
            totalHealth = maxHealth;
            Destroy(other.gameObject);
        }
    }
    /// <summary>
    /// Controls Blinking and Invinicibility
    /// </summary>
    /// <returns></returns>
    IEnumerator SetInvincibility()
    {
        canTakeDamage = false;

        // Blink duration and blink speed settings
        float blinkDuration = 5f;
        float blinkSpeed = 0.2f;

        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            SetRenderersVisibility(!AreRenderersVisible()); // Toggle visibility of all renderers
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }

        SetRenderersVisibility(true); // Ensure all renderers are visible at the end
        canTakeDamage = true;
    }

    /// <summary>
    /// Sets the visibility of all renderers
    /// </summary>
    /// <param name="visible"></param>
    private void SetRenderersVisibility(bool visible)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }

    /// <summary>
    /// Checks if any renderer is currently visible
    /// </summary>
    /// <returns></returns>
    private bool AreRenderersVisible()
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer.enabled)
            {
                return true;
            }
        }
        return false;
    }
}


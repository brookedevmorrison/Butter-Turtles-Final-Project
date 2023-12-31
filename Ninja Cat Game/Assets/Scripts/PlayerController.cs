using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Morrison, Brooke & Melendrez, Servando
/// 12/8/23
/// This script controls the full movment of the player. 
/// </summary>
public class playerController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rigidbodyRef;
    public float jumpForce = 10f;
    public float totalHealth = 5f;
    public float maxHealth = 5f;
    public float healthVal = 50f;
 
    private bool canTakeDamage = false;
    private Renderer[] renderers;
  
    private Rigidbody rb;
    private Camera mainCamera;
    //Shooting Variables
    public GameObject ninjaStarPrefab;
    public float throwForce = 10f;


    //waypoint gameobject for hard enemy script
    public GameObject waypoint;
    public float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigidbody component off of this object and stores a reference to it
        rigidbodyRef = GetComponent<Rigidbody>();

        // Get the Renderer components of the player and its children
        renderers = GetComponentsInChildren<Renderer>();
        // Ensure that all renderers are initially visible
        SetRenderersVisibility(true);
        // Set canTakeDamage to true after initializing renderers
        canTakeDamage = true;
        // Camera
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
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
        //player movement
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowNinjaStar();
        }
        Die();
    }
    void ThrowNinjaStar()
    {
        if (ninjaStarPrefab != null && mainCamera != null)
        {
            // Instantiate the ninja star
            GameObject ninjaStar = Instantiate(ninjaStarPrefab, transform.position, Quaternion.identity);

            // Calculate the direction in which to throw the ninja star based on camera orientation
            Vector3 throwDirection = mainCamera.transform.forward;

            // Get the Rigidbody of the ninja star
            Rigidbody ninjaStarRb = ninjaStar.GetComponent<Rigidbody>();

            // Apply force to the thrown ninja star
            if (ninjaStarRb != null)
            {
                ninjaStarRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Ninja star does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogWarning("Ninja star prefab or main camera not assigned.");
        }
    }
    void MovePlayer()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Ignore the vertical component to ensure the player doesn't move up or down based on camera angle
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction based on camera orientation
        Vector3 moveDirection = (cameraForward * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal")).normalized;

        // Move the player based on the calculated direction and deltaTime
        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
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
    /// Game Over when Health Drops to 0
    /// </summary>
    public void Die()
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
 

    /// <summary>
    /// makes sure the player is touching the ground before they are allowed to jump
    /// </summary>
    /// <summary>
    /// makes stuff happen when you hit certain tagged objects
    /// </summary>
    /// <param name="other">The object that is being collided with</param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && canTakeDamage)
        {
            totalHealth -= 1f;
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
        if (other.gameObject.tag == "Lava")
        {
            SceneManager.LoadScene(2);  
            Debug.Log("Game Ends");
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


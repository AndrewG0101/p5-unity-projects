using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    private GameObject focalPoint;
    public float speed = 5.0f;
    public float maxVelocitySq;
    public bool hasPowerup = false;
    public GameObject powerupIndicator;
    public ParticleSystem explosionParticle;
    public bool gameOver;
    private float ySpeed;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerupIndicator.transform.position = transform.position + new Vector3(0, 0f, 0);

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false; 
        }
        if(transform.position.y < -10)
        {
            gameOver = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(8);
        hasPowerup = false; 
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        } 
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game Over");
            gameOver = true;
            explosionParticle.Play();
        }
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        { 
            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to " + hasPowerup);
             hasPowerup = true;
             gameOver = false;
            Destroy(collision.gameObject);
            spawnManager.UpdateScore(5);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }
}

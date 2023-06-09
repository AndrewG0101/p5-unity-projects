using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager;
    private float minSpeed = 12;
    private float maxSpeed = 16; 
    private float maxTorque = 10;
    private float xRange = 4; 
    private float ySpawnPos = -2;

    public ParticleSystem explosionParticle; // cool effects
    public int pointValue; // keeps track of points 

    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>(); // rigidbody 
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        //throw object into the air 
        targetRb.AddForce(RandomForce(), ForceMode.Impulse); 
        //spin object
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse); 

        transform.position = RandomSpawnPos(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() // Using the Mouse to pay the game
    {
        if(gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); 
            gameManager.UpdateScore(pointValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if(!gameObject.CompareTag("Bad"))// bad game objects will not make you lose
        {
            gameManager.GameOver();// stops the game 
        }
    }
    
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}

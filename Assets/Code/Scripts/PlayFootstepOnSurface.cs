using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstepOnSurface : MonoBehaviour
{ 
    public float minVolume = 1.0f;
    public float maxVolume = 1.0f;

    public float minPitch = 1.0f;
    public float maxPitch = 1.0f;

    public AudioClip[] footstepsOnGrass;
    public AudioClip[] footstepsOnDirt;
    public AudioClip[] footstepsOnSand;
    public AudioClip[] footstepsOnWater;
    public AudioClip[] footstepsOnRock;

    public string surface;
    
    [Header("Footstep Settings")]
    public float footstepCooldown = 0.3f;  // Base time between steps
    public float footstepDistance = 2.0f;  // Distance needed to travel for a step
    private float lastFootstepTime = 0f;
    private Vector3 lastPosition;
    private float distanceTraveled = 0f;
    
    [Header("Movement Detection")]
    public float minMovementSpeed = 0.1f;  // Minimum speed to play footsteps
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isMoving = false;

    // Flag to track water collision
    private bool isInWater = false;
    
    // Define terrain texture indices
    [Header("Terrain Texture Settings")]
    [Tooltip("Index of the grass texture in the terrain's splat map")]
    public int grassTextureIndex = 0;
    [Tooltip("Index of the dirt texture in the terrain's splat map")]
    public int dirtTextureIndex = 1;
    [Tooltip("Index of the sand texture in the terrain's splat map")]
    public int sandTextureIndex = 6;
    [Tooltip("Index of the forest birch texture in the terrain's splat map")]
    public int forestBirchTextureIndex = 4;
    [Tooltip("Index of the forest texture in the terrain's splat map")]
    public int forestTextureIndex = 5;
    [Tooltip("Index of the rock texture in the terrain's splat map")]
    public int rockTextureIndex = 2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Check if we're moving
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        isMoving = horizontalVelocity.magnitude > minMovementSpeed;
        
        // Calculate distance traveled
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        
        // If we're grounded, moving, and have traveled enough distance
        if (isGrounded && isMoving && distanceTraveled >= footstepDistance && Time.time > lastFootstepTime + footstepCooldown)
        {
            PlayFootstepSoundSurface();
            lastFootstepTime = Time.time;
            distanceTraveled = 0f;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
        isGrounded = true;
    }
    
    void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
        isGrounded = true;
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isInWater = false;
        }
        
        // Check if we've left the ground
        if (collision.gameObject.GetComponent<Terrain>() != null || 
            collision.gameObject.CompareTag("Grass") ||
            collision.gameObject.CompareTag("Sand") ||
            collision.gameObject.CompareTag("Dirt") ||
            collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = false;
        }
    }
    
    private void HandleCollision(Collision collision)
    {
        // First check for special tagged objects (water and rock)
        if (collision.gameObject.CompareTag("Water"))
        {
            surface = "Water";
            isInWater = true;
        }
        else if (collision.gameObject.CompareTag("Rock"))
        {
            surface = "Rock";
        }
        // Only proceed with other checks if not in water
        else if (!isInWater)
        {
            // Then check if we're colliding with a terrain
            if (collision.gameObject.GetComponent<Terrain>() != null)
            {
                // We hit a terrain, determine the texture at the contact point
                Vector3 contactPoint = collision.contacts[0].point;
                DetermineTerrainTexture(contactPoint);
            }
            // Fall back to tag-based detection for other objects
            else if (collision.gameObject.CompareTag("Grass"))
            {
                surface = "Grass";
            }
            else if (collision.gameObject.CompareTag("Sand"))
            {
                surface = "Sand";
            }
            else if (collision.gameObject.CompareTag("Dirt"))
            {
                surface = "Dirt";
            }
        }
    }

    // Add trigger detection for water
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && Time.time > lastFootstepTime + footstepCooldown)
        {
            surface = "Water";
            isInWater = true;
            
            PlayFootstepSoundSurface();
            lastFootstepTime = Time.time;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
    
    private void DetermineTerrainTexture(Vector3 worldPos)
    {
        // Get the terrain
        Terrain terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;
        
        // Convert world position to terrain position
        Vector3 terrainPos = worldPos - terrain.transform.position;
        
        // Calculate terrain texture coordinates (0-1)
        float normX = terrainPos.x / terrainData.size.x;
        float normZ = terrainPos.z / terrainData.size.z;
        
        // Make sure we're within bounds
        if (normX < 0 || normX > 1 || normZ < 0 || normZ > 1)
        {
            return;
        }
        
        // Get the splat map coordinates
        float mapX = normX * terrainData.alphamapWidth;
        float mapZ = normZ * terrainData.alphamapHeight;
        
        // Get the splat data for this cell
        float[,,] splatmapData = terrainData.GetAlphamaps(
            Mathf.FloorToInt(mapX),
            Mathf.FloorToInt(mapZ), 1, 1);
        
        // Get the dominant texture
        float highestValue = 0;
        int dominantTextureIndex = 0;
        
        // For each texture on the terrain
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            // The strength of this texture at this point
            float textureStrength = splatmapData[0, 0, i];
            
            if (textureStrength > highestValue)
            {
                highestValue = textureStrength;
                dominantTextureIndex = i;
            }
        }
        
        // Set the surface based on the dominant texture index
        if (dominantTextureIndex == grassTextureIndex)
        {
            surface = "Grass";
        }
        else if (dominantTextureIndex == dirtTextureIndex)
        {
            surface = "Dirt";
        }
        else if (dominantTextureIndex == sandTextureIndex)
        {
            surface = "Sand";
        }
        else if (dominantTextureIndex == forestBirchTextureIndex)
        {
            surface = "Grass";
        }
        else if (dominantTextureIndex == forestTextureIndex)
        {
            surface = "Grass";
        }
        else if (dominantTextureIndex == rockTextureIndex)
        {
            surface = "Rock";
        }
        else
        {
            // Default to a surface type if none matches
            surface = "Dirt";
        }
    }
    
    void PlayFootstepSoundSurface()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = Random.Range(minVolume, maxVolume);
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        switch (surface)
        { 
            case "Grass":
                audioSource.clip = footstepsOnGrass[Random.Range(0, footstepsOnGrass.Length)];
                break;
            case "Sand":
                audioSource.clip = footstepsOnSand[Random.Range(0, footstepsOnSand.Length)];
                break;
            case "Dirt":
                audioSource.clip = footstepsOnDirt[Random.Range(0, footstepsOnDirt.Length)];
                break;
            case "Water":
                audioSource.clip = footstepsOnWater[Random.Range(0, footstepsOnWater.Length)];
                break;
            case "Rock":
                audioSource.clip = footstepsOnRock[Random.Range(0, footstepsOnRock.Length)];
                break;
            default:
                break;
        } 

        audioSource.Play();
    }
}
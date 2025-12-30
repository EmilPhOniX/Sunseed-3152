using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerPlanting : MonoBehaviour
{
    [Header("Planting action")]
    [Tooltip("Input action reference for planting action")]
    public InputActionReference planteActionRef;

    [Header("Watering action")]
    [Tooltip("Input action reference for watering action")]
    public InputActionReference waterActionRef;

    [Header("Seed prefab")]
    [Tooltip("Prefab of the seed to be planted")]
    public GameObject SeedPrefab;

    [Header("Water particle effect")]
    [Tooltip("Particle system for watering effect")]
    public ParticleSystem waterEffect;
    
    [Header("UI Manager")]
    [Tooltip("Reference to the UI Manager")]
    public UIManager UIManager;

    private AudioSource audioSource;

    [Header("Sounds")]
    [Tooltip("Audio source for planting sound")]
    public AudioClip plantSound;
    [Tooltip("Audio source for watering sound")]
    public AudioClip waterSound;
    [Tooltip("Audio source for harvesting sound")]
    public AudioClip harvestSound;
    [Tooltip("Audio source for can't plant sound")]
    public AudioClip cantPlanteSound;

    private Collider collision;
    private SeedController seedScript;
    private int isInNoSpawnZone = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("AudioSource component is missing on PlayerPlanting.");
        }
    }

    private void OnEnable()
    {
        planteActionRef.action.performed += OnPlante;
        waterActionRef.action.performed += OnWater;
    }

    private void OnDisable()
    {
        planteActionRef.action.performed -= OnPlante;
        waterActionRef.action.performed -= OnWater;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Seed"))
        {
            collision = other;
            seedScript = other.GetComponent<SeedController>();
        }
        if (other.gameObject.CompareTag("NoSpawnZone")) 
        {
            isInNoSpawnZone++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Seed"))
        {
            collision = null;
            seedScript = null;
        }
        if (other.gameObject.CompareTag("NoSpawnZone"))
        {
            isInNoSpawnZone--;
            if (isInNoSpawnZone < 0) isInNoSpawnZone = 0;
        }
    }

    private void OnPlante(InputAction.CallbackContext context)
    {
        if (UIManager.isPaused) return;

        if (!collision && isInNoSpawnZone == 0)
        {
            Instantiate(SeedPrefab, new Vector3(transform.position.x, 0.9f, transform.position.z + 1.5f), Quaternion.identity);
            if (audioSource) audioSource.PlayOneShot(plantSound);
            else Debug.LogWarning("AudioSource component is missing on PlayerPlanting.");
        }

        else if (seedScript != null && seedScript.GetIsReady) Harvest();

        else         
        {
            Debug.Log("Can't plant here");
            audioSource.PlayOneShot(cantPlanteSound);
        }
    }

    private void Harvest()
    {
        Debug.Log("Harvested the seed");
        UIManager.UpdateScore(1);
        audioSource.PlayOneShot(harvestSound);
        Destroy(collision.gameObject);
        collision = null;
        seedScript = null;
        isInNoSpawnZone--;
        if (isInNoSpawnZone < 0) isInNoSpawnZone = 0;
    }

    private void OnWater(InputAction.CallbackContext context)
    {
        if (UIManager.isPaused) return;

        if (collision)
        {
            if (seedScript && !seedScript.GetIsWatered)
            {
                seedScript.OnWatered();
                if (waterEffect) waterEffect.Play();
                audioSource.PlayOneShot(waterSound);
                Debug.Log("Watered the seed");
            }
            else Debug.Log("Seed is already watered");
        }
        else Debug.Log("Nothing to water");
    }
}

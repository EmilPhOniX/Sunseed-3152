using UnityEngine;

public class SeedController : MonoBehaviour
{
    [Header("Prefabs states")]
    public GameObject seedPlantedPrefab;
    public GameObject seedWatteredPrefab;
    public GameObject seedGrowingPrefab;
    public GameObject seedReadyPrefab;

    private GameObject currentSeedInstance;
    private bool isWatered = false;
    private bool isGrowing = false;
    private bool isReady = false;

    public bool GetIsReady => isReady;
    public bool GetIsGrowing => isGrowing;
    public bool GetIsWatered => isWatered;

    void Start()
    {
        SpawnModel(seedPlantedPrefab);
    }

    public void OnWatered()
    {
        if (isWatered) return;
        Debug.Log("Seed watered by the SeedController.");

        isWatered = true;

        SpawnModel(seedWatteredPrefab);
    }

    public void OnGrowing()
    {
        if (!isWatered || isGrowing) return;
        Debug.Log("Seed is growing.");
        isGrowing = true;
        SpawnModel(seedGrowingPrefab);
    }

    public void OnReady()
    {
        if (!isGrowing || isReady) return;
        Debug.Log("Seed is ready to harvest.");
        isReady = true;
        SpawnModel(seedReadyPrefab);
    }

    private void SpawnModel(GameObject prefab)
    {
        if (currentSeedInstance != null)
        {
            Destroy(currentSeedInstance);
        }
        currentSeedInstance = Instantiate(prefab, this.transform.position, Quaternion.identity, transform);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}

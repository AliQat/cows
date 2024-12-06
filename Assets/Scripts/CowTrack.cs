using UnityEngine;

public class CowTrack : MonoBehaviour
{
    public float trackSpeed = 3f;
    public float startX = -12f;  
    public float endX = 12f;     
    public float trackY = -2f;    
    public float spawnInterval = 4f;
    public GameObject cowPrefab;

    public float minPopUpDelay = 1f;
    public float maxPopUpDelay = 3f;
    public float popUpChance = 0.7f;
    public float yOffset = 1f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCow();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnCow()
    {
        Vector2 spawnPosition = new Vector2(startX, trackY);
        GameObject cow = Instantiate(cowPrefab, spawnPosition, Quaternion.identity);
        CowBehavior cowBehavior = cow.GetComponent<CowBehavior>();

        float randomDelay = Random.Range(minPopUpDelay, maxPopUpDelay);
        cowBehavior.Initialize(yOffset, randomDelay, popUpChance);
    }
}
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TrackConfiguration
{
    public GameObject cartPrefab;
    public GameObject cowPrefab;
    [Header("Track Properties")]
    public float trackY = 0f;
    public float scale = 1f;
    public float trackSpeed = 3f;
    public float spawnInterval = 4f;
    public float yOffset = 1f;
    [Header("Sorting")]
    public string sortingLayerName = "Default";
    public int baseOrderInLayer = 0;
}

public class TrackManager : MonoBehaviour
{
    [Header("Track Setups")]
    public TrackConfiguration backTrack;
    public TrackConfiguration middleTrack;
    public TrackConfiguration frontTrack;

    [Header("Spawn Settings")]
    public float startX = -12f;
    public float endX = 12f;
    public float minPopUpDelay = 1f;
    public float maxPopUpDelay = 3f;
    public float popUpChance = 0.7f;

    private List<TrackConfiguration> tracks;
    private List<float> nextSpawnTimes = new List<float>();

    void Start()
    {
        tracks = new List<TrackConfiguration>
        {
            backTrack,
            middleTrack,
            frontTrack
        };

        foreach (var track in tracks)
        {
            nextSpawnTimes.Add(Time.time + Random.Range(0f, track.spawnInterval));
        }
    }


    // Refactored cause this function is cursed
    void SetupDefaultTrackValues() // I fucking hate layers dawg
    {
        // Back track (furthest)
        backTrack.trackY = 3f;
        backTrack.scale = 0.7f;
        backTrack.trackSpeed = 2f;
        backTrack.spawnInterval = 5f;
        backTrack.yOffset = 0.7f;
        backTrack.sortingLayerName = "Default";    
        backTrack.baseOrderInLayer = 0;            

        // Middle track
        middleTrack.trackY = 1f;
        middleTrack.scale = 1f;
        middleTrack.trackSpeed = 3f;
        middleTrack.spawnInterval = 4f;
        middleTrack.yOffset = 1f;
        middleTrack.sortingLayerName = "Default";  
        middleTrack.baseOrderInLayer = 10;         

        // Front track (closest)
        frontTrack.trackY = -1f;
        frontTrack.scale = 1.3f;
        frontTrack.trackSpeed = 4f;
        frontTrack.spawnInterval = 3f;
        frontTrack.yOffset = 1f;
        frontTrack.sortingLayerName = "Default";   
        frontTrack.baseOrderInLayer = 20;         
    }

    
    void Update()
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            if (Time.time >= nextSpawnTimes[i])
            {
                SpawnCartAndCow(i);
                nextSpawnTimes[i] = Time.time + tracks[i].spawnInterval;
            }
        }
    }

    void SpawnCartAndCow(int trackIndex)
    {
        TrackConfiguration track = tracks[trackIndex];
        Vector2 spawnPosition = new Vector2(startX, track.trackY);

        GameObject cart = Instantiate(track.cartPrefab, spawnPosition, Quaternion.identity);
        CartBehavior cartBehavior = cart.GetComponent<CartBehavior>();

        cart.transform.localScale = Vector3.one * track.scale;

        GameObject cow = Instantiate(track.cowPrefab, spawnPosition, Quaternion.identity, cart.transform);
        CowBehavior cowBehavior = cow.GetComponent<CowBehavior>();

        SetupSortingLayers(cart, cow, track);

        cartBehavior.Initialize(endX);
        cartBehavior.trackSpeed = track.trackSpeed;
        cowBehavior.Initialize(track.yOffset, Random.Range(minPopUpDelay, maxPopUpDelay), popUpChance);
    }

    void SetupSortingLayers(GameObject cart, GameObject cow, TrackConfiguration track)
    {
        SpriteRenderer cartRenderer = cart.GetComponent<SpriteRenderer>();
        SpriteRenderer cowRenderer = cow.GetComponent<SpriteRenderer>();

        cowRenderer.sortingLayerName = track.sortingLayerName;
        cowRenderer.sortingOrder = track.baseOrderInLayer;

        cartRenderer.sortingLayerName = track.sortingLayerName;
        cartRenderer.sortingOrder = track.baseOrderInLayer + 1;
    }

}
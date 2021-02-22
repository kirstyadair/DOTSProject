using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public enum TokenColours
{
    Red,
    Blue,
    Green,
    Orange,
    Yellow,
    Pink,
    Purple,
    Cyan,
    Null
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject tokenPrefab;

    private int numSpawned = 0;
    private int numToSpawn = 20;

    private Entity tokenEntityPrefab;
    private EntityManager manager;

    public Entity hitToken;
    public TokenColours hitTokenColour;
    public List<Entity> tokensToMatch = new List<Entity>();

    public bool attemptMatch = false;
    public bool canAttemptNextMatch = true;

    public bool refreshDynamicBuffers = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        tokenEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(tokenPrefab, settings);
    }

    // Update is called once per frame
    void Update()
    {
        if (numSpawned < numToSpawn)
        {
            manager.Instantiate(tokenEntityPrefab);
            numSpawned++;
        }
    }*/
}

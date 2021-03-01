using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject spawner;
    public Vector3[] spawnerTransforms;
    public Entity hitToken;
    public TokenColours hitTokenColour;
    public List<Entity> tokensToMatch = new List<Entity>();
    public List<float> tokenDistances = new List<float>();

    public bool attemptMatch = false;
    public bool canAttemptNextMatch = true;

    public bool refreshDynamicBuffers = false;

    public Text objectiveText;
    public Text moveText;

    [Header("Objective")] 
    public TokenColours objectiveColour;
    public int objectiveAmount;
    public int movesAllowed;

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

    public void AddToSpawners(int numToSpawn)
    {
        for (int i = 0; i < spawnerTransforms.Length; i++)
        {
            GameObject spawnedSpawner = Instantiate(spawner, spawnerTransforms[i], Quaternion.identity);
            spawner.GetComponent<SpawnerAuthoringComponent>()._numToSpawn = numToSpawn/spawnerTransforms.Length + 1;
        }
    }

    private void Update()
    {
        if (objectiveAmount >= 0) objectiveText.text = objectiveAmount.ToString();

        moveText.text = movesAllowed.ToString();
        
        if (objectiveAmount <= 0)
        {
            objectiveText.text = "0";
            Debug.Log("Win!");
        }

        if (movesAllowed <= 0)
        {
            Debug.Log("Lose :(");
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

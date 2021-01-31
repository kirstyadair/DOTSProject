using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Token
{
    public GameObject prefab;
    public int maxIntToSpawn;
}

public class SpawnerAuthoringComponent : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] 
    private Token[] _spawnerPrefabs;
    
    [SerializeField] 
    private float _spawnRate;

    [SerializeField] 
    private int _numToSpawn;
    
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        foreach (Token prefab in _spawnerPrefabs)
        {
            referencedPrefabs.Add(prefab.prefab);
        }
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        List<Entity> entities = new List<Entity>();
        List<int> chances = new List<int>();
        foreach (Token prefab in _spawnerPrefabs)
        {
            entities.Add(conversionSystem.GetPrimaryEntity(prefab.prefab));
            chances.Add(prefab.maxIntToSpawn);
        }

        dstManager.AddComponentData(entity, new Spawner
        {
            spawnerPrefabs = entities.ToArray(),
            spawnerPrefabChances = chances.ToArray(),
            timeBetweenSpawns = 1 / _spawnRate,
            timeToNextSpawn = 0,
            numToSpawn = _numToSpawn,
            numAlreadySpawned = 0
        });
    }
}

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
            redSpawnerPrefab = entities[0],
            redSpawnerPrefabChances = chances[0],
            blueSpawnerPrefab = entities[1],
            blueSpawnerPrefabChances = chances[1],
            greenSpawnerPrefab = entities[2],
            greenSpawnerPrefabChances = chances[2],
            orangeSpawnerPrefab = entities[3],
            orangeSpawnerPrefabChances = chances[3],
            yellowSpawnerPrefab = entities[4],
            yellowSpawnerPrefabChances = chances[4],
            pinkSpawnerPrefab = entities[5],
            pinkSpawnerPrefabChances = chances[5],
            purpleSpawnerPrefab = entities[6],
            purpleSpawnerPrefabChances = chances[6],
            cyanSpawnerPrefab = entities[7],
            cyanSpawnerPrefabChances = chances[7],
            timeBetweenSpawns = 1 / _spawnRate,
            timeToNextSpawn = 0,
            numToSpawn = _numToSpawn,
            numAlreadySpawned = 0
        });
    }
}

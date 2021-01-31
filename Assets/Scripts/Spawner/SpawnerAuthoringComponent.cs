using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoringComponent : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] 
    private GameObject _spawnerPrefab;
    
    [SerializeField] 
    private float _spawnRate;

    [SerializeField] 
    private int _numToSpawn;
    
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(_spawnerPrefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Spawner
        {
            spawnerPrefab = conversionSystem.GetPrimaryEntity(_spawnerPrefab),
            timeBetweenSpawns = 1 / _spawnRate,
            timeToNextSpawn = 0,
            numToSpawn = _numToSpawn,
            numAlreadySpawned = 0
        });
    }
}

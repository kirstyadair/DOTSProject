using Unity.Entities;
using UnityEngine;

public struct Spawner : IComponentData
{
    public Entity spawnerPrefab;
    public float timeBetweenSpawns;
    public float timeToNextSpawn;
    public int numToSpawn;
    public int numAlreadySpawned;
}

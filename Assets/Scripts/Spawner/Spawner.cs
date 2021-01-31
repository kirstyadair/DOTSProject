using Unity.Entities;
using UnityEngine;

public struct Spawner : IComponentData
{
    public Entity[] spawnerPrefabs;
    public int[] spawnerPrefabChances;
    public float timeBetweenSpawns;
    public float timeToNextSpawn;
    public int numToSpawn;
    public int numAlreadySpawned;
}

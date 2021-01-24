using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData
{
    public int numToSpawn;
    public int numAlreadySpawned;
    public Entity tokenToSpawn;
}

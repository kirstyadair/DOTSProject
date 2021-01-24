using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

/*[AlwaysSynchronizeSystem]
public class Spawner : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entities.WithStructuralChanges().ForEach((ref SpawnerData spawnerData) =>
        {
            if (spawnerData.numAlreadySpawned < spawnerData.numToSpawn)
            {
                manager.Instantiate(spawnerData.tokenToSpawn);
                spawnerData.numAlreadySpawned++;
            }
        }).Run();

        return default;
    }
}*/

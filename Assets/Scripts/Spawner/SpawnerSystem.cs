using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class SpawnerSystem : JobComponentSystem
{
    private struct SpawnerJob : IJobForEachWithEntity<Spawner, LocalToWorld>
    {
        private float _deltaTime;
        private EntityCommandBuffer.Concurrent _ecb;
        
        public SpawnerJob(float deltaTime, EntityCommandBuffer.Concurrent ecb)
        {
            _ecb = ecb;
            _deltaTime = deltaTime;
        }
        
        public void Execute(Entity entity, int index, ref Spawner spawner, ref LocalToWorld localToWorld)
        {
            spawner.timeToNextSpawn -= _deltaTime;

            if (spawner.timeToNextSpawn < 0)
            {
                spawner.timeToNextSpawn = spawner.timeBetweenSpawns;
                Entity spawnedPrefab = _ecb.Instantiate(index, spawner.spawnerPrefab);

                _ecb.SetComponent(index, spawnedPrefab, new Translation
                {
                    Value = localToWorld.Position
                });
            }
        }
    }
    
    private EndSimulationEntityCommandBufferSystem _esecbs;

    protected override void OnCreate()
    {
        _esecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        SpawnerJob job = new SpawnerJob(UnityEngine.Time.deltaTime, _esecbs.CreateCommandBuffer().ToConcurrent());

        JobHandle jobHandle = job.Schedule(this, inputDeps);
        _esecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

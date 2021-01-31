using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

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

            if (spawner.timeToNextSpawn < 0 && spawner.numAlreadySpawned < spawner.numToSpawn)
            {
                spawner.numAlreadySpawned++;
                spawner.timeToNextSpawn = spawner.timeBetweenSpawns;
                Entity spawnedPrefab = _ecb.Instantiate(index, spawner.spawnerPrefab);
                float3 currentPos = localToWorld.Position;
                float3 newPos = new float3(Random.Range(currentPos.x - 1.0f, currentPos.x + 1.0f), Random.Range(currentPos.y - 1.0f, currentPos.y + 1.0f), Random.Range(currentPos.z - 1.0f, currentPos.z + 1.0f));

                _ecb.SetComponent(index, spawnedPrefab, new Translation
                {
                    Value = newPos
                });
            }
        }
    }
    
    private BeginSimulationEntityCommandBufferSystem _bsecbs;

    protected override void OnCreate()
    {
        _bsecbs = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        SpawnerJob job = new SpawnerJob(UnityEngine.Time.deltaTime, _bsecbs.CreateCommandBuffer().ToConcurrent());

        JobHandle jobHandle = job.Schedule(this, inputDeps);
        _bsecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

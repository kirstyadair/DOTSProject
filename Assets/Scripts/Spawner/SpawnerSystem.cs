using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SpawnerSystem : JobComponentSystem
{
    private struct SpawnerJob : IJobForEachWithEntity<Spawner, LocalToWorld>
    {
        private float _deltaTime;
        private Random _random;
        private EntityCommandBuffer.Concurrent _ecb;
        
        public SpawnerJob(float deltaTime, EntityCommandBuffer.Concurrent ecb, Random random)
        {
            _ecb = ecb;
            _deltaTime = deltaTime;
            _random = random;
        }
        
        public void Execute(Entity entity, int index, ref Spawner spawner, ref LocalToWorld localToWorld)
        {
            spawner.timeToNextSpawn -= _deltaTime;

            if (spawner.timeToNextSpawn < 0 && spawner.numAlreadySpawned < spawner.numToSpawn)
            {
                spawner.numAlreadySpawned++;
                spawner.timeToNextSpawn = spawner.timeBetweenSpawns;

                int randomValue = _random.NextInt(0, 100);
                Entity selectedPrefab = spawner.spawnerPrefabs[0];

                for (int i = 0; i < spawner.spawnerPrefabs.Length; i++)
                {
                    if (spawner.spawnerPrefabChances[i] == 0) continue;

                    if (randomValue <= spawner.spawnerPrefabChances[i])
                    {
                        selectedPrefab = spawner.spawnerPrefabs[i];
                        break;
                    }
                }
                
                Entity spawnedPrefab = _ecb.Instantiate(index, selectedPrefab);

                _ecb.SetComponent(index, spawnedPrefab, new Translation
                {
                    Value = localToWorld.Position + _random.NextFloat(-1, 1)
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
        SpawnerJob job = new SpawnerJob(UnityEngine.Time.deltaTime, _bsecbs.CreateCommandBuffer().ToConcurrent(), new Random((uint) UnityEngine.Random.Range(0, int.MaxValue)));

        JobHandle jobHandle = job.Schedule(this, inputDeps);
        _bsecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

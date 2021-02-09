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
        private EntityCommandBuffer.ParallelWriter _ecb;
        
        public SpawnerJob(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, Random random)
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
                List<Entity> allPrefabs = new List<Entity>();
                allPrefabs.Add(spawner.redSpawnerPrefab);
                allPrefabs.Add(spawner.blueSpawnerPrefab);
                allPrefabs.Add(spawner.greenSpawnerPrefab);
                allPrefabs.Add(spawner.orangeSpawnerPrefab);
                allPrefabs.Add(spawner.yellowSpawnerPrefab);
                allPrefabs.Add(spawner.pinkSpawnerPrefab);
                allPrefabs.Add(spawner.purpleSpawnerPrefab);
                allPrefabs.Add(spawner.cyanSpawnerPrefab);
                
                List<int> allChances = new List<int>();
                allChances.Add(spawner.redSpawnerPrefabChances);
                allChances.Add(spawner.blueSpawnerPrefabChances);
                allChances.Add(spawner.greenSpawnerPrefabChances);
                allChances.Add(spawner.orangeSpawnerPrefabChances);
                allChances.Add(spawner.yellowSpawnerPrefabChances);
                allChances.Add(spawner.pinkSpawnerPrefabChances);
                allChances.Add(spawner.purpleSpawnerPrefabChances);
                allChances.Add(spawner.cyanSpawnerPrefabChances);
                
                Entity selectedPrefab = allPrefabs[0];

                for (int i = 0; i < allPrefabs.Count; i++)
                {
                    if (allChances[i] == 0) continue;

                    if (randomValue <= allChances[i])
                    {
                        selectedPrefab = allPrefabs[i];
                        break;
                    }
                }
                
                Entity spawnedPrefab = _ecb.Instantiate(index, selectedPrefab);
                float3 newPos = new float3(localToWorld.Position.x + _random.NextFloat(-1, 1),
                    localToWorld.Position.y + _random.NextFloat(-1, 1), localToWorld.Position.z);

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
        SpawnerJob job = new SpawnerJob(UnityEngine.Time.deltaTime, _bsecbs.CreateCommandBuffer().AsParallelWriter(), new Random((uint) UnityEngine.Random.Range(0, int.MaxValue)));

        JobHandle jobHandle = job.Schedule(this, inputDeps);
        _bsecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

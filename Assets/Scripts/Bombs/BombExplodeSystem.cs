using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class BombExplodeSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = job.Schedule(this, inputDeps);

        return jobHandle;
    }
}

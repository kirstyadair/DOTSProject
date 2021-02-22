using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class InputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (GameManager.Instance.refreshDynamicBuffers)
        {
            GameManager.Instance.refreshDynamicBuffers = false;
            
            Entities
                .WithAll<TokenAuthoringComponent>()
                .WithoutBurst()
                .ForEach((Entity entity) =>
                {
                    EntityManager.GetBuffer<EntityBufferElement>(entity).Clear();
                }).Run();
        }

        return default;
    }
}

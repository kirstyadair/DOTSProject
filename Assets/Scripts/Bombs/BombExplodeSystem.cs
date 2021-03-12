using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class BombExplodeSystem : JobComponentSystem
{
    private EntityManager _entityManager;
    
    protected override void OnCreate()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = Entities
            .WithAny<TokenAuthoringComponent, BombAuthoringComponent>()
            .ForEach((Entity entity) =>
        {
            
        }).Schedule(inputDeps);

        return jobHandle;
    }
}

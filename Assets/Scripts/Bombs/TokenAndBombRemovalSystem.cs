using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class TokenAndBombRemovalSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithAny<BombAuthoringComponent, TokenAuthoringComponent>()
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity) =>
        {
            if (EntityManager.HasComponent<BombAuthoringComponent>(entity))
            {
                if (EntityManager.GetComponentData<BombAuthoringComponent>(entity).hasExploded)
                {
                    EntityManager.DestroyEntity(entity);
                }
            }
            else if (EntityManager.HasComponent<TokenAuthoringComponent>(entity))
            {
                if (EntityManager.GetComponentData<TokenAuthoringComponent>(entity).hitByBomb)
                {
                    GameManager.Instance.attemptMatch = true;
                    GameManager.Instance.tokensToMatch.Add(entity);
                }
            }
        }).Run();

        return default;
    }
}

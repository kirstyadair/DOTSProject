using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class BombMergingSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithAll<BombAuthoringComponent>()
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, in BombAuthoringComponent bac, in LocalToWorld localToWorld) =>
            {
                if (bac.toUpgrade)
                {
                    if (bac.type == BombType.Line)
                    {
                        GameManager.Instance.SpawnBomb(BombType.Cross, localToWorld.Position);
                        EntityManager.DestroyEntity(entity);
                    }
                    else if (bac.type == BombType.Cross)
                    {
                        GameManager.Instance.SpawnBomb(BombType.Area, localToWorld.Position);
                        EntityManager.DestroyEntity(entity);
                    }
                }
            }).Run();

        return default;
    }
}

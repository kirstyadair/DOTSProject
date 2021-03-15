
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class BombExplodeSystem : JobComponentSystem
{
    private static EntityManager _entityManager;

    protected override void OnCreate()
    {
        base.OnCreate();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entity detonatingBomb = Entity.Null;
        BombType typeOfBombToDetonate = BombType.Line;
        LocalToWorld detonatingBombLocalToWorld = new LocalToWorld();

        foreach (Entity e in _entityManager.GetAllEntities())
        {
            if (_entityManager.HasComponent<BombAuthoringComponent>(e))
            {
                BombAuthoringComponent bac = _entityManager.GetComponentData<BombAuthoringComponent>(e);
                if (bac.toExplode && !bac.hasExploded)
                {
                    typeOfBombToDetonate = bac.type;
                    detonatingBomb = e;
                    detonatingBombLocalToWorld = _entityManager.GetComponentData<LocalToWorld>(detonatingBomb);
                    break;
                }
            }
        }

        if (detonatingBomb != Entity.Null)
        {
            JobHandle jobHandle = Entities
                .WithoutBurst()
                .WithAny<TokenAuthoringComponent, BombAuthoringComponent>()
                .ForEach((Entity entity, in LocalToWorld localToWorld) =>
                {
                    BombAuthoringComponent bac;
                    TokenAuthoringComponent tac;

                    if (_entityManager.HasComponent<BombAuthoringComponent>(entity))
                    {
                        bac = _entityManager.GetComponentData<BombAuthoringComponent>(entity);
                        if (entity == detonatingBomb)
                        {
                            bac.hasExploded = true;
                        }

                        if (CheckDistance(localToWorld, typeOfBombToDetonate, detonatingBombLocalToWorld))
                        {
                            bac.toExplode = true;
                        }
                        
                        _entityManager.SetComponentData(entity, bac);
                    }
                    else if (_entityManager.HasComponent<TokenAuthoringComponent>(entity))
                    {
                        tac = _entityManager.GetComponentData<TokenAuthoringComponent>(entity);

                        if (CheckDistance(localToWorld, typeOfBombToDetonate, detonatingBombLocalToWorld))
                        {
                            tac.hitByBomb = true;
                        }
                        
                        _entityManager.SetComponentData(entity, tac);
                    }

                    
                }).Schedule(inputDeps);
            
            return jobHandle;
        }
        
        return default;
    }

    private static bool CheckDistance(LocalToWorld ltw, BombType type, LocalToWorld bombLtw)
    {
        // Find the distance between this entity and the detonated bomb
        float dist = Mathf.Sqrt(((ltw.Position.x - bombLtw.Position.x) * (ltw.Position.x - bombLtw.Position.x)) + ((ltw.Position.y - bombLtw.Position.y) * (ltw.Position.y - bombLtw.Position.y)));

        if (type == BombType.Line && dist <= 1.5f)
        {
            return true;
        }

        if (type == BombType.Cross && dist <= 2f)
        {
            return true;
        }

        if (type == BombType.Area && dist < 3f)
        {
            return true;
        }
        
        // Check if this distance is within the bounds of the bomb type

        return false;
    }
}

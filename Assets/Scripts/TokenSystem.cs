using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class TokenSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        GameManager gm = GameManager.Instance;
        if (gm.attemptMatch)
        {
            if (EntityManager.HasComponent<TokenAuthoringComponent>(gm.hitToken))
            {
                Entities
                .WithAll<TokenAuthoringComponent>()
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, TokenAuthoringComponent tokenAuthoringComponent, LocalToWorld position) =>
                {
                    if (entity == gm.hitToken)
                    {
                        if (!gm.tokensToMatch.Contains(gm.hitToken)) gm.tokensToMatch.Add(gm.hitToken);
                        
                        foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                        {
                            if (tokenAuthoringComponent.colour != gm.hitTokenColour) continue;
                            
                            if (!gm.tokensToMatch.Contains(e.Value))
                            {
                                gm.tokensToMatch.Add(e.Value);
                            }
                        }
                    }
                    else
                    {
                        bool ignoreThisEntity = true;
                        
                        foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                        {
                            if (tokenAuthoringComponent.colour != gm.hitTokenColour) continue;
                            
                            if (gm.tokensToMatch.Contains(e.Value))
                            {
                                ignoreThisEntity = false;
                            }
                        }

                        if (!ignoreThisEntity)
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (tokenAuthoringComponent.colour != gm.hitTokenColour) continue;
                            
                                if (!gm.tokensToMatch.Contains(e.Value))
                                {
                                    gm.tokensToMatch.Add(e.Value);
                                }
                            }
                        }
                    }
                }).Run();   
            }
            else if (EntityManager.HasComponent<BombAuthoringComponent>(gm.hitToken))
            {
                BombAuthoringComponent bombAC = EntityManager.GetComponentData<BombAuthoringComponent>(gm.hitToken);
                if (bombAC.type == BombType.Line)
                {
                    Debug.Log(Physics.OverlapBox(EntityManager.GetComponentData<LocalToWorld>(gm.hitToken).Position,
                        new Vector2(2, 1)).Length);
                }
                else if (bombAC.type == BombType.Cross)
                {
                    
                }
                else
                {
                    
                }
            }
        }
        else
        {
            if (gm.tokensToMatch.Count > 1)
            {
                gm.AddToSpawners(gm.tokensToMatch.Count);
                
                foreach (Entity e in gm.tokensToMatch)
                {
                    if (e != Entity.Null)
                    {
                        if (EntityManager.HasComponent<TokenAuthoringComponent>(e))
                        {
                            if (EntityManager.GetComponentData<TokenAuthoringComponent>(e)
                                .colour == gm.objectiveColour) gm.objectiveAmount--;

                            if (gm.tokensToMatch.Count >= 4 && e == gm.hitToken)
                            {
                                gm.SpawnBomb(BombType.Line, EntityManager.GetComponentData<LocalToWorld>(e).Position);
                            }
                            EntityManager.DestroyEntity(e);
                        }
                        else if (EntityManager.HasComponent<BombAuthoringComponent>(e))
                        {
                            BombAuthoringComponent bac = EntityManager.GetComponentData<BombAuthoringComponent>(e);
                            bac.toExplode = true;
                            EntityManager.AddComponentData(e, bac);
                        }
                        
                    }
                }

            }
            
            gm.tokensToMatch.Clear();
        }

        return default;
    }
}

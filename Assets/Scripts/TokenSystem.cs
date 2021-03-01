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
        if (GameManager.Instance.attemptMatch && GameManager.Instance.canAttemptNextMatch)
        {
            GameManager.Instance.canAttemptNextMatch = false;
            
            Entities
                .WithAll<TokenAuthoringComponent>()
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, TokenAuthoringComponent tokenAuthoringComponent, LocalToWorld position) =>
                {
                    if (entity == GameManager.Instance.hitToken)
                    {
                        GameManager.Instance.tokensToMatch.Add(GameManager.Instance.hitToken);
                        
                        foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                        {
                            if (tokenAuthoringComponent.colour != GameManager.Instance.hitTokenColour) continue;
                            
                            if (!GameManager.Instance.tokensToMatch.Contains(e.Value))
                            {
                                GameManager.Instance.tokensToMatch.Add(e.Value);
                                tokenAuthoringComponent.beingRemoved = true;
                            }
                        }
                    }
                    else
                    {
                        bool ignoreThisEntity = true;
                        
                        foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                        {
                            if (tokenAuthoringComponent.colour != GameManager.Instance.hitTokenColour) continue;
                            
                            if (GameManager.Instance.tokensToMatch.Contains(e.Value))
                            {
                                ignoreThisEntity = false;
                            }
                        }

                        if (!ignoreThisEntity)
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (tokenAuthoringComponent.colour != GameManager.Instance.hitTokenColour) continue;
                            
                                if (!GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                        }
                    }
                }).Run();
        }
        else
        {
            if (GameManager.Instance.tokensToMatch.Count > 1)
            {
                bool objective = false;
                if (EntityManager.GetComponentData<TokenAuthoringComponent>(GameManager.Instance.tokensToMatch[0])
                    .colour == GameManager.Instance.objectiveColour)
                {
                    objective = true;
                }
                
                GameManager.Instance.AddToSpawners(GameManager.Instance.tokensToMatch.Count);
                
                foreach (Entity e in GameManager.Instance.tokensToMatch)
                {
                    if (e != Entity.Null)
                    {
                        if (objective) GameManager.Instance.objectiveAmount--;
                        EntityManager.DestroyEntity(e);
                    }
                }

            }
            
            GameManager.Instance.tokensToMatch.Clear();
        }

        return default;
    }
}

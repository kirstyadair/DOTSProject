using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class TokenSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (GameManager.Instance.attemptMatch)
        {
            Entities
                .WithAll<TokenAuthoringComponent>()
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, TokenAuthoringComponent tokenAuthoringComponent) =>
                {
                    switch (tokenAuthoringComponent.colour)
                    {
                        case TokenColours.Red:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Red>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Blue:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Blue>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Green:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Green>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Orange:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Orange>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Yellow:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Yellow>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Pink:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Pink>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Purple:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Purple>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Cyan:
                        {
                            foreach (Entity e in EntityManager.GetComponentData<Cyan>(entity).touchingMatchingTokens)
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }

                    foreach (Entity e in GameManager.Instance.tokensToMatch)
                    {
                        if (e != Entity.Null)
                        {
                            EntityManager.DestroyEntity(e);
                        }
                       
                    }
                }).Run();
        }

        return default;
    }
}

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
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Blue:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Green:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Orange:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Yellow:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Pink:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Purple:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
                                    tokenAuthoringComponent.beingRemoved = true;
                                }
                            }
                            break;
                        }
                        case TokenColours.Cyan:
                        {
                            foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                            {
                                if (!tokenAuthoringComponent.beingRemoved && !GameManager.Instance.tokensToMatch.Contains(e.Value))
                                {
                                    GameManager.Instance.tokensToMatch.Add(e.Value);
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

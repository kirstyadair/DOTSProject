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
        if (GameManager.Instance.attemptMatch && GameManager.Instance.canAttemptNextMatch)
        {
            GameManager.Instance.canAttemptNextMatch = false;
            
            Entities
                .WithAll<TokenAuthoringComponent>()
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, TokenAuthoringComponent tokenAuthoringComponent) =>
                {
                    foreach (EntityBufferElement e in EntityManager.GetBuffer<EntityBufferElement>(entity))
                    {
                        if (tokenAuthoringComponent.colour != GameManager.Instance.hitTokenColour) continue;
                            
                        if (!GameManager.Instance.tokensToMatch.Contains(e.Value))
                        {
                            //Debug.Log("Colour: " + tokenAuthoringComponent.colour);
                            GameManager.Instance.tokensToMatch.Add(e.Value);
                            tokenAuthoringComponent.beingRemoved = true;
                        }
                    }
                }).Run();
        }
        else
        {
            if (GameManager.Instance.tokensToMatch.Count > 0)
            {
                Debug.Log(GameManager.Instance.tokensToMatch.Count);
                
                foreach (Entity e in GameManager.Instance.tokensToMatch)
                {
                    if (e != Entity.Null)
                    {
                        EntityManager.DestroyEntity(e);
                    }
                }

                GameManager.Instance.tokensToMatch.Clear();
            }
        }

        return default;
    }
}

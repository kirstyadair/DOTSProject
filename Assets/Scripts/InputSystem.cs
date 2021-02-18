using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

/*[AlwaysSynchronizeSystem]
public class InputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (Input.GetKey(KeyCode.A))
        {
            Entities
                .WithAll<TokenAuthoringComponent>()
                .WithoutBurst()
                .ForEach((Entity entity, TokenAuthoringComponent tokenAuthoringComponent) =>
                {
                    if (tokenAuthoringComponent.colour == GameManager.Instance.colourToMatch && !tokenAuthoringComponent.beingRemoved)
                    {
                        GameManager.Instance.tokensToMatch.Add(entity);
                        tokenAuthoringComponent.beingRemoved = true;
                    
                        Debug.Log("Removing " + entity + ", of colour " + tokenAuthoringComponent.colour);
                    }
                }).Run();
        }

        return default;
    }
}*/

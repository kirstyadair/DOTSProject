using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.UIElements;
using RaycastHit = Unity.Physics.RaycastHit;

// From https://www.youtube.com/watch?v=B3SFWm9gkL8
public class RaycastScript : MonoBehaviour
{
    private Entity Raycast(float3 from, float3 to)
    {
        BuildPhysicsWorld buildPhysicsWorld =
            World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

        RaycastInput raycastInput = new RaycastInput()
        {
            Start = from,
            End = to,
            Filter = new CollisionFilter()
            {
                BelongsTo = 1,
                CollidesWith = 1
            }
        };
        
        RaycastHit raycastHit = new RaycastHit();

        if (collisionWorld.CastRay(raycastInput, out raycastHit))
        {
            Entity hit = buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hit;
        }
        else
        {
            return Entity.Null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameManager.Instance.hitToken = Raycast(ray.origin, ray.direction * 10000f);
            Debug.Log(GameManager.Instance.hitToken);
        }
    }
}

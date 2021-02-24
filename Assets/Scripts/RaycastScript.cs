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
    private EntityManager _entityManager;
    private float _timeBetweenRefreshingTokens = 0.5f;
    private float _timeToNextRefresh = 0;
    
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

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        _timeToNextRefresh -= Time.deltaTime;
        GameManager.Instance.refreshDynamicBuffers = false;
        
        if (_timeToNextRefresh <= 0)
        {
            _timeToNextRefresh = _timeBetweenRefreshingTokens;
            GameManager.Instance.refreshDynamicBuffers = true;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.tokenDistances.Clear();
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameManager.Instance.hitToken = Raycast(ray.origin, ray.direction * 10000f);

            if (GameManager.Instance.hitToken != null)
            {
                GameManager.Instance.hitTokenColour = _entityManager.GetComponentData<TokenAuthoringComponent>(GameManager.Instance.hitToken).colour;
            }
            
            GameManager.Instance.attemptMatch = true;
            GameManager.Instance.canAttemptNextMatch = true;
            Entity e = GameManager.Instance.hitToken;
        }

        if (Input.GetMouseButtonUp(0))
        {
            GameManager.Instance.attemptMatch = false;
            GameManager.Instance.canAttemptNextMatch = false;
            GameManager.Instance.hitTokenColour = TokenColours.Null;
        }
    }
}

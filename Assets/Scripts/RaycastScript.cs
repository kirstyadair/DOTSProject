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
    private GameManager _gameManager;
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
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        _timeToNextRefresh -= Time.deltaTime;
        _gameManager.refreshDynamicBuffers = false;
        
        if (_timeToNextRefresh <= 0)
        {
            _timeToNextRefresh = _timeBetweenRefreshingTokens;
            _gameManager.refreshDynamicBuffers = true;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_gameManager.movesAllowed <= 0) return;
            
            _gameManager.tokenDistances.Clear();
            _gameManager.movesAllowed--;
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            _gameManager.hitToken = Raycast(ray.origin, ray.direction * 10000f);

            if (_gameManager.hitToken != null)
            {
                _gameManager.hitTokenColour = _entityManager.GetComponentData<TokenAuthoringComponent>(_gameManager.hitToken).colour;
            }
            
            _gameManager.attemptMatch = true;
            _gameManager.canAttemptNextMatch = true;
            Entity e = _gameManager.hitToken;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _gameManager.attemptMatch = false;
            _gameManager.canAttemptNextMatch = false;
            _gameManager.hitTokenColour = TokenColours.Null;
        }
    }
}

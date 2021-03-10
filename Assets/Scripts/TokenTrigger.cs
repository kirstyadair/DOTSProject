using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using CollisionWorld = Unity.U2D.Entities.Physics.CollisionWorld;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class TokenTrigger : JobComponentSystem
{
    public struct TokenTriggerJob : ICollisionEventsJob
    {
        public ComponentDataFromEntity<Red> red;
        public ComponentDataFromEntity<Blue> blue;
        public ComponentDataFromEntity<Green> green;
        public ComponentDataFromEntity<Orange> orange;
        public ComponentDataFromEntity<Yellow> yellow;
        public ComponentDataFromEntity<Pink> pink;
        public ComponentDataFromEntity<Purple> purple;
        public ComponentDataFromEntity<Cyan> cyan;
        public ComponentDataFromEntity<LineBomb> lineBomb;
        public ComponentDataFromEntity<CrossBomb> crossBomb;
        public EntityCommandBuffer ecb;
        
        public void Execute(CollisionEvent triggerEvent)
        {
            Entity a = triggerEvent.Entities.EntityA;
            Entity b = triggerEvent.Entities.EntityB;

            if (!_entityManager.HasComponent<EntityBufferElement>(a) || !_entityManager.HasComponent<EntityBufferElement>(b))
            {
                if (_entityManager.HasComponent<BombAuthoringComponent>(a) && _entityManager.HasComponent<BombAuthoringComponent>(b))
                {
                    BombAuthoringComponent aBAC = _entityManager.GetComponentData<BombAuthoringComponent>(a);
                    BombAuthoringComponent bBAC = _entityManager.GetComponentData<BombAuthoringComponent>(b);

                    if (aBAC.type == bBAC.type && !aBAC.toUpgrade && !bBAC.toUpgrade)
                    {
                        ecb.DestroyEntity(a);
                        bBAC.toUpgrade = true;
                        _entityManager.AddComponentData(b, bBAC);
                    }
                    
                }
                return;
            }

            DynamicBuffer<EntityBufferElement> aBuffer = _entityManager.GetBuffer<EntityBufferElement>(a);
            DynamicBuffer<EntityBufferElement> bBuffer = _entityManager.GetBuffer<EntityBufferElement>(b);

            if (red.HasComponent(a) && red.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (blue.HasComponent(a) && blue.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (green.HasComponent(a) && green.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (orange.HasComponent(a) && orange.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (yellow.HasComponent(a) && yellow.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (pink.HasComponent(a) && pink.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (purple.HasComponent(a) && purple.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;
                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
            else if (cyan.HasComponent(a) && cyan.HasComponent(b))
            {
                if (CheckForExistingEntity(b, aBuffer) || CheckForExistingEntity(a, bBuffer)) return;

                bBuffer.Add(new EntityBufferElement {Value = a});
                aBuffer.Add(new EntityBufferElement {Value = b});
            }
        }
    }

    [ReadOnly] private BuildPhysicsWorld _buildPhysicsWorld;
    [ReadOnly] private StepPhysicsWorld _stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem _ecb;
    private static EntityManager _entityManager;

    protected override void OnCreate()
    {
        base.OnCreate();
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        _ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var triggerJob = new TokenTriggerJob();
        triggerJob.red = GetComponentDataFromEntity<Red>();
        triggerJob.blue = GetComponentDataFromEntity<Blue>();
        triggerJob.green = GetComponentDataFromEntity<Green>();
        triggerJob.orange = GetComponentDataFromEntity<Orange>();
        triggerJob.yellow = GetComponentDataFromEntity<Yellow>();
        triggerJob.pink = GetComponentDataFromEntity<Pink>();
        triggerJob.purple = GetComponentDataFromEntity<Purple>();
        triggerJob.cyan = GetComponentDataFromEntity<Cyan>();
        triggerJob.lineBomb = GetComponentDataFromEntity<LineBomb>();
        triggerJob.crossBomb = GetComponentDataFromEntity<CrossBomb>();
        
        EntityCommandBuffer newECB = _ecb.CreateCommandBuffer();
        triggerJob.ecb = newECB;

        JobHandle job = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
        
        _ecb.AddJobHandleForProducer(job);
        job.Complete();
        return job;
    }

    static private bool CheckForExistingEntity(Entity entity, DynamicBuffer<EntityBufferElement> dynamicBuffer)
    {
        bool contains = false;

        for (int i = 0; i < dynamicBuffer.Length; i++)
        {
            if (dynamicBuffer[i].Value == entity) contains = true;
        }
        
        return contains;
    }
}


using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class TokenTrigger : JobComponentSystem
{
    public struct TokenTriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<Red> red;
        public ComponentDataFromEntity<Blue> blue;
        public ComponentDataFromEntity<Green> green;
        public ComponentDataFromEntity<Orange> orange;
        public ComponentDataFromEntity<Yellow> yellow;
        public ComponentDataFromEntity<Pink> pink;
        public ComponentDataFromEntity<Purple> purple;
        public ComponentDataFromEntity<Cyan> cyan;
            
        public void Execute(TriggerEvent triggerEvent)
        {
            
            Entity a = triggerEvent.Entities.EntityA;
            Entity b = triggerEvent.Entities.EntityB;

            if (red.HasComponent(a) && red.HasComponent(b))
            {
                if (red[b].touchingMatchingTokens.Contains(a) || red[a].touchingMatchingTokens.Contains(b)) return;
                red[b].touchingMatchingTokens.Add(a);
                red[a].touchingMatchingTokens.Add(b);
            }
            else if (blue.HasComponent(a) && blue.HasComponent(b))
            {
                if (blue[b].touchingMatchingTokens.Contains(a) || blue[a].touchingMatchingTokens.Contains(b)) return;
                blue[b].touchingMatchingTokens.Add(a);
                blue[a].touchingMatchingTokens.Add(b);
            }
            else if (green.HasComponent(a) && green.HasComponent(b))
            {
                if (green[b].touchingMatchingTokens.Contains(a) || green[a].touchingMatchingTokens.Contains(b)) return;
                green[b].touchingMatchingTokens.Add(a);
                green[a].touchingMatchingTokens.Add(b);
            }
            else if (orange.HasComponent(a) && orange.HasComponent(b))
            {
                if (orange[b].touchingMatchingTokens.Contains(a) || orange[a].touchingMatchingTokens.Contains(b)) return;
                orange[b].touchingMatchingTokens.Add(a);
                orange[a].touchingMatchingTokens.Add(b);
            }
            else if (yellow.HasComponent(a) && yellow.HasComponent(b))
            {
                if (yellow[b].touchingMatchingTokens.Contains(a) || yellow[a].touchingMatchingTokens.Contains(b)) return;
                yellow[b].touchingMatchingTokens.Add(a);
                yellow[a].touchingMatchingTokens.Add(b);
            }
            else if (pink.HasComponent(a) && pink.HasComponent(b))
            {
                if (pink[b].touchingMatchingTokens.Contains(a) || pink[a].touchingMatchingTokens.Contains(b)) return;
                pink[b].touchingMatchingTokens.Add(a);
                pink[a].touchingMatchingTokens.Add(b);
            }
            else if (purple.HasComponent(a) && purple.HasComponent(b))
            {
                if (purple[b].touchingMatchingTokens.Contains(a) || purple[a].touchingMatchingTokens.Contains(b)) return;
                purple[b].touchingMatchingTokens.Add(a);
                purple[a].touchingMatchingTokens.Add(b);
            }
            else if (cyan.HasComponent(a) && cyan.HasComponent(b))
            {
                if (cyan[b].touchingMatchingTokens.Contains(a) || cyan[a].touchingMatchingTokens.Contains(b)) return;
                cyan[b].touchingMatchingTokens.Add(a);
                cyan[a].touchingMatchingTokens.Add(b);
            }
        }
    }

    private BuildPhysicsWorld _buildPhysicsWorld;
    private StepPhysicsWorld _stepPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
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

        JobHandle job = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
        job.Complete();
        return job;
    }
}


using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Pink : IComponentData
{
    public List<Entity> touchingMatchingTokens;
}

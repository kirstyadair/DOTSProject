using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Orange : IComponentData
{
    public List<Entity> touchingMatchingTokens;
}

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct TokenAuthoringComponent : IComponentData
{
    public TokenColours colour;
    public bool hitByBomb;
}

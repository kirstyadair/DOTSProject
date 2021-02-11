using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public class TokenAuthoringComponent : IComponentData
{
    public TokenColours colour;
    public bool beingRemoved = false;
}

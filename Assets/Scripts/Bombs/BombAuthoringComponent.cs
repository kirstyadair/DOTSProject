using Unity.Entities;

[GenerateAuthoringComponent]
public struct BombAuthoringComponent : IComponentData
{
    public BombType type;
    public bool toUpgrade;
    public bool toExplode;
}

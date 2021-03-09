using Unity.Entities;

[GenerateAuthoringComponent]
public struct BombAuthoringComponent : IComponentData
{
    public BombType type;
    public bool merging;
    public bool toExplode;
}

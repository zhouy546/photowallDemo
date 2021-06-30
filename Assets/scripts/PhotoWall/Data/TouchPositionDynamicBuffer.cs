using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
[InternalBufferCapacity(3)]
public struct TouchPositionDynamicBuffer : IBufferElementData
{
    public float3 TouchPosition;
}

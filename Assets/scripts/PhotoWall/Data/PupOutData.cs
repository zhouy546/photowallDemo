using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PupOutData : IComponentData
{
    public float timeOffset;
    public float speed;
    public bool isBlack;
    public float TargetzOffset;
    public float currentzOffset;

    public float TargetScaleFactor;
    public float currentScaleFactor;

}

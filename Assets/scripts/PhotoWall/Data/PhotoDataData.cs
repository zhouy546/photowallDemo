using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PhotoDataData : IComponentData
{
    public int id;

    public float rito;

    public int Y_index;
    public int X_index;

    public float XOffset;
    public float YOffset;
    public float defaultXOffset;
    public float defaultYOffset;
    public float DefaultY;
    public float DefaultX;

    //public bool 
}

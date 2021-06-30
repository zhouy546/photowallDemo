using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace dotsTest
{
    [GenerateAuthoringComponent]
    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }
}
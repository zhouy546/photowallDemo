using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

    [GenerateAuthoringComponent]
    public struct MoveSpeed : IComponentData
    {
        public float Value;
        public float Red;
        public float xdis;
      
        public float Ydir;

        public float Yoffset;
    public float Xoffset;


    public float ypos;
    public float xpos;
}

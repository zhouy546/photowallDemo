using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SyncTouchPositionSystem :ComponentSystem
{
    protected override void OnUpdate()
    {

        float3[] float3s = photowall_spawner.instance.valueArrays;


        if (photowall_spawner.instance.isStartMoveSystem)
        {
            Entities.WithAll<PicQuadTag>().ForEach((DynamicBuffer<TouchPositionDynamicBuffer> buffers) =>
            {
                buffers.Clear();

                for (int i = 0; i < float3s.Length; i++)
                {
                    buffers.Add(new TouchPositionDynamicBuffer()
                    {
                        TouchPosition = float3s[i]
                    });
                }
            });
        }
    }
}

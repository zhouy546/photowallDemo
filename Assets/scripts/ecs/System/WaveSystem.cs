using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;
namespace dotsTest
{
    [BurstCompile]
    public class WaveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float elapsedTime = (float)Time.ElapsedTime;

            Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) =>
           {
               float zposition = waveData.amplitude * math.sin((float)elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
               trans.Value = new float3(trans.Value.x, trans.Value.y, zposition);
           }).ScheduleParallel();
        }
    }



    //public class WaveSystem : JobComponentSystem
    //{
    //    protected override JobHandle OnUpdate(JobHandle inputDeps)
    //    {
    //        float elapsedTime = (float)Time.ElapsedTime;

    //        JobHandle jobHandle = Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) =>
    //       {
    //           float zposition = waveData.amplitude * math.sin((float)elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
    //           trans.Value = new float3(trans.Value.x, trans.Value.y, zposition);
    //       }).Schedule(inputDeps);

    //        return jobHandle;
    //    }
    //}


    //public class WaveSystem : SystemBase
    //{
    //    protected override void OnUpdate()
    //    {
    //        float elapsedTime = (float)Time.ElapsedTime;

    //         Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) =>
    //        {
    //            float zposition = waveData.amplitude * math.sin((float)elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
    //            trans.Value = new float3(trans.Value.x, trans.Value.y, zposition);
    //        }).Schedule();

    //        Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) =>
    //        {
    //            float zposition = waveData.amplitude * math.sin((float)elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
    //            trans.Value = new float3(trans.Value.x, trans.Value.y, zposition);
    //        }).Schedule();

    //        Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) =>
    //        {
    //            float zposition = waveData.amplitude * math.sin((float)elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
    //            trans.Value = new float3(trans.Value.x, trans.Value.y, zposition);
    //        }).Schedule();

    //    }
    //}
}


using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PupOutSystem : SystemBase
{

    public bool temp;
    public int randomFloat;
    protected override void OnUpdate()
    {
        if (temp != photowall_spawner.instance.isTurnOnPupOutSystem)
        {
            temp = photowall_spawner.instance.isTurnOnPupOutSystem;
            if (!temp)
            {
                Entities.WithoutBurst().WithAll<PicQuadTag>().ForEach(( RenderMesh rendermesh,ref PupOutData pupOutData) =>
                {
                    randomFloat = Mathf.FloorToInt(UnityEngine.Random.Range(0, 1.1f));
                    rendermesh.material.SetFloat("_dir", randomFloat);

                    pupOutData.isBlack = randomFloat == 1 ? false : true;

                }).Run();
            }
        }


        if (photowall_spawner.instance.isTurnOnPupOutSystem)
        {

            Entities.WithAll<PicQuadTag>().ForEach((ref Translation trans, ref NonUniformScale nonUniformScale, ref PupOutData pupOutData, ref PhotoDataData photoDataData) =>
            {

                if (!pupOutData.isBlack/*&&photoDataData.YOffset == 0*/)
                {
                    //…Ë÷√¥Û–°  
                    pupOutData.currentScaleFactor = pupOutData.currentScaleFactor + (pupOutData.TargetScaleFactor - pupOutData.currentScaleFactor) * 0.05f;
                    nonUniformScale.Value = new float3(photoDataData.rito * pupOutData.currentScaleFactor, 1 * pupOutData.currentScaleFactor, 1);
                }

            }).ScheduleParallel();
        }
        else
        {
            Entities.WithAll<PicQuadTag>().ForEach((ref Translation trans, ref NonUniformScale nonUniformScale, ref PupOutData pupOutData, ref PhotoDataData photoDataData) =>
            {

                //if (!pupOutData.isBlack/* && photoDataData.YOffset == 0*/)
                //{

                pupOutData.currentScaleFactor = pupOutData.currentScaleFactor + (1 /**MyUtility.Utility.Maping (photoDataData.YOffset,0f,20f,1,0,true)*/ - pupOutData.currentScaleFactor) * 0.05f;


                nonUniformScale.Value = new float3(photoDataData.rito * pupOutData.currentScaleFactor, 1 * pupOutData.currentScaleFactor, 1);
                //}

            }).ScheduleParallel();
        }





    }


}

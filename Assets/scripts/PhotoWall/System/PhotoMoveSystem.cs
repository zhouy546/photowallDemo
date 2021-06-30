using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;
using System;
using Unity.Collections;

[BurstCompile]
public class PhotoMoveSystem : SystemBase
{
    //-----------------------移动到最左边 事件------------------------------
    public event EventHandler OnPicMoveToLeft;

    private DOTSEvents_NextFrame<PicMoveToLeftEvemt> dotsevent;

    public struct PicMoveToLeftEvemt : IComponentData
    {
        //public double ElapsedTime;
    }

    public event EventHandler OnPicMoveToRight;

    private DOTSEvents_NextFrame<PicMoveToRightEvemt> dotsRightevent;

    public struct PicMoveToRightEvemt : IComponentData
    {
        //public double ElapsedTime;
    }


    //-----------------------移动到最左边 事件END------------------------------


    protected override void OnCreate()
    {
        dotsevent = new DOTSEvents_NextFrame<PicMoveToLeftEvemt>(World);
        dotsRightevent = new DOTSEvents_NextFrame<PicMoveToRightEvemt>(World);
    }


    protected override void OnStartRunning()
    {
        Debug.Log("OnSystemStart");
    }

    protected override void OnUpdate()
    {

        //Debug.Log(photowall_spawner.instance.isStartMoveSystem);

        if (photowall_spawner.instance.isStartMoveSystem)
        {
            float deltaTime = (float)Time.DeltaTime;

            DOTSEvents_NextFrame<PicMoveToLeftEvemt>.EventTrigger eventTrigger = dotsevent.GetEventTrigger();

            DOTSEvents_NextFrame<PicMoveToRightEvemt>.EventTrigger RighteventTrigger = dotsRightevent.GetEventTrigger();

            double elapsedTime = Time.ElapsedTime;

            float movedir = ValueSheet.moveDir;

            bool isTurnOnPupOutSystem = photowall_spawner.instance.isTurnOnPupOutSystem;



        Entities.WithAll<PicQuadTag>().ForEach((int entityInQueryIndex,ref DynamicBuffer<TouchPositionDynamicBuffer> touchBuffer, ref Translation trans, ref MoveSpeed moveSpeed, ref PhotoDataData photoDataData, ref PupOutData pupOutData) =>
            {

                ////设置图片弹出Z轴移动

                //if (!pupOutData.isBlack)
                //{
                //    pupOutData.currentzOffset = pupOutData.currentzOffset + (pupOutData.TargetzOffset - pupOutData.currentzOffset) * 0.05f;
                //}
                //else
                //{
                //    pupOutData.currentzOffset = pupOutData.currentzOffset + (0 - pupOutData.currentzOffset) * 0.05f;
                //}


                //X轴移动
                photoDataData.DefaultX = photoDataData.DefaultX + movedir * deltaTime * moveSpeed.Value;

                //y轴移动



                moveSpeed.Yoffset = 0;
                moveSpeed.Xoffset = 0;
                foreach (var item in touchBuffer)
                {
                    //float Ox1 = item.TouchPosition.x;
                    //float Oy1 = item.TouchPosition.y;
                    //float Ax2 = photoDataData.DefaultX;
                    //float Ay2  = photoDataData.DefaultY;
                    //float OA = math.sqrt((photoDataData.DefaultX - item.TouchPosition.x) * (photoDataData.DefaultX - item.TouchPosition.x) + (photoDataData.DefaultY - item.TouchPosition.y) * (Ay2 - item.TouchPosition.y));

                    moveSpeed.xdis = math.sqrt((photoDataData.DefaultX - item.TouchPosition.x) * (photoDataData.DefaultX - item.TouchPosition.x) + (photoDataData.DefaultY - item.TouchPosition.y) * (photoDataData.DefaultY - item.TouchPosition.y));

                    //   moveSpeed.Red = item.TouchPosition.z



                    if (moveSpeed.xdis - item.TouchPosition.z < 0)
                    {

                        moveSpeed.Yoffset = moveSpeed.Yoffset + (photoDataData.DefaultY - item.TouchPosition.y) / math.sqrt((photoDataData.DefaultX - item.TouchPosition.x) * (photoDataData.DefaultX - item.TouchPosition.x) + (photoDataData.DefaultY - item.TouchPosition.y) * (photoDataData.DefaultY - item.TouchPosition.y)) * moveSpeed.Red + item.TouchPosition.y;
                        moveSpeed.Yoffset = moveSpeed.Yoffset - photoDataData.DefaultY;

                        moveSpeed.Xoffset = moveSpeed.Xoffset + (photoDataData.DefaultX - item.TouchPosition.x) / math.sqrt((photoDataData.DefaultX - item.TouchPosition.x) * (photoDataData.DefaultX - item.TouchPosition.x) + (photoDataData.DefaultY - item.TouchPosition.y) * (photoDataData.DefaultY - item.TouchPosition.y)) * moveSpeed.Red + item.TouchPosition.x;
                        moveSpeed.Xoffset = moveSpeed.Xoffset - photoDataData.DefaultX;
                    }
                    else
                    {


                    }
                }
       
                moveSpeed.ypos = moveSpeed.ypos+ ((photoDataData.DefaultY + moveSpeed.Yoffset)-moveSpeed.ypos )*0.05F;
                moveSpeed.xpos = moveSpeed.xpos + ((photoDataData.DefaultX + moveSpeed.Xoffset)-moveSpeed.xpos)*0.05F;
                //设置位置
                trans.Value = new float3(moveSpeed.xpos, moveSpeed.ypos, 0);


            }).ScheduleParallel();


            //改变方向
            Entities.WithAll<PicQuadTag>().ForEach((int entityInQueryIndex, ref Translation trans, ref MoveSpeed moveSpeed) =>
            {
                float xpos = trans.Value.x;

                if (xpos <= -25)
                {
                    eventTrigger.TriggerEvent(entityInQueryIndex, new PicMoveToLeftEvemt { });
                }else if(xpos>=25){
                    RighteventTrigger.TriggerEvent(entityInQueryIndex, new PicMoveToRightEvemt { });
                }

            }).ScheduleParallel();

            dotsevent.CaptureEvents(Dependency, (PicMoveToLeftEvemt basicEvent) =>
            {
                OnPicMoveToLeft?.Invoke(this, EventArgs.Empty);
            });

            dotsRightevent.CaptureEvents(Dependency, (PicMoveToRightEvemt basicEvent) =>
            {
                OnPicMoveToRight?.Invoke(this, EventArgs.Empty);
            });

        }
        
    }

       
}

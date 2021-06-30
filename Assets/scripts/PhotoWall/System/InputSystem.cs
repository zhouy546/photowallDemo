using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InputSystem : ComponentSystem
{

    private float3 startPosition;

    protected override void OnUpdate()
    {
        if (!ValueSheet.OnDrag)
        {

            if (Input.GetMouseButtonDown(0))
            {
                photowall_spawner.instance.selectionAreaTransform.gameObject.SetActive(true);
                startPosition = UtilsClass.GetMouseWorldPosition();
                photowall_spawner.instance.selectionAreaTransform.position = startPosition;

            }

            if (Input.GetMouseButton(0))
            {

                float3 selectionAreaSize = (float3)UtilsClass.GetMouseWorldPosition() - startPosition;

                photowall_spawner.instance.selectionAreaTransform.localScale = selectionAreaSize;

            }

            if (Input.GetMouseButtonUp(0))
            {


                photowall_spawner.instance.selectionAreaTransform.gameObject.SetActive(false);

                float3 endPosition = UtilsClass.GetMouseWorldPosition();
                float3 lowerLeftPosition = new float3(math.min(startPosition.x, endPosition.x), math.min(startPosition.y, endPosition.y), 0);
                float3 upperRightPosition = new float3(math.max(startPosition.x, endPosition.x), math.max(startPosition.y, endPosition.y), 0);

                float selectionAreaMinSize = 1;
                float selectionAreaSize = math.distance(lowerLeftPosition, upperRightPosition);
                if (selectionAreaSize < selectionAreaMinSize)
                {
                    lowerLeftPosition += new float3(-1.2f, -1, 0) * (selectionAreaMinSize - selectionAreaSize) * .5f;
                    upperRightPosition += new float3(1.2f, 1, 0) * (selectionAreaMinSize - selectionAreaSize) * .5f;

                }

                //deselect all selected entities
                Entities.WithAll<UnitSelected, PicQuadTag>().ForEach((Entity entity) => {

                    PostUpdateCommands.RemoveComponent<UnitSelected>(entity);
                });

                Entities.WithAll<PicQuadTag>().ForEach((Entity entity, ref Translation translation, ref PhotoDataData _photoData) => {
                    float3 entityPosition = translation.Value;
                    if (entityPosition.x >= lowerLeftPosition.x &&
                    entityPosition.y >= lowerLeftPosition.y &&
                    entityPosition.x <= upperRightPosition.x &&
                    entityPosition.y <= upperRightPosition.y)
                    {
                        PostUpdateCommands.AddComponent(entity, new UnitSelected());

                        int tempIndex = getMinXDisIndex(photowall_spawner.instance.valueArrays, entityPosition.x);

                        Debug.Log("第几个点位：" + tempIndex);

                        EventCenter.Broadcast(EventDefine.Onclick, _photoData.id, tempIndex);
                        //Debug.Log(_photoData.id);
                    }
                });
            }
        }


    }

    int getMinXDisIndex(float3[] TargetPos,float transX)
    {
        Dictionary<int, float> temp = new Dictionary<int, float>();

        for (int i = 0; i < TargetPos.Length; i++)
        {
            float dis = Mathf.Abs(TargetPos[i].x - transX);
                temp.Add(i, dis);
        }

        float tempval = 1000;
        int tempKey = 0;

        foreach (var item in temp)
        {
            if (tempval > item.Value)
            {
                tempval = item.Value;
                tempKey = item.Key;
            }
        }
        temp.Clear();

        return tempKey;
    }
}

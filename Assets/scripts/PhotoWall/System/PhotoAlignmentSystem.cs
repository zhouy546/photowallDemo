using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PhotoAlignmentSystem : SystemBase
{


    protected override void OnUpdate()
    {
        if (photowall_spawner.instance.isStartAlignmentSystem)
        {


            float spaceingX = ValueSheet.spacingX;

            float spaceingY = ValueSheet.spacingY;

            float offsetX = ValueSheet.Xoffset;

            float offsetY = ValueSheet.Yoffset;

            float xvalue = 0;

            float tempspacing=0;

            int ySize = ValueSheet.ySize;

            int xSize = ValueSheet.xSize;

            int index=0;

            int yindex = -1;

            Entities.WithAll<PicQuadTag>().ForEach((ref Translation trans, ref PhotoDataData photoDataData) =>
            {
                if (index % xSize == 0)
                {
                    index = 0;
                    xvalue = 0;
                    tempspacing = 0;
                    yindex++;
                }

                if (index != 0)
                {
                    xvalue += photoDataData.rito / 2 + spaceingX;
                    tempspacing += spaceingX;
                }

                photoDataData.XOffset = xvalue;

                xvalue = xvalue + photoDataData.rito / 2;

                photoDataData.X_index = index;

                photoDataData.Y_index = yindex;

                photoDataData.DefaultY = yindex * spaceingY + offsetY;

                photoDataData.DefaultX = photoDataData.XOffset + offsetX;


                photoDataData.defaultYOffset = yindex * spaceingY;

                photoDataData.defaultXOffset = tempspacing;

                trans.Value = new float3(photoDataData.XOffset+ offsetX, yindex * spaceingY+ offsetY, trans.Value.z);

                index++;
            }).Run();
             //
            //   
        }
    }


}

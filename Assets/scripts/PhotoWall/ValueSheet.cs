using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public static class ValueSheet 
{

    public static bool OnDrag;

    public static string host = "127.0.0.1";
    public static string id = "zhouy546";
    public static string pwd = "Hydeno1234";
    public static string database = "photowall";
    public static string tableName = "pic_table";
    public static string Charset = "utf8";
    public static string ConnectionStr = string.Format("database={0};server={1};user={2};password={3};port={4};charset={5};",database, host, id, pwd, "3306", Charset);

    public static int SQLRowCount;

    public static float moveDir=1;
 

    //Õº∆¨«Ω¥Û–°
    public static int xSize = 23;
    public static int ySize = 10;
    //Õº∆¨«Ω º‰æ‡
     public static float spacingX = .1f;
    public static float spacingY = 1.1f;

    public static float ImpactRad = 2.5f;

    public static float Xoffset = -10;
    public static float Yoffset = -4;

    public static List<float2> touchpositions = new List<float2>();

    public static  float Maping(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp)
    {
        float outVal = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);

        if (clamp)
        {
            if (outputMax < outputMin)
            {
                if (outVal < outputMax) outVal = outputMax;
                else if (outVal > outputMin) outVal = outputMin;
            }
            else
            {
                if (outVal > outputMax) outVal = outputMax;
                else if (outVal < outputMin) outVal = outputMin;
            }
        }


        return outVal;
    }
}

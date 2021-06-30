using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using YUNQI_MySql;

public class PicToTheLeftEvent : MonoBehaviour
{
    m_MySql m_MySql;

    // Start is called before the first frame update
    void Start()
    {
        m_MySql = new m_MySql();

        EventCenter.AddListener<int,int>(EventDefine.Onclick, onClick);

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PhotoMoveSystem>().OnPicMoveToLeft += PicLeft;

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PhotoMoveSystem>().OnPicMoveToRight += PicRightt;
    }

    private void PicLeft(object sender, System.EventArgs e)
    {
        ValueSheet.moveDir = -1;
    }

    private void PicRightt(object sender, System.EventArgs e)
    {
        ValueSheet.moveDir = 1;
    }

    private void onClick(int index,int _NodeIndex)
    {
        Debug.Log(index);

       List<string> temp =   m_MySql.getRowByID(index, ValueSheet.ConnectionStr,ValueSheet.tableName);

        string title = temp[1];
        string Subtitle = temp[2];
        string Intro = temp[3];
        string url = temp[4];
        EventCenter.Broadcast(EventDefine.SetNode, title, Subtitle, Intro, url, _NodeIndex);

    }
}

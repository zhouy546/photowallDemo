using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsGroup : MonoBehaviour
{
    public List<SlotCtr> slotCtrs = new List<SlotCtr>();

    public GameObject UiPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateSlotNode();
        EventCenter.AddListener<string,string,string,string,int>(EventDefine.SetNode, SetNode);
    }

    void SetNode(string _title,string _subtitle, string _intro,string _url,int nodeIndex)
    {
        slotCtrs[nodeIndex].SetNode(_title, _subtitle, _intro, _url);
    }

    void CreateSlotNode()
    {
        for (int i = 0; i < photowall_spawner.instance.valueArrays.Length; i++)
        {
            GameObject g = Instantiate(UiPrefab);

            g.transform.SetParent(this.transform);

            g.transform.position = new Vector3(photowall_spawner.instance.valueArrays[i].x, photowall_spawner.instance.valueArrays[i].y, 0);

            SlotCtr slotCtr = g.GetComponent<SlotCtr>();

            slotCtr.ID = i;

            slotCtrs.Add(slotCtr);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using Unity.Mathematics;

public class SlotCtr : MonoBehaviour,IDragHandler,IEndDragHandler
{
    public Text TitleText;
    public Text subTitleText;
    public Text introText;
    public Image Pic;

    public bool isOpen;

    public GameObject[] gameObjects;

    public int ID;

    public void Start()
    {
    }

    public void Update()
    {
        if (isOpen)
        {

            photowall_spawner.instance.valueArrays[ID].z = photowall_spawner.instance.valueArrays[ID].z + (ValueSheet.ImpactRad - photowall_spawner.instance.valueArrays[ID].z) * 0.05f;

        }
        else
        {

            photowall_spawner.instance.valueArrays[ID].z = photowall_spawner.instance.valueArrays[ID].z + (0 - photowall_spawner.instance.valueArrays[ID].z) * 0.05f;

        }
    }

    public void SetNode(string _titleText, string _subtitleText, string _introText, string spritUrl)
    {

        Open();
        Debug.Log("_titleText:  " + _titleText + "_subtitleText: " + _subtitleText + "_introText:  " + _introText + "spriteUrl" + spritUrl);

        TitleText.text = _titleText;
        subTitleText.text = _subtitleText;
        introText.text = _introText;
        LoadPic(spritUrl);

    }
    async void LoadPic(string spritUrl)
    {
        Texture2D _texture = await MyUtility.Utility.GetRemoteTexture(spritUrl);
        Pic.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));

        int spriteWidth =(int) (Pic.GetComponent<RectTransform>().rect.height * _texture.width) / _texture.height;

        //Debug.Log(spriteWidth);
        Pic.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteWidth, Pic.GetComponent<RectTransform>().sizeDelta.y);


    }

    public void Open()
    {
        isOpen = true;

        foreach (var item in gameObjects)
        {
            item.SetActive(true);
        }
    }

    public void Close()
    {
        isOpen = false;

        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {

        ValueSheet.OnDrag = true;
        Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
        this.transform.localPosition = new Vector3(worldPos.x, worldPos.y, this.transform.localPosition.z);
        photowall_spawner.instance.valueArrays[ID] = new float3(worldPos.x, worldPos.y, photowall_spawner.instance.valueArrays[ID].z);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        StartCoroutine(turnOffDrage());
    }

    IEnumerator turnOffDrage()
    {
        yield return new WaitForSeconds(0.1f);

        ValueSheet.OnDrag = false;

    }
}

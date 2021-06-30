using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUNQI_MySql;
using MyUtility;
using Unity.Collections;
using System.Text;
using Unity.Rendering;
using UnityEngine.Networking;
using CodeMonkey.Utils;

public class photowall_spawner : MonoBehaviour
{
    public  bool isStartMoveSystem;
    public  bool isStartAlignmentSystem;
    public bool isTurnOnPupOutSystem;


    public static    photowall_spawner instance;

    public Transform selectionAreaTransform;

    [SerializeField] private GameObject gameObjectPrefab;

    private Entity entityPrefab;

    private World defaultWorld;
    private EntityManager entityManager;


    [SerializeField] Shader quadShader;
    [SerializeField] Material quadMat;
    [SerializeField] Mesh quadMesh;
    m_MySql m_MySql;

    public float3[] valueArrays = new float3[3];

    
    private void Awake()
    {
        instance = this;

      //  ValueSheet.touchPositions.
    }
    // Start is called before the first frame update
    void Start()
    {
        ValueSheet.touchpositions.Add(new float2(1, 1));

        ValueSheet.touchpositions.Add(new float2(-5, 1));

        Application.targetFrameRate = 60;
        EventCenter.AddListener(EventDefine.OnReadSqlComplete, startCreateInstance);
        EventCenter.AddListener(EventDefine.OnSpawnerComplete, TurnOnPhotoAlignmentSystem);

        ReadSql();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isTurnOnPupOutSystem = !isTurnOnPupOutSystem;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            valueArrays[0].z = math.lerp(valueArrays[0].z, 2.5f, 0.1f);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            valueArrays[0].z = math.lerp(valueArrays[0].z, 0, 0.1f);
        }
    }


    public void ReadSql()
    {
        m_MySql = new m_MySql();

        MySqlConnection con = m_MySql.getMySqlConnect(ValueSheet.ConnectionStr);

        DataTableCollection  table =  m_MySql.GetTable(ValueSheet.ConnectionStr, "select * from pic_table");

        ValueSheet.SQLRowCount = table[0].Rows.Count;

        EventCenter.Broadcast(EventDefine.OnReadSqlComplete);
    }

    public void startCreateInstance()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        InstantiateEntityGrid(ValueSheet.xSize, ValueSheet.ySize);
    }



    private void InstantiateEntityGrid(int dimX, int dimY)
    {
        int index = 0;
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                int SQLi = index % ValueSheet.SQLRowCount;  

                List<string> temp = m_MySql.getRowByRowNum(SQLi, ValueSheet.ConnectionStr, ValueSheet.tableName);

                PhotoDataData tempPhotoData = new PhotoDataData()
                {
                    id = int.Parse(temp[0]),
                };



               // MakeEntity(i ,  j, tempPhotoData);
                MakeEntityFromPrefab(i, j, tempPhotoData);
                index++;

            }
        }

      EventCenter.Broadcast(EventDefine.OnSpawnerComplete);
    }



    private void MakeEntityFromPrefab(int _X_index, int _Y_index, PhotoDataData _photoDataData)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);

        entityManager.AddComponentData(myEntity, new NonUniformScale
        {
            Value = new float3(1f, 1f, 1f)
        });



        entityManager.AddComponentData(myEntity, new MoveSpeed
        {
            Value = -0.25f,
            Red = 2.5f,
            xdis = 0f,
            ypos = 0f,
            xpos = 0F,
        });

        DynamicBuffer<TouchPositionDynamicBuffer> bynamicBuffer = entityManager.AddBuffer<TouchPositionDynamicBuffer>(myEntity);

        for (int k = 0; k < valueArrays.Length; k++)
        {
            bynamicBuffer.Add(new TouchPositionDynamicBuffer { TouchPosition = valueArrays[k] });
        }

        Material QuaMmaterial = new Material(quadShader);
        int random = Mathf.FloorToInt(UnityEngine.Random.Range(0, 1.1f));
        QuaMmaterial.SetFloat("_dir", random);
        entityManager.SetSharedComponentData(myEntity, new RenderMesh
        {
            mesh = quadMesh,

            material = QuaMmaterial
        });


        float timeOffset = UnityEngine.Random.Range(0, 10f);
        entityManager.SetComponentData(myEntity, new PupOutData
        {
            isBlack = random == 1 ? false : true,
            timeOffset = timeOffset,
            speed = 0.1f,
            TargetzOffset = -0.25f,
            TargetScaleFactor = 1.2f,
            currentScaleFactor = 1
        });


        entityManager.SetComponentData(myEntity, new PhotoDataData
        {
            id = _photoDataData.id,
            X_index = _X_index,
            Y_index = _Y_index,
            YOffset = 0
        });

        List<string> temp = m_MySql.getRowByID(_photoDataData.id, ValueSheet.ConnectionStr, ValueSheet.tableName);

        string url = temp[4];

        SetTex(myEntity, url);
    }




    private void MakeEntity(int _X_index, int _Y_index, PhotoDataData _photoDataData)
    {
         entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(NonUniformScale),
            typeof(PhotoDataData),
            typeof(PupOutData),
            typeof(MoveSpeed),
            typeof(PicQuadTag),
            typeof(TouchPositionDynamicBuffer)

            );

        Entity myEntity = entityManager.CreateEntity(archetype);
        

        DynamicBuffer<TouchPositionDynamicBuffer> bynamicBuffer = entityManager.AddBuffer<TouchPositionDynamicBuffer>(myEntity);

        for (int k = 0; k < valueArrays.Length; k++)
        {
            bynamicBuffer.Add(new TouchPositionDynamicBuffer { TouchPosition = valueArrays[k] });
        }



        entityManager.AddComponentData(myEntity, new MoveSpeed
        {
            Value = -0.25f,
            Red = 2.5f,
            xdis = 0f,
            ypos = 0f,
            xpos = 0F,
        });

        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = new float3(0f, 0f, 0f)
        });


        Material QuaMmaterial = new Material(quadShader);
        QuaMmaterial.CopyPropertiesFromMaterial(quadMat);
        int random = Mathf.FloorToInt(UnityEngine.Random.Range(0, 1.1f));
        QuaMmaterial.SetInt("_BlackWhite", random);
        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = quadMesh,

            material = QuaMmaterial
        });


        float timeOffset =UnityEngine.Random.Range(0, 10f);
        entityManager.AddComponentData(myEntity, new PupOutData
        {
            isBlack = random == 1 ? false : true,
            timeOffset = timeOffset,
            speed = 0.1f,
            TargetzOffset = -0.25f,
            TargetScaleFactor = 1.2f,
            currentScaleFactor =1
        });


        entityManager.AddComponentData(myEntity, new PhotoDataData
        {
            id = _photoDataData.id,
            X_index = _X_index,
            Y_index = _Y_index,
            YOffset = 0
        });
        List<string> temp = m_MySql.getRowByID(_photoDataData.id, ValueSheet.ConnectionStr, ValueSheet.tableName);

        string url = temp[3];

        SetTex(myEntity, url);
    }


    async void SetTex(Entity _mEntity,string _imageUrl)
    {
        Texture2D _texture = await MyUtility.Utility.GetRemoteTexture(_imageUrl);

        entityManager.GetSharedComponentData<RenderMesh>(_mEntity).material.SetTexture("_MainTex", _texture);

        setScale(_mEntity, _texture);
    }

    private void setScale(Entity _mEntity, Texture2D _texture)
    {
        float _rito = (float)_texture.width / (float)_texture.height;

        entityManager.SetComponentData(_mEntity, new NonUniformScale
        {

            Value = new float3(_rito, 1f, 1f)
        });

        entityManager.SetComponentData(_mEntity, new PhotoDataData
        {
            id = entityManager.GetComponentData<PhotoDataData>(_mEntity).id,
            X_index = entityManager.GetComponentData<PhotoDataData>(_mEntity).X_index,
            Y_index = entityManager.GetComponentData<PhotoDataData>(_mEntity).Y_index,
            rito = _rito
        });
    }


    private void TurnOnPhotoAlignmentSystem()
    {
        StartCoroutine(turnOnPhotoAlignmentSystem());
    }

    IEnumerator turnOnPhotoAlignmentSystem()
    {
        isStartAlignmentSystem = true;
        yield return new WaitForSeconds(1f);
        isStartAlignmentSystem = false;

        isStartMoveSystem = true;


        //StartCoroutine(PupOut());
    }

    IEnumerator PupOut()
    {
        yield return new WaitForSeconds(.5f);
        isTurnOnPupOutSystem = !isTurnOnPupOutSystem;
        yield return new WaitForSeconds(5f);
        isTurnOnPupOutSystem = !isTurnOnPupOutSystem;
        StartCoroutine(PupOut());
    }


}

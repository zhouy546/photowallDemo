using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace dotsTest
{

    public class spawner : MonoBehaviour
    {

        [SerializeField] private Mesh unitMesh;
        [SerializeField] private Material unitMaterial;
        [SerializeField] private GameObject gameObjectPrefab;

        private Entity entityPrefab;
        private World defaultWorld;
        private EntityManager entityManager;

        [SerializeField] int xSize = 10;
        [SerializeField] int ySize = 10;
        [Range(0.1f, 2f)]
        [SerializeField] float spacing = 1.1f;
        // Start is called before the first frame update
        void Start()
        {
            defaultWorld = World.DefaultGameObjectInjectionWorld;
            entityManager = defaultWorld.EntityManager;

            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
            entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

            InstantiateEntityGrid(xSize, ySize, spacing);


            //    MakeEntity();

        }

        private void InstantiateEntity(float3 position)
        {
            Entity mEntity = entityManager.Instantiate(entityPrefab);
            entityManager.SetComponentData(mEntity, new Translation {
                Value = position
            });
        }

        private void InstantiateEntityGrid(int dimX, int dimY,float spaceing = 1.1f)
        {
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    InstantiateEntity(new float3(i * spaceing, j * spaceing, 0f));
                }
            }
        }

        private void MakeEntity()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityArchetype archetype = entityManager.CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(Scale),
                typeof(NonUniformScale)
                );

            Entity myEntity =   entityManager.CreateEntity(archetype);
            entityManager.AddComponentData(myEntity, new Translation
            {
                Value = new float3(2f, 0f, 4f)
            });

            entityManager.AddComponentData(myEntity, new Scale
            {
                Value = 8f
            });

            entityManager.AddSharedComponentData(myEntity, new RenderMesh {
                mesh = unitMesh,
                material = unitMaterial
            });
        }
    }

}

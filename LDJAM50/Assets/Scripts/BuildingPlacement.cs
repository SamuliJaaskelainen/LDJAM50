using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public enum BuildingType
    {
        SmallHouse,
        TallHouse,
        LargeHouse,
        Mansion,
        Apartment,
        Greenhouse,
        Farm,
        Park,
        Market,
        Brewery,
        Mill,
        Cathedral,
        Temple,
        Shrine,
        Keep,
        Lighthouse,
        Wall
    }

    [System.Serializable]
    public class BuildingData
    {
        public BuildingType buildingType;
        public int money;
        public int population;
        public int faith;
        public List<Synergy> synergies = new List<Synergy>();
    }

    [System.Serializable]
    public class Synergy
    {
        public BuildingType buildingType;
        public int money;
        public int population;
        public int faith;
    }

    public class BuildingPlacement : MonoBehaviour
    {
        [SerializeField] string jsonFileName;
        [SerializeField] BuildingType type;
        [SerializeField] List<Synergy> synergies = new List<Synergy>();
        [SerializeField] int baseMoney;
        [SerializeField] int basePopulation;
        [SerializeField] int baseFaith;

        Rigidbody rb;
        float radius;
        bool firstCollision = true;

        void Awake()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            radius = sphereCollider.radius;
            rb = GetComponent<Rigidbody>();
            Destroy(sphereCollider);

            TextAsset jsonData = Resources.Load<TextAsset>(jsonFileName);
            if (jsonData)
            {
                BuildingData buildingData = JsonUtility.FromJson<BuildingData>(jsonData.text);
                type = buildingData.buildingType;
                baseMoney = buildingData.money;
                basePopulation = buildingData.population;
                baseFaith = buildingData.faith;
                synergies = buildingData.synergies;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (firstCollision)
            {
                GlobalData.money += baseMoney;
                GlobalData.population += basePopulation;
                GlobalData.faith += baseFaith;

                if (baseMoney > 0)
                {
                    VfxPlayer.Instance.PlayVfx(0, transform.position, baseMoney);
                }
                if (basePopulation > 0)
                {
                    VfxPlayer.Instance.PlayVfx(1, transform.position, basePopulation);
                }
                if (baseFaith > 0)
                {
                    VfxPlayer.Instance.PlayVfx(2, transform.position, baseFaith);
                }

                Collider[] collisions = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider col in collisions)
                {
                    Rigidbody colRb = col.attachedRigidbody;
                    if (colRb)
                    {
                        BuildingPlacement building = colRb.GetComponent<BuildingPlacement>();
                        if (building)
                        {
                            if (building.enabled)
                            {
                                foreach (Synergy synergy in synergies)
                                {
                                    if (synergy.buildingType == building.type)
                                    {
                                        GlobalData.money += synergy.money;
                                        GlobalData.population += synergy.population;
                                        GlobalData.faith += synergy.faith;

                                        if (synergy.money > 0)
                                        {
                                            VfxPlayer.Instance.PlayVfx(0, building.transform.position, synergy.money);
                                        }
                                        if (synergy.population > 0)
                                        {
                                            VfxPlayer.Instance.PlayVfx(1, building.transform.position, synergy.population);
                                        }
                                        if (synergy.faith > 0)
                                        {
                                            VfxPlayer.Instance.PlayVfx(2, building.transform.position, synergy.faith);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                firstCollision = false;
            }
        }

        void Update()
        {
            if (rb.centerOfMass.y + transform.position.y < GlobalData.seaLevel)
            {
                if (firstCollision)
                {
                    GlobalData.population -= basePopulation;
                }
                MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in renderers)
                {
                    renderer.material = Resources.Load<Material>("AtlasWet");
                }
                transform.SendMessage("Splash", SendMessageOptions.DontRequireReceiver);
                firstCollision = false;
                enabled = false;
            }
        }
    }
}
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
    public class Synergy
    {
        public BuildingType buildingType;
        public int money;
        public int population;
        public int faith;
    }

    public class BuildingPlacement : MonoBehaviour
    {
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
        }

        void OnCollisionEnter(Collision other)
        {
            if (firstCollision)
            {
                GlobalData.money += baseMoney;
                GlobalData.population += basePopulation;
                GlobalData.faith += baseFaith;

                Collider[] collisions = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider col in collisions)
                {
                    BuildingPlacement building = GetComponent<BuildingPlacement>();
                    if (building)
                    {
                        foreach (Synergy synergy in synergies)
                        {
                            if (synergy.buildingType == building.type)
                            {
                                GlobalData.money += synergy.money;
                                GlobalData.population += synergy.population;
                                GlobalData.faith += synergy.faith;
                            }
                        }
                    }
                }
                firstCollision = false;
            }
        }

        void Update()
        {
            if (rb.centerOfMass.y + transform.position.y < GlobalData.seaLevel + 1.0f)
            {
                GlobalData.population -= basePopulation;
                MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in renderers)
                {
                    renderer.material = Resources.Load<Material>("AtlasWet");
                }
                enabled = false;
            }
        }
    }
}

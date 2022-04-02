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
        Marker,
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

        float radius;

        void Awake()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            radius = sphereCollider.radius;
            Destroy(sphereCollider);
        }

        void OnCollisionEnter(Collision other)
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

        }
    }
}

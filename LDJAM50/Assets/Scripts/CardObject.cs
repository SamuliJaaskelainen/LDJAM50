using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public Card card;

    public void Init(int handSize)
    {
        transform.position += Vector3.right * card.number * (60.0f / handSize);
        GameObject prefab = Instantiate(card.prefabToSpawn, transform.position, Quaternion.identity, transform) as GameObject;
        prefab.GetComponent<Rigidbody>().isKinematic = true;
        prefab.GetComponent<Building.BuildingPlacement>().enabled = false;
        prefab.GetComponent<SphereCollider>().enabled = false;
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = gameObject.layer;
            if (child.transform.GetComponent<MeshCollider>())
            {
                child.transform.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}

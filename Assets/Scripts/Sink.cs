using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Sink : MonoBehaviour, IInteractable
{
    private string _emptyBucket = "bucket_empty";
    public GameObject prefab;
    public Transform sinkSpawn;

    public void Interact(GameObject fromObject)
    {
        if (fromObject.transform.Find("Hand").GetChild(0).GetComponent<Item>().itemName == _emptyBucket)
        {
            Destroy(fromObject.transform.Find("Hand").GetChild(0).gameObject);
            GameObject newBucket = Instantiate(prefab,sinkSpawn.transform.position , Quaternion.identity);
            newBucket.transform.parent = sinkSpawn.transform;
        }
    }


}

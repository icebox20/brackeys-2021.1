using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Interact(GameObject fromObject)
    {
        bool holdingItem = fromObject.GetComponent<InteractionManager>().holdingItem;
        if (!holdingItem)
        {
            Pickup(fromObject.GetComponent<InteractionManager>());
            fromObject.GetComponent<InteractionManager>().holdingItem = true;
        }
        else
        {
            Drop(fromObject.GetComponent<InteractionManager>());
            fromObject.GetComponent<InteractionManager>().holdingItem = false;
        }
    }
    
    void Pickup(InteractionManager IM)
    {
        transform.position = IM.handLocation.transform.position;
        transform.parent = IM.gameObject.transform.Find("Hand");
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    void Drop(InteractionManager IM)
    {
        transform.parent = null;
        _rb.constraints = RigidbodyConstraints.None;
    }
}

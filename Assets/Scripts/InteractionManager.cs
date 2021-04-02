using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject handLocation;
    public List<GameObject> nearByInteractables;
    public Collider interactionRange;
    public bool holdingItem;
    //private bool _player2;
    private KeyCode _interactButton;
    private float _tempColour;
    public List<Color> _materials = new List<Color>();
    public List<Material> _changedMaterials = new List<Material>();
    
    
    void Start()
    {
        holdingItem = false;
        bool _player2 = GetComponent<Player>().GetSecondPlayer();
        if (_player2)
        {
            _interactButton = KeyCode.F;
        }
        else
        {
            _interactButton = KeyCode.E;
        }
        
    }
    
    void Update()
    {    
        if (nearByInteractables.Count > 0)
        {

            for (int i = 0; i < nearByInteractables.Count; i++)
            {
                if (nearByInteractables[i] == null)
                {
                    nearByInteractables.RemoveAt(i);
                }
            }
            if (Input.GetKeyDown(_interactButton))
            {
                if (holdingItem && nearByInteractables.Count > 1)
                {
                    nearByInteractables[1].GetComponent<IInteractable>().Interact(gameObject);
                }
                else
                {
                    GameObject closestInteractable = GetClosestInteractable();
                    closestInteractable.GetComponent<IInteractable>().Interact(gameObject);
                }

            }
        }

        if (handLocation.transform.childCount > 0)
        {
            holdingItem = true;
        }
        else
        {
            holdingItem = false;
        }
    }

    GameObject GetClosestInteractable()
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in nearByInteractables)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(name + " Enter: " + other.name);
        if (other.TryGetComponent(out IInteractable test))
        {
            foreach (var renderer in other.gameObject.GetComponentsInChildren<Renderer>())
            {
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty("_Color"))
                    {
                        _materials.Add(material.color);
                        _changedMaterials.Add(material);
                    
                        material.color = new Color(material.color.r,1.0f,material.color.b,material.color.a);
                    }

                }
            }
            nearByInteractables.Add(other.gameObject);
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(name + " Exit: " + other.name);
        if (other.TryGetComponent(out IInteractable test))
        {
            int i = 0;
            foreach (var material in _changedMaterials)
            {
                material.color = _materials[i];
                i++;
            }
            _materials.Clear();
            _changedMaterials.Clear();
            nearByInteractables.Remove(other.gameObject);
        }
    }
}

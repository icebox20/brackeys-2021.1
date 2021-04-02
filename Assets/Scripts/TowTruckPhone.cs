using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TowTruckPhone : MonoBehaviour, IInteractable
{
    private TaskTowAway _taskTowAway;
    
    public void Interact(GameObject fromObject)
    {
        GameLogic.Instance.score -= 50;

        if (FindObjectsOfType<TaskTowAway>().Length > 0)
        {
            _taskTowAway = GameObject.FindObjectOfType<TaskTowAway>();
            Destroy(_taskTowAway);
        }

    }
}

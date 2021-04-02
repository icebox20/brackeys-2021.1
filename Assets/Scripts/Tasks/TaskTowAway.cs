using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TaskTowAway : MonoBehaviour, IInteractable, ITask
{
   public void Interact(GameObject fromObject)
   {
   }

   public string GetTaskName()
   {
      return "Crashed";
   }

   public Sprite GetIcon()
   {
      return Resources.Load<Sprite>("crash");
   }
}

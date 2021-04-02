using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public float speed;
    public GameObject FixingSpot;
    public Transform DespawnSpot;
    private bool atFixingSpot = false;
    public int tasksLeft;
    public bool repairsDone;
    public bool atDespawnSpot;
    private Rigidbody _rigidbody;
    public List<Sprite> taskIcons;

    private void Start()
    {
        Vector3 relativePos = FixingSpot.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
        repairsDone = false;
        atDespawnSpot = false;
        

    }

    private void Update()
    {
        if (FixingSpot != null)
        {
            if (!atFixingSpot)
            {
                //Vector3 direction = FixingSpot.transform.position - transform.position;
                //direction /= Time.fixedDeltaTime;
                //direction = Vector3.ClampMagnitude(direction, speed);
                
                //_rigidbody.velocity = direction;
                transform.position = Vector3.MoveTowards(transform.position,FixingSpot.transform.position,speed * Time.deltaTime);
            }
            if (transform.position == FixingSpot.transform.position)
            {
                atFixingSpot = true;
                transform.parent = FixingSpot.transform;
            }   
        }

        goAway();

        tasksLeft = GetComponents<IInteractable>().Length;

        if (tasksLeft == 0)
        {
            repairsDone = true;
            speed = 10;
            GameLogic.Instance.finishVehicleUI(gameObject);
            GameLogic.Instance.finishVehicle(gameObject);
            if (atDespawnSpot)
            {
                Destroy(this.gameObject);
            }
        }
        
        taskIcons.Clear();
        
        foreach (var icons in GetComponents<ITask>())
        {
            taskIcons.Add(icons.GetIcon());
        }
    }

    void goAway()
    {
        if (repairsDone)
        {
            transform.position = Vector3.MoveTowards(transform.position,DespawnSpot.position,speed * Time.deltaTime);
        }
        if (transform.position == DespawnSpot.position)
        {
            atDespawnSpot = true;
        }  
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.collider.gameObject.name);
        if (other.collider.transform.parent != null)
        {
            if (other.collider.transform.parent.TryGetComponent(out VehicleMovement test))
            {
                Debug.Log("crashed");
                crashed();
            }
        }
    }

    void crashed()
    {
        atFixingSpot = true;
        gameObject.AddComponent<TaskTowAway>();
    }

    //[ContextMenu ("Move Car")]
    //void moveToFixingSpot()
    //{
    //    Vector3 direction = (FixingSpot.transform.position - this.transform.position).normalized;
    //    transform.Translate(direction * speed * Time.deltaTime);
    //}
}

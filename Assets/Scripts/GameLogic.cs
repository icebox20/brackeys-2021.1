using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameLogic : Singleton<GameLogic>
{
    public float time;
    public bool paused;
    public int score;
    public int currentLevel;
    public GameObject[] spawnPoints;
    public GameObject[] fixingSpots;
    public Transform[] despawnSpots;
    public SOLevel[] levels;
    private float spawningTime;
    public int currentEvent;
    public List<Tuple<float,GameObject,SOEvent, List<Component>>> queue = new List<Tuple<float, GameObject,SOEvent,List<Component>>>();
    public List<Tuple<float,GameObject,SOEvent, List<Component>>> ui = new List<Tuple<float, GameObject,SOEvent,List<Component>>>();
    void Start()
    {
        score = levels[currentLevel - 1].startingScore;
        currentEvent = 0;
        currentLevel = 1;
        paused = true;
    }

    public void Reset2()
    {
        time = 0;
        queue.Clear();
        ui.Clear();
        currentEvent = 0;
    }

    void Update()
    {
        if (!paused)
        {
            time += Time.deltaTime;


            if (Time.time > spawningTime)
            {
                spawnVehicle();
                spawningTime += levels[currentLevel - 1].spawningInterval;
            }

            if (queue.Count() > 0)
            {
                foreach (var data in queue)
                {
                    if (Time.time > data.Item1)
                    {
                        Debug.Log("Event Failed");
                        failTask();
                        queue.Remove(data);
                    }
                }
 
            }

            for (int i = 0; i < queue.Count; i++)
            {
                //queue[i] = Tuple.Create<float, GameObject, SOEvent, List<Component>>(queue[i].Item1 - Time.deltaTime,
                //    queue[i].Item2, queue[i].Item3, queue[i].Item4);
            }
        }

        if (score <= 0)
        {
            GameManager.Instance.GameOver();
        }

        if (currentEvent == levels[currentLevel - 1].events.Length && queue.Count == 0)
        {
            SceneManager.LoadScene(1);
        }

        // task failed
    }

    void spawnVehicle()
    {
        if (currentLevel > levels.Length || currentEvent > (levels[currentLevel - 1].events.Length - 1))
        {
            return;
        }

        int validSpawnPointIndex = GetValidSpawnPoint();
        if (validSpawnPointIndex == -1)
        {
            return;
        }
        
        GameObject spawnLocation = spawnPoints[validSpawnPointIndex];
        
        Debug.Log("Trigger Spawning Event");
        GameObject prefab = levels[currentLevel - 1].events[currentEvent].car;
        
        GameObject newCar = Instantiate(prefab,spawnLocation.transform.position , Quaternion.identity);
        newCar.transform.parent = spawnLocation.transform;

        newCar.GetComponent<VehicleMovement>().FixingSpot = fixingSpots[validSpawnPointIndex];
        newCar.GetComponent<VehicleMovement>().DespawnSpot = despawnSpots[validSpawnPointIndex];
        //Tuple<float, GameObject> eventFailTime = new Tuple<float, GameObject>(Time.time + levels[currentLevel - 1].events[currentEvent].timeLimit, newCar);

        List<Component> foo = new List<Component>();

        for (int i = 0; i < levels[currentLevel - 1].events[currentEvent].tasks.Length; i++)
        {
            Type type = Type.GetType(levels[currentLevel - 1].events[currentEvent].tasks[i].task);
            Component task = newCar.AddComponent(type);
            foo.Add(task);
        }
        
        queue.Add(new Tuple<float, GameObject, SOEvent, List<Component>>(Time.time + levels[currentLevel - 1].events[currentEvent].timeLimit, newCar.gameObject,levels[currentLevel - 1].events[currentEvent], foo));
        ui.Add(new Tuple<float, GameObject, SOEvent, List<Component>>(Time.time + levels[currentLevel - 1].events[currentEvent].timeLimit, newCar.gameObject,levels[currentLevel - 1].events[currentEvent], foo));
        currentEvent++;
    }

    int GetValidSpawnPoint()
    {
        int tryingSpawnPoint;
        for (int i = 0; i < levels[currentLevel - 1].events.Length; i++)
        {
            tryingSpawnPoint = Random.Range(0, spawnPoints.Length);
            //Debug.Log("Trying:" + tryingSpawnPoint);
            if (spawnPoints[tryingSpawnPoint].transform.childCount == 0 && fixingSpots[tryingSpawnPoint].transform.childCount == 0)
            {
                return tryingSpawnPoint;
            }
        }

        return Random.Range(0, spawnPoints.Length);
        //return -1;
    }

    void failTask()
    {
        score -= queue.First().Item3.onFail;
        //Destroy(queue.First().Item2);
    }

    [ContextMenu ("Finish Task 1")]
    void finishTask1()
    {
        score += queue.First().Item3.onSuccess;
        Destroy(queue.First().Item2);
        queue.RemoveAt(0);
    }
    
    public void finishVehicle(GameObject toFinish)
    {
        int kill = -1;
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].Item2 == toFinish)
            {
                kill = i;
            }
        }

        if (kill == -1)
        {
            return;
        }
        
        score += queue[kill].Item3.onSuccess;
        //Destroy(queue[kill].Item2);
        queue.RemoveAt(kill);
        //ui.RemoveAt(kill);
    }
    
    public void finishVehicleUI(GameObject toFinish)
    {
        int kill = -1;
        for (int i = 0; i < ui.Count; i++)
        {
            if (ui[i].Item2 == toFinish)
            {
                kill = i;
            }
        }

        if (kill == -1)
        {
            return;
        }
        
        //Destroy(queue[kill].Item2);
        ui.RemoveAt(kill);
        //ui.RemoveAt(kill);
    }

    public void finishTask()
    {
        
    }
}

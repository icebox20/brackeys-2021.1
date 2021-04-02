using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.VFX;

public class TaskWashCar : MonoBehaviour, IInteractable, ITask
{
    public string taskName;
    public string requiredTool;
    public GameObject prefab;
    private ProgessBar _progessBar;
    private bool _timing;
    public float _timer;
    private GameObject _fromObject;
    private KeyCode _interactButton;
    public Sprite icon;
    private GameObject _bubblesEffect;
    private GameObject _bubbles;

    private void Start()
    {
        requiredTool = "bucket_water";
        taskName = "Wash Car";
        prefab = Resources.Load("Bucket") as GameObject;
        _timing = false;
        _progessBar = GetComponentInChildren<ProgessBar>();
        _timer = 4;
        icon = Resources.Load<Sprite>("carwash");
        _bubblesEffect = Resources.Load<GameObject>("Bubbles");
    }

    public void Interact(GameObject fromObject)
    {
        _timing = true;
        _fromObject = fromObject;
        bool _player2 = fromObject.GetComponent<Player>().GetSecondPlayer();

        _bubbles = Instantiate(_bubblesEffect, _fromObject.transform);
        
        if (_player2)
        {
            _interactButton = KeyCode.F;
        }
        else
        {
            _interactButton = KeyCode.E;
        }
    }
    
    private void Update()
    {
        if (Input.GetKey(_interactButton) && _timing)
        {
            _timer -= Time.deltaTime;
            _progessBar.precentage = _timer / 4;
            _progessBar.enable = true;
        }
        if (Input.GetKeyUp(_interactButton))
        {
            Destroy(_bubbles);
            _timer = 4;
            _timing = false;
            _progessBar.enable = false;
        }
        if (_timer < 0)
        {
            Destroy(_bubbles);
            _progessBar.enable = false;
            task();
        }
    }
    
    void task()
    {
        if (_fromObject.transform.Find("Hand").childCount > 0)
        {
            if (_fromObject.transform.Find("Hand").GetChild(0).GetComponent<Item>().itemName == requiredTool)
            {
                _fromObject.GetComponent<InteractionManager>().nearByInteractables.Remove(gameObject);
                
                Destroy(_fromObject.transform.Find("Hand").GetChild(0).gameObject);
                GameObject newBucket = Instantiate(prefab,_fromObject.transform.Find("Hand").position , Quaternion.identity);
                //newBucket.transform.parent = fromObject.transform.Find("Hand");
                Destroy(this);
            }
        }
    }

    public string GetTaskName()
    {
        return taskName;
    }
    
    public Sprite GetIcon()
    {
        return icon;
    }
}


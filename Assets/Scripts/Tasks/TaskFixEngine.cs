using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.UIElements;
using UnityEngine;

public class TaskFixEngine : MonoBehaviour, IInteractable, ITask
{
    public string taskName;
    public string requiredTool;
    public GameObject smoke;
    public float _timer;
    private bool _timing;
    private GameObject _fromObject;
    private ProgessBar _progessBar;
    private KeyCode _interactButton;
    public Sprite icon;

    private void Start()
    {
        taskName = "Fix Engine";
        requiredTool = "wrench";
        smoke = this.transform.Find("Smoke").gameObject;
        smoke.SetActive(true);
        _timing = false;
        _progessBar = GetComponentInChildren<ProgessBar>();
        _timer = 4;
        icon = Resources.Load<Sprite>("fixengine");
    }

    public void Interact(GameObject fromObject)
    {
        _timing = true;
        _fromObject = fromObject;
        bool _player2 = fromObject.GetComponent<Player>().GetSecondPlayer();
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
            _timer = 4;
            _timing = false;
            _progessBar.enable = false;
        }
        if (_timer < 0)
        {
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
                smoke.SetActive(false); 
                _fromObject.GetComponent<InteractionManager>().nearByInteractables.Remove(gameObject);
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

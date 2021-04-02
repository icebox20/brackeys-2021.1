using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgessBar : MonoBehaviour
{
    public Slider _slider;
    public GameObject _sliderGameObject;
    public float precentage;
    public bool enable;
        
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _sliderGameObject = transform.GetChild(0).gameObject;
        enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CameraMovement.Instance.transform);
        _slider.value = (precentage * (_slider.maxValue - _slider.minValue)) + _slider.minValue ;
        _sliderGameObject.SetActive(enable);
    }
}

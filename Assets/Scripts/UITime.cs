using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITime : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    public string FormatTime( float time )
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds );
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = FormatTime(Time.time);
    }
}

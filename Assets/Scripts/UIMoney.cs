using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMoney : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "$" + GameLogic.Instance.score;
    }
}

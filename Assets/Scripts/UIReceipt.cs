using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class UIReceipt : MonoBehaviour
{
    private GameObject _prefab;
    private List<GameObject> _receipts = new List<GameObject>();
    private int _offset = 50;
    void Start()
    {
        _prefab = Resources.Load<GameObject>("TaskReceipt");
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
        if (_receipts.Count > 0)
        {
            foreach (var data in _receipts)
            {
                Destroy(data);
            }
        }

        if (GameLogic.Instance.ui.Count > 0)
        {
            int j = 0;
            foreach (var data in GameLogic.Instance.ui)
            {
                GameObject newObject = Instantiate(_prefab, transform);
                newObject.transform.localPosition = Vector3.zero + new Vector3(_offset*j,0,0);

                newObject.transform.GetChild(0).GetComponent<Image>().sprite = data.Item3.carIcon;
                int i = 1;
                foreach (var task in data.Item2.GetComponents<ITask>())
                {
                    newObject.transform.GetChild(i).GetComponent<Image>().sprite = task.GetIcon();
                    i++;
                }

                newObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = FormatTime(data.Item1 - Time.time);
                
                _receipts.Add(newObject);
                j++;
            }
        }

    }
}

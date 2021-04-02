using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class UIDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI TMP;
    void Start()
    {
        TMP = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string text = "Score: " + GameLogic.Instance.score.ToString() + "\n";
        text += "Current Time: " + GameLogic.Instance.time + "\n";
        for (int i = 0; i < GameLogic.Instance.queue.Count; i++)
        {
            text += GameLogic.Instance.queue[i].Item3.name + "\n";
            text += "Time Left: " + UnityEngine.Mathf.Round(Mathf.Abs(GameLogic.Instance.queue[i].Item1 - Time.time)).ToString() + "s" + "\n";

            foreach (var task in GameLogic.Instance.queue[i].Item2.GetComponents<ITask>())
            {
                text += "\t" + task.GetTaskName() + "\n";
            }
            
            //text += GameLogic.Instance.queue.First().Item2.;

        }
        TMP.text = text;
        
    }
}

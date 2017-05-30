using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour {

    private Toggle tg;
    
    private void Start()
    {
        tg = GetComponent<Toggle>();
    }

    public void change(bool val)
    {
        if (val == tg.isOn)
            tg.isOn = !tg.isOn;
    }
}

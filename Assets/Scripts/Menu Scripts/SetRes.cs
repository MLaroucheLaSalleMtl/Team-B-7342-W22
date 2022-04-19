using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetRes : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private Resolution[] _GFXRes;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        _GFXRes = Screen.resolutions;
        List<string> dropOptions = new List<string>();
        int pos = 0;
        int i = 0;
        Resolution currRes = Screen.currentResolution;
        foreach (Resolution r in _GFXRes)
        {
            string val = r.ToString();
            dropOptions.Add(val);
            if (r.width == currRes.width &&
                r.height == currRes.height &&
                r.refreshRate == currRes.refreshRate)
            {
                pos = i;
            }
            i++;
        }
        dropdown.AddOptions(dropOptions);
        dropdown.value = pos;
    }

    public void SetResolution()
    {
        Resolution r = _GFXRes[dropdown.value];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRate);
    }
}

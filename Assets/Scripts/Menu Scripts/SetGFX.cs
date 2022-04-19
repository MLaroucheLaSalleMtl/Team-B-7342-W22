using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetGFX : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private string[] _GFXNames;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        _GFXNames = QualitySettings.names;
        List<string> dropOptions = new List<string>();
        foreach (string s in _GFXNames)
        {
            //Debug.Log("Added :" + s);
            dropOptions.Add(s);
        }
        dropdown.AddOptions(dropOptions);
        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void SetGfx()
    {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }

}

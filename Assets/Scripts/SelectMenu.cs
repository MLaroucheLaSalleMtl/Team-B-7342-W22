using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] panels = null;
    [SerializeField] private Selectable[] defaultItem = null;

    //Turn off all panels, turn on panel number (pos) . Use pos = -1 to turn off all panels 
    public void PanelToggle(int pos)
    {
        for (int i = 0; i < panels.Length; i++) //for all the panels in my array
        {
            panels[i].SetActive(pos == i); //turn on/off the panels
            if (pos == i) //on the active panel
            {
                defaultItem[i].Select(); //select the default item
            }

        }
    }

    IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        PanelToggle(-1);
    }
}

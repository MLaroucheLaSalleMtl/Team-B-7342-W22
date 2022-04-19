using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainToSetting : MonoBehaviour
{
    [SerializeField] private GameObject[] panels = null;
    [SerializeField] private Selectable[] defaultItem = null;
    [SerializeField] private Animator anim;
    int position;
    bool animate = true;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        PanelToggle(0);
    }

    public void PanelToggle(int pos)
    {
        animate = !animate;
        anim.SetBool("ToSetting", animate);
        position = pos;
        Invoke("ActivatePanel", 0.4f);
    }

    public void ActivatePanel()
    {
        for (int i = 0; i < panels.Length; i++) //for all the panels in my array
        {
            panels[i].SetActive(position == i); //turn on/off the panels
            if (position == i) //on the active panel
            {
                defaultItem[i].Select(); //select the default item
            }

        }
    }

    public void SavePrefs()
    {
        PlayerPrefs.Save();
    }
}

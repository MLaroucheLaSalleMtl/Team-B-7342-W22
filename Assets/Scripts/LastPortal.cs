using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadNextScene();
            GameManager.Instance.level = 4;
            PlayerPrefs.SetInt("level", 4);
            PlayerPrefs.Save();
        }
    }
}

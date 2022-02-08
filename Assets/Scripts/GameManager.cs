using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton declaration
    public static GameManager instance = null;
    private bool gameOver = false;
    private bool pause = false;
    [SerializeField] private Transform player;
    [SerializeField] private SelectMenu selectMenu;

    public bool GameOver { get => gameOver;}
    public bool Pause { get => pause; }
    public Transform Player { get => player;}
    private float oldTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        selectMenu.PanelToggle(-1); //tell selectMenu to hide all panels
        Cursor.lockState = CursorLockMode.Locked; //lock the cursor in the middle of the screen
        Cursor.visible = false; //don't show cursor
    }

    public void PuaseOrPlay()
    {
        if (!gameOver)
        {
            pause = !pause;
            selectMenu.PanelToggle(pause ? 0 : -1);
            float temp = oldTime;
            oldTime = Time.timeScale;
            Time.timeScale = temp;
            Cursor.lockState = (temp > 0f) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = (temp > 0f) ? false : true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    [SerializeField] private int level = 0;
    [SerializeField] private Transform destination;


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
            GameManager.Instance.level = this.level;
            PlayerPrefs.SetInt("level", this.level);
            PlayerPrefs.Save();
            GameManager.Instance.UpdateGameState(GameState.Cutscene);
            GameManager.Instance.Player.position = destination.position;
            GameManager.Instance.MainCamera.GetComponent<CameraRotator>().RotateClockwise();
            GameManager.Instance.MapCamera.GetComponent<MapRotator>().RotateMapClockwise();
            Invoke("EndCutscene", 1f);
        }
    }

    private void EndCutscene()
    {
        GameManager.Instance.UpdateGameState(GameState.GamePlay);
    }
}

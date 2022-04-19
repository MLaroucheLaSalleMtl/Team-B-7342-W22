using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance) GameManager.Instance.UpdateGameState(GameState.Main);
    }
}

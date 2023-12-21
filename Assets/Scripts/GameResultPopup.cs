using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultPopup : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> titles = new List<GameObject>();
    
    void Awake()
    {
        foreach(var tmp in titles)
        {
            tmp.gameObject.SetActive(false);
        }
    }

    public void GameLose()
    {
        titles[0].gameObject.SetActive(true);
    }
    
    public void GameVictory()
    {
        titles[1].gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUtillity : MonoBehaviour
{
    public static bool bPause;

    public GameObject pausePanel;

    public void Start()
    {
        bPause = true; 
    }
    public void PauseGame()
    {
        if (bPause == false)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        /*일시정지 비활성화*/
        else 
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        bPause = !bPause;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            Pause();
        }
    }
    public void Pause()
    {
        if (PauseUI.activeSelf)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0.0f;
        }

    }
    public void Resume() 
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit.");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_UI : MonoBehaviour
{
    // Start is called before the first frame update
   public void QuitButton()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}

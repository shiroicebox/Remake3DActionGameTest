using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void MoveTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

}

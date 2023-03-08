using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadGameplaySceneP1()
    {
        GlobalStates.SetPlayerNo(1);
        SceneManager.LoadScene("GameplayScene");
    }

    public void LoadGameplaySceneP2()
    {
        GlobalStates.SetPlayerNo(2);
        SceneManager.LoadScene("GameplayScene");
    }

    public void ExitApp()
    {
        Debug.Log("Exit Button works!");
        Application.Quit();
    }
}

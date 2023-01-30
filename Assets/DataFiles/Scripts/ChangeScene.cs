using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadGameplayScene() {
        SceneManager.LoadScene("GameplayScene");
    }

    public void ExitApp() {
        Debug.Log("Exit Button works!");
        Application.Quit();
    }
}

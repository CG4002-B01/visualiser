using System.Collections;
using UnityEngine;
// using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;

public class ARStarter : MonoBehaviour
{
    void Start()
    {
        // LoadARScene();
        #if UNITY_EDITOR
            LoadARScene();
        #else
            StartCoroutine(StartXR());
        #endif
    }

    public IEnumerator StartXR()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            yield return null;
            LoadARScene();
        }
    }

    void LoadARScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }
}

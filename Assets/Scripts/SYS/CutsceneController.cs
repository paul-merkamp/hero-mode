using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public GameObject loadingUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            loadingUI.SetActive(true);
            StartCoroutine(LoadSceneAsync());
        }
    }

        private IEnumerator LoadSceneAsync()
    {        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}

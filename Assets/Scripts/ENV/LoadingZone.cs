using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    public string sceneName;
    public GameObject loadingUI;

    public bool requiresInteract = false;

    public void Trigger()
    {
        loadingUI.SetActive(true);
        StartCoroutine(LoadSceneAsync());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!requiresInteract)
            {
                Trigger();
            }
        }
    }

    private IEnumerator LoadSceneAsync()
    {        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}

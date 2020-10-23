using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private string sceneToLoadName;

    public void LoadScene()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        var loadingScreenInstance = Instantiate(loadingScreen);
        var loadingAnimator = loadingScreenInstance.GetComponent<Animator>();

        var animationTime = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;


        DontDestroyOnLoad(loadingScreenInstance);
        var loading = SceneManager.LoadSceneAsync(sceneToLoadName);

        loading.allowSceneActivation = false;

        while (loading.progress < 0.9f)
        {
            yield return new WaitForSeconds(animationTime);
        }

        loading.allowSceneActivation = true;
        loadingAnimator.SetTrigger("EndLoading");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game closed.");
    }


}

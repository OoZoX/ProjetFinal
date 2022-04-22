using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    List<string> SceneToLoad;


    List<string> SceneLoaded;

    private bool _firstSceneUpload = false;

    public static LoadScene Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if(InputPlayer.Instance != null)
        {
            if (InputPlayer.Instance.m_KeyBoardEsc)
            {
                LoadMenu();
            }
        }
    }

    public async void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void LoadLevel()
    {

        _firstSceneUpload = false;
        SceneLoaded = new List<string>();

        foreach (var scene in SceneToLoad)
        {
            StartCoroutine(LoadThisScene(scene));
        }

        

    }

    private void UnloadAllScene()
    {
        List<string> TabScene = GetActiveSceneName();

        foreach (string scene in TabScene)
        {
            Debug.Log(scene);
            UnloadThisScene(scene);
        }
    }

    private List<string> GetActiveSceneName()
    {
        var scenesName = new List<string>();

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            scenesName.Add(SceneManager.GetSceneAt(i).name);
        }

        return scenesName;
    }


    private IEnumerator LoadThisScene(string scene)
    {
        AsyncOperation asyncLoad;
        if (!_firstSceneUpload)
        {
             asyncLoad = SceneManager.LoadSceneAsync(scene);
            _firstSceneUpload = true;
        }
        else
        {
             asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        
        bool sceneLoad = false;

        while (SceneLoaded.Count != SceneToLoad.Count)
        {
            if(asyncLoad.isDone && !sceneLoad)
            {
                SceneLoaded.Add(scene);
                sceneLoad = true;
            }
                

            yield return null;
        }
    }

    private async void UnloadThisScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
}

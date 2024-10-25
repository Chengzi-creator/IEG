using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private void Awake()
    {   
        startButton.onClick.AddListener(OnstartButtonClick);
    }
    
    private void Update()
    {
       
    }

    private void OnstartButtonClick()
    {
        StartCoroutine(LoadScene("Game"));
    }
    
    private IEnumerator LoadScene(string sceneName)
    {
        //加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

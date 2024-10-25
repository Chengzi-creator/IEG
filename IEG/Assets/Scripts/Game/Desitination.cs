using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Desitination : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadScene("GameOver"));
        }
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

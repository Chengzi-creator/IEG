using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject PauseMasks;
    [SerializeField] private GameObject SetupMasks;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button setupButton;
    [SerializeField] private Button menuButton;
    private bool isPaused = false;

    private void Awake()
    {
        PauseMasks.SetActive(false); //先隐藏
        SetupMasks.SetActive(false);
        exitButton.onClick.AddListener(OnexitButtonClick); //监听
        setupButton.onClick.AddListener(OnsetupButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
    }
    
    
    private void Update()
    {
        //检测是否按下ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    //两次按ESC
    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; //暂停游戏时间
            PauseMasks.SetActive(true); // 启用暂停菜单
            SetupMasks.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f; //恢复游戏时间
            PauseMasks.SetActive(false); // 禁用暂停菜单
            SetupMasks.SetActive(false);
        }
    }

    private void OnexitButtonClick()
    {
        //当点击退出
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    private void OnbackButtonClick()
    {
        ResumeGame(); //恢复游戏
    }
    
    private void OnsetupButtonClick()
    {
        SetupMasks.SetActive(true);
        PauseMasks.SetActive(false);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 恢复游戏时间
        PauseMasks.SetActive(false); // 禁用暂停菜单
    }

    private void OnmenuButtonClick()
    {
        if (SetupMasks.activeSelf)
        {
            SetupMasks.SetActive(false);
            PauseMasks.SetActive(true);
        }
        else
        {
            TogglePause();
        }
    }
    
    private IEnumerator LoadAndRestartScene(string sceneName)
    {
        //加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        //恢复游戏时间
        Time.timeScale = 1f;
    }

}

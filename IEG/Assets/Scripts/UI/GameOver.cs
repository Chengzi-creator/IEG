using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject storyMasks;
    [SerializeField] private GameObject overMasks;
    [SerializeField] private ScrollRect scrollRect; 
    [SerializeField] private Button exitButton;
    [SerializeField] private Button storyButton;
    [SerializeField] private TextMeshProUGUI storyText; 
    
    private void Awake()
    {   
        storyMasks.SetActive(false);
        overMasks.SetActive(true);
        exitButton.onClick.AddListener(OnexitButtonClick);
        storyButton.onClick.AddListener(OnstoryButtonClick);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            storyMasks.SetActive(false);
            overMasks.SetActive(true);
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

    private void OnstoryButtonClick()
    {
        storyMasks.SetActive(true);
        overMasks.SetActive(false);
        
        LoadStoryText();
    }
    
    
    private void LoadStoryText()
    {

        string story = "    你一直以为,自己拥有了许多能力便可以随心所欲.\n" +
                       "    但在无数次的追寻中,你感到了一种无法言喻的沉重感.跳得再高,爬得再远,战斗得再激烈,那股空缺感却从未消散.于是,你将那些曾经引以为傲的能力一一剥离,想摆脱这里的一切.你以为,抛弃这些负担会让你更自由.\n" +
                       "    但当你失去了这些能力后,你迷失在了一个看似永无止境的梦境中,四分五裂的自我化作了不同的影子,在这片世界中徘徊.\n" +
                       "    那些影子化为与你长相相同的存在,它们站在你的面前,一步步要求你完成试炼,重新找回曾经的力量.它们是你缺失的部分:你的渴望,你的执念,你的欲望.\n" +
                       "    但真正的自由,不在于控制一切,而在于舍弃一些自己看似需要其实却没那么必要的东西,学会接受自己的不完整.\n" +
                       "    当你真正认识到这一点的时候,也就是挣脱梦境的那一刻了.\n" +
                       "(按ESC退出)";
        
        storyText.text = story;
    }
    
}

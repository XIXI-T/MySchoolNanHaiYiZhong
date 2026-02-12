using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*---------------------------------------*/
/*
提示框调用方法
在别的脚本文件写入[SerializeField] private TipBackGround [对象名];然后在inspector中把Tip对象直接拖到脚本组件的输入框中来引用脚本
然后就可以直接通过StartCoroutine([对象名].Tip("你好，这里是测试内容！"));来调用提示框
提示框限制字数16个字，懒得写提示框跟随文本字数伸缩的功能了，有需求再改
*/
/*---------------------------------------*/
public class TipBackGround : MonoBehaviour
{
    bool TipRunning = false;//用于标记协程是否启动的布尔变量
    private Text ContentText;
    private CanvasGroup Alpha;
    void Start()
    {
        Transform Content = transform.Find("Content");//引用子对象
        ContentText = Content.GetComponent<Text>();//获取子对象（文本）的Text组件
        Alpha = GetComponent<CanvasGroup>();//获取自己的canvagroup组件
        Alpha.alpha=0f;
    }
    public bool TipSwitch = false;//控制ConstantTip关闭的开关，bool值变成true就关闭开关
    public IEnumerator ConstantTip(string content, float WaitForTime = 1f, 
    float OutPutTextWiatForTime = 0.1f)
    //调出提示框 content：提示的内容  waitfortime：渐显/渐隐动画持续时间
    //OutPutTextWiatForTime在此为文字逐字输出的时间间隔
    //ConstantTip和普通Tip的区别是ConstantTip要用TipSwitch控制开关且不限开关时间
    {
        if (!TipRunning)
        {
            TipRunning = true;//占用协程
            if(content.Length > 16)
            TipSwitch = false;//初始化开关
            yield return StartCoroutine(SafeConstantTip(content, WaitForTime, OutPutTextWiatForTime));
            TipRunning = false;//解除占用
            TipSwitch = false;//还原
        }
        else
        {
            //不做Debug提示避免出现轰炸控制台影响性能的情况
        }
    }
    public IEnumerator Tip(string content, float ConstantTime = 1f,
     float WaitForTime = 0.5f, float OutPutTextWiatForTime = 0.1f)
    //调出提示框 content：提示的内容 ConstantTime：提示框持续时间 waitfortime：渐显/渐隐动画持续时间
    //OutPutTextWiatForTime在此为文字逐字输出的时间间隔
    {
        if (!TipRunning)//检测协程是否在进行
        {
            TipRunning = true;//占用协程
            if(content.Length > 16)
            {
                Debug.Log("提示: 输入Tip的内容过长, 部分内容无法正常显示");
            }
            yield return StartCoroutine(SafeTip(content, ConstantTime, WaitForTime, OutPutTextWiatForTime));
            TipRunning = false;//取消占用，协程结束
        }
        else
        {
            Debug.Log("Tip正在被占用 此次调用取消");
            yield break;
        }
    }
    private IEnumerator SafeTip(string content, float ConstantTime = 1f,
     float WaitForTime = 0.5f, float OutPutTextWiatForTime = 0.1f)
    //调出提示框 content：提示的内容 ConstantTime：提示框持续时间 waitfortime：渐显/渐隐动画持续时间
    //OutPutTextWiatForTime在此为文字逐字输出的时间间隔
    {
        StartCoroutine(OutPutTheText(content, OutPutTextWiatForTime));//同时启动文字并行输出
        for(int i = 0; i < 100; i++)//渐显
        {
            Alpha.alpha += 0.01f;
            yield return new WaitForSeconds(WaitForTime / 100);
        }
        yield return new WaitForSeconds(ConstantTime);
        for(int i = 0; i < 100; i++)//渐隐
        {
            Alpha.alpha -= 0.01f;
            yield return new WaitForSeconds(WaitForTime/100);
        }
    }
    private IEnumerator SafeConstantTip(string content, float WaitForTime = 0.5f, 
     float OutPutTextWiatForTime = 0.1f)
    //调出提示框 content：提示的内容  waitfortime：渐显/渐隐动画持续时间
    //OutPutTextWiatForTime在此为文字逐字输出的时间间隔
    {
        StartCoroutine(OutPutTheText(content, OutPutTextWiatForTime));//同时启动文字并行输出
        for(int i = 0; i < 100; i++)//渐显
        {
            Alpha.alpha += 0.01f;
            yield return new WaitForSeconds(WaitForTime / 100);
        }
        while (!TipSwitch)
        {
            yield return null;
        }//等待控制提示框的开关变动
        for(int i = 0; i < 100; i++)//渐隐
        {
            Alpha.alpha -= 0.01f;
            yield return new WaitForSeconds(WaitForTime/100);
        }
    }
    IEnumerator OutPutTheText(string content, float WaitForTime = 0.1f)
    //这是文字输出的协程函数，content参数同上，WaitForTime在此为文字逐字输出的时间间隔
    {
        ContentText.text = "";//初始化文本
        for(int i = 0; i < content.Length; i++)
        {
            ContentText.text += content[i];
            yield return new WaitForSeconds(WaitForTime);
        }
    }
}

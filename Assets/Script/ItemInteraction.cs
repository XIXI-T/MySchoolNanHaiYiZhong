using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ItemInteraction : MonoBehaviour
{
    [Header("交互按键")]
    public KeyCode InterectKey = KeyCode.Space;//指定按下的按键

    [Header("触发事件")]
    public UnityEvent OnInteract;//交互所用到的事件   当作接口（广义上的）使用

    [SerializeField] private TipBackGround script;//准备调用提示框
    public string ItemIntroduction = "按下空格键交互";//物品提示介绍
    public bool IsPlayInRange = false;//检测角色是否在触发范围内
    private void OnTriggerStay2D(Collider2D collider)//当玩家进入触发范围
    {
        if (collider.CompareTag("Player"))//触发的对象是否是Player
        {
            IsPlayInRange = true;
            StartCoroutine(PlayerInRange());//启动交互检测
        }       
    }
    public bool CoroutineRunning = false;
    IEnumerator PlayerInRange()//界面交互检测
    {
        if (CoroutineRunning)
        {
            Debug.Log("协程正在运行中, 此次调用中断");
            yield break;
        }
        CoroutineRunning = true;
        StartCoroutine(script.ConstantTip(ItemIntroduction));      
        while (IsPlayInRange)
        {
            if (Input.GetKeyDown(InterectKey))//检测到按下空格键
            {
                StartEvent();//调用交互事件
                break;
            }
            yield return null;
        }
        script.TipSwitch = true;//关闭提示框
        while(script.TipSwitch == true)
        {
            yield return null;
        }
        CoroutineRunning =false;
    }
    private void StartEvent()//交互事件
    {
        OnInteract?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            IsPlayInRange = false;
        }
    }
}
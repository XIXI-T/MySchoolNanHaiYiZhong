using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*---------------------------------------*/
/*
场景切换触发器使用方法
从预制体中搬出场景切换触发器（Trigger-TurnScene）和黑幕（包含在Canvas中），摆到想要的位置，可以通过collider2d组件调整触发器的触发范围
在脚本组件中把黑幕（Panel）对象拖进script和Dark中，
然后再scene name中输入想要切换的场景名字（目标场景要在Unity的Build setting中设置）
设置完成，此后可以把触发器调成完全透明，就看不见了，主角进入触发区域就会切换场景
*/
/*---------------------------------------*/
public class TurnNewScene : MonoBehaviour
{
    [SerializeField] private HideOrShow script;
    public string SceneName;//需要填写切换到场景的名称
    public GameObject Dark;//需要引用黑幕
    private void OnTriggerEnter2D(Collider2D collider)//当玩家进入触发范围
    {
        if (collider.CompareTag("Player"))//触发的对象是否是Player
        {
            Debug.Log("玩家触发触发点");
            StartCoroutine(TurnScene());//开始切换场景
        }       
    }
    IEnumerator TurnScene()//切换场景
    {
        DontDestroyOnLoad(gameObject);//防止场景切换销毁协程导致后续代码无法执行
        yield return StartCoroutine(script.ShowMe());//渐显
        TurnToNewScene();
        yield return StartCoroutine(script.HideMe());//渐隐
        Destroy(gameObject);//动画完成后销毁自己
    }
    void TurnToNewScene()//切换到新场景
    {
        SceneManager.LoadScene(SceneName);
    }
    public void OpenEvent(string abc)
    {
        StartCoroutine(TurnScene());
    }
}

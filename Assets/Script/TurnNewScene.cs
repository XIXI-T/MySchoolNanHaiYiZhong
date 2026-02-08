using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}

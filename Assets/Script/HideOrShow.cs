using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShow : MonoBehaviour
{
    private CanvasGroup transparency;//透明度控制
    public float WaitForTime = 0.5f;//动画时间
    GameObject rootObject;
    void Start()
    {
        transparency = GetComponent<CanvasGroup>();
        transparency.alpha=0f;
    }
    public IEnumerator ShowMe()//显示动画（供外部以启动协程方式调用）
    {
        rootObject = transform.parent.gameObject;
        DontDestroyOnLoad(rootObject);
        DontDestroyOnLoad(gameObject);//防止场景切换销毁协程导致后续代码无法执行
        yield return StartCoroutine(Show());//调用子协程
    }
    public IEnumerator HideMe()//隐藏动画（供外部以启动协程方式调用）
    {
        yield return StartCoroutine(Hide());
        Destroy(gameObject);//动画完成后销毁自己
        Destroy(rootObject);
    }
    IEnumerator Show()//渐显过程
    {
        transparency.alpha = 0f;
        for(int i = 0; i < 100; i++)
        {
            transparency.alpha += 0.01f;
            yield return new WaitForSeconds(WaitForTime/100);
        }
    }
    IEnumerator Hide()//渐隐过程
    {
        transparency.alpha = 1f;
        for(int i = 0; i < 100; i++)
        {
            transparency.alpha -= 0.01f;
            yield return new WaitForSeconds(WaitForTime/100);
        }
    }
}

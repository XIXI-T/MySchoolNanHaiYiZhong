using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saving : MonoBehaviour
{
    [SerializeField] private Backpack script;//存档存储位置
    public string SavePosition = "./save.json";
    public class SaveContent//存储内容
    {
        public string SceneName;//场景名称
        public Vector3 PlayerPosition;//主角坐标
        public List<Item> items;//背包内容
    }
    public void SaveGame()
    {
        try
        {
            GameObject Player;
            Player = GameObject.Find("Player");
            var saveContent = new SaveContent();
            saveContent.SceneName = SceneManager.GetActiveScene().name;//获取场景
            saveContent.PlayerPosition = Player.transform.position;//获取主角坐标
            saveContent.items = script.BackpackContent;//获取背包内容
            string Saving = JsonUtility.ToJson(saveContent, true);//转json
            File.WriteAllText(SavePosition, Saving);//存储
        }
        catch
        {
            Debug.LogError("存档保存过程中遇到问题, 存档失败! ");
        }
    }
    public void LoadGame()//加载场景
    {
        var saveContent = new SaveContent();
        try
        {
            if (File.Exists(SavePosition))//检测文件是否存在
            {
                string Reading = File.ReadAllText(SavePosition);//读取存档
                saveContent = JsonUtility.FromJson<SaveContent>(Reading);//转入类
            }
            else
            {
                Debug.LogError("存档文件不存在! ");
            }
        }
        catch
        {
            Debug.LogError("存档读取过程中遇到问题, 读取失败! ");
            return;
        }
        StartCoroutine(LoadSceneAndWait(saveContent.SceneName, saveContent));//加载场景并完成一系列加载操作
        
    }
    IEnumerator LoadSceneAndWait(string SceneName, SaveContent saveContent)//加载场景并等待场景加载完成
    {
        AsyncOperation AsyncLoad = SceneManager.LoadSceneAsync(SceneName);//异步加载防止堵塞协程
        while(!AsyncLoad.isDone)//等待场景加载完成
        {
            yield return null;
        }
        GameObject Player;
        Player = GameObject.Find("Player");
        Player.transform.position = saveContent.PlayerPosition;//改变坐标
        script.BackpackContent = saveContent.items;//上传背包信息
    }
}

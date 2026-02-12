using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Item//物品
{
    public string id;//编号
    public string name;//名称
    public string introduction;//介绍
    public string ImagePath;//贴图存储位置

    [System.NonSerialized]//不序列化防止被写进json文件
    public Sprite image;//贴图
}
public class Backpack : MonoBehaviour
{
    List<Item> ItemList = new List<Item>();//用于所有存储物品的容器
    public string SaveFile;//物品配置文件存储位置
    public List<Item> BackpackContent = new List<Item>();//背包存储的内容
    public KeyCode OpenTheBackpackKey = KeyCode.B;//打开背包的按键
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();//获取canvasGroup组件
        SaveFile = Application.persistentDataPath + "/ItemConfig.json";//初始化配置文件存储位置
        WriteJson();
        ReadJson(ref ItemList);//读取配置文件获取物品列表
    }
    void Update()
    {
        if (Input.GetKeyDown(OpenTheBackpackKey))//打开背包
        {
            StartCoroutine(BackpackInteraction());
        }
    }
    bool CoroutineRunning = false;//检测协程是否在运行的布尔变量
    IEnumerator BackpackInteraction()//背包交互
    {
        if (CoroutineRunning)//检测协程是否在运行
        {
            Debug.Log("协程正在运行中，本次协程中断");
            yield break;
        }
        CoroutineRunning =true;//占用协程
        yield return StartCoroutine(Open());//打开背包界面
        while (true)
        {
            if (Input.GetKeyDown(OpenTheBackpackKey))
            {
                break;
            }
            
            yield return null;
        }
        yield return StartCoroutine(Close());//关闭背包界面
        CoroutineRunning = false;//取消占用
    }
    IEnumerator Open(float WaitForTime = 0.5f)//打开界面
    {
        for(int i = 0; i < 100; i++)//渐显
        {
            canvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(WaitForTime / 100);
        }
        canvasGroup.alpha = 1f;
    }
    IEnumerator Close(float WaitForTime = 0.5f)//关闭界面
    {
        for(int i = 0; i < 100; i++)//渐隐
        {
            canvasGroup.alpha -= 0.01f;
            yield return new WaitForSeconds(WaitForTime/100);
        }
        canvasGroup.alpha = 0f;
    }
    void ReadJson(ref List<Item> AimItem)//读取物品json信息 AimItem：指向写入读取信息的参数指针
    {
        if (File.Exists(SaveFile))//检测文件是否存在
        {
            Debug.Log("读取文件位置：" + SaveFile);
            string JsonConfig = File.ReadAllText(SaveFile);//读取文件
            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(JsonConfig);
            // AimItem = JsonUtility.FromJson<List<Item>>(JsonConfig);//将json格式文本转化为item类并赋值 List直接转化json会出问题
            AimItem = wrapper?.items ?? new List<Item>();//(a != null) ? a->items : new List<Item>();
            Debug.Log("读取配置文件ItemConfig完成! 总共{AimItem.Count}件");
            foreach(Item item in AimItem)//批量读取贴图
            {
                if (string.IsNullOrEmpty(item.ImagePath))//检查贴图是否存在
                {
                    item.image = Resources.Load<Sprite>(item.ImagePath);//加载贴图
                }
                else
                {
                    Debug.Log("{item.id}物品使用默认贴图(default)");
                    item.image = Resources.Load<Sprite>("Resoures/Icon/default.png");//使用默认贴图
                }
            }
        }
        else
        {
            Debug.LogError("SaveFile配置文件丢失! 游戏物品数据读取失败! ");
        }
    }


    //----------------------------------------------仅供开发时填入使用--------------------------------------------//
    void WriteJson()
    {
        List<Item> SaveItem = new List<Item>();
        SaveItem.Clear();
        /*--------------------------------------------添加物品信息--------------------------------------------*/
        Item ceshi1 = new Item();//测试1
        ceshi1.id = "001";
        ceshi1.name = "测试";
        ceshi1.introduction = "这是一个测试物品";
        ceshi1.ImagePath = "./Resoures/Icon/ceshi1.png";
        SaveItem.Add(ceshi1);

        Item ceshi2 = new Item();//测试2
        ceshi2.id = "002";
        ceshi2.name = "测个毛";
        ceshi2.introduction = "哎呀";
        ceshi2.ImagePath = "./Resoures/Icon/ceshi2.png";
        SaveItem.Add(ceshi2);
        /*--------------------------------------------添加物品信息--------------------------------------------*/
        ItemListWrapper wrapper = new ItemListWrapper();
        wrapper.items = SaveItem;
        string JsonConfig = JsonUtility.ToJson(wrapper, true);//转化为json格式
        File.WriteAllText(SaveFile, JsonConfig);//保存
    }
    //包装
    [System.Serializable]
    public class ItemListWrapper //包装便于json读写操作防止jsonutility不认
    {
        public List<Item> items = new List<Item>();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public float speed = 3;//设置速度
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        transform.position = new Vector2(0, 0);
    }
    void Update()
    {
        anim.SetBool("IsWalking", false);
        Awake();
        float x = Input.GetAxisRaw("Horizontal");   //读取键盘上左右按键输入，返回值为 -1  0  1
        float y = Input.GetAxisRaw("Vertical");     //读取键盘上上下按键输入，返回值为 -1  0  1
        Vector2 direction = new Vector2(x, y);      //方向向量
        if(direction.magnitude > 0)
        {
            anim.SetBool("IsWalking", true);
            anim.SetFloat("Horizontal", x);
            anim.SetFloat("Vertical", y);
        }
        else
        {
            anim.SetBool("Iswalking", false);
        }
        transform.Translate(direction * speed * Time.deltaTime);//移动
    }
}
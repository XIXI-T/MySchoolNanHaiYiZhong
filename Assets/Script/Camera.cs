using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Camera : MonoBehaviour
{
    GameObject Player;
    private Vector2 Camera_xy = new Vector2(0, 0);//摄像机位置
    public float CameraSpeed = 1f;//摄像机跟随速度  
    public Vector3 CameraDeviation;//摄像机偏移量
    void Start()
    {
        Player = GameObject.Find("Player");
    }
    void LateUpdate()
    {
        // Vector2 CameraPosition = new Vector2(Player.transform.position.x + Camera_xy.x,
        // Player.transform.position.y + Camera_xy.y);
        // transform.position = CameraPosition;
        // Camera_xy.x > Player.transform.position.x ? Camera_xy.x-- : Camera_xy.x++;
        Camera_xy = transform.position;
        Vector2 Smooth = Vector2.Lerp(Camera_xy, Player.transform.position, CameraSpeed * Time.deltaTime);
        transform.position = new Vector3(Smooth.x, Smooth.y, -10) + CameraDeviation;
    }
}

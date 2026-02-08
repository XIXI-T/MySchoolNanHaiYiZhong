using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    GameObject Player;
    public float Camera_x = 0, Camera_y = 0;//摄像机偏移量  
    void Start()
    {
        Player = GameObject.Find("Player");
    }
    void Update()
    {
        Vector3 CameraPosition = new Vector3(Player.transform.position.x + Camera_x,
        Player.transform.position.y + Camera_y,
        -10);
        transform.position = CameraPosition;
    }
}

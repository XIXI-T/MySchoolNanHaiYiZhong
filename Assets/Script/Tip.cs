using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    private GameObject background;
    void Start()
    {
        background = transform.parent?.gameObject;
    }
    IEnumerator ShowTheTip(string content)
    {
        StartCoroutine()
    }
}

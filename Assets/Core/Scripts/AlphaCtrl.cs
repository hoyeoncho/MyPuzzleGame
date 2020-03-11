using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaCtrl : MonoBehaviour
{
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = transform.GetComponent<Image>().color;
        color.a = 0f;
        transform.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (color.a >= 1) return;
        color.a += 0.01f;
        transform.GetComponent<Image>().color = color;
    }
}

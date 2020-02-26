﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    Image img;


    public void initialize(int v, Point p, Sprite piece)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        SetIndex(p);
        img.sprite = piece;
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdeateName();
    }

    public void ResetPosition()
    {
        pos  = new Vector2(50 + (100 * index.x), -50 - (100 * index.y));
    }

    void UpdeateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }
}

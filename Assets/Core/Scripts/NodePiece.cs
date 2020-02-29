﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    bool updating;
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

    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 25f);
    }

    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move *  Time.deltaTime * 25f;
    }


    public bool UpdatePiece()
    {
        if(Vector3.Distance(rect.anchoredPosition,pos)>1)
        {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    void UpdeateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    //public void OnPointerDown(PointerEventData eventData) : 버튼을 클릭/터치하는 순간 실행됨
    //public void OnPointerUp(PointerEventData eventDate) : 버튼 클릭/터치를 떼는 순간 실행됨
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating) return;
        MovePieces.instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Let go" + transform.name);
        MovePieces.instance.DropPiece();
    }
}

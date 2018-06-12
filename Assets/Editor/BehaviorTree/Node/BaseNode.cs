using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseNode
{
    public int id;

    public string name;

    public string title;

    public string desc;

    public string type = "BaseNode";

    public Dictionary<string, List<string>> properties;

    public int properties_total_id;

    public List<BaseNode> childs;

    public Rect rect;

    public bool isDragged;

    public GUIStyle style;

    public float node_width = 120;

    public float node_height = 50;

    public ConnectionPoint inPoint;

    public ConnectionPoint outPoint;

    private Action<BaseNode> _onClickSelf;

    public BaseNode(Vector2 position, Action<BaseNode> OnClickSelf, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint)
    {
        childs = new List<BaseNode>();
        properties = new Dictionary<string, List<string>>();
        properties_total_id = 0;
        style = BTEditorWindow.nodeStyle;
        rect = new Rect(position.x, position.y, node_width, node_height);
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint);
        _onClickSelf = OnClickSelf;
    }

    public void InitLogic()
    {
        id = BTEditorManager.node_total_id;
        name = type + "_" + id;
        title = name;
        NodeData.Add(this);
    }

    public void Draw()
    {
        inPoint.Draw();

        outPoint.Draw();

        GUI.Box(rect, type, style);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        _onClickSelf(this);
                        isDragged = true;
                        GUI.changed = true;
                    }
                }
                else
                {
                    GUI.changed = true;
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void SetPos(Vector2 pos)
    {
        this.rect.x = pos.x;
        this.rect.y = pos.y;
    }

    public Rect GetRect()
    {
        return this.rect;
    }
}
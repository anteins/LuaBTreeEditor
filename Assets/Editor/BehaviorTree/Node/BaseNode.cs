using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseNode : BTObject
{
    public string name;
    public NodeType type = NodeType.BaseNode;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Dictionary<string, List<string>> properties;

    public string desc;
    public Rect rect;
    public bool isDragged;
    public GUIStyle style;
    public float node_width = 120;
    public float node_height = 50;
    public List<BaseNode> childs;

    protected Action<BaseNode> onClickSelf;

    public BaseNode()
    {
        base.GenId();
        this.childs = new List<BaseNode>();
        this.properties = new Dictionary<string, List<string>>();
        this.style = BTEditorWindow.nodeStyle;
    }

    public virtual void Init(Vector2 position, Action<BaseNode> OnClickSelf)
    {
        this.name = type + "_" + id;
        this.rect = new Rect(position.x, position.y, node_width, node_height);
        this.inPoint = BTEditorManager.CreateConnectionPoint(this, ConnectionPointType.In);
        this.outPoint = BTEditorManager.CreateConnectionPoint(this, ConnectionPointType.Out);
        this.onClickSelf = OnClickSelf;
    }

    public virtual void Draw()
    {
        //连接点
        inPoint.Draw();
        outPoint.Draw();
        //节点框
        GUI.Box(rect, name.ToString(), style);
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
                        //单击节点（左键）
                        onClickSelf(this);
                        isDragged = true;
                        GUI.changed = true;
                    }
                    else if (e.button == 1)
                    {
                        //单击节点（右键）
                        ProcessContextMenu(e.mousePosition);
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

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Delete Node"), false, () =>
        {

        });
        genericMenu.ShowAsContext();
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
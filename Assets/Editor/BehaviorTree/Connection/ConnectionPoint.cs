using System;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint : BTObject
{
    public Rect rect;
    public ConnectionPointType type;
    public BaseNode node;
    public GUIStyle style;

    public ConnectionPoint(BaseNode node, ConnectionPointType type)
    {
        base.GenId();

        this.node = node;
        this.type = type;
        switch (this.type)
        {
            case ConnectionPointType.In:
                this.style = BTEditorWindow.inPointStyle;
                break;

            case ConnectionPointType.Out:
                this.style = BTEditorWindow.outPointStyle;
                break;
        }
        this.rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
        switch (type)
        {
            case ConnectionPointType.In:
            rect.x = node.rect.x - rect.width + 8f;
            break;

            case ConnectionPointType.Out:
            rect.x = node.rect.x + node.rect.width - 8f;
            break;
        }

        if (GUI.Button(rect, "", style))
        {
            ProcessEvents(Event.current);
        }
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.Used:
                if (rect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        switch (type)
                        {
                            case ConnectionPointType.In:
                                BTEditorManager.OnClickInPoint(this);
                                break;
                            case ConnectionPointType.Out:
                                BTEditorManager.OnClickOutPoint(this);
                                break;
                        }
                        GUI.changed = true;
                    }
                }
                else
                {
                    GUI.changed = true;
                }
                break;

            case EventType.MouseUp:
                break;

            case EventType.MouseDrag:
                break;
        }
        return false;
    }
}
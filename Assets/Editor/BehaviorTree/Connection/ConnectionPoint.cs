using System;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{
    public Rect rect;

    public ConnectionPointType type;

    public BaseNode node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    private bool _isClickingDown;

    private int click_count = 0;

    public ConnectionPoint(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
    {
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
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
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
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }

        if (_isClickingDown)
        {
            click_count = click_count + 1;
            Debug.Log("_isClickingDown" + click_count);
        }
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
                        Debug.Log("[ConnectionPoint] Click Down!");
                        _isClickingDown = true;
                        GUI.changed = true;
                    }
                }
                else
                {
                    GUI.changed = true;
                }
                break;

            case EventType.MouseUp:
                Debug.Log("[ConnectionPoint] Click Up!");
                _isClickingDown = false;
                click_count = 0;
                break;

            case EventType.MouseDrag:
                break;
        }

        return false;
    }
}
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Connection : BTObject
{
    public string connectId;
    public BaseNodeData targetNode;
    public List<SlotData> slotList;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    private Vector3 _centerPos;

    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint outPoint, ConnectionPoint inPoint, Action<Connection> OnClickRemoveConnection)
    {
        base.GenId();

        this.connectId = outPoint.node.id + "_" + inPoint.node.id;
        this.slotList = new List<SlotData>();

        this.outPoint = outPoint;
        this.inPoint = inPoint;
       
        BuildTreeConnection();

        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        BTUtils.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f
        );

        // 取消连线
        this._centerPos = (inPoint.rect.center + outPoint.rect.center) * 0.5f;
        if (Handles.Button(this._centerPos, Quaternion.identity, 4, 8, Handles.RectangleCap))
        {
            //just draw
        }
    }

    public bool ProcessEvents(Event e)
    {
        float distance = Vector3.Distance(this._centerPos, e.mousePosition);
        if (distance < 6.0f)
        {
            //Debug.Log("distance " + distance);
            if(e.type != EventType.Layout && e.type != EventType.Repaint)
            {
                if (e.button == 0)
                {
                    BTEditorManager.OnClickConnection(this);
                    GUI.changed = true;
                    return true;
                }
                else if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                    GUI.changed = true;
                    return true;
                }
            }
        }

        return false;
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Delete Connection"), false, () =>
        {
            RemoveTreeConnection();
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        });
        genericMenu.ShowAsContext();
    }

    private void BuildTreeConnection()
    {
        //out's child is in
        this.outPoint.node.childs.Add(this.inPoint.node);

        //out's child is in
        BaseNodeData outNodeData = NodeDataManager.Get(this.outPoint.node);
        BaseNodeData inNodeData = NodeDataManager.Get(this.inPoint.node);

        ConnectionData connectionData = NodeDataManager.CreateConnectionData(this);
        connectionData.targetNodeId = inNodeData.id;

        outNodeData.connectionList.Add(connectionData);
    }

    private void RemoveTreeConnection()
    {
        //out's child remove in
        this.outPoint.node.childs.Remove(this.inPoint.node);

        //out's child remove in
        BaseNodeData outNodeData = NodeDataManager.Get(this.outPoint.node);
        BaseNodeData inNodeData = NodeDataManager.Get(this.inPoint.node);

        NodeDataManager.Remove(this, outNodeData);
    }
}
using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint outPoint, ConnectionPoint inPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.outPoint = outPoint;
        this.inPoint = inPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;

        BuildTreeConnection();
    }

    public void Draw()
    {
        BTUtils.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f
        );

        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
        {
            RemoveTreeConnection();

            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
    }

    private void BuildTreeConnection()
    {
        //out's child is in
        this.outPoint.node.childs.Add(this.inPoint.node);

        //out's child is in
        NodeLuaInfo outLogic = NodeData.Get(this.outPoint.node);
        NodeLuaInfo inLogic = NodeData.Get(this.inPoint.node);
        outLogic.logic_childs.Add(inLogic);
    }

    private void RemoveTreeConnection()
    {
        //out's child remove in
        this.outPoint.node.childs.Remove(this.inPoint.node);

        //out's child remove in
        NodeLuaInfo outLogic = NodeData.Get(this.outPoint.node);
        NodeLuaInfo inLogic = NodeData.Get(this.inPoint.node);
        outLogic.logic_childs.Remove(inLogic);
    }
}
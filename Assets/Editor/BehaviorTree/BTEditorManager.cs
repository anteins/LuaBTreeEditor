using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BTEditorManager
{
    public static BaseNode rootNode;
    public static int curObjectId = 0;

    private static List<BaseNode> nodes_list = new List<BaseNode>();
    private static List<Connection> connection_list = new List<Connection>();
    private static List<ConnectionPoint> connectionPoint_list = new List<ConnectionPoint>();

    public static BaseNode selectedNode { get; set; }
    public static Connection selectedConnection { get; set; }
    public static ConnectionPoint selectedInPoint { get; set; }
    public static ConnectionPoint selectedOutPoint { get; set; }

    public static void Reset()
    {
        nodes_list.Clear();
        connection_list.Clear();
        connectionPoint_list.Clear();
    }

    public static void Draw()
    {
        DrawNodes();
        DrawConnections();
        DrawConnectionLine(Event.current);
    }

    public static void DrawNodes()
    {
        if (nodes_list != null)
        {
            for (int i = 0; i < nodes_list.Count; i++)
            {
                nodes_list[i].Draw();
            }
        }
    }

    public static void DrawConnections()
    {
        if (connection_list != null)
        {
            for (int i = 0; i < connection_list.Count; i++)
            {
                connection_list[i].Draw();
            }
        }
    }

    public static void DrawConnectionLine(Event e)
    {
        //click inPoint
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            BTUtils.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f
            );

            GUI.changed = true;
        }

        //click outPoint
        if (selectedOutPoint != null && selectedInPoint == null)
        {
            BTUtils.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f
            );

            GUI.changed = true;
        }
    }

    public static bool ProcessEvents(Event e)
    {
        if (ProcessNodesEvents(e))
            return true;

        if (ProcessConnectionEvents(e))
            return true;

        //if (ProcessConnectPointEvents(e))
        //    return true;

        return false;
    }

    public static bool ProcessNodesEvents(Event e)
    {
        if (nodes_list != null)
        {
            for (int i = 0; i < nodes_list.Count; i++)
            {
                bool guiChanged = nodes_list[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                    return true;
                }
            }
        }

        return false;
    }

    public static bool ProcessConnectionEvents(Event e)
    {
        if (connection_list != null)
        {
            for (int i = 0; i < connection_list.Count; i++)
            {
                bool guiChanged = connection_list[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                    return true;
                }
            }
        }

        return false;
    }

    public static bool ProcessConnectPointEvents(Event e)
    {
        if (connection_list != null)
        {
            for (int i = 0; i < connection_list.Count; i++)
            {
                Connection connection = connection_list[i];
                if (connection.inPoint.ProcessEvents(e))
                {
                    GUI.changed = true;
                    return true;
                }
                if (connection.outPoint.ProcessEvents(e))
                {
                    GUI.changed = true;
                    return true;
                }
            }
        }

        return false;
    }

    public static void OnClickNode(BaseNode node)
    {
        selectedNode = node;
        BTEditorWindow.UpdateSubWindow(selectedNode);
    }

    public static void OnClickConnection(Connection connection)
    {
        selectedConnection = connection;
        BTEditorWindow.UpdateSubWindow(selectedConnection);
    }

    public static void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;
        //out ---> in success
        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection(selectedOutPoint, selectedInPoint);
            }
            ClearConnectionSelection();
        }
    }

    public static void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        //in ---> out success
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection(selectedOutPoint, selectedInPoint);
            }
            ClearConnectionSelection();
        }
    }

    public static void OnClickRemoveConnection(Connection connection)
    {
        connection_list.Remove(connection);
    }

    public static Connection CreateConnection(ConnectionPoint outPoint, ConnectionPoint inPoint)
    {
        Connection connection = new Connection(outPoint, inPoint, OnClickRemoveConnection);
        //new Data
        NodeDataManager.CreateConnectionData(connection);
        connection_list.Add(connection);
        return connection;
    }

    public static ConnectionPoint CreateConnectionPoint(BaseNode node, ConnectionPointType type)
    {
        ConnectionPoint connectionPoint = new ConnectionPoint(node, type);
        //new Data
        NodeDataManager.CreateConnectionPointData(connectionPoint);
        connectionPoint_list.Add(connectionPoint);
        return connectionPoint;
    }

    public static T GetObject<T>(BTBaseData baseData) where T : BTObject
    {
        return GetObject<T>(baseData.id);
    }

    public static T GetObject<T>(int id) where T : BTObject
    {
        T obj = default(T);
        string obj_type = typeof(T).ToString();
        switch (obj_type)
        {
            case "BaseNode":
                foreach (BaseNode node in nodes_list)
                {
                    if(node.id == id)
                    {
                        obj = node as T;
                        break;
                    }
                }
                break;
            case "Connection":
                foreach (Connection connection in connection_list)
                {
                    if (connection.id == id)
                    {
                        obj = connection as T;
                        break;
                    }
                }
                break;
            case "ConnectionPoint":
                foreach (ConnectionPoint point in connectionPoint_list)
                {
                    if (point.id == id)
                    {
                        obj = point as T;
                        break;
                    }
                }
                break;
        }

        return obj;
    }

    public static void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    public static void OnDrag(Vector2 delta)
    {
        if (nodes_list != null)
        {
            for (int i = 0; i < nodes_list.Count; i++)
            {
                nodes_list[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    public static void SaveCurrentNode()
    {
        if (selectedNode != null)
        {
            BaseNodeData nodeData = NodeDataManager.Get(selectedNode);
            if(nodeData != null)
            {
                nodeData.Serialize(selectedNode);
            }
        }
    }

    public static void SaveCurrentConnection()
    {
        if (selectedConnection != null)
        {
            ConnectionData data = NodeDataManager.Get(selectedConnection);
            if(data != null)
            {
                data.Serialize(selectedConnection);
            }
        }
    }

    public static BaseNode AddNode<T>(Vector2 mousePosition) where T:BaseNode
    {
        if (nodes_list == null)
        {
            nodes_list = new List<BaseNode>();
        }

        NodeType node_type = (NodeType)Enum.Parse(typeof(NodeType), typeof(T).ToString());
        return __addNode(node_type, mousePosition);
    }

    private static BaseNode __addNode(NodeType node_type, Vector2 mousePosition)
    {
        BaseNode node = null;
        switch (node_type)
        {
            case NodeType.BehaviourNode:
                node = new BehaviourNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.ExcelNode:
                node = new ExcelNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<ExcelNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.ActionNode:
                node = new ActionNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.ConditionNode:
                node = new ConditionNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.WaitNode:
                node = new WaitNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.SequenceNode:
                node = new SequenceNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.SelectorNode:
                node = new SelectorNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
            case NodeType.LoopNode:
                node = new LoopNode();
                node.Init(mousePosition, OnClickNode);
                NodeDataManager.CreateNodeData<BaseNodeData>(node);
                nodes_list.Add(node);
                break;
        }

        if(node != null && nodes_list.Count == 1)
        {
            rootNode = node;
        }
        return node;
    }

    public static void RemoveNode(BaseNode node)
    {
        if (nodes_list == null)
        {
            return;
        }

        for(int i=0; i< nodes_list.Count; i++)
        {
            if(node.id == nodes_list[i].id)
            {
                nodes_list.RemoveAt(i);
                break;
            }
        }
    }

    public static void Save()
    {
        if(rootNode == null)
        {
            Debug.Log("can not Save, please set a root node.");
            return;
        }

        PyConfigGenWorker genWorker = new PyConfigGenWorker();
        genWorker.Export(rootNode, "gacha");
    }

    public static void Load(string jsonPath = "")
    {
        PyConfigGenWorker genWorker = new PyConfigGenWorker();
        genWorker.Import("gacha");
    }

    public static void Clear()
    {
        if (nodes_list != null)
        {
            nodes_list.Clear();
        }

        if (connection_list != null)
        {
            connection_list.Clear();
        }

        curObjectId = 0;

        selectedNode = null;
        selectedConnection = null;

        if (rootNode != null)
        {
            BTUtils.RemoveTree(rootNode);
            RemoveNode(rootNode);
        }

        rootNode = null;
    }
}

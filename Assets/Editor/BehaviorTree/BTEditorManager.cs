using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BTEditorManager
{
    private static List<BaseNode> nodes_list;

    public static BaseNode root_node;

    public static BaseNode current_node;

    private static List<Connection> connections;

    private static ConnectionPoint selectedInPoint;

    private static ConnectionPoint selectedOutPoint;

    public static int node_total_id = 0;

    private static Vector2 drag;

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
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
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

    public static void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        ProcessNodesEvents(Event.current);

        //ProcessConnectionPointsEvents(Event.current);
    }

    public static void ProcessNodesEvents(Event e)
    {
        if (nodes_list != null)
        {
            for (int i = 0; i < nodes_list.Count; i++)
            {
                bool guiChanged = nodes_list[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    public static void OnClickSelf(BaseNode node)
    {
        current_node = node;
        BTEditorWindow.OnSelectNode(node);
    }

    public static void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        //out ---> in success
        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();

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
                CreateConnection();
            }
            ClearConnectionSelection();
        }
    }

    public static void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    public static void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedOutPoint, selectedInPoint, OnClickRemoveConnection));
    }

    public static void CreateConnection(ConnectionPoint inPoint, ConnectionPoint outPoint)
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(inPoint, outPoint, OnClickRemoveConnection));
    }

    public static void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    public static void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (nodes_list != null)
        {
            for (int i = 0; i < nodes_list.Count; i++)
            {
                nodes_list[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    public static void UpdateCurrentNode(string curNode_title, string curNode_description, Dictionary<string, List<string>> current_properties)
    {
        //set data
        if (current_node != null)
        {
            current_node.title = curNode_title;
            current_node.desc = curNode_description;
            current_node.properties = current_properties;
            NodeData.UpdateLogic(current_node);
        }
    }

    public static BaseNode AddRootNode(Vector2 mousePosition, NodeLuaInfo logic = null)
    {
        if (root_node == null)
        {
            root_node = AddNode<BehaviourNode>(mousePosition, logic);
        }

        return root_node;
    }

    public static BaseNode AddNode(string type_s, Vector2 mousePosition, NodeLuaInfo logic = null)
    {
        NodeType node_type = (NodeType)Enum.Parse(typeof(NodeType), type_s);

        return _AddNode(node_type, mousePosition, logic);
    }

    public static BaseNode AddNode<T>(Vector2 mousePosition, NodeLuaInfo logic = null) where T:BaseNode
    {
        if (nodes_list == null)
        {
            nodes_list = new List<BaseNode>();
        }

        Type type = typeof(T);

        NodeType node_type = (NodeType)Enum.Parse(typeof(NodeType), type.ToString());

        return _AddNode(node_type, mousePosition, logic);
    }

    private static BaseNode _AddNode(NodeType node_type, Vector2 mousePosition, NodeLuaInfo logic = null)
    {
        BaseNode node = null;
        switch (node_type)
        {
            case NodeType.BehaviourNode:
                node = new BehaviourNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.ActionNode:
                node = new ActionNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.ConditionNode:
                node = new ConditionNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.WaitNode:
                node = new WaitNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.SequenceNode:
                node = new SequenceNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.SelectorNode:
                node = new SelectorNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;

            case NodeType.LoopNode:
                node = new LoopNode(mousePosition, OnClickSelf, OnClickInPoint, OnClickOutPoint);
                nodes_list.Add(node);
                break;
        }
        node_total_id = node_total_id + 1;

        if (logic != null)
        {
            node.id = logic.id;

            node.name = logic.name;

            node.type = logic.type;

            node.title = logic.title;

            node.desc = logic.desc;

            node.properties = logic.properties;

            node.properties_total_id = logic.properties_total_id;

            node.SetPos(new Vector2(logic.x, logic.y));

            //Debug.Log("Load x: " + logic.x + "  y: " + logic.y );
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
        if(root_node == null)
        {
            Debug.Log("can not Save, please set a root node.");
            return;
        }
        
        //保存之前，先同步一次node节点信息
        BTUtils.DumpTree(root_node, (BaseNode node) =>
        {
            NodeData.UpdateLogic(node);

            //Debug.Log("Save " + node.id + "  " + node.name + "   childs: " + node.childs.Count + "  x: " + node.rect.x + "  y: " + node.rect.y);
        });
        
        NodeLuaInfo logic = NodeData.Get(root_node);

        string path = BTUtils.GetGenPath() + BTUtils.jsonFile;

        BTUtils.SaveJsonToFile<NodeLuaInfo>(logic, path);
    }

    public static void Load(string jsonPath = "")
    {
        jsonPath = BTUtils.GetGenPath() + BTUtils.jsonFile;

        NodeLuaInfo root_logic = BTUtils.GetJsonFromFile<NodeLuaInfo>(jsonPath);

        Clear();

        float g_x = 0;

        float g_y = 0;

        int last_deepth = -1;

        int cur_index = 1;

        CreateTree(root_logic, null, null, 0, (int deepth, NodeLuaInfo last_logic, BaseNode last_node, BaseNode this_node) =>
        {
            if (last_deepth == -1)
            {
                last_deepth = deepth;
                //Debug.Log(string.Format("----------------------init [{0}]----------------------", deepth));
            }
            if (last_deepth != deepth)
            {
                cur_index = 1;
                //Debug.Log(string.Format("----------------------change [{0}]----------------------", deepth));
            }
            last_deepth = deepth;

            //root node
            if (this_node.id == 0)
            {
                g_x = 10;

                g_y = BTEditorWindow._window.position.height / 2;
            }
            else
            {
                if (last_node != null)
                {
                    //float offset_one_y = last_node.rect.height + 35;

                    //float sub_height = offset_one_y * last_logic.logic_childs.Count;

                    //float start_y = last_node.rect.y - sub_height / 2;

                    //float offset_y = sub_height / last_logic.logic_childs.Count;

                    //g_y = start_y + offset_one_y * cur_index;

                    //g_x = last_node.rect.x + last_node.rect.width;

                    //cur_index = cur_index + 1;

                    //Debug.Log(string.Format("Load connect {0}, {1} -------> {2}, {3}", last_node.id, last_node.name, this_node.id, this_node.name));

                    CreateConnection(last_node.outPoint, this_node.inPoint);
                }
            }

            //this_node.SetPos(new Vector2(g_x, g_y));
        });
    }

    public static void CreateTree(NodeLuaInfo this_logic, NodeLuaInfo last_logic, BaseNode last_node, int index, Action<int, NodeLuaInfo, BaseNode, BaseNode> setupAction)
    {
        NodeType node_type = (NodeType)Enum.Parse(typeof(NodeType), this_logic.type);

        BaseNode this_node = null;

        if (this_logic.id == 0)
        {
            this_node = AddRootNode(new Vector2(0, 0), this_logic);
        }
        else
        {
            this_node = AddNode(this_logic.type, new Vector2(0, 0), this_logic);
        }

        setupAction(index, last_logic, last_node, this_node);

        if (this_logic.logic_childs.Count > 0)
        {
            for (int i = 0; i < this_logic.logic_childs.Count; i++)
            {
                CreateTree(
                    this_logic.logic_childs[i], //this
                    this_logic, this_node, //last
                    index + 1, //deepth
                    setupAction
                );
            }
        }
    }

    public static void Clear()
    {
        if (nodes_list != null)
        {
            nodes_list.Clear();
        }

        if (connections != null)
        {
            connections.Clear();
        }

        node_total_id = 0;

        current_node = null;

        if (root_node != null)
        {
            BTUtils.RemoveTree(root_node);

            RemoveNode(root_node);
        }

        root_node = null;
    }
}

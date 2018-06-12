using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class NodeLuaInfo
{
    [SerializeField]
    public int id;

    [SerializeField]
    public string name;

    [SerializeField]
    public string title;

    [SerializeField]
    public string desc;

    [SerializeField]
    public string type;

    [SerializeField]
    public Dictionary<string, List<string>> properties;

    [SerializeField]
    public int properties_total_id;

    [SerializeField]
    public List<NodeLuaInfo> logic_childs;

    [SerializeField]
    public int x;

    [SerializeField]
    public int y;

    public Dictionary<string, List<string>> Properties
    {
        get
        {
            return properties;
        }

        set
        {
            properties = value;
        }
    }

    public int Properties_total_id
    {
        get
        {
            return properties_total_id;
        }

        set
        {
            properties_total_id = value;
        }
    }

    public NodeLuaInfo()
    {
        id = 0;

        name = "";

        title = "";

        desc = "";

        type = "";

        properties_total_id = 0;

        x = 0;

        y = 0;

        properties = new Dictionary<string, List<string>>();

        logic_childs = new List<NodeLuaInfo>();
    }

    public NodeLuaInfo(BaseNode node)
    {
        logic_childs = new List<NodeLuaInfo>();

        NodeData.SetupToLogic(node, this);
    }
}

public class NodeData
{
    private static List<NodeLuaInfo> logic_node_list = new List<NodeLuaInfo>();

    public static NodeLuaInfo Get(BaseNode node)
    {
        NodeLuaInfo logic = null;
        for (int i=0; i < logic_node_list.Count; i++)
        {
            if(node.id == logic_node_list[i].id)
            {
                logic = logic_node_list[i];
                break;
            }
        }

        return logic;
    }

    public static List<NodeLuaInfo> GetAll()
    {
        return logic_node_list;
    }

    public static void Add(BaseNode node)
    {
        if(logic_node_list != null)
        {
            NodeLuaInfo logic = new NodeLuaInfo(node);

            logic_node_list.Add(logic);
        }
    }

    public static void Remove(BaseNode node)
    {
        if (logic_node_list != null)
        {
            NodeLuaInfo logic = Get(node);

            for (int i = 0; i < logic_node_list.Count; i++)
            {
                if (logic.id == logic_node_list[i].id)
                {
                    logic_node_list.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public static void UpdateLogic(BaseNode node)
    {
        if (logic_node_list != null)
        {
            for (int i = 0; i < logic_node_list.Count; i++)
            {
                if (node.id == logic_node_list[i].id)
                {
                    SetupToLogic(node, logic_node_list[i]);
                    break;
                }
            }
        }
    }

    public static void SetupToLogic(BaseNode node, NodeLuaInfo logic)
    {
        logic.id = node.id;
        logic.name = node.name;
        logic.type = node.type;
        logic.title = node.title;
        logic.desc = node.desc;
        logic.Properties = node.properties;
        logic.Properties_total_id = node.properties_total_id;
        logic.x = (int)node.rect.x;
        logic.y = (int)node.rect.y;
    }

}
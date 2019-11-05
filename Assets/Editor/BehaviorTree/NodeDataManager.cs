using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataManager
{
    private static List<BaseNodeData> s_nodeDataList = new List<BaseNodeData>();
    private static List<ConnectionData> s_connectionDataList = new List<ConnectionData>();
    private static List<ConnectionPointData> s_connectionPointDataList = new List<ConnectionPointData>();

    public static void Reset()
    {
        s_nodeDataList.Clear();
        s_connectionDataList.Clear();
        s_connectionPointDataList.Clear();
    }

    /// <summary>
    /// 获取节点数据
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static BaseNodeData Get(BaseNode node)
    {
        return Get(node.id);
    }

    /// <summary>
    /// 获取节点数据
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static BaseNodeData Get(int id)
    {
        BaseNodeData nodeData = null;
        for (int i = 0; i < s_nodeDataList.Count; i++)
        {
            if (id == s_nodeDataList[i].id)
            {
                nodeData = s_nodeDataList[i];
                break;
            }
        }

        return nodeData;
    }

    /// <summary>
    /// 获取连接数据
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static ConnectionData Get(Connection connection)
    {
        ConnectionData data = null;
        for (int i = 0; i < s_connectionDataList.Count; i++)
        {
            if (connection.connectId.Equals(s_connectionDataList[i].connectId))
            {
                data = s_connectionDataList[i];
                break;
            }
        }

        return data;
    }

    /// <summary>
    /// 获取连接数据
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static ConnectionPointData Get(ConnectionPoint connectionPoint)
    {
        ConnectionPointData data = null;
        for (int i = 0; i < s_connectionPointDataList.Count; i++)
        {
            if (connectionPoint.id.Equals(s_connectionPointDataList[i].id))
            {
                data = s_connectionPointDataList[i];
                break;
            }
        }

        return data;
    }

    /// <summary>
    /// 创建节点数据
    /// </summary>
    /// <param name="node"></param>
    public static T CreateNodeData<T>(BaseNode node) where T : BaseNodeData, new()
    {
        T data = default(T);
        if (s_nodeDataList != null)
        {
            data = new T();
            data.Serialize(node);
            s_nodeDataList.Add(data);
        }

        return data;
    }

    /// <summary>
    /// 创建连接数据
    /// </summary>
    /// <param name="node"></param>
    public static ConnectionData CreateConnectionData(Connection connection)
    {
        ConnectionData connectionData = null;
        if (s_nodeDataList != null)
        {
            connectionData = new ConnectionData();
            connectionData.Serialize(connection);
            s_connectionDataList.Add(connectionData);
        }
        return connectionData;
    }

    /// <summary>
    /// 创建连接点数据
    /// </summary>
    /// <param name="node"></param>
    public static ConnectionPointData CreateConnectionPointData(ConnectionPoint connectionPoint)
    {
        ConnectionPointData pointData = null;
        pointData = new ConnectionPointData();
        pointData.Serialize(connectionPoint);
        s_connectionPointDataList.Add(pointData);
        return pointData;
    }

    public static void Remove(BaseNode node)
    {
        BaseNodeData logicInfo = Get(node);
        for (int i = 0; i < s_nodeDataList.Count; i++)
        {
            if (logicInfo.id == s_nodeDataList[i].id)
            {
                s_nodeDataList.RemoveAt(i);
                break;
            }
        }
    }

    public static void Remove(Connection connection, BaseNodeData outNodeData)
    {
        ConnectionData data = Get(connection);
        for (int i = 0; i < s_connectionDataList.Count; i++)
        {
            if (data.id == s_connectionDataList[i].id)
            {
                s_connectionDataList.RemoveAt(i);
                outNodeData.connectionList.Remove(s_connectionDataList[i]);
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyConfigGenWorker
{
    public void Export(BaseNode rootNode, string configName)
    {
        string configPath = BTUtils.GetGenPath() + configName + ".json";
        string fullConfigPath = BTUtils.GetGenPath() + configName + "_full.json";
        string nodeMapPath = BTUtils.GetGenPath() + configName + "_node_map.json";

        Dictionary<string, BaseNodeData> baseNodeDataDict = new Dictionary<string, BaseNodeData>();
        Dictionary<string, MergePyData> dataDict = new Dictionary<string, MergePyData>();
        BTUtils.DumpTree(rootNode, (BaseNode node) =>
        {
            BaseNodeData nodeData = NodeDataManager.Get(node);
            if(nodeData != null)
            {
                nodeData.Serialize(node);
                dataDict[node.name] = new MergePyData(node);
                baseNodeDataDict[node.name] = nodeData;
            }
        });

        BTUtils.SaveJsonToFile<Dictionary<string, MergePyData>>(dataDict, configPath);

        BaseNodeData rootNodeData = NodeDataManager.Get(rootNode);
        BTUtils.SaveJsonToFile<BaseNodeData>(rootNodeData, fullConfigPath);

        BTUtils.SaveJsonToFile<Dictionary<string, BaseNodeData>>(baseNodeDataDict, nodeMapPath);
    }

    private static Dictionary<string, BaseNodeData> nodeMap = new Dictionary<string, BaseNodeData>();
    public void Import(string configName)
    {
        BTEditorManager.Clear();

        //string configPath = BTUtils.GetGenPath() + configName + ".json";
        //string fullConfigPath = BTUtils.GetGenPath() + configName + "_full.json";
        //BaseNodeData rootNodeData = BTUtils.GetJsonFromFile<BaseNodeData>(fullConfigPath);
        string nodeMapPath = BTUtils.GetGenPath() + configName + "_node_map.json";
        nodeMap = BTUtils.GetJsonFromFile<Dictionary<string, BaseNodeData>>(nodeMapPath);

        BaseNodeData rootNodeData = GetNodeMapData(0);
        CreateTree(0, null, rootNodeData, null);
    }

    private void CreateTree(int deepth, BaseNodeData lastNodeData, BaseNodeData nodeDummyData, ConnectionData connectionData)
    {
        if (deepth >= 10)
            return;

        //创建当前节点
        BaseNode node = null;
        if (nodeDummyData != null)
        {
            node = BTEditorManager.AddNode<ExcelNode>(new Vector2(nodeDummyData.x, nodeDummyData.y));
            nodeDummyData.DeSerialize(ref node);
        }

        //创建connection连线
        if (lastNodeData != null && connectionData != null)
        {
            BaseNode lastNode = BTEditorManager.GetObject<BaseNode>(lastNodeData.id);
            Connection connection = BTEditorManager.CreateConnection(lastNode.outPoint, node.inPoint);
            connectionData.DeSerialize(ref connection);
        }

        //遍历下一个connection
        for (int i = 0; i < nodeDummyData.connectionList.Count; i++)
        {
            ConnectionData nextConnectionData = nodeDummyData.connectionList[i];
            BaseNodeData targetNodeData = GetNodeMapData(nextConnectionData.targetNodeId);
            if (targetNodeData == null)
            {
                Debug.LogError(string.Format("找不到{0}的节点!", nextConnectionData.targetNodeId));
                return;
            }
               
            CreateTree(
                deepth + 1, //deepth
                nodeDummyData, //last
                targetNodeData, //this
                nextConnectionData
            );
        }
    }

    private static BaseNodeData GetNodeMapData(int id)
    {
        BaseNodeData result = null;
        if (nodeMap != null)
        {
            foreach (var kv in nodeMap)
            {
                string nodeName = kv.Key;
                BaseNodeData nodeData = kv.Value;
                if (nodeData.id.Equals(id))
                {
                    result = nodeData;
                    break;
                }
            }
        }

        return result;
    }
}

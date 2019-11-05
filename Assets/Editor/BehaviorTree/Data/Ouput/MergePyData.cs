using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MergePyData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string file;
    [SerializeField]
    public bool is_collect_info;
    [SerializeField]
    public List<Dictionary<string, string>> connections;

    public MergePyData(BaseNode node)
    {
        this.connections = new List<Dictionary<string, string>>();

        if (node.type == NodeType.ExcelNode)
        {
            ExcelNodeData data = (ExcelNodeData)NodeDataManager.Get(node);
            if (data != null)
            {
                this.name = data.name;
                this.file = data.file;
                this.is_collect_info = false;
                if (data.properties.ContainsKey("is_collect_info") && data.properties["is_collect_info"][0] == "True")
                {
                    this.is_collect_info = true;
                }

                if (data.connectionList != null)
                {
                    foreach (ConnectionData connection in data.connectionList)
                    {
                        foreach (SlotData slotData in connection.slotList)
                        {
                            Dictionary<string, string> slotDic = new Dictionary<string, string>()
                            {
                                {"in_slot", slotData.in_slot },
                                {"node", slotData.node },
                                {"out_slot", slotData.out_slot }
                            };

                            connections.Add(slotDic);
                        }
                    }
                }
            }
        }
        
    }
}
using System;
using UnityEngine;

[Serializable]
public class SlotData
{
    [SerializeField]
    public string out_slot;
    [SerializeField]
    public string node;
    [SerializeField]
    public string in_slot;

    public SlotData()
    {
    }

    public void SetupConnect(ConnectionData connectionData)
    {
        int index = connectionData.slotList.Count + 1;
        this.out_slot = "out_" + index;
        this.node = "node_" + index;
        this.in_slot = "in_" + index;

        BaseNode baseNode = BTEditorManager.GetObject<BaseNode>(connectionData.targetNodeId);
        this.node = baseNode.name;
    }
}
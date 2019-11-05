using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseNodeData : BTBaseData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public NodeType type;
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public ConnectionPointData inPoint;
    [SerializeField]
    public ConnectionPointData outPoint;
    [SerializeField]
    public Dictionary<string, List<string>> properties;
    [SerializeField]
    public List<ConnectionData> connectionList;

    public BaseNodeData()
    {
        this.properties = new Dictionary<string, List<string>>();
        this.connectionList = new List<ConnectionData>();
    }

    public virtual void Serialize(BaseNode node)
    {
        base.SetId(node);

        this.name = node.name;
        this.x = (int)node.rect.x;
        this.y = (int)node.rect.y;
        this.outPoint = NodeDataManager.Get(node.outPoint);
        this.inPoint = NodeDataManager.Get(node.inPoint);
        this.properties = node.properties;
    }

    public virtual void DeSerialize(ref BaseNode node)
    {
        BaseNodeData oldData = NodeDataManager.Get(node);
        //this可能不是node对应的那份数据
        oldData.Sync(this);

        node.id = this.id;
        node.name = this.name;
        node.rect.x = this.x;
        node.rect.y = this.y;
        //node.outPoint = BTEditorManager.GetObject<ConnectionPoint>(this.outPoint);
        //node.inPoint = BTEditorManager.GetObject<ConnectionPoint>(this.inPoint);
        node.properties = this.properties;
    }

    public virtual void Sync(BaseNodeData otherData)
    {
        this.id = otherData.id;
        this.name = otherData.name;
        this.x = (int)otherData.x;
        this.y = (int)otherData.y;
        this.outPoint = otherData.outPoint;
        this.inPoint = otherData.inPoint;
        this.properties = otherData.properties;
    }
}
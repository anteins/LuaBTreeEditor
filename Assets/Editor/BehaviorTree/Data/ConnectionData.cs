using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConnectionData : BTBaseData
{
    [SerializeField]
    public string connectId;
    [SerializeField]
    public int targetNodeId;
    [SerializeField]
    public List<SlotData> slotList;
    [SerializeField]
    public ConnectionPointData inPoint;
    [SerializeField]
    public ConnectionPointData outPoint;

    public ConnectionData()
    {
        this.slotList = new List<SlotData>();
    }

    public virtual void Serialize(Connection connection)
    {
        base.SetId(connection);

        //Debug.Log(">connection data " + this.id);
        this.connectId = connection.connectId;
        this.slotList.Clear();
        foreach (SlotData slot in connection.slotList)
        {
            this.slotList.Add(slot);
        }

        this.outPoint = NodeDataManager.Get(connection.outPoint);
        this.inPoint = NodeDataManager.Get(connection.inPoint);
    }

    public virtual void DeSerialize(ref Connection connection)
    {
        base.SetObjectId(connection);

        connection.connectId = this.connectId;
        connection.slotList.Clear();
        foreach (SlotData slot in this.slotList)
        {
            connection.slotList.Add(slot);
        }

        //connection.outPoint = BTEditorManager.GetObject<ConnectionPoint>(this.outPoint);
        //connection.inPoint = BTEditorManager.GetObject<ConnectionPoint>(this.inPoint);
    }
}
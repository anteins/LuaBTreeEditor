using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConnectionPointData : BTBaseData
{
    [SerializeField]
    public ConnectionPointType type;

    public ConnectionPointData()
    {
    }

    public virtual void Serialize(ConnectionPoint connectionPoint)
    {
        base.SetId(connectionPoint);
        //Debug.Log(">point data " + this.id);
        this.type = connectionPoint.type;
    }

    public virtual void DeSerialize(ref ConnectionPoint connectionPoint)
    {
        base.SetObjectId(connectionPoint);

        connectionPoint.type = this.type;
    }
}
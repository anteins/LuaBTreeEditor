using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BTBaseData
{
    [SerializeField]
    public int id;

    public void SetId(BTObject btObject)
    {
        this.id = btObject.id;
    }

    public void SetObjectId(BTObject btObject)
    {
        btObject.id = this.id;
    }
}
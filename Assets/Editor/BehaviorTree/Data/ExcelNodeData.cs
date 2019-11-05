using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExcelNodeData : BaseNodeData
{
    [SerializeField]
    public string file;

    public override void Serialize(BaseNode node)
    {
        base.Serialize(node);
        ExcelNode excelNode = (ExcelNode)node;
        this.file = excelNode.file;
    }

    public override void DeSerialize(ref BaseNode node)
    {
        base.DeSerialize(ref node);
        ExcelNode excelNode = (ExcelNode)node;
        excelNode.file = this.file;
    }
}

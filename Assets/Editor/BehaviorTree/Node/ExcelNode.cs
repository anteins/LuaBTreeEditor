using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelNode : BaseNode
{
    public string file;

    public ExcelNode() : base()
    {
        type = NodeType.ExcelNode;
        desc = "ExcelNode desc...";
    }

    public override void Init(Vector2 position, Action<BaseNode> OnClickSelf)
    {
        base.Init(position, OnClickSelf);
        this.file = name + ".xls";
    }
}
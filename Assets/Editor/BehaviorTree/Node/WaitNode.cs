using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaitNode : BaseNode
{
    public WaitNode() : base()
    {
        type = NodeType.WaitNode;
        desc = "WaitNode desc...";
    }
}
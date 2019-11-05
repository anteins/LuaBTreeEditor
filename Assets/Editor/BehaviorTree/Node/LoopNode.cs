using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoopNode : BaseNode
{
    public LoopNode() : base()
    {
        type = NodeType.LoopNode;
        desc = "LoopNode desc...";
    }
}
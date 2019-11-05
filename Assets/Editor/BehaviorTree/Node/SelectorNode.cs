using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectorNode : BaseNode
{
    public SelectorNode() : base()
    {
        type = NodeType.SelectorNode;
        desc = "SelectorNode desc...";
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionNode : BaseNode
{
    public ActionNode() : base()
    {
        type = NodeType.ActionNode;
        desc = "ActionNode desc...";
    }
}
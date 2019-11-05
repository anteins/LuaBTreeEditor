using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviourNode : BaseNode
{
    public BehaviourNode() : base()
    {
        type = NodeType.BehaviourNode;
        desc = "BehaviourNode desc...";
    }
}
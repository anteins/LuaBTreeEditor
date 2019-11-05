using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SequenceNode : BaseNode
{
    public SequenceNode() : base()
    {
        type = NodeType.SequenceNode;
        desc = "SequenceNode desc...";
    }
}
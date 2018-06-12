using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SequenceNode : BaseNode
{
    public SequenceNode(Vector2 position,
        Action<BaseNode> OnClickSelf,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "SequenceNode";

        desc = "SequenceNode desc...";

        InitLogic();
    }
}
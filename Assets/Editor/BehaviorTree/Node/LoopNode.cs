using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoopNode : BaseNode
{
    public LoopNode(Vector2 position,
        Action<BaseNode> OnClickSelf,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "LoopNode";

        desc = "LoopNode desc...";

        InitLogic();
    }
}
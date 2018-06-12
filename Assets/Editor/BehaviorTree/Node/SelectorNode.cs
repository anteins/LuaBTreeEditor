using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectorNode : BaseNode
{
    public SelectorNode(Vector2 position,
        Action<BaseNode> OnClickSelf,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "SelectorNode";

        desc = "SelectorNode desc...";

        InitLogic();
    }
}
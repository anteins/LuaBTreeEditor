using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaitNode : BaseNode
{
    public WaitNode(Vector2 position,
        Action<BaseNode> OnClickSelf, 
        Action<ConnectionPoint> OnClickInPoint, 
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "WaitNode";

        desc = "WaitNode desc...";

        InitLogic();
    }
}
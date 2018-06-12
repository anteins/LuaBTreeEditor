using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviourNode : BaseNode
{
    public BehaviourNode(Vector2 position,
        Action<BaseNode> OnClickSelf,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "BehaviourNode";

        desc = "BehaviourNode desc...";

        InitLogic();
    }
}
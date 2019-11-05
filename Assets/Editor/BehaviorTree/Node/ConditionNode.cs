using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConditionNode : BaseNode
{
    public ConditionNode() : base()
    {
        type = NodeType.ConditionNode;

        desc = @"
            判定节点
            创建这个节点的时候传入一个能够获取判定值的方法，这个节点会根据运行到这个节点时，传入方法的调用后返回的值，改变当前节点的状态。nil 或者 false 转换为FAILED，否则转换为SUCCESS
        ";
    }
}
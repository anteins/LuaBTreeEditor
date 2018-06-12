using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConditionNode : BaseNode
{
    public ConditionNode(Vector2 position,
        Action<BaseNode> OnClickSelf,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint) : base(position, OnClickSelf, OnClickInPoint, OnClickOutPoint)
    {
        type = "ConditionNode";

        desc = @"
            �ж��ڵ�
            ��������ڵ��ʱ����һ���ܹ���ȡ�ж�ֵ�ķ���������ڵ��������е�����ڵ�ʱ�����뷽���ĵ��ú󷵻ص�ֵ���ı䵱ǰ�ڵ��״̬��nil ���� false ת��ΪFAILED������ת��ΪSUCCESS
        ";

        InitLogic();
    }
}
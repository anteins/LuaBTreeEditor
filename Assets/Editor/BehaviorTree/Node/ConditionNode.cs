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
            �ж��ڵ�
            ��������ڵ��ʱ����һ���ܹ���ȡ�ж�ֵ�ķ���������ڵ��������е�����ڵ�ʱ�����뷽���ĵ��ú󷵻ص�ֵ���ı䵱ǰ�ڵ��״̬��nil ���� false ת��ΪFAILED������ת��ΪSUCCESS
        ";
    }
}
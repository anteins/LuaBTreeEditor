using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BTObject
{
    public int id;

    public void GenId()
    {
        this.id = BTEditorManager.curObjectId++;
    }
}
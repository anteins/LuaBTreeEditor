using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BTEditorWindow : EditorWindow
{
    private float menuBarHeight = 20f;
    private Rect menuBar;

    public static GUIStyle nodeStyle;

    public static GUIStyle selectedNodeStyle;

    public static GUIStyle inPointStyle;

    public static GUIStyle outPointStyle;

    public static BTEditorWindow _window;

    private Vector3 scrollPos = Vector2.zero;

    private List<Vector2> _v2FieldList = new List<Vector2>();

    public Rect window_Rect;

    public Rect subWindow_Rect;

    private static BaseNode current_node = null;

    private static Dictionary<string, List<string>> current_properties = new Dictionary<string, List<string>>();

    private static List<string> current_properties_remove_list = new List<string>();

    private static int current_properties_total_id = 0;

    private static string current_description = "";

    private static string current_title = "";

    private EditorGUILayout scale = null;

    [MenuItem("行为树/显示窗口")]
    public static void ShowWindow ()
    {
        if (_window == null)
        {
            _window = EditorWindow.GetWindow<BTEditorWindow>();
        }
        
        Reset();
        Vector2 initPosition = new Vector2(10, _window.position.height / 2 - 25);
        BaseNode root = BTEditorManager.AddRootNode(initPosition);
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        //字体大小
        nodeStyle.fontSize = 12;
        //文字颜色
        nodeStyle.normal.textColor = new Color(0.1f, 0.8f, 0.1f);
        //文字位置上下左右居中
        nodeStyle.alignment = TextAnchor.MiddleCenter;

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    public void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add RootNode"), false, () => BTEditorManager.AddRootNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add ActionNode"), false, () => BTEditorManager.AddNode<ActionNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add ConditionNode"), false, () => BTEditorManager.AddNode<ConditionNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add WaitNode"), false, () => BTEditorManager.AddNode<WaitNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add SequenceNode"), false, () => BTEditorManager.AddNode<SequenceNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add SelectorNode"), false, () => BTEditorManager.AddNode<SelectorNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add LoopNode"), false, () => BTEditorManager.AddNode<LoopNode>(mousePosition));
        
        genericMenu.ShowAsContext();
    }

    // Update is called once per frame
    public void OnGUI ()
    {
        DrawWindowUI();

        ProcessMainEvents(Event.current);

        //if (GUI.changed)
        //{


        //}

        Repaint();
    }

    private void DrawWindowUI()
    {
        DrawMenuBar();

        BeginWindows();

        DrawNodeWindow();

        BTEditorManager.Draw();

        EndWindows();
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            BTEditorManager.Save();
        }

        GUILayout.Space(5);
        if(GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            BTEditorManager.Load();
        }

        if (GUILayout.Button(new GUIContent("Clear"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            BTEditorManager.Clear();
        }

        if (GUILayout.Button(new GUIContent("Print"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            BTUtils.DumpTree(BTEditorManager.root_node);
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawNodeWindow()
    {
        if(_window == null)
        {
            _window = EditorWindow.GetWindow<BTEditorWindow>();
        }
        subWindow_Rect = GUI.Window(22, new Rect(_window.position.width * 2 / 3, 0, _window.position.width / 3, _window.position.height), DrawSubUI, "node info");
    }

    public void DrawSubUI(int id)
    {
        //title
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title", GUILayout.MaxWidth(80));
        current_title = EditorGUILayout.TextArea(current_title, GUILayout.MaxHeight(25));
        GUILayout.EndHorizontal();

        //desc
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Description", GUILayout.MaxWidth(80));
        current_description = EditorGUILayout.TextArea(current_description, GUILayout.MaxHeight(75));
        GUILayout.EndHorizontal();

        foreach (string key in current_properties.Keys)
        {
            GUILayout.BeginHorizontal("properties");
            current_properties[key][0] = EditorGUILayout.TextArea(current_properties[key][0], GUILayout.MaxHeight(25));
            current_properties[key][1] = EditorGUILayout.TextArea(current_properties[key][1], GUILayout.MaxHeight(25));
            if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
            {
                current_properties_remove_list.Add(key);
            }
            GUILayout.EndHorizontal();
        }
        for(int i =0; i< current_properties_remove_list.Count; i++)
        {
            string remove_key = current_properties_remove_list[i];
            current_properties.Remove(remove_key);
            GUI.changed = true;
        }
        current_properties_remove_list.Clear();

        if (GUILayout.Button("+", GUILayout.MaxWidth(30)))
        {
            if (current_node != null)
            {
                if (current_properties != null && current_properties.Count < 7)
                {
                    current_properties_total_id = current_properties_total_id + 1;
                    current_properties.Add(current_properties_total_id.ToString(), new List<string> { string.Format("New_{0}", current_properties_total_id.ToString()), "Default" });
                    GUI.changed = true;
                }
            }
        }

        if (GUILayout.Button("Apply", GUILayout.MaxWidth(subWindow_Rect.width)))
        {
            BTEditorManager.UpdateCurrentNode(current_title, current_description, current_properties);
        }
    }

    public static void OnSelectNode(BaseNode node)
    {
        current_node = node;
        NodeLuaInfo logic = NodeData.Get(current_node);
        current_title = logic.title;
        current_description = logic.desc;
        current_properties = logic.Properties;
        current_properties_total_id = logic.Properties_total_id;
        
        //Debug.Log(string.Format("当前node \nid:{0} \nname:{1} \ntitle:{2} \ndesc:{3}", logic.id, logic.name, logic.title, logic.desc));
    }

    public static void Reset()
    {
        BTEditorManager.Clear();
    }

    public static void CreateNode(NodeLuaInfo root_logic)
    {
       
    }

    private void ProcessMainEvents(Event e)
    {
        if (subWindow_Rect.Contains(e.mousePosition))
        {
            ProcessSubWindowEvents(e);
        }
        else
        {
            ProcessNodeWindowEvents(e);
        }
    }

    public void ProcessSubWindowEvents(Event e)
    {
    }

    public void ProcessNodeWindowEvents(Event e)
    {
        //logic node 
        BTEditorManager.ProcessEvents(Event.current);

        //main editor
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {

                    BTEditorManager.ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0)
                {
                    BTEditorManager.ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    BTEditorManager.OnDrag(e.delta);
                }
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum SubViewType
{
    None,
    Node,
    Connection,
}

public class BTEditorWindow : EditorWindow
{
    private float menuBarHeight = 20f;
    private Rect menuBar;

    public static GUIStyle nodeStyle;
    public static GUIStyle selectedNodeStyle;

    public static GUIStyle inPointStyle;
    public static GUIStyle outPointStyle;

    public static BTEditorWindow _window;
    public Rect window_Rect;
    public Rect subWindow_Rect;
    private Vector2 m_ScrollPosition;

    private static SubViewType _curSubView = SubViewType.None;
    private static string _subTitle = string.Empty;
    //Properties View
    private static List<string> _properties_remove_list = new List<string>();
    //Slots View
    private static List<SlotData> _slots_remove_list = new List<SlotData>();

    [MenuItem("节点/显示窗口")]
    public static void ShowWindow()
    {
        if (_window == null)
        {
            _window = EditorWindow.GetWindow<BTEditorWindow>();
        }

        Reset();
        Vector2 initPosition = new Vector2(10, _window.position.height / 2 - 25);
        //BaseNode root = BTEditorManager.AddRootNode(initPosition);

        NodeDataManager.Reset();
        BTEditorManager.Reset();
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

    public static void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        //genericMenu.AddItem(new GUIContent("Add RootNode"), false, () => BTEditorManager.AddRootNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add ExcelNode"), false, () => BTEditorManager.AddNode<ExcelNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add ActionNode"), false, () => BTEditorManager.AddNode<ActionNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add ConditionNode"), false, () => BTEditorManager.AddNode<ConditionNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add WaitNode"), false, () => BTEditorManager.AddNode<WaitNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add SequenceNode"), false, () => BTEditorManager.AddNode<SequenceNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add SelectorNode"), false, () => BTEditorManager.AddNode<SelectorNode>(mousePosition));
        genericMenu.AddItem(new GUIContent("Add LoopNode"), false, () => BTEditorManager.AddNode<LoopNode>(mousePosition));

        genericMenu.ShowAsContext();
    }

    public void OnGUI ()
    {
        DrawWindowUI();
        ProcessMainEvents(Event.current);
        Repaint();
    }

    private void DrawWindowUI()
    {
        DrawMenuBar();

        BeginWindows();//Begin Render
        DrawNodeWindow();
        BTEditorManager.Draw();
        EndWindows();//End Render
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
            BTUtils.DumpTree(BTEditorManager.rootNode);
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
        subWindow_Rect = GUI.Window(22, new Rect(_window.position.width * 2 / 3, 0, _window.position.width / 3, _window.position.height), DrawSubUI, _subTitle);
    }

    public void DrawSubUI(int id)
    {
        if (BTEditorManager.selectedNode == null && BTEditorManager.selectedConnection == null)
        {
            GUILayout.Label("没有选中任何节点", GUILayout.MaxWidth(180));
            return;
        }

        m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, GUILayout.Width(subWindow_Rect.width - 15), GUILayout.Height(400));
        switch (_curSubView)
        {
            case SubViewType.Node:
                DrawProperties();
                var node = BTEditorManager.selectedNode;
                if (node.properties != null)
                {
                    BTEditorManager.SaveCurrentNode();
                }
                break;
            case SubViewType.Connection:
                DrawSlots();
                var connection = BTEditorManager.selectedConnection;
                if (connection.slotList != null)
                {
                    BTEditorManager.SaveCurrentConnection();
                }
                break;
            default:
                break;
        }
        GUI.changed = true;
        GUILayout.EndScrollView();
    }

    private void DrawProperties()
    {
        var node = BTEditorManager.selectedNode;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", GUILayout.MaxWidth(80));
        node.name = EditorGUILayout.TextArea(node.name, GUILayout.MaxHeight(25));
        GUILayout.EndHorizontal();

        if(BTEditorManager.selectedNode.type == NodeType.ExcelNode)
        {
            ExcelNode excelNode = (ExcelNode)BTEditorManager.selectedNode;
            GUILayout.BeginHorizontal();
            GUILayout.Label("File", GUILayout.MaxWidth(80));
            excelNode.file = EditorGUILayout.TextArea(excelNode.file, GUILayout.MaxHeight(25));
            GUILayout.EndHorizontal();
        }

        GUILayout.Label("Properties");
        foreach (string key in node.properties.Keys)
        {
            GUILayout.BeginHorizontal("");
            node.properties[key][0] = EditorGUILayout.TextArea(node.properties[key][0], GUILayout.MaxHeight(25));
            node.properties[key][1] = EditorGUILayout.TextArea(node.properties[key][1], GUILayout.MaxHeight(25));
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _properties_remove_list.Add(key);
            }
            GUILayout.EndHorizontal();
        }

        for (int i = 0; i < _properties_remove_list.Count; i++)
        {
            string remove_key = _properties_remove_list[i];
            node.properties.Remove(remove_key);
            GUI.changed = true;
        }

        _properties_remove_list.Clear();
        if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
        {
            int count = node.properties.Count + 1;
            node.properties.Add(count.ToString(), new List<string> { string.Format("Properties_{0}", count.ToString()), "Default" });
            GUI.changed = true;
        }
    }

    private void DrawSlots()
    {
        var connection = BTEditorManager.selectedConnection;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", GUILayout.MaxWidth(80));
        connection.connectId = EditorGUILayout.TextArea(connection.connectId, GUILayout.MaxHeight(25));
        GUILayout.EndHorizontal();

        GUILayout.Label("Slots");
        for (int i = 0; i < connection.slotList.Count; i++)
        {
            GUILayout.BeginHorizontal("");
            connection.slotList[i].out_slot = EditorGUILayout.TextArea(connection.slotList[i].out_slot, GUILayout.MaxHeight(25));
            connection.slotList[i].in_slot = EditorGUILayout.TextArea(connection.slotList[i].in_slot, GUILayout.MaxHeight(25));

            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _slots_remove_list.Add(connection.slotList[i]);
            }
            GUILayout.EndHorizontal();
        }

        for (int i = 0; i < _slots_remove_list.Count; i++)
        {
            SlotData remove_key = _slots_remove_list[i];
            connection.slotList.Remove(remove_key);
            GUI.changed = true;
        }

        _slots_remove_list.Clear();
        if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
        {
            var connectionData = NodeDataManager.Get(connection);
            SlotData slotData = new SlotData();
            slotData.SetupConnect(connectionData);
            connection.slotList.Add(slotData);
            GUI.changed = true;
        }
    }

    public static void UpdateSubWindow(BaseNode node)
    {
        _subTitle = "node info";
        _curSubView = SubViewType.Node;

        //取消聚焦
        GUI.FocusControl(null);
    }

    public static void UpdateSubWindow(Connection connection)
    {
        _subTitle = "connection info";
        _curSubView = SubViewType.Connection;

        //取消聚焦
        GUI.FocusControl(null);
    }

    public static void Reset()
    {
        BTEditorManager.Clear();
    }

    private void ProcessMainEvents(Event e)
    {
        if (subWindow_Rect.Contains(e.mousePosition))
        {
            ProcessSubWindowEvents(e);
        }
        else
        {
            ProcessMainWindowEvents(e);
        }
    }

    public void ProcessMainWindowEvents(Event e)
    {
        if (BTEditorManager.ProcessEvents(Event.current))
        {
            return;
        }

        switch (e.type)
        {
            case EventType.MouseDown:
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

    public void ProcessSubWindowEvents(Event e)
    {
    }
}

using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    Dialogue selectedDialogue = null;
    [NonSerialized]
    GUIStyle nodeStyle;
    [NonSerialized]
    GUIStyle playerNodeStyles;
    [NonSerialized]
    DialogueNode draggingNode = null;
    [NonSerialized]
    Vector2 draggingOffset;
    [NonSerialized]
    DialogueNode creatingNode = null;
    [NonSerialized]
    DialogueNode deleatingNode = null;
    [NonSerialized]
    DialogueNode linkingParentNode = null;
    Vector2 scrollPosition;
    [NonSerialized]
    bool draggingCanvas=false;
    [NonSerialized]
    Vector2 draggingCanvasOffset;

    const float canvasSize = 4000;
    const float backgroundSize = 50;

    // Callback to open the Dialogue editor window from Windows tab
    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "DialogueEditor");
    }

    // Adding this attribute to a static method will make the method be called when unity is about to open an asset
    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        // Only call the method when a dialogue scriptable object is opened
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
        if(dialogue!=null)
        {
            ShowEditorWindow();
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        playerNodeStyles = new GUIStyle();
        playerNodeStyles.normal.background = EditorGUIUtility.Load("node6") as Texture2D;
        playerNodeStyles.padding = new RectOffset(20, 20, 20, 20);
        playerNodeStyles.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnSelectionChanged()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue;
        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint();
        }
            
    }

    // OnGUI is called for rendering and handling GUI events.
    private void OnGUI()
    {
        // Autolayout helps draw the fields on GUI in the order they are presented
        if (selectedDialogue == null)
            EditorGUILayout.LabelField("No Dialogue selected");
        else
        {
            ProcessEvents();
            scrollPosition= EditorGUILayout.BeginScrollView(scrollPosition);
            Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            Texture2D backgroundTex = Resources.Load("background") as Texture2D;
            Rect texCoords = new Rect(0, 0, canvasSize/ backgroundSize, canvasSize / backgroundSize);
            GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            EditorGUILayout.EndScrollView();
            if(creatingNode!=null)
            {
               
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
            if (deleatingNode != null)
            {
                selectedDialogue.DeleteNode(deleatingNode);
                deleatingNode = null;
            }
            
        }
    }

   
    private void ProcessEvents()
    {
        // When we click on the mouse while no node is seleced
        if(Event.current.type == EventType.MouseDown && draggingNode==null)
        {
            // Search for nodes at point of mouse click
            draggingNode = GetNodeAtPoint(Event.current.mousePosition+scrollPosition);

            // If a node was found at point of mouse click
            if(draggingNode!=null)
            {
                // Setting dragging offser
                draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;

                // Setting Active Objeect as dragging node
                Selection.activeObject = draggingNode;
            }
            // if a node was not found
            else
            {
                // dragging canvas is set true
                draggingCanvas = true;
                // Setting dragging canvas offset
                draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                // Setting active object to selected dialogue
                Selection.activeObject = selectedDialogue;
                // We basically scroll in the editor window
            }
        }
        // if we are dragging and dragging node is not null
        else if(Event.current.type == EventType.MouseDrag && draggingNode!=null)
        {
            // setting the position of dragging node to mouse position + offset
            draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
            GUI.changed = true;
        }
        // if we are dragging and dragging node is null
        else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
        {
            // setting the scroll position to offset - mouse position
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            GUI.changed = true;
        }
        // when mouse up and dragging node is not null we set dragging node to null
        else if(Event.current.type == EventType.MouseUp && draggingNode!=null)
        {
            draggingNode = null;
        }
        // when mouse up and dragging canvas is true we set dragging canvas to false
        else if (Event.current.type == EventType.MouseUp && draggingCanvas)
        {
            draggingCanvas = false;
        }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;
        foreach(DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if(node.GetRect().Contains(point))
            {
                foundNode = node;
            }
        }
        return foundNode;
    }

    private void DrawNode(DialogueNode node)
    {
        GUIStyle style = nodeStyle;
       if(node.IsPlayerSpeaking())
        {
            style = playerNodeStyles;
        }
        GUILayout.BeginArea(node.GetRect(), style);

        node.SetText(EditorGUILayout.TextField(node.GetText()));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            creatingNode = node;
        }
        DwarLinkButtons(node);
        if (GUILayout.Button("Delete"))
        {
            deleatingNode = node;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DwarLinkButtons(DialogueNode node)
    {
        if (linkingParentNode == null)
        {
            if (GUILayout.Button("Link"))
            {
                linkingParentNode = node;
            }
        }
        else if(linkingParentNode==node)
        {
            if (GUILayout.Button("Cancel"))
            {
                linkingParentNode = null;
            }
        }
        else if(linkingParentNode.GetChildren().Contains(node.name))
        {
            if (GUILayout.Button("Unlink"))
            {
                linkingParentNode.RemoveChild(node.name);
                linkingParentNode = null;
            }
        }
        else
        {
            if (GUILayout.Button("Child"))
            {
                linkingParentNode.AddChild(node.name);
                linkingParentNode = null;
            }
        }
    }


    // Links are drawn using Bazier curves from the xMax on x axis and center point in y axis
    // of RectTransform of parent node to the xMin on x axis and center point in y axis 
    // of RectTransform of child node
    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
            Vector3 controlPointOffset = endPosition - startPosition;
            controlPointOffset.y = 0;
            controlPointOffset.x *= 0.8f;
            Handles.DrawBezier(startPosition, endPosition, 
                startPosition + controlPointOffset,  endPosition - controlPointOffset, 
                Color.white, null, 4f);
        }
    }

}

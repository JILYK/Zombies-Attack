using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PivotToolsScript : EditorWindow
{
    private readonly Stack<(RectTransform, Vector2)> undoStack = new();

    public void OnGUI()
    {
        GUILayout.Label("Pivot Maker", EditorStyles.boldLabel);

        if (GUILayout.Button("Adjust Anchors"))
        {
            AdjustAnchors();
        }
    }

    [MenuItem("Tools/Pivot Maker Tools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PivotToolsScript));
    }

    private void AdjustAnchors()
    {
        var canvas = GameObject.Find("Canvas");

        foreach (var o in canvas.GetComponentsInChildren<Transform>())
        {
            if (o == null)
            {
                return;
            }
 
            var r = o.GetComponent<RectTransform>();
 
            if (r == null || r.parent == null)
            {
                continue;
            }
 
            Undo.RecordObject(o, "SnapAnchors");
            AnchorRect(r);            
        }
    }
    
    static void AnchorRect(RectTransform r)
    {
        var p = r.parent.GetComponent<RectTransform>();
 
        var offsetMin = r.offsetMin;
        var offsetMax = r.offsetMax;
        var _anchorMin = r.anchorMin;
        var _anchorMax = r.anchorMax;
 
        var parent_width = p.rect.width;
        var parent_height = p.rect.height;
 
        var anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
            _anchorMin.y + (offsetMin.y / parent_height));
        var anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
            _anchorMax.y + (offsetMax.y / parent_height));
 
        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
 
        r.offsetMin = new Vector2(0, 0);
        r.offsetMax = new Vector2(0, 0);
        r.pivot = new Vector2(0.5f, 0.5f);
    }
}
using UnityEngine;
using UnityEditor;

public class ToolBuilderW_ : EditorWindow
{
    [MenuItem("Tools/House Builder")]
    public static void ShowWindow()
    {
        GetWindow<ToolBuilderW_>("House Builder Tool");
    }
    private int DiscSize;
    private bool visibleArea;
    Vector3 prevPos;
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void OnGUI()
    {
        GUILayout.Space(10);
        GUIStyle TitleStyle = new()
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.softBlue},
            alignment = TextAnchor.MiddleCenter
        };
        GUIStyle subTytle = new()
        {
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            normal = { textColor = Color.white},
            alignment = TextAnchor.MiddleCenter
        };

        GUILayout.Label("House Builder Tool", TitleStyle);

        EditorGUILayout.LabelField("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", TitleStyle);

        GUILayout.Space(10);

        GUILayout.Label("Disc Setting", TitleStyle);

        GUILayout.Space(10);

        GUILayout.Label("Use this to set the size of the disc that will be drawn in the scene view.", subTytle);

        DiscSize = EditorGUILayout.IntSlider("Disc Size", DiscSize, 1, 100);
    
        GUILayout.Space(10);

        visibleArea = EditorGUILayout.Toggle("Visible Area", visibleArea);

        GUILayout.Space(10);

        EditorGUILayout.LabelField("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", TitleStyle);
        
        GUILayout.Space(10);

        GUILayout.Label("How many doors?", TitleStyle);

        GUILayout.Space(10);

        GUILayout.Label("Set the doors", subTytle);

        GUILayout.Space(10);

        if(GUILayout.Button("1 Door"))
        {
            // Handle 1 Door button click
        }

        GUILayout.Space(10);

        if(GUILayout.Button("2 Doors"))
        {
            // Handle 2 Doors button click
        }

        GUILayout.Space(10);

        if(GUILayout.Button("3 Doors"))
        {
            // Handle 3 Doors button click
        }

        GUILayout.Space(10);


    }
    void OnSceneGUI(SceneView sceneView)
    {
        if(!visibleArea)return;
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane ground = new(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float hit))
        {
            Vector3 hitMarker = ray.GetPoint(hit);
            if (visibleArea)
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(hitMarker, Vector3.up, DiscSize);   
            }
        }
        sceneView.Repaint();
    }
}

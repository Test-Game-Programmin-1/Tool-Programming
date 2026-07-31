using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ToolBuilderW_ : EditorWindow
{
    #region tool builder window

    [MenuItem("Tools/House Builder")]
    public static void ShowWindow()
    {
        GetWindow<ToolBuilderW_>("House Builder Tool");
    }
    #endregion

    private int DiscSize;
    private bool visibleArea;
    Vector3 prevPos;
    class CategoryData
    {
        public string Name;
        public string[] RoomPath;
        public string[] RoomName;
        public GameObject[] RoomAssets;
    }
    const string path = "Assets/Prefab";
    readonly List<CategoryData> categoryList = new();
    CategoryData selectedCategory;
    private void OnEnable()
    {
        FoldScanner();
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void FoldScanner()
    {
        categoryList.Clear();
        selectedCategory = null;
        if(!AssetDatabase.IsValidFolder(path)) return;
        string RoomFolderPath = Path.GetFullPath(path);
        string[] Dirs = Directory.GetDirectories(RoomFolderPath);
        foreach (string dir in Dirs)
        {
            string categoryName = Path.GetFileName(dir);
            string assetFolderPath = path + "/" + categoryName;
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { assetFolderPath });
            if (guids.Length ==0) continue;

            var categoryDatas = new CategoryData
            {
                Name = categoryName,
                RoomPath = new string[guids.Length],
                RoomName = new string[guids.Length],
                RoomAssets = new GameObject[guids.Length]
            };
            for (int i = 0; i < guids.Length; i++)
            {
                categoryDatas.RoomPath[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                categoryDatas.RoomName[i] = Path.GetFileNameWithoutExtension(categoryDatas.RoomPath[i]);
                categoryDatas.RoomAssets[i] = AssetDatabase.LoadAssetAtPath<GameObject>(categoryDatas.RoomPath[i]);
            }
            categoryList.Add(categoryDatas);
        }
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

        foreach (CategoryData category in categoryList)
        {
            if(GUILayout.Button(category.Name))
            {
                selectedCategory = category;
            }
        }
    }
    void OnSceneGUI(SceneView sceneView)
    {
        ButtonOver();
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
    void ButtonOver()
    {
        float buttonSize = 100f;
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10 ,100, Screen.height - 20)); 
        if(selectedCategory != null)
        {
            for (int i = 0; i < selectedCategory.RoomAssets.Length; i++)
            {
                GameObject prefab = selectedCategory.RoomAssets[i];
                Texture2D preview = AssetPreview.GetAssetPreview(prefab);
                GUIContent content;
                if(preview != null) content = new GUIContent(preview, selectedCategory.RoomName[i]);
                else content = new GUIContent(selectedCategory.RoomName[i]);
                if(GUILayout.Button(content, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    
                }
            }
        }
        GUILayout.EndArea();
        Handles.EndGUI();
        SceneView.RepaintAll();
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;


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
    class CategoryData
    {
        public string Name;
        public string[] RoomPath;
        public string[] RoomName;
        public GameObject[] RoomAssets;
        public Mesh MeshPrefab;
        public Material[] materialPrefab;
        public Matrix4x4 localMatrixPref;

    }
    const string path = "Assets/Prefab";
    readonly List<CategoryData> categoryList = new();
    CategoryData selectedCategory;

    readonly List<GameObject> SpawnedRooms = new();
    static GameObject container;
    static readonly List<CategoryData> roomParts = new();
    static GameObject selectedPrefab;

    float curRotY = 0f;
    Vector3 prevPos;
    Quaternion prevRot;
    bool isCurrentlySnaped = false;
    private void OnEnable()
    {
        FoldScanner();
        RefreshSpawnedRooms();
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
        if(categoryList.Count > 0)
        {
            selectedCategory = categoryList[0];
            SelectedPrefab(selectedCategory.RoomAssets[0]);
        }
    }

    private void RefreshSpawnedRooms()
    {
        SpawnedRooms.Clear();
        DoorsController_[] DC_ = FindObjectsByType<DoorsController_>(FindObjectsSortMode.None);
        foreach(var room in DC_)
        {
            SpawnedRooms.Add(room.gameObject);
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
                if(category.RoomAssets.Length > 0) SelectedPrefab(category.RoomAssets[0]);
            }
        }
    }
    void OnSceneGUI(SceneView sceneView)
    {
        ButtonOver();
        Preview(sceneView);
    }
    void Preview(SceneView sceneView)
    {
        Event e = Event.current;
        if(e.type == EventType.KeyDown && e.shift)
        {
            if(e.keyCode == KeyCode.Q)
            {
                curRotY -= 90;
                e.Use();
            }
            if(e.keyCode == KeyCode.E)
            {
                curRotY += 90;
                e.Use();
            }
        }
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane ground = new(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float hit))
        {
            Vector3 hitMarker = ray.GetPoint(hit);
            Quaternion Rotation = Quaternion.Euler(0, curRotY, 0);
            isCurrentlySnaped = Snap(hitMarker, Rotation, out prevPos, out prevRot );
        
            DrawHouse(sceneView);
            
            if(e.type == EventType.MouseDown && e.button == 0)
            {
                if (!isCurrentlySnaped)
                {
                    e.Use();
                    return;
                }
                if(container == null) container =  new GameObject("New Room");
                else container = GameObject.Find("New Room");
                GameObject gameOBJ = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab, container.transform);
                gameOBJ.transform.SetPositionAndRotation(prevPos, prevRot);

                Undo.RegisterCreatedObjectUndo(gameOBJ, "spawn room");
                UpdateDoorStatus(gameOBJ);
                SpawnedRooms.Add(gameOBJ);
                e.Use();
            }
        }
        
        sceneView.Repaint();
    }
    bool Snap(Vector3 basePos, Quaternion BaseRot, out Vector3 finalPos, out Quaternion finalRot)
    {
        finalPos = basePos;
        finalRot = BaseRot;

        if(selectedPrefab == null) return false;
        DoorsController_ selDoorController = selectedPrefab.GetComponent<DoorsController_>();
        if(selDoorController == null || selDoorController.doorsInfo.Count == 0) return false;

        SpawnedRooms.RemoveAll(x => x == null);

        float closestdist = float.MaxValue;
        bool foundSnap = false;

        foreach (var selcRoom in selDoorController.doorsInfo)//riferito ad ogni porta nella stanza selezionata 
        {
            if (selcRoom.collider == null) continue;
            Quaternion localDoorRot = Quaternion.Inverse(selectedPrefab.transform.rotation) * selcRoom.collider.transform.rotation;
            Vector3 localDoorPos = Quaternion.Inverse(selectedPrefab.transform.rotation) * (selcRoom.collider.transform.position - selectedPrefab.transform.position);

            Vector3 curPrevDoorWorldPos  = basePos + (BaseRot * localDoorPos);       
            Vector3 curPrevDoorWorldFor = BaseRot * localDoorRot * Vector3.forward;
            foreach (var AlrExRoom in SpawnedRooms)//per ogni stanza gia presente
            {
                DoorsController_ exRoom = AlrExRoom.GetComponent<DoorsController_>();
                if (AlrExRoom == null) continue;
                foreach (var targetDoor in exRoom.doorsInfo)//per ogni porta di una stanza
                {
                    if(targetDoor.collider == null || targetDoor.occupied)continue;
                    float distance = Vector3.Distance(curPrevDoorWorldPos, targetDoor.collider.transform.position);
                    if (distance <= DiscSize && distance < closestdist && Vector3.Dot(curPrevDoorWorldFor, targetDoor.collider.transform.forward) < -0.7f)
                    {
                        Quaternion desiredDoorRotation = Quaternion.LookRotation(-targetDoor.collider.transform.forward, Vector3.up);
                        Quaternion selectedDoorRot = desiredDoorRotation * Quaternion.Inverse(localDoorRot);
                        Vector3 TargetDoorPosition = targetDoor.collider.transform.position - (selectedDoorRot * localDoorPos);

                        closestdist = distance;
                        finalPos = TargetDoorPosition;
                        finalRot = selectedDoorRot;
                        foundSnap = true;
                    }
                }
            }
        }
        return foundSnap;

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
                    SelectedPrefab(prefab);
                }
            }
        }
        GUILayout.Space(20);
        if(GUILayout.Button("UNDO", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            if(SpawnedRooms.Count > 0)
            {
                UndoButton();
            }
        }
        GUILayout.EndArea();
        Handles.EndGUI();
        SceneView.RepaintAll();
    }

    private void UndoButton()
    {
        SpawnedRooms.RemoveAll(x => x == null);
        if(SpawnedRooms.Count > 0)
        {
           GameObject lastRoom = SpawnedRooms[SpawnedRooms.Count - 1];
           SpawnedRooms.RemoveAt(SpawnedRooms.Count - 1);
           Undo.DestroyObjectImmediate(lastRoom); 
        }
        GUIUtility.ExitGUI();
    }

    void DrawHouse(SceneView sceneView)
    {
        if (visibleArea)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(prevPos, Vector3.up, DiscSize);   
        }
        if(selectedPrefab != null && roomParts.Count > 0)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            Matrix4x4 matrix = Matrix4x4.TRS(prevPos, prevRot, selectedPrefab.transform.localScale);
            foreach(var piece in roomParts)
            {
                if(piece.MeshPrefab == null || piece.materialPrefab == null) continue;
                Matrix4x4 finalMatrix = matrix * piece.localMatrixPref;
                for(int i = 0; i < piece.materialPrefab.Length; i++)
                {
                    Graphics.DrawMesh(piece.MeshPrefab, finalMatrix, piece.materialPrefab[i], 0, sceneView.camera, i);
                }
            }
        }
    }
    void SelectedPrefab(GameObject Sys32)
    {
        selectedPrefab = Sys32;
        roomParts.Clear();
        if(Sys32 == null) return;
        MeshFilter[] pieceFilters = Sys32.GetComponentsInChildren<MeshFilter>();

        foreach(var pf in pieceFilters)
        {
            MeshRenderer pieceRenderers = pf.GetComponent<MeshRenderer>();
            if(/*int i = 0; i < X; i++ */ pf.sharedMesh != null && pieceRenderers != null)
            {
                Matrix4x4 _localMatrixes = Sys32.transform.worldToLocalMatrix * pf.transform.localToWorldMatrix;
                CategoryData piece = new()
                {
                    MeshPrefab = pf.sharedMesh,
                    materialPrefab = pieceRenderers.sharedMaterials,
                    localMatrixPref = _localMatrixes
                };
                roomParts.Add(piece);
            }
        }
    }
    void UpdateDoorStatus(GameObject NewRoomSpawned)
    {
        DoorsController_ newDoor= NewRoomSpawned.GetComponent<DoorsController_>();
        if(newDoor == null) return;
        foreach(var exRoomObj in SpawnedRooms)
        {
            DoorsController_ exRoom = exRoomObj.GetComponent<DoorsController_>();
            if(exRoom == newDoor || exRoom == null) continue;
            foreach (var targetDoor in exRoom.doorsInfo)
            {
                if(targetDoor.collider == null || targetDoor.occupied) continue;
                foreach(var newDoors in newDoor.doorsInfo)
                {
                    if(newDoors.collider == null) continue;
                    if(Vector3.Distance(newDoors.collider.transform.position, targetDoor.collider.transform.position) < 0.05f)
                    {
                        Undo.RecordObject(exRoom, " Undo doors");
                        Undo.RecordObject(newDoor, " Undo doors");
                        targetDoor.occupied = true;
                        newDoors.occupied = true;
                    }
                }
            }
        }
    }
}
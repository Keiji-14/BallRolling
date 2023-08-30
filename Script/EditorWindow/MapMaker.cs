using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MapMaker : EditorWindow
{
    private bool isSetObjectMethod;
    private bool isDestroyObjectMethod;
    private Vector3 objectPlacementPosition; // オブジェクトの配置位置
    float gridSize = 2.0f; // グリッドのサイズ

    Transform stageTransform;
    GameObject previewObject;
    GameObject objectToGenerate;
    private int selectedObjectIndex = 0;

    [MenuItem("Window/MapMaker")]
    static void Open()
    {
        var window = GetWindow<MapMaker>();
        window.titleContent = new GUIContent("MapMaker");
    }

    void OnGUI()
    {
        stageTransform = (Transform)EditorGUILayout.ObjectField("ステージ:", stageTransform, typeof(Transform), true);
        objectToGenerate = (GameObject)EditorGUILayout.ObjectField("設置オブジェクト:", objectToGenerate, typeof(GameObject), true);
        
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Box");
        gridSize = EditorGUILayout.FloatField("グリッド線:", gridSize);
        EditorGUILayout.BeginHorizontal();
        // 登録と解除の切り替えボタン
        if (GUILayout.Button(isSetObjectMethod ? "オブジェクト設置モード解除" : "オブジェクト設置モード"))
        {
            if (isSetObjectMethod)
            {
                SceneView.duringSceneGui -= OnSetObjectGUI; // オブジェクト設置メソッドの解除
            }
            else
            {
                SceneView.duringSceneGui += OnSetObjectGUI; // オブジェクト設置メソッドの登録
                if (isDestroyObjectMethod)
                {
                    SceneView.duringSceneGui -= OnDestroyObjectGUI; // オブジェクト削除メソッドの解除
                    isDestroyObjectMethod = !isDestroyObjectMethod;
                }
            }
            
            isSetObjectMethod = !isSetObjectMethod; // 状態を切り替え
        }
        if (GUILayout.Button(isDestroyObjectMethod ? "オブジェクト削除モード解除" : "オブジェクト削除モード"))
        {
            if (isDestroyObjectMethod)
            {
                SceneView.duringSceneGui -= OnDestroyObjectGUI; // オブジェクト削除メソッドの解除
            }
            else
            {
                SceneView.duringSceneGui += OnDestroyObjectGUI; // オブジェクト削除メソッドの登録
                if (isSetObjectMethod)
                {
                    SceneView.duringSceneGui -= OnSetObjectGUI; // オブジェクト設置メソッドの解除
                    isSetObjectMethod = !isSetObjectMethod;
                }
            }

            isDestroyObjectMethod = !isDestroyObjectMethod; // 状態を切り替え
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Stageにアタッチしているステージ情報を削除します。");
        if (GUILayout.Button("ステージを削除"))
        {
            if (stageTransform != null)
            {
                bool confirmed = EditorUtility.DisplayDialog("確認", "本当にステージを削除しますか？\nこの機能は取り消し出来ません。", "OK", "キャンセル");
                if (confirmed)
                {
                    DestroyStage(stageTransform);
                }
            }
            else
            {
                Debug.LogWarning("Stageがnullです。");
            }
        }
        EditorGUILayout.EndVertical();
    }

    // オブジェクトを設置するメソッド
    void OnSetObjectGUI(SceneView sceneView)
    {
        // グリッド線を描画
        DrawGrid();

        // クリック位置をグリッドに合わせて計算
        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            objectPlacementPosition = hitInfo.point + hitInfo.normal * 0.5f;

            objectPlacementPosition.x = Mathf.Floor(objectPlacementPosition.x / gridSize) * gridSize + gridSize;
            objectPlacementPosition.y = 0.0f;
            objectPlacementPosition.z = Mathf.Floor(objectPlacementPosition.z / gridSize) * gridSize + gridSize;

            if (previewObject == null)
            {
                previewObject = Instantiate(objectToGenerate, objectPlacementPosition, Quaternion.identity);
                //previewObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.5f); // 透明な色
            }
            else
            {
                previewObject.transform.position = objectPlacementPosition;
            }

            if (objectToGenerate != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // 既にオブジェクトが配置されていない場合にのみ生成
                if (!CheckObjectOverlap(objectPlacementPosition))
                {
                    Instantiate(objectToGenerate, objectPlacementPosition, Quaternion.identity, stageTransform);
                    Event.current.Use();
                }

                DestroyImmediate(previewObject);
                previewObject = null;
            }
        }
    }

    // オブジェクトの重なりを確認するメソッド
    bool CheckObjectOverlap(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, gridSize / 2f);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != objectToGenerate && collider.gameObject != stageTransform.gameObject)
            {
                // 他のオブジェクトと重なっている場合は true を返す
                return true; 
            }
        }

        // 重なりがない場合は false を返す
        return false;
    }

    // オブジェクトを削除するメソッド
    void OnDestroyObjectGUI(SceneView sceneView)
    {
        // グリッド線を描画
        DrawGrid();

        // クリック位置をグリッドに合わせて計算
        Vector3 clickPosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(clickPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // レイがヒットしたオブジェクトを取得
                GameObject objectToDelete = hitInfo.collider.gameObject;

                // レイヤーマスクを使って削除対象のオブジェクトが特定のレイヤーに含まれるか確認することもできます
                if (objectToDelete.layer == LayerMask.NameToLayer("StageObject") && objectToDelete != null)
                {
                    DestroyImmediate(objectToDelete);
                    Event.current.Use();
                }
            }
        }
    }

    // グリッド線を引く
    private void DrawGrid()
    {
        Handles.color = Color.gray;
        for (float x = -20; x <= 20; x += gridSize)
        {
            for (float z = -20; z <= 20; z += gridSize)
            {
                Vector3 from = new Vector3(x, 0, z);
                Vector3 to = new Vector3(x, 0, z + gridSize);
                Handles.DrawLine(from, to);

                from = new Vector3(x + gridSize, 0, z);
                to = new Vector3(x, 0, z);
                Handles.DrawLine(from, to);
            }
        }
    }

    // 制作中のステージの子オブジェクトを削除する
    private void DestroyStage(Transform stageChild)
    {
        for (int i = stageChild.childCount - 1; i >= 0; i--)
        {
            Transform child = stageChild.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
}

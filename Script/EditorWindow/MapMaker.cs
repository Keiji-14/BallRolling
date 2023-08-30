using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MapMaker : EditorWindow
{
    private bool isSetObjectMethod;
    private bool isDestroyObjectMethod;
    private Vector3 objectPlacementPosition; // �I�u�W�F�N�g�̔z�u�ʒu
    float gridSize = 2.0f; // �O���b�h�̃T�C�Y

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
        stageTransform = (Transform)EditorGUILayout.ObjectField("�X�e�[�W:", stageTransform, typeof(Transform), true);
        objectToGenerate = (GameObject)EditorGUILayout.ObjectField("�ݒu�I�u�W�F�N�g:", objectToGenerate, typeof(GameObject), true);
        
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Box");
        gridSize = EditorGUILayout.FloatField("�O���b�h��:", gridSize);
        EditorGUILayout.BeginHorizontal();
        // �o�^�Ɖ����̐؂�ւ��{�^��
        if (GUILayout.Button(isSetObjectMethod ? "�I�u�W�F�N�g�ݒu���[�h����" : "�I�u�W�F�N�g�ݒu���[�h"))
        {
            if (isSetObjectMethod)
            {
                SceneView.duringSceneGui -= OnSetObjectGUI; // �I�u�W�F�N�g�ݒu���\�b�h�̉���
            }
            else
            {
                SceneView.duringSceneGui += OnSetObjectGUI; // �I�u�W�F�N�g�ݒu���\�b�h�̓o�^
                if (isDestroyObjectMethod)
                {
                    SceneView.duringSceneGui -= OnDestroyObjectGUI; // �I�u�W�F�N�g�폜���\�b�h�̉���
                    isDestroyObjectMethod = !isDestroyObjectMethod;
                }
            }
            
            isSetObjectMethod = !isSetObjectMethod; // ��Ԃ�؂�ւ�
        }
        if (GUILayout.Button(isDestroyObjectMethod ? "�I�u�W�F�N�g�폜���[�h����" : "�I�u�W�F�N�g�폜���[�h"))
        {
            if (isDestroyObjectMethod)
            {
                SceneView.duringSceneGui -= OnDestroyObjectGUI; // �I�u�W�F�N�g�폜���\�b�h�̉���
            }
            else
            {
                SceneView.duringSceneGui += OnDestroyObjectGUI; // �I�u�W�F�N�g�폜���\�b�h�̓o�^
                if (isSetObjectMethod)
                {
                    SceneView.duringSceneGui -= OnSetObjectGUI; // �I�u�W�F�N�g�ݒu���\�b�h�̉���
                    isSetObjectMethod = !isSetObjectMethod;
                }
            }

            isDestroyObjectMethod = !isDestroyObjectMethod; // ��Ԃ�؂�ւ�
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Stage�ɃA�^�b�`���Ă���X�e�[�W�����폜���܂��B");
        if (GUILayout.Button("�X�e�[�W���폜"))
        {
            if (stageTransform != null)
            {
                bool confirmed = EditorUtility.DisplayDialog("�m�F", "�{���ɃX�e�[�W���폜���܂����H\n���̋@�\�͎������o���܂���B", "OK", "�L�����Z��");
                if (confirmed)
                {
                    DestroyStage(stageTransform);
                }
            }
            else
            {
                Debug.LogWarning("Stage��null�ł��B");
            }
        }
        EditorGUILayout.EndVertical();
    }

    // �I�u�W�F�N�g��ݒu���郁�\�b�h
    void OnSetObjectGUI(SceneView sceneView)
    {
        // �O���b�h����`��
        DrawGrid();

        // �N���b�N�ʒu���O���b�h�ɍ��킹�Čv�Z
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
                //previewObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.5f); // �����ȐF
            }
            else
            {
                previewObject.transform.position = objectPlacementPosition;
            }

            if (objectToGenerate != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // ���ɃI�u�W�F�N�g���z�u����Ă��Ȃ��ꍇ�ɂ̂ݐ���
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

    // �I�u�W�F�N�g�̏d�Ȃ���m�F���郁�\�b�h
    bool CheckObjectOverlap(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, gridSize / 2f);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != objectToGenerate && collider.gameObject != stageTransform.gameObject)
            {
                // ���̃I�u�W�F�N�g�Əd�Ȃ��Ă���ꍇ�� true ��Ԃ�
                return true; 
            }
        }

        // �d�Ȃ肪�Ȃ��ꍇ�� false ��Ԃ�
        return false;
    }

    // �I�u�W�F�N�g���폜���郁�\�b�h
    void OnDestroyObjectGUI(SceneView sceneView)
    {
        // �O���b�h����`��
        DrawGrid();

        // �N���b�N�ʒu���O���b�h�ɍ��킹�Čv�Z
        Vector3 clickPosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(clickPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                // ���C���q�b�g�����I�u�W�F�N�g���擾
                GameObject objectToDelete = hitInfo.collider.gameObject;

                // ���C���[�}�X�N���g���č폜�Ώۂ̃I�u�W�F�N�g������̃��C���[�Ɋ܂܂�邩�m�F���邱�Ƃ��ł��܂�
                if (objectToDelete.layer == LayerMask.NameToLayer("StageObject") && objectToDelete != null)
                {
                    DestroyImmediate(objectToDelete);
                    Event.current.Use();
                }
            }
        }
    }

    // �O���b�h��������
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

    // ���쒆�̃X�e�[�W�̎q�I�u�W�F�N�g���폜����
    private void DestroyStage(Transform stageChild)
    {
        for (int i = stageChild.childCount - 1; i >= 0; i--)
        {
            Transform child = stageChild.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
}

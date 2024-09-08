using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(Shape)), CanEditMultipleObjects]
public class ShapeEditor : Editor
{
    private Color cachedCubeColor;
    private Color cachedSphereColor;
    bool cubesActive = true;
    bool spheresActive = true;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select all cubes"))
            {
                SelectAllShapes<Cube>();
            }

            if (GUILayout.Button("Select all spheres"))
            {
                SelectAllShapes<Sphere>();
            }

            if (GUILayout.Button("Clear Selection"))
            {
                Selection.objects = null;
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            GUI.backgroundColor = cubesActive ? Color.green : Color.red;
            if (GUILayout.Button("Enable/Disable all cubes"))
            {
                cubesActive = !cubesActive;
                ToggleAllShapes<Cube>();
            }

            GUI.backgroundColor = spheresActive ? Color.green : Color.red;
            if (GUILayout.Button("Enable/Disable all spheres"))
            {
                spheresActive = !spheresActive;
                ToggleAllShapes<Sphere>();
            }

            GUI.backgroundColor = Color.white;
        }

        SerializedProperty sizeProperty = serializedObject.FindProperty("cubeSize");
        SerializedProperty radiusProperty = serializedObject.FindProperty("sphereRadius");

        if (sizeProperty != null)
        {
            EditorGUILayout.PropertyField(sizeProperty);
            if (sizeProperty?.floatValue < 0)
            {
                EditorGUILayout.HelpBox($"The size of the cubes cannot be less than 0!", MessageType.Warning);
            }
            else
            {
                foreach (GameObject selectedObj in Selection.objects)
                {
                    ResizeAllShapes<Cube>(sizeProperty.floatValue);
                }
            }
            sizeProperty.serializedObject.ApplyModifiedProperties();
        }

        if (radiusProperty != null)
        {
            EditorGUILayout.PropertyField(radiusProperty);
            if (radiusProperty?.floatValue < 0)
            {
                EditorGUILayout.HelpBox($"The radius of the spheres cannot be less than 0!", MessageType.Warning);
            }
            else
            {
                foreach (GameObject selectedObj in Selection.objects)
                {
                    ResizeAllShapes<Sphere>(radiusProperty.floatValue * 2f);
                }
            }
            radiusProperty.serializedObject.ApplyModifiedProperties();
        }
    }

    private void SelectAllShapes<T>() where T : Shape
    {
        T [] allShapes = FindObjectsOfType<T>();

        GameObject [] allShapeObjects = allShapes.Select(cube => cube.gameObject).ToArray();

        Selection.objects = allShapeObjects;
    }

    private void ToggleAllShapes<T>() where T : Shape
    {
        foreach (T shape in FindObjectsOfType<T>(true))
        {
            shape.gameObject.SetActive(!shape.gameObject.activeSelf);
        }
    }

    private void ResizeAllShapes<T>(float size) where T : Shape
    {
        foreach (T shape in FindObjectsOfType<T>(true))
        {
            shape.transform.localScale = Vector3.one * size;
        }
    }

}

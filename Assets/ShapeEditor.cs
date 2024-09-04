using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Shape)), CanEditMultipleObjects]
public class ShapeEditor : Editor
{
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
            if (GUILayout.Button("Toggle all cubes"))
            {
                ToggleAllShapes<Cube>();
            }

            if (GUILayout.Button("Toggle all spheres"))
            {
                ToggleAllShapes<Sphere>();
            }
        }

        serializedObject.Update();
        var health = serializedObject.FindProperty("health");
        EditorGUILayout.PropertyField(health);
        serializedObject.ApplyModifiedProperties();
        
    }

    private void SelectAllShapes<T>() where T : Shape
    {
        T [] allShapes = FindObjectsOfType<T>();

        GameObject [] allShapeObjects = allShapes.Select(cube => cube.gameObject).ToArray();

        Selection.objects = allShapeObjects;
    }

    private void ToggleAllShapes<T>() where T : Shape
    {
        T [] allShapes = FindObjectsOfType<T>();

        GameObject [] allShapeObjects = allShapes.Select(cube => cube.gameObject).ToArray();

        foreach (T shape in FindObjectsOfType<T>(true))
        {
            shape.gameObject.SetActive(!shape.gameObject.activeSelf);
        }
    }
}

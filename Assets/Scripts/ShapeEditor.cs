using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(Shape)), CanEditMultipleObjects]
public class ShapeEditor : Editor
{
    bool cubesActive = true;
    bool spheresActive = true;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        // Draw selection GUI in horizontal pattern
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
        
        // Draw toggle GUI in horizontal pattern underneath the selection GUI
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

        // Search for the size properties of the shapes
        SerializedProperty sizeProperty = serializedObject.FindProperty("cubeSize");
        SerializedProperty radiusProperty = serializedObject.FindProperty("sphereRadius");

        // Resize the shapes based on the property values
        if (sizeProperty != null)
        {
            EditorGUILayout.PropertyField(sizeProperty);
            ResizeAllShapes<Cube>(sizeProperty.floatValue);
            sizeProperty.serializedObject.ApplyModifiedProperties();
        }

        if (radiusProperty != null)
        {
            EditorGUILayout.PropertyField(radiusProperty);
            ResizeAllShapes<Sphere>(radiusProperty.floatValue * 2f);
            radiusProperty.serializedObject.ApplyModifiedProperties();
        }
    }

    // Finds all objects of type T and selects them
    private void SelectAllShapes<T>() where T : Shape
    {
        T [] allShapes = FindObjectsOfType<T>();

        GameObject [] allShapeObjects = allShapes.Select(cube => cube.gameObject).ToArray();

        Selection.objects = allShapeObjects;
    }

    // Finds all objects of type T and toggles them on/off
    private void ToggleAllShapes<T>() where T : Shape
    {
        foreach (T shape in FindObjectsOfType<T>(true))
        {
            shape.gameObject.SetActive(!shape.gameObject.activeSelf);
        }
    }

    // Resizes all objects of type T given a size value
    private void ResizeAllShapes<T>(float sizeValue) where T : Shape
    {
        // Specifies a rule that the size cannot be less than 0 and displays a warning if so
        if (sizeValue < 0)
        {
            EditorGUILayout.HelpBox($"The size of the cubes cannot be less than 0!", MessageType.Warning);
        }
        else
        {
            foreach (T shape in FindObjectsOfType<T>(true))
            {
                shape.transform.localScale = Vector3.one * sizeValue;
            }
        }
    }

}

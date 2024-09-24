using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// This class contain custom drawer for ReadOnly attribute
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// Unity Method for drawing GUI in editor
    /// <param name="position">Position.</param>
    /// <param name="property">Property.</param>
    /// <param name="label">Label.</param>
    /// 

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Saving previous GUI enabled value
        var presviousGUIState = GUI.enabled;
        //Disabling edit for property
        GUI.enabled = false;
        //Drawing property
        EditorGUI.PropertyField(position, property, label);
        //Setting onyl GUI enabled value
        GUI.enabled = presviousGUIState;
    }




}

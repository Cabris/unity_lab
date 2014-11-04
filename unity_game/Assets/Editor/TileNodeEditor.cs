using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ActionSelector))]
public class TileNodeEditor : Editor
{
	public SerializedProperty typeMask;
	
	void OnEnable()
	{
		typeMask = serializedObject.FindProperty("actionTypeMask");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		typeMask.intValue = (int)((ActionSelector.ActionType)EditorGUILayout.EnumMaskField("Action Type Mask", (ActionSelector.ActionType)typeMask.intValue));
		
		serializedObject.ApplyModifiedProperties();
	}
}
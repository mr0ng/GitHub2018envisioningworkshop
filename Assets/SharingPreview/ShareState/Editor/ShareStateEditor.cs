using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShareState))]
public class ShareStateEditor : Editor
{
	#region Fields
	SerializedProperty watches;
	GameObject         sourceObject;
	Component          com;
	MemberInfo         member;
	float              frequency;
	#endregion

	#region Unity Events
	void OnEnable()
	{
		watches = serializedObject.FindProperty("watches");
	}
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if (((Component)target).GetComponent<PhotonView>() == null)
			EditorGUILayout.HelpBox("A PhotonView is required for this component to work! Either add it now in the editor, or add it at runtime.", MessageType.Warning);

		ShowWatchList();
		EditorGUILayout.Space();
		ShowAddWatch();
		
		serializedObject.ApplyModifiedProperties();
	}
	#endregion

	#region GUI Drawing Methods
	void ShowWatchList()
	{
		// Display a box for the header
		float line = EditorGUIUtility.singleLineHeight;
		Rect  rect = EditorGUILayout.GetControlRect( false, line+6 );
		rect.yMax += 2;
		EditorGUI.DrawRect(rect, new Color(0,0,0, 0.1f));
		EditorGUI.LabelField(new Rect(rect.x+4, rect.y+4, rect.width-8, line), "Sync fields:", EditorStyles.boldLabel);

		// display a box for the field list
		rect = EditorGUILayout.GetControlRect( false, (line+2) * watches.arraySize + 6 );
		EditorGUI.DrawRect(rect, new Color(1,1,1, 0.4f));
		rect.x    += 4;
		rect.xMax -= 8;
		rect.y    += 4;
		
		// display existing 'watches'
		for (int i = 0; i < watches.arraySize; i++)
		{
			SerializedProperty watch = watches.GetArrayElementAtIndex(i);
			float y = rect.y+i*(line+2);
			
			// watch name
			Component com = (Component)watch.FindPropertyRelative("component").objectReferenceValue;
			string  label = string.Format("{0}{1}.{2}",
				com.gameObject != ((Component)target).gameObject ? com.gameObject.name + "->" : "",
				com.GetType().Name,
				watch.FindPropertyRelative("member").stringValue );
			Vector2 size  = EditorStyles.label.CalcSize(new GUIContent(label));
			EditorGUI.LabelField(new Rect(rect.x, y, size.x, line), label);
			
			// watch interval field
			SerializedProperty freq = watches.GetArrayElementAtIndex(i).FindPropertyRelative("frequency");
			EditorGUI.LabelField(new Rect(rect.xMax-110, y, 110-60, line), "interval" );
			freq.floatValue = EditorGUI.FloatField(new Rect(rect.xMax-60, y, 60-16, line), freq.floatValue );
			
			// remove watch
			if (GUI.Button(new Rect(rect.xMax-16, y, 16, line), "X"))
			{
				watches.DeleteArrayElementAtIndex(i);
				i--;
			}
		}
	}
	void ShowAddWatch()
	{
		GameObject thisObject = ((Component)target).gameObject;
		if (sourceObject == null)
			sourceObject = thisObject;

		Rect rect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight );
		
		// Display the info we've already picked
		float width = 60;
		EditorGUI.LabelField(new Rect(rect.x, rect.y, width, rect.height), "Add field:", EditorStyles.toolbarButton);
		rect.x     += width;
		rect.width -= width;
		if (com != null && DrawDeleteable(com.GetType().Name, ref rect))
		{
			com    = null;
			member = null;
		}
		if (member != null && DrawDeleteable(member.Name, ref rect))
		{
			member = null;
		}

		// And now, draw the selection part
		if (com == null)
		{
			// GameObject component picker
			List<Component> coms = new List<Component>(sourceObject.GetComponents<Component>());
			int             com  = EditorGUI.Popup(rect, -1, coms.ConvertAll(a=>new GUIContent(a.GetType().Name)).ToArray(), EditorStyles.toolbarPopup);
			EditorGUI.DrawRect(rect, new Color(1,1,1, 0.4f));
			if (com != -1)
				this.com = coms[com];
		}
		else if (member == null)
		{
			// Get all fields and properties from the current component
			List<MemberInfo> fields = new List<MemberInfo>();
			fields.AddRange(com.GetType().GetProperties( BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic ));
			fields.AddRange(com.GetType().GetFields    ( BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic ));
			fields.Sort((a,b)=>a.Name.CompareTo(b.Name));
			
			// filter fields that aren't shareable
			for (int i = 0; i < fields.Count; i++)
			{
				PropertyInfo prop = fields[i] as PropertyInfo;
				FieldInfo    f    = fields[i] as FieldInfo;
				if ((prop != null && (!ShareStateTypes.CanSerialize(prop.PropertyType) || ( !prop.CanRead || !prop.CanWrite))) ||
					(f    != null && (!ShareStateTypes.CanSerialize(f.FieldType))))
				{
					fields.RemoveAt(i);
					i--;
				}
			}
			
			// pick a field
			int field = EditorGUI.Popup(rect, -1, fields.ConvertAll(a=>new GUIContent(a.Name)).ToArray(), EditorStyles.toolbarPopup);
			EditorGUI.DrawRect(rect, new Color(1,1,1, 0.4f));
			if (field != -1)
			{
				member    = fields[field];
				frequency = 0.5f;
			}
		}
		else
		{
			// display the update interval item
			frequency = EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width-32, rect.height+2), frequency);
			// And here we do adding an item to the list
			if (GUI.Button(new Rect(rect.xMax-32, rect.y, 32, rect.height), "+", EditorStyles.toolbarButton))
			{
				watches.InsertArrayElementAtIndex(watches.arraySize);
				SerializedProperty watch = watches.GetArrayElementAtIndex(watches.arraySize-1);
				watch.FindPropertyRelative("component").objectReferenceValue = com;
				watch.FindPropertyRelative("member"   ).stringValue          = member.Name;
				watch.FindPropertyRelative("frequency").floatValue           = frequency;

				com    = null;
				member = null;
			}
		}

		GameObject newObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Field Source", "Where does this pick Components from? Must be a child object of this GameObject."), sourceObject, typeof(GameObject), true);
		// make sure the source object is a child of this object, or is this object
		if (newObject != sourceObject)
		{
			Transform curr  = newObject == null ? null : newObject.transform;
			bool      valid = false;
			while(curr != null)
			{
				if (curr == thisObject.transform)
				{
					valid = true;
					break;
				}
				curr = curr.parent;
			}
			if (!valid)
				newObject = null;

			sourceObject = newObject;
			com          = null;
			member       = null;
		}
	}
	static bool DrawDeleteable(string aType, ref Rect aRect)
	{
		// delete button
		const int cButtonWidth = 20;
		bool deleted = GUI.Button(new Rect(aRect.x, aRect.y, cButtonWidth, aRect.height+2), "X", EditorStyles.toolbarButton);
		aRect.x     += cButtonWidth;
		aRect.width -= cButtonWidth;

		if (deleted)
			return true;

		// name field
		float width = EditorStyles.toolbarButton.CalcSize(new GUIContent(aType)).x;
		EditorGUI.LabelField(new Rect(aRect.x, aRect.y, width, aRect.height), aType, EditorStyles.toolbarButton );
		aRect.x     += width;
		aRect.width -= width;

		return false;
	}
	#endregion
}

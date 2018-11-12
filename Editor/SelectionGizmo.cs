#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace J1Jeong
{
	public class SelectionGizmoHotkeys : EditorWindow
	{
		static class Strings
		{
			internal const string UnityEditor_AnnotationUtility = "UnityEditor.AnnotationUtility";
			internal const string use3dGizmos = "use3dGizmos";
			internal const string iconSize = "iconSize";
			internal const string prefs_use3dGizmos = "SelectionGizmo.use3dGizmos";
			internal const string prefs_iconSize = "SelectionGizmo.iconSize";
			internal const string showSelectionOutline = "showSelectionOutline";
			internal const string showSelectionWire = "showSelectionWire";
			internal const string showGrid = "showGrid";
		}


#if !UNITY_5
        static float iconSize = 0;
        static bool use3dGizmos = false;
#endif
        [MenuItem( "Window/Hotkeys/Gizmo Icons %g", priority=101)]
		public static void HideGizmoIcons()
		{
			Assembly asm = Assembly.GetAssembly( typeof(Editor) );
			Type type = asm.GetType( Strings.UnityEditor_AnnotationUtility );
			if( type != null )
			{
				PropertyInfo use3dGizmosProperty = type.GetProperty( Strings.use3dGizmos, BindingFlags.Static | BindingFlags.NonPublic );
				PropertyInfo iconSizeProperty = type.GetProperty( Strings.iconSize, BindingFlags.Static | BindingFlags.NonPublic );

				float nowIconSize = (float) iconSizeProperty.GetValue( asm, null );
				if( nowIconSize > 0 ) // to hide
                {
#if UNITY_5
					EditorPrefs.SetFloat(Strings.prefs_use3dGizmos, nowIconSize);
#endif
                    iconSize = nowIconSize;
                    iconSizeProperty.SetValue( asm, 0, null );

#if UNITY_5
					bool use3dGizmos = (bool) use3dGizmosProperty.GetValue( asm, null );
					EditorPrefs.SetBool(Strings.prefs_use3dGizmos, use3dGizmos);
#else
                    use3dGizmos = (bool)use3dGizmosProperty.GetValue(asm, null);
#endif
                    use3dGizmosProperty.SetValue( asm, true, null );
				}
				else // to show
                {
#if UNITY_5
					float iconSize = EditorPrefs.GetFloat(Strings.prefs_iconSize);
#endif
                    if (iconSize <= 0)
						iconSize = 0.03162277f; // Mathf.Pow(10f, -3f + 3f * 0.5f), see to Convert01ToTexelWorldSize()
					iconSizeProperty.SetValue( asm, iconSize, null );

#if UNITY_5
					bool use3dGizmos = EditorPrefs.GetBool(Strings.prefs_use3dGizmos);
#endif
					use3dGizmosProperty.SetValue( asm, use3dGizmos, null );
				}
			}
		}
		

		[MenuItem( "Window/Hotkeys/Show Grid %#g", priority= 101)]
		public static void ShowGrid()
		{
			Assembly asm = Assembly.GetAssembly( typeof(Editor) );
			Type type = asm.GetType( Strings.UnityEditor_AnnotationUtility );
			if( type != null )
			{
				PropertyInfo property = type.GetProperty( Strings.showGrid, BindingFlags.Static | BindingFlags.NonPublic );
				bool flag = (bool) property.GetValue( asm, null );
				property.SetValue( asm, !flag, null );
			}
		}

#if UNITY_5_5_OR_NEWER
		[MenuItem( "Window/Hotkeys/Selection Outline %h", priority=101 )]
		public static void HideOutline()
		{
			Assembly asm = Assembly.GetAssembly( typeof(Editor) );
			Type type = asm.GetType( Strings.UnityEditor_AnnotationUtility );
			if( type != null )
			{
				PropertyInfo property = type.GetProperty( Strings.showSelectionOutline, BindingFlags.Static | BindingFlags.NonPublic );
				bool flag = (bool) property.GetValue( asm, null );
				property.SetValue( asm, !flag, null );
			}
		}

		[MenuItem( "Window/Hotkeys/Selection Wire %#h", priority=101 )]
		public static void HideWireframe()
		{
			Assembly asm = Assembly.GetAssembly( typeof(Editor) );
			Type type = asm.GetType( Strings.UnityEditor_AnnotationUtility );
			if( type != null )
			{
				PropertyInfo property = type.GetProperty( Strings.showSelectionWire, BindingFlags.Static | BindingFlags.NonPublic );
				bool flag = (bool) property.GetValue( asm, null );
				property.SetValue( asm, !flag, null );
			}
		}
#else
        // for Unity Old Version
        static List<GameObject> hideObjects = new List<GameObject>();

	    [MenuItem("Window/Hotkeys/Hide WireFrame %h", priority=101)]
	    static void HideWireframe()
	    {
		    bool IsAllHide = true;

		    for (int i = 0; i < Selection.gameObjects.Length; i++)
		    {
                GameObject s = Selection.gameObjects[i];
                Renderer rend = s.GetComponent<Renderer>();

                if (rend)
                {
                    if (hideObjects.Contains(s) == false)
                    {
                        IsAllHide = false;
                        break;
                    }
                }
                else if (Selection.gameObjects.Length == 1)
                {
                    Renderer[] children = s.GetComponentsInChildren<Renderer>();
                    for (int j = 0; j < children.Length; j++)
                    {
                        if (hideObjects.Contains(children[i].gameObject) == false)
                        {
                            IsAllHide = false;
                            break;
                        }
                    }
                }
            }

		    // Unselected
		    for (int i = 0; i < hideObjects.Count; i++)
		    {
			    GameObject s = hideObjects[i];
			    if (s != null && s.activeSelf)
			    {
				    Renderer rend = s.GetComponent<Renderer>();
				    if (rend)
					    EditorUtility.SetSelectedWireframeHidden(rend, false); // show
			    }
		    }
		    hideObjects.Clear();

            for (int i = 0; i < Selection.gameObjects.Length; i++)
		    {
			    GameObject s = Selection.gameObjects[i];
			    Renderer rend = s.GetComponent<Renderer>();

                if (rend)
                {
                    if (IsAllHide)
                    {
                        // Hide before compile.
                        EditorUtility.SetSelectedWireframeHidden(rend, false); // show
                    }
                    else
                    {
                        EditorUtility.SetSelectedWireframeHidden(rend, true); // hide
                        hideObjects.Add(s);
                    }
			    }
			    else if(Selection.gameObjects.Length == 1)
			    {
                    if (IsAllHide)
                        break;

                    Renderer[] children = s.GetComponentsInChildren<Renderer>();
                    for (int j = 0; j < children.Length; j++)
                    {
                        EditorUtility.SetSelectedWireframeHidden(children[j], true); // hide
                        hideObjects.Add(children[j].gameObject);
                    }
			    }
		    }
	    }

	    [MenuItem("Window/Hotkeys/Hide WireFrame %h", true)]
	    static bool HideWireframeValidate()
	    {
		    return Selection.activeGameObject != null;
	    }
#endif
	}
}
#endif
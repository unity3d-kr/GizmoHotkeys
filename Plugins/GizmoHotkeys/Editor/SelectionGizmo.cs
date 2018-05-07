using System;
using System.Reflection;
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

		[MenuItem( "Window/Hotkeys/Gizmo Icons %g" )]
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
					EditorPrefs.SetFloat(Strings.prefs_use3dGizmos, nowIconSize);
					iconSizeProperty.SetValue( asm, 0, null );
					
					bool use3dGizmos = (bool) use3dGizmosProperty.GetValue( asm, null );
					EditorPrefs.SetBool(Strings.prefs_use3dGizmos, use3dGizmos);
					use3dGizmosProperty.SetValue( asm, true, null );
				}
				else // to show
				{
					float iconSize = EditorPrefs.GetFloat(Strings.prefs_iconSize);
					if (iconSize <= 0)
						iconSize = 0.03162277f; // Mathf.Pow(10f, -3f + 3f * 0.5f), see to Convert01ToTexelWorldSize()
					iconSizeProperty.SetValue( asm, iconSize, null );

					bool use3dGizmos = EditorPrefs.GetBool(Strings.prefs_use3dGizmos);
					use3dGizmosProperty.SetValue( asm, use3dGizmos, null );
				}
			}
		}
		

		[MenuItem( "Window/Hotkeys/Show Grid %#g" )]
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
		[MenuItem( "Window/Hotkeys/Selection Outline %h" )]
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

		[MenuItem( "Window/Hotkeys/Selection Wire %#h" )]
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
#endif
	}
}
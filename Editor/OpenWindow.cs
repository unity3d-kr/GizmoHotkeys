using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace J1Jeong
{
	public class OpenWindow : EditorWindow
	{
		[MenuItem( "Window/Hotkeys/Lighting Settings %L", priority=101)]
		static void OpenLightingSettings()
		{
			EditorApplication.ExecuteMenuItem( "Window/Lighting/Settings" );
		}

		[MenuItem( "Window/Hotkeys/Light Expolorer %#L", priority=101 )]
		static void OpenLightExplorer()
		{
			EditorApplication.ExecuteMenuItem( "Window/Lighting/Light Explorer" );
		}

		[MenuItem( "Window/Hotkeys/Frame Debugger %F7", priority=101 )]
		static void OpenFrameDebugger()
		{
			EditorApplication.ExecuteMenuItem( "Window/Frame Debugger" );
		}
	}
}
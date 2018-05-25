using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

public class OpenWindow : EditorWindow
{
	[MenuItem("Scene View/Open/Lighting Settings %L")]
	static void OpenLightingSettings()
	{
		EditorApplication.ExecuteMenuItem("Window/Lighting/Settings");
	}

	[MenuItem("Scene View/Open/Light Expolorer #%L")]
	static void OpenLightExplorer()
	{
		EditorApplication.ExecuteMenuItem("Window/Lighting/Light Explorer");
	}
	
	[MenuItem("Scene View/Open/Frame Debugger %F7")]
	static void OpenFrameDebugger()
	{
		EditorApplication.ExecuteMenuItem("Window/Frame Debugger");
	}
}
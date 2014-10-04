using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using UnityEditor;
//Used for File IO
using System.IO;
using System.Diagnostics;

namespace simul
{
	class SimulImports
	{
		static SimulImports()
		{
			CopyDependencyDllsToProjectDir();
		}
		public static void ReplaceDepthCameraWithTrueSkyCamera()
		{
			UnityEngine.Object[] brokenList = Resources.FindObjectsOfTypeAll(typeof(Camera));
			foreach (UnityEngine.Object o in brokenList)
			{
				UnityEngine.Debug.Log(o);
				GameObject g = (GameObject)o;
				Component[] components = g.GetComponents<Component>();
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i] == null)
					{
						g.AddComponent<TrueSkyCamera>();
					}
				}
			}
		}
		public static void Init()
		{
			ReplaceDepthCameraWithTrueSkyCamera();
		}
		public static bool CopyDependencyDllsToProjectDir()
		{
			String currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
			char s = Path.DirectorySeparatorChar;
			String x86Path = Environment.CurrentDirectory + s + "Assets" + s + "Plugins" + s + "x86";
			String dllPath = x86Path + s + "dependencies";
			if (currentPath.Contains(dllPath) == false)
			{
				Environment.SetEnvironmentVariable("PATH", dllPath + Path.PathSeparator + currentPath, EnvironmentVariableTarget.Process);
			}
			if (!File.Exists(Environment.GetEnvironmentVariable("WINDIR") + @"\system32\msvcr110.dll"))
			{
				UnityEngine.Debug.Log("Can't find msvcr110.dll - will install");
				// Use ProcessStartInfo class
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = false;
				startInfo.UseShellExecute = false;
				startInfo.FileName = x86Path + s + "vcredist_x86.exe";
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				//startInfo.Arguments = "/q";
				try
				{
					// Start the process with the info we specified.
					// Call WaitForExit and then the using statement will close.
					using (Process exeProcess = Process.Start(startInfo))
					{
						exeProcess.WaitForExit();
					}
				}
				catch
				{
					// Log error.
				}
			}
			return true;
		}
#if UNITY_IPHONE || UNITY_XBOX360
	// On iOS and Xbox 360 plugins are statically linked into
	// the executable, so we have to use __Internal as the
	// library name.
	public const string editor_dll ="__Internal";
#else
		// Other platforms load plugins dynamically, so pass the name
		// of the plugin's dynamic library.
		public const string editor_dll = "TrueSkyUI_MD";
#endif
	}
	[CustomEditor(typeof(Sequence))]
	public class SequenceEditor : Editor
	{
		public static Sequence sequence = null;
		SequenceEditor()
		{
			//UnityEngine.Debug.Log("SequenceEditor constr");
			SimulImports.CopyDependencyDllsToProjectDir();
		}
		~SequenceEditor()
		{
			// Leave these active - we DO NOT need an instance of SequenceEditor to process the callbacks.
			//SetOnPropertiesChangedCallback(null);
			//SetOnTimeChangedCallback(null);
			//UnityEngine.Debug.Log("~SequenceEditor destr");
			HideSequencer();
		}

		#region GetHandle
		public class HandleFinder
		{
			public bool bUnityHandleSet = false;
			public HandleRef unityWindowHandle;
			public bool EnumWindowsCallBack(IntPtr hWnd, IntPtr lParam)
			{
				int procid;
				GetWindowThreadProcessId(new HandleRef(this, hWnd), out procid);

				int currentPID = System.Diagnostics.Process.GetCurrentProcess().Id;

				if (procid == currentPID)
				{
					unityWindowHandle = new HandleRef(this, hWnd);
					bUnityHandleSet = true;
					return false;
				}
				return true;
			}
			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
		}
		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);
		public static System.IntPtr handle
		{
			get
			{
				if (_handle == (System.IntPtr)0)
				{
					HandleFinder hf = new HandleFinder();
					if (!hf.bUnityHandleSet)
					{
						IntPtr extraData = (IntPtr)0;
						EnumWindows(hf.EnumWindowsCallBack, extraData);
					}
					if (!hf.bUnityHandleSet)
						return _handle;
					_handle = (System.IntPtr)hf.unityWindowHandle;// Process.GetCurrentProcess().MainWindowHandle;
				}
				return _handle;
			}
			set
			{
			}
		}
		static System.IntPtr _handle = (System.IntPtr)0;
		#endregion
		#region Imports
		// We want Unity-style controls, but we have to specify this.
		enum Style
		{
			DEFAULT_STYLE = 0
			,
			UNREAL_STYLE = 1
				,
			UNITY_STYLE = 2
				, UNITY_STYLE_DEFERRED = 3
		};

		[DllImport(SimulImports.editor_dll)]
		private static extern void EnableUILogging(string logfile);
		[DllImport(SimulImports.editor_dll)]
		private static extern void OpenUI(System.IntPtr OwnerHWND, int[] pVisibleRect, int[] pParentRect, System.IntPtr Env, Style style);
		[DllImport(SimulImports.editor_dll)]
		private static extern void CloseUI(System.IntPtr OwnerHWND);
		[DllImport(SimulImports.editor_dll)]
		private static extern void HideUI(System.IntPtr OwnerHWND);

		[DllImport(SimulImports.editor_dll)]
		private static extern void StaticSetString(System.IntPtr OwnerHWND, string name, string value);
		[DllImport(SimulImports.editor_dll)]
		private static extern void StaticSetSequence(System.IntPtr OwnerHWND, string SequenceAsText);
		[DllImport(SimulImports.editor_dll)]
		private static extern void StaticSetFloat(System.IntPtr OwnerHWND, string name, float value);
		[DllImport(SimulImports.editor_dll)]
		private static extern void StaticSetMatrix(System.IntPtr OwnerHWND, string name, float[] value);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		delegate void TOnSequenceChangeCallback(int hwnd, string newSequenceState);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		delegate void TOnTimeChangedCallback(int hwnd, float time);

		[DllImport(SimulImports.editor_dll)]
		private static extern void SetOnPropertiesChangedCallback(TOnSequenceChangeCallback CallbackFunc);
		[DllImport(SimulImports.editor_dll)]
		private static extern void SetOnTimeChangedCallback(TOnTimeChangedCallback CallbackFunc);

		static TOnSequenceChangeCallback onPropertiesChangedCallback =
			(int hwnd, string newSequenceState) =>
			{
				//System.IntPtr hwnd=(System.IntPtr)value;
				//string filename = (Application.dataPath + "/../" + AssetDatabase.GetAssetPath(sequence));
				// The currently edited file has changed.
				// Now any trueSKY instance that references the currently edited sequence must reload it.
				//SequenceEditor.ReimportSequenceAsset();
				UnityEngine.Debug.Log("Sequence changed");
				sequence.Load(newSequenceState);

				UnityEngine.Object[] trueSkies;
				trueSkies = FindObjectsOfType(typeof(trueSKY));
				foreach (UnityEngine.Object t in trueSkies)
				{
					trueSKY trueSky = (trueSKY)t;
					if (trueSky.sequence == sequence)
						trueSky.Reload();
				}
				EditorUtility.SetDirty(sequence);
				AssetDatabase.SaveAssets();
			};
		static TOnTimeChangedCallback onTimeChangedCallback =
			(int hwnd, float time) =>
			{
				UnityEngine.Object[] trueSkies;
				trueSkies = FindObjectsOfType(typeof(trueSKY));
				foreach (UnityEngine.Object t in trueSkies)
				{
					trueSKY trueSky = (trueSKY)t;
					if (trueSky.sequence == sequence)
					{
						trueSky.time = time;
						EditorUtility.SetDirty(trueSky);
					}
				}
			};
		#endregion
		static trueSKY trueSky()
		{
			UnityEngine.Object[] trueSkies;
			trueSkies = FindObjectsOfType(typeof(trueSKY));
			foreach (UnityEngine.Object t in trueSkies)
			{
				trueSKY trueSky = (trueSKY)t;
				if (trueSky.sequence == sequence)
					return trueSky;
			}
			return null;
		}
		static bool show_when_possible = false;
		public void OnEnable()
		{
			show_when_possible = true;
		}
		public void OnDisable()
		{
			HideSequencer();
		}
		static bool show = true;
		public override void OnInspectorGUI()
		{
			SimulImports.CopyDependencyDllsToProjectDir();
			sequence = (Sequence)target;
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Show Sequencer...") || show_when_possible)
			{
				show = true;
			}
			EditorGUILayout.EndVertical();
			if (Event.current.type == EventType.Repaint)
			{
				if (show)
					EditorApplication.delayCall += ShowSequencer;
				show = false;
			}
		}
		private static float[] MatrixToFloatArray(Matrix4x4 m)
		{
			float[] a = { m.m00,m.m01,m.m02,m.m03
				   ,m.m10,m.m11,m.m12,m.m13
				   ,m.m20,m.m21,m.m22,m.m23
				   ,m.m30,m.m31,m.m32,m.m33};
			return a;
		}
		public static void ShowSequencer()
		{
			if (sequence == null)
			{
				UnityEngine.Debug.LogError("Null sequence");
				return;
			}
			string local_path = AssetDatabase.GetAssetPath(sequence);
			if (!local_path.Contains(".asset"))
			{
				UnityEngine.Debug.LogError("Filename not found for this sequence " + sequence.ToString());
				return;
			}
			show_when_possible = false;
			// Disable this when not needed:
			EnableUILogging("trueSKYUnityUI.log");
			int[] r = { 16, 16, 512, 512 };
			System.IntPtr Env = (System.IntPtr)0;
			SetOnPropertiesChangedCallback(onPropertiesChangedCallback);
			SetOnTimeChangedCallback(onTimeChangedCallback);

			OpenUI(handle, r, r, Env, Style.UNITY_STYLE);

			// Edit the .sq file that this asset is imported from.
			local_path = local_path.Replace(".asset", ".sq");
			//UnityEngine.Debug.Log("local path is "+local_path);
			string filename = Path.GetFullPath(Path.Combine(Path.Combine(Application.dataPath, ".."), local_path));
			//UnityEngine.Debug.Log("Filename is "+filename);
			StaticSetString(handle, "filename_dont_load", filename);
			if (sequence.SequenceAsText != null)
				StaticSetSequence(handle, sequence.SequenceAsText);

			trueSKY t = trueSky();
			// Initialize time from the trueSKY object
			if (t != null)
				StaticSetFloat(handle, "time", t.time);
			// When changes are made, we will force reimport of that asset.
			if (Camera.main != null)
			{
				TrueSkyCamera trueSkyCamera = Camera.main.gameObject.GetComponent<TrueSkyCamera>();
				if (trueSkyCamera != null)
					StaticSetMatrix(handle, "ViewMatrix", trueSkyCamera.ViewMatrixToTrueSkyFormat(trueSkyCamera.GetRenderStyle()));
			}
		}
		public static void HideSequencer()
		{
			HideUI(handle);
		}
	}
}
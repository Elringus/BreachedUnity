using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
//Used for File IO
using System.IO;

namespace simul
{
	[CustomEditor(typeof(trueSKY))]
	public class trueSKYEditor : Editor
	{
		[MenuItem("Help/trueSKY Documentation...", false, 1000)]
		public static void CreateSequenceAsset()
		{
			string path = @"https://www.simul.co/wp-content/uploads/documentation/unity/TrueSkyUnity.html";// "Simul/Documentation/TrueSkyUnity.html";
			//UnityEngine.Debug.Log();
			Help.BrowseURL(path);
		}
		bool recomp = false;
		bool prepareUpload = false;
		public override void OnInspectorGUI()
		{
			trueSKY trueSky = (trueSKY)target;
			/*	if(GUILayout.Button("trueSKY documentation"))
				{
					trueSKY.RecompileShaders();
					string path=@"https://www.simul.co/wp-content/uploads/documentation/unity/TrueSkyUnity.html";// "Simul/Documentation/TrueSkyUnity.html";
					//UnityEngine.Debug.Log();
					Help.BrowseURL(path);
				}*/
			EditorGUILayout.BeginHorizontal();
			string gv = SystemInfo.graphicsDeviceVersion;
			EditorGUILayout.LabelField("Unity Renderer", gv);
			if (gv.Contains("Direct3D 11"))
				EditorGUILayout.LabelField("GOOD", GUILayout.Width(48));
			else
				EditorGUILayout.LabelField("Unsupported", GUILayout.Width(48));
			EditorGUILayout.EndHorizontal();
			/*
			EditorGUILayout.BeginHorizontal();
			trueSky.LicenseKey		=EditorGUILayout.TextField("License Key", trueSky.LicenseKey);
			EditorGUILayout.LabelField(trueSky.GetLicenseExpiration(), GUILayout.Width(48));
			EditorGUILayout.EndHorizontal();
			*/
			EditorGUILayout.BeginVertical();
			trueSky.RenderInEditMode = EditorGUILayout.Toggle("Render in Edit Mode", trueSky.RenderInEditMode);
			trueSky.CloudShadowing = EditorGUILayout.Slider("Cloud Shadowing", trueSky.CloudShadowing, 0.0F, 1.0F);
			trueSky.CloudShadowSharpness=EditorGUILayout.Slider("Shadow sharpness",trueSky.CloudShadowSharpness,0.0F,0.1F);
			trueSky.CloudSteps = EditorGUILayout.IntSlider("Cloud Steps", trueSky.CloudSteps, 80, 250);
			trueSky.Downscale = EditorGUILayout.IntSlider("Downscale", trueSky.Downscale, 1, 3);
			if (PlayerSettings.colorSpace != ColorSpace.Linear)
			{
				trueSky.Gamma = EditorGUILayout.Slider("Gamma", trueSky.Gamma, 0.4F, 1.0F);
			}
			else
				trueSky.Gamma = EditorGUILayout.Slider("Gamma", trueSky.Gamma, 1.0F, 1.0F);
			EditorGUILayout.EndVertical();
			//EditorGUILayout.EndVertical();
			trueSky.sequence = (Sequence)EditorGUILayout.ObjectField("Sequence Asset", trueSky.sequence, typeof(Sequence), false);

			EditorGUILayout.Space();
			trueSky.time = EditorGUILayout.FloatField("Time", trueSky.time);
			trueSky.speed = EditorGUILayout.FloatField("Speed", trueSky.speed);
			trueSky.ShowDiagnostics = EditorGUILayout.BeginToggleGroup("Diagnostics", trueSky.ShowDiagnostics);
			trueSky.ShowCompositing = EditorGUILayout.Toggle("ShowCompositing", trueSky.ShowCompositing);
			trueSky.ShowCloudCrossSections = EditorGUILayout.Toggle("ShowCloudCrossSections", trueSky.ShowCloudCrossSections);
			trueSky.ShowAtmosphericTables = EditorGUILayout.Toggle("ShowAtmosphericTables", trueSky.ShowAtmosphericTables);
			trueSky.ShowCelestialDisplay = EditorGUILayout.Toggle("ShowCelestialDisplay", trueSky.ShowCelestialDisplay);
			EditorGUILayout.EndToggleGroup();
			if (trueSKY.advancedMode)
			{
				if (GUILayout.Button("Recompile Shaders"))
				{
					recomp = true;
				}
				if (GUILayout.Button("PrepareForUpload"))
				{
					prepareUpload = true;
				}
			}
#if UNITY_4_5
		EditorGUILayout.BeginHorizontal();
		if(trueSKY.advancedMode)
		if(GUILayout.Button("Export Package"))
		{
			string dir="C:/Program Files (x86)/CruiseControl.NET/server/release/dev/";
			string[] aFilePaths=Directory.GetFiles(dir,"trueSkyAlpha-3.0.8.*.unitypackage");
			int largest=1;
			foreach(string p in aFilePaths)
			{
				string pat = @"trueSky-.*\..*\..*\.(.*)\.unitypackage";
				  // Instantiate the regular expression object.
				Regex r = new Regex(pat, RegexOptions.IgnoreCase);
			  // Match the regular expression pattern against a text string.
				Match m=r.Match(p);
				int matchCount = 0;
				string str=p+": --- ";
				while (m.Success) 
				{
					str+=(" Match"+(++matchCount));
					
					{
						Group g = m.Groups[1];
						string numstr=g.ToString();
						str+=(" Group "+numstr);
						int ct=Convert.ToInt32(numstr);
						if(ct>largest)
							largest=ct;
					}
					m = m.NextMatch();
				}
			}
			largest++;
			string fileName=dir+"trueSky-3.0.0."+largest.ToString("D4")+".unitypackage";
			UnityEngine.Debug.Log(fileName);
			ExportPackage(fileName);
		}
        EditorGUILayout.EndHorizontal();
#endif
			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
				SceneView.RepaintAll();
			}
			if (recomp)
			{
				trueSKY.RecompileShaders();
				recomp = false;
			}
			if (prepareUpload)
			{
				PrepareForUpload();
				prepareUpload = false;
			}
		}
		void PrepareForUpload()
		{
			// This is to get around Unity's problems with Assets folders. We will copy the contents of the Gizmos and Plugins folders into the Simul folder. When the 
			// plugin is installed in a new project, these folders will be moved back down to the root.
			char s = Path.DirectorySeparatorChar;
			String assetsPath = Environment.CurrentDirectory + s + "Assets";
			// 1. The gizmos
			String gizmosPath = assetsPath + s + "Gizmos";
			if (!Directory.Exists(gizmosPath))
				Directory.Delete(gizmosPath, true);
			DirectoryCopy.Copy(gizmosPath, assetsPath + s + "Simul" + s + "Gizmos", true, true, false, true);
			// 2. The plugins folder
			String pluginsPath = assetsPath + s + "Plugins";
			if (!Directory.Exists(pluginsPath))
				Directory.Delete(pluginsPath, true);
			DirectoryCopy.Copy(pluginsPath, assetsPath + s + "Simul" + s + "Plugins", true, true, false, true);
		}
#if UNITY_4_5
	void ExportPackage(string fileName)
	{
		List<string> paths=new List<string>();
		AddAssetPathToExport("Simul",paths);
		AssetDatabase.ExportPackage(paths.ToArray(),fileName,ExportPackageOptions.Default);
		//Interactive|ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
		UnityEngine.Debug.Log("Exported: "+fileName);
	}
	void AddAssetPathToExport(string asset_path,List<string> paths)
	{
		//UnityEngine.Debug.Log("Application.dataPath: "+Application.dataPath);
		string full_path=Path.GetFullPath(Path.Combine(Application.dataPath,asset_path));
		//UnityEngine.Debug.Log("Add Path: "+full_path);
		string[] aFilePaths=Directory.GetFiles(full_path,"*.*",SearchOption.AllDirectories);
 
		// enumerate through the list of files loading the assets they represent and getting their type
		foreach (string p in aFilePaths)
		{
			string asset_filename=p.Substring(Application.dataPath.Length-6);// remove asset path and slash.
			if(!asset_filename.Contains(".meta"))
				continue;
			asset_filename=asset_filename.Replace(".meta","");
			if(paths.Contains(asset_filename))
				continue;
			asset_filename=asset_filename.Replace("\\","/");
			//UnityEngine.Debug.Log("Add: "+asset_filename);
			paths.Add(asset_filename);
         }
	}
#endif
	}
}
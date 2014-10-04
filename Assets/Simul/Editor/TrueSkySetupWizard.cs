using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace simul
{
	public class TrueSkySetupWizard : EditorWindow
	{
		enum Stage
		{
			PRE_START, START, FIND_SEQUENCE, FIND_CAMERA, FIND_TRUESKY, FIND_SUN, FINISH
		};
		Stage stage = Stage.PRE_START;
		[MenuItem("GameObject/Remove trueSKY from Scene", false, 200000)]
		public static void RemoveTrueSky()
		{
			UnityEngine.Object[] objects = FindObjectsOfType(typeof(Light));
			foreach (UnityEngine.Object t in objects)
			{
				Light l = (Light)t;
				if (l.GetComponent<SimulSun>() != null)
					DestroyImmediate(l.GetComponent<SimulSun>());
			}
			objects = FindObjectsOfType(typeof(Camera));
			foreach (UnityEngine.Object t in objects)
			{
				Camera c = (Camera)t;
				if (c.GetComponent<TrueSkyCamera>() != null)
					DestroyImmediate(c.GetComponent<TrueSkyCamera>());
			}
			objects = FindObjectsOfType(typeof(trueSKY));
			foreach (UnityEngine.Object o in objects)
			{
				trueSKY ts = (trueSKY)o;
				if (ts != null && ts.gameObject != null)
				{
					DestroyImmediate(ts.gameObject);
					break;
				}
			}
		}
		[MenuItem("GameObject/Create Other/Initialize trueSKY in Scene", false, 100000)]
		public static void InitTrueSkySequence1()
		{
			InitTrueSkySequence();
		}
		[MenuItem("GameObject/Initialize trueSKY in Scene", false, 100000)]
		public static void InitTrueSkySequence()
		{
			TrueSkySetupWizard w = (TrueSkySetupWizard)EditorWindow.GetWindow(typeof(TrueSkySetupWizard));
			w.title = "trueSKY";
		}
		void GetSceneFilename()
		{
			sceneFilename = EditorApplication.currentScene;
		}
		void OnGUI()
		{
			if (stage == Stage.PRE_START)
			{
				DirectoryCopy.CopyPluginsAndGizmosToAssetsFolder();
				stage = Stage.START;
			}
			GUIStyle textStyle = new GUIStyle();
			textStyle.wordWrap = true;
			GUILayout.Label("Initialize trueSKY in Scene", EditorStyles.boldLabel);

			if (stage == Stage.START)
			{
				GetSceneFilename();
				if (sceneFilename.Length > 0)
					GUILayout.Label("This wizard will initialize trueSKY for the current scene:\n\n" + sceneFilename + ".", textStyle);
				else
				{
					GUILayout.Label("This wizard will initialize trueSKY for the current scene.\nThe current scene has not yet been saved - plase do this first, so the wizard knows where to put the trueSKY data.", textStyle);
				}
			}
			if (stage == Stage.FIND_SEQUENCE)
			{
				FindSequence();
				if (sequence != null)
				{
					GUILayout.Label("A sequence was found in the current scene directory. You can change it if necessary:", textStyle);
				}
				else
				{
					GUILayout.Label("No sequence was found in the current scene directory, You can select one, or one will be created.", textStyle);
				}
				sequence = (Sequence)EditorGUILayout.ObjectField("Sequence Asset", sequence, typeof(Sequence), false);
			}
			if (stage == Stage.FIND_CAMERA)
			{
				FindCamera();
				if (mainCamera != null)
				{
					GUILayout.Label("A main camera was found in the current scene. The TrueSkyCamera script will be assigned to this camera, or one of your choosing.", textStyle);
				}
				else
				{
					GUILayout.Label("No main camera was found in the current scene, You can select one, or one will be created. In either case, trhe TrueSkyCamera script will be assigned to the camera.", textStyle);
				}
				mainCamera = (Camera)EditorGUILayout.ObjectField("Camera", mainCamera, typeof(Camera), true);

			}
			if (stage == Stage.FIND_TRUESKY)
			{
				FindTrueSky();
				if (trueSky != null)
				{
					GUILayout.Label("A trueSKY GameObject " + trueSky.name + " was found in the current scene.", textStyle);
					//	trueSky=(trueSKY)EditorGUILayout.ObjectField("trueSKY",trueSky,typeof(trueSKY),true);
				}
				else
				{
					GUILayout.Label("No trueSKY GameObject was found in the current scene, one will be created.", textStyle);
				}
			}
			if (stage == Stage.FIND_SUN)
			{
				FindSun();
				if (simulSun != null)
				{
					GUILayout.Label("A Directional Light GameObject " + lightGameObject.name + " was found in the current scene, with the SimulSun script assigned to it.", textStyle);
				}
				else if (lightGameObject != null)
				{
					GUILayout.Label("A Directional Light GameObject " + lightGameObject.name + " was found in the current scene. The SimulSun script will be assigned to it.", textStyle);
				}
				else
				{
					GUILayout.Label("No Directional Light GameObject was found in the current scene, one will be created.\nThe SimulSun script will be assigned to the light.", textStyle);
				}
			}
			if (stage == Stage.FINISH)
			{
				GUILayout.Label("When you click Finish, trueSKY will be initialized for this scene.", textStyle);

				//EditorGUILayout.LabelField("Remove standard distance fog",GUILayout.Width(48));
				removeFog = GUILayout.Toggle(removeFog, "Remove standard distance fog");
				//EditorGUILayout.LabelField("Remove skybox from camera",GUILayout.Width(48));
				removeSkybox = GUILayout.Toggle(removeSkybox, "Remove skybox from camera");
			}
			GUILayout.FlexibleSpace();
			textStyle.alignment = TextAnchor.MiddleRight;
			GUILayout.Label(GetBottomText(), textStyle);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (stage == Stage.START)
			{
				if (GUILayout.Button("Cancel"))
					Close();
				if (sceneFilename.Length == 0)
					GUI.enabled = false;
				if (GUILayout.Button("Next"))
					OnWizardNext();
				if (sceneFilename.Length == 0)
					GUI.enabled = true;
			}
			else if (stage < Stage.FINISH)
			{
				if (GUILayout.Button("Back"))
					OnWizardBack();
				if (GUILayout.Button("Next"))
					OnWizardNext();
			}
			else
			{
				if (GUILayout.Button("Back"))
					OnWizardBack();
				if (GUILayout.Button("Finish"))
				{
					Finish();
					Close();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
		string GetBottomText()
		{
			if (stage == Stage.START)
				return "Click Next to begin.";
			else if (stage < Stage.FINISH)
				return "Click Next to proceed.";
			else
				return "Click Finish to create the trueSKY assets, objects, and components.";
		}
		// When the user pressed the "Apply" button OnWizardOtherButton is called.
		void OnWizardOtherButton()
		{
			Close();
		}

		void OnWizardBack()
		{
			stage--;
		}
		Sequence sequence = null;
		Camera mainCamera = null;
		trueSKY trueSky = null;
		GameObject lightGameObject = null;
		SimulSun simulSun;
		public bool removeFog = true;
		public bool removeSkybox = true;
		string sceneFilename;
		void FindSequence()
		{
			// 1. Is there a sequence asset in the current scene's assets directory?
			string dir = Path.GetDirectoryName(sceneFilename);
			// Find any sequence asset:
			string[] assetFiles = Directory.GetFiles(dir, "*.asset");
			foreach (string p in assetFiles)
			{
				Sequence sq = AssetDatabase.LoadAssetAtPath(p, typeof(Sequence)) as Sequence;
				if (sq != null)
				{
					sequence = sq;
				}
			}
		}
		void FindCamera()
		{
			// Now we find the main camera, and add the TrueSkyCamera.cs script to it, IF it is not already present:
			//		GameObject.Find("myObject").camera;
			mainCamera = Camera.main;
		}
		void Finish()
		{
			if (sequence == null)
			{
				string sequenceFilename = sceneFilename.Replace(".unity", "_sq.asset");
				sequence = CustomAssetUtility.CreateAsset<Sequence>(sequenceFilename);
			}
			TrueSkyCamera trueSkyCamera = mainCamera.gameObject.GetComponent<TrueSkyCamera>();
			if (trueSkyCamera == null)
			{
				trueSkyCamera = mainCamera.gameObject.AddComponent("TrueSkyCamera") as TrueSkyCamera;
			}
			if (trueSky == null)
			{
				GameObject g = new GameObject("trueSky");
				trueSky = g.AddComponent<trueSKY>();
			}
			if (lightGameObject == null)
			{
				lightGameObject = new GameObject("Sunlight");
				Light light = lightGameObject.AddComponent<Light>();
				light.type = LightType.Directional;
			}
			if (simulSun == null)
			{
				simulSun = lightGameObject.AddComponent<SimulSun>();
			}
			simulSun.trueSky = trueSky;
			if (removeFog)
			{
				RenderSettings.fog = false;
			}
			if (removeSkybox && mainCamera != null)
			{
				if (mainCamera.clearFlags != CameraClearFlags.SolidColor)
				{
					mainCamera.clearFlags = CameraClearFlags.SolidColor;
					mainCamera.backgroundColor = Color.black;
				}
			}
			// Now the sequence must be assigned to the trueSKY object.
			trueSky.sequence = sequence;
			trueSky.time = 0.5F;
		}
		void FindTrueSky()
		{
			// And we need a trueSKY object in the scene.
			UnityEngine.Object[] trueSkies;
			trueSkies = FindObjectsOfType(typeof(trueSKY));
			foreach (UnityEngine.Object t in trueSkies)
			{
				trueSky = (trueSKY)t;
			}
		}
		void FindSun()
		{
			// And we need a trueSKY object in the scene.
			UnityEngine.Object[] lights;
			lights = FindObjectsOfType(typeof(Light));
			foreach (UnityEngine.Object t in lights)
			{
				Light l = (Light)t;
				if (l.GetComponent<SimulSun>() != null)
				{
					lightGameObject = l.gameObject;
					simulSun = (SimulSun)l.GetComponent<SimulSun>();
					return;
				}
				else if (lightGameObject == null)
					lightGameObject = l.gameObject;
			}
		}
		void OnWizardNext()
		{
			stage++;
		}
	}
}
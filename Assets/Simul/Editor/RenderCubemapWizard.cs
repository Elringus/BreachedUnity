using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;

// Render scene from a given point into a static cube map.
	// Place this script in Editor folder of your project.
	// Then use the cubemap with one of Reflective shaders!
public class RenderCubemapWizard : ScriptableWizard
	{
		public Transform renderFromPosition;
		public Cubemap cubemap;
		
		void OnWizardUpdate ()
		{
			helpString = "Select transform to render from and cubemap to render into";
			isValid = (renderFromPosition != null) && (cubemap != null);
		}

		void OnWizardCreate()
		{
			// create temporary camera for rendering
			GameObject go=new GameObject("CubemapCamera");
			//Camera c=
				go.AddComponent<Camera>();
			// place it on the object
			go.transform.position = renderFromPosition.position;
			go.transform.rotation=Quaternion.identity;
			go.gameObject.AddComponent("TrueSkyCamera") ;
			// render into cubemap		
			go.camera.RenderToCubemap( cubemap );
			
			// destroy temporary camera
			DestroyImmediate( go );
		}
		
	[MenuItem("GameObject/Render into Cubemap")]
		static void RenderCubemap()
	{
			ScriptableWizard.DisplayWizard<RenderCubemapWizard>("Render cubemap");
		}
	}
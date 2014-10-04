using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;

// Attach this script to an object that uses a Reflective shader.
// Realtime reflective cubemaps!
[ExecuteInEditMode()]

class RealTimeCubemap:MonoBehaviour
{
	public int cubemapSize=32;
	public float nearClip=1.0F;
	public float farClip=50000.0F;
	public LayerMask layerMask=63;
	public bool oneFacePerFrame=false;
	public Camera cam=null;
	public Cubemap cubemap=null;
	static int last_face=0;
//	private RenderTexture rtex;
	void Start()
	{
		// render all six faces at startup
		UpdateCubemap(63);
	}

	void LateUpdate()
	{
		if(oneFacePerFrame)
		{
			int faceToRender=last_face;
			last_face++;
			last_face=last_face%6;
			int faceMask=1<<faceToRender;
			UpdateCubemap(faceMask);
		}
		else
		{
			UpdateCubemap(63); // all six faces
		}
	}

	void UpdateCubemap(int faceMask)
	{
		if(!cubemap)
			return;
		if(!cam)
			return;
		/*
		TrueSkyCamera trueSkyCamera=mainCamera.gameObject.GetComponent<TrueSkyCamera>();
		if(!cam)
		{
			GameObject go=new GameObject("CubemapCamera",typeof(Camera));
		//	TrueSkyCamera trueSkyCamera=go.AddComponent("TrueSkyCamera") as TrueSkyCamera;
			go.hideFlags=HideFlags.HideAndDontSave;
			go.transform.position=transform.position;
			go.transform.rotation=Quaternion.identity;
			cam=go.cam;
			cam.cullingMask=layerMask;
			cam.nearClipPlane=nearClip;
			cam.farClipPlane=farClip;
			cam.enabled=true;
		}
		if(!rtex)
		{
			rtex=new RenderTexture(cubemapSize,cubemapSize,16);
			rtex.isPowerOfTwo=true;
			rtex.isCubemap=true;
			rtex.hideFlags=HideFlags.HideAndDontSave;
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
			{
				foreach(Material m in r.sharedMaterials)
				{
					if(m.HasProperty("_Cube"))
					{
						m.SetTexture("_Cube",rtex);
					}
				}
			}
		}*/

		cam.enabled=false;
		cam.transform.position=transform.position;
		cam.transform.rotation=Quaternion.identity;
		//UnityEngine.Debug.Log(" render cubemap"+faceMask);
		if(!cam.RenderToCubemap(cubemap,faceMask))
		{
			UnityEngine.Debug.LogError("Can't render cubemap");
		}
	}

	void OnDisable()
	{
	}
}
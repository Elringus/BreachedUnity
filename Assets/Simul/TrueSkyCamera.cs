using UnityEngine;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace simul
{
	[ExecuteInEditMode]
	public class TrueSkyCamera : MonoBehaviour
	{
		public enum RenderStyle
		{
			DEFAULT_STYLE = 0
			,UNREAL_STYLE = 1
			,UNITY_STYLE_FORWARD= 2
			,UNITY_STYLE_DEFERRED = 3
			,UNITY_STYLE_FORWARD_FLIPPED = 4
		};

	#region imports
		// An event ID that will hopefully be sufficiently unique to trueSKY - if not, change this.
		const int TRUESKY_EVENT_ID=13476;
		[DllImport(SimulImports.renderer_dll)]
		private static extern void UnitySetRenderFrameValues(int view_id,float[] viewMatrix4x4,float[] projMatrix4x4,System.IntPtr fullResDepthTexture2D
			,RenderStyle renderStyle);
		[DllImport(SimulImports.renderer_dll)]
		private static extern int StaticGetOrAddView(System.IntPtr ident);
		[DllImport(SimulImports.renderer_dll)]
		private static extern int StaticOnDeviceChanged(System.IntPtr device);
	#endregion
		private int view_ident;
		private static int last_view_ident=0;
		TrueSkyCamera()
		{
			view_ident=last_view_ident+1;
			last_view_ident++;
			//UnityEngine.Debug.Log("view_ident "+view_ident); 
		}
		private static Mutex mut = new Mutex();
		public bool flippedView=false;
		static Texture2D _dummyTexture;
		RenderTexture _depthTexture;
		static Material _flippedDepthMaterial=null;
		static Material _deferredDepthMaterial=null;
		static Shader _flippedShader=null;
		static Shader _deferredShader=null;
		void OnPreRender()
		{
			if(!enabled||!gameObject.activeInHierarchy)
			{
				UnityEngine.Debug.Log("OnPreRender disabled"); 
				return;
			}
			SimulImports.Init();
			mut.WaitOne();
			if (_depthTexture == null||_depthTexture.width!=camera.pixelWidth||_depthTexture.height!=camera.pixelHeight)
			{
				//Debug.Log("Resizing depth texture: "+camera.pixelWidth+" x "+camera.pixelHeight);
				_depthTexture=new RenderTexture((int)camera.pixelWidth,(int)camera.pixelHeight,32,RenderTextureFormat.ARGBFloat);
				_depthTexture.Create();
			}
			// Unity can't Blit without a source texture, so we create a small, unused dummy texture.
			if(_dummyTexture==null)
				_dummyTexture=new Texture2D(8,8,TextureFormat.Alpha8,false);
			camera.depthTextureMode|=DepthTextureMode.Depth;
			mut.ReleaseMutex();
			StaticGetOrAddView((System.IntPtr)view_ident);
		}
		private float[] ProjMatrixToTrueSkyFormat(RenderStyle renderStyle)
		{
			Matrix4x4 m=camera.projectionMatrix;
			if(renderStyle==RenderStyle.UNITY_STYLE_DEFERRED)
			{
				float[] a= { m.m00,m.m01,m.m02,m.m03
						   ,m.m10,-m.m11,m.m12,m.m13
						   ,m.m20,m.m21,m.m22,m.m23
						   ,m.m30,m.m31,m.m32,m.m33};
				return a;
			}
			else
			{
				float[] a= { m.m00,m.m01,m.m02,m.m03
						   ,m.m10,m.m11,m.m12,m.m13
						   ,m.m20,m.m21,m.m22,m.m23
						   ,m.m30,m.m31,m.m32,m.m33};
				return a;
			}
		}
		public float[] ViewMatrixToTrueSkyFormat(RenderStyle renderStyle)
		{
			Matrix4x4 m=camera.worldToCameraMatrix;
			if(flippedView&&renderStyle!=RenderStyle.UNITY_STYLE_DEFERRED)
			{
				// transpose the matrix, and swap the two middle rows:
				float[] a= { -m.m02, m.m12,-m.m22,-m.m32
							,-m.m00, m.m10,-m.m20,-m.m30
							, m.m01,-m.m11, m.m21, m.m31
							, m.m03,-m.m13, m.m23, m.m33};

				return a;
			}
			else
			{
				// transpose the matrix, and swap the two middle rows:
				float[] a= { m.m00,m.m10,m.m20,m.m30
							,m.m02,m.m12,m.m22,m.m32
							,m.m01,m.m11,m.m21,m.m31
							,m.m03,m.m13,m.m23,m.m33};
				return a;
			}
		}
		public RenderStyle GetRenderStyle()
		{
			RenderStyle renderStyle = RenderStyle.UNITY_STYLE_DEFERRED;
			if (this.camera.actualRenderingPath != RenderingPath.DeferredLighting)
			{
				if (flippedView)
					renderStyle=RenderStyle.UNITY_STYLE_DEFERRED;
				else
					renderStyle = RenderStyle.UNITY_STYLE_FORWARD;
			}
			return renderStyle;
		}
		void OnPostRender()
		{
			if(!enabled||!gameObject.activeInHierarchy)
			{
			//	UnityEngine.Debug.Log("OnPostRender disabled"); 
				return;
			}
			mut.WaitOne();
			RenderTexture old_rtex	=RenderTexture.active;
			RenderStyle renderStyle = GetRenderStyle();
			if(renderStyle!=RenderStyle.UNITY_STYLE_DEFERRED)
			{
				if(_flippedDepthMaterial==null)
				{
					_flippedShader=Resources.Load("FlippedDepthShader",typeof(Shader)) as Shader;
					if(_flippedShader!=null)
						_flippedDepthMaterial=new Material(_flippedShader);
					else
						UnityEngine.Debug.LogError("Shader not found: trueSKY needs flippedDepthShader.shader, located in the Assets/Simul/Resources directory");
				}
				Graphics.Blit(_dummyTexture,_depthTexture,_flippedDepthMaterial);
			}
			else
			{
				if(_deferredDepthMaterial==null)
				{
					_deferredShader=Resources.Load("DeferredDepthShader",typeof(Shader)) as Shader;
					if(_deferredShader!=null)
						_deferredDepthMaterial=new Material(_deferredShader);
					else
						UnityEngine.Debug.LogError("Shader not found: trueSKY needs DeferredDepthShader.shader, located in the Assets/Simul/Resources directory");
				}
				Graphics.Blit(_dummyTexture,_depthTexture,_deferredDepthMaterial);
			}
			RenderTexture.active	=old_rtex;
			int view_id=StaticGetOrAddView((System.IntPtr)view_ident);
			//UnityEngine.Debug.Log("view_ident "+view_ident+" view_id "+view_id); 
			UnitySetRenderFrameValues(view_id,ViewMatrixToTrueSkyFormat(renderStyle),ProjMatrixToTrueSkyFormat(renderStyle)
				,_depthTexture.GetNativeTexturePtr(),renderStyle);
			/* Issue a plugin event with arbitrary integer identifier.
			  The plugin can distinguish between different
			  things it needs to do based on this ID. In our case, the id means "draw the trueSKY environment". */
			GL.IssuePluginEvent(TRUESKY_EVENT_ID+view_id);
			//UnityEngine.Debug.Log("TRUESKY_EVENT_ID"); 
			mut.ReleaseMutex();
		}
		bool _initialized = false;
		void Awake()
		{
			if (_initialized)
				return;
			_initialized=true;
		}
		static void OnDisable()
		{
			_flippedDepthMaterial=null;
			_deferredDepthMaterial=null;
			_flippedShader=null;
			_deferredShader=null;
		}
	}
}
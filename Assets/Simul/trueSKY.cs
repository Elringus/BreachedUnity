//#define TRUESKY_LOGGING
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
namespace simul
{
	class SimulImports
	{
		static bool _initialized = false;
		[DllImport(renderer_dll)]
		private static extern void StaticPushPath(string name, string path);
		[DllImport(renderer_dll)]
		private static extern void StaticPopPath(string name);
#if SIMUL_DEBUG_CALLBACK
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	delegate void TDebugOutputCallback(string output);
	[DllImport(SimulImports.renderer_dll)]
	private static extern void StaticSetDebugOutputCallback(TDebugOutputCallback cb);

	private static Mutex logMutex=new Mutex();
	static string debug_log;
	static TDebugOutputCallback debugOutputCallback =
		(string s) =>
		{
			logMutex.WaitOne();
			debug_log+=s;
			logMutex.ReleaseMutex();
		};

	static void OutputTrueSkyDebug()
	{
		logMutex.WaitOne();
		UnityEngine.Debug.Log(debug_log);
		logMutex.ReleaseMutex();
		debug_log="";
	}
#endif
		static SimulImports()
		{
#if SIMUL_DEBUG_CALLBACK
	//	StaticSetDebugOutputCallback(debugOutputCallback);
#endif
		}
		int instanceCount = 0;
		SimulImports()
		{
			instanceCount++;
		}
		~SimulImports()
		{
			instanceCount--;
			if (_initialized && instanceCount == 0)
			{
				StaticPopPath("ShaderBinaryPath");
				StaticPopPath("ShaderPath");
				StaticPopPath("TexturePath");
#if SIMUL_DEBUG_CALLBACK
			StaticSetDebugOutputCallback(null);
#endif
			}
		}
		public static void Init()
		{
			if (_initialized)
				return;
			StaticPushPath("ShaderBinaryPath", Application.dataPath + @"\Simul\shaderbin");
			StaticPushPath("ShaderPath", Application.dataPath + @"\Simul\Platform\DirectX11\HLSL");
			StaticPushPath("TexturePath", Application.dataPath + @"\Simul\Media\Textures");
			_initialized = true;
		}
#if UNITY_IPHONE || UNITY_XBOX360
	// On iOS and Xbox 360 plugins are statically linked into
	// the executable, so we have to use __Internal as the
	// library name.
	public const string renderer_dll ="__Internal";
#else
		// Other platforms load plugins dynamically, so pass the name
		// of the plugin's dynamic library. C:/Simul/dev/Simul/exe/Win32/VC11/Release/
		public const string renderer_dll = @"TrueSkyPluginRender_MT";
#endif
	}

	[ExecuteInEditMode]
	public class trueSKY : MonoBehaviour
	{
		#region Imports
		// Native plugin rendering events are only called if a plugin is used
		// by some script. This means we have to DllImport at least
		// one function in some active script.
		// For this example, we'll call into plugin's SetTimeFromUnity
		// function and pass the current time so the plugin can animate.
		[DllImport(SimulImports.renderer_dll)]		private static extern void SetTimeFromUnity(float t);
		[DllImport(SimulImports.renderer_dll)]		private static extern void SetTextureFromUnity(System.IntPtr texture);

		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticEnableLogging(string logfile);
		[DllImport(SimulImports.renderer_dll)]		private static extern int StaticInitInterface();
		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticPushPath(string name, string path);

		[DllImport(SimulImports.renderer_dll)]		private static extern int StaticTick(float deltaTime);

		// We import StaticSetSequenceTxt(const char *) rather than StaticSetSequence(std::string), as const char * converts from c# string.
		[DllImport(SimulImports.renderer_dll)]		private static extern int StaticSetSequenceTxt(string SequenceInput);
		[DllImport(SimulImports.renderer_dll)]		private static extern float StaticGetRenderFloat(string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticSetRenderFloat(string name,float value);
		[DllImport(SimulImports.renderer_dll)]		private static extern int StaticGetRenderInt(string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticSetRenderInt(string name,int value);
		[DllImport(SimulImports.renderer_dll)] 		private static extern void StaticSetRenderBool(string name, bool value);
		[DllImport(SimulImports.renderer_dll)]		private static extern bool StaticGetRenderBool(string name);

		// These are for keyframe editing:
		[DllImport(SimulImports.renderer_dll)]		private static extern int StaticRenderGetNumKeyframes(int layer);
		[DllImport(SimulImports.renderer_dll)]		private static extern uint	StaticRenderInsertKeyframe		(int layer,float t );
		[DllImport(SimulImports.renderer_dll)]		private static extern void	StaticRenderDeleteKeyframe		(uint uid );
		[DllImport(SimulImports.renderer_dll)]		private static extern uint	StaticRenderGetKeyframeByIndex	(int layer,int index);

		// Getting and changing properties of keyframes.
		[DllImport(SimulImports.renderer_dll)]		private static extern bool StaticRenderKeyframeHasFloat(uint uid,string name);
		
		[DllImport(SimulImports.renderer_dll)]		private static extern void	StaticRenderKeyframeSetFloat	(uint uid,string name,float value);
		[DllImport(SimulImports.renderer_dll)]		private static extern float StaticRenderKeyframeGetFloat	(uint uid,string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern bool StaticRenderKeyframeHasInt		(uint uid,string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern void	StaticRenderKeyframeSetInt		(uint uid,string name,int value);
		[DllImport(SimulImports.renderer_dll)]		private static extern int	StaticRenderKeyframeGetInt		(uint uid,string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern bool StaticRenderKeyframeHasBool		(uint uid,string name);
		[DllImport(SimulImports.renderer_dll)]		private static extern void	StaticRenderKeyframeSetBool		(uint uid,string name,bool value);
		[DllImport(SimulImports.renderer_dll)]		private static extern bool	StaticRenderKeyframeGetBool		(uint uid,string name);


		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticGetRenderString(string name, StringBuilder str, int len);
		[DllImport(SimulImports.renderer_dll)]		private static extern void StaticSetRenderString(string name, string value);
		[DllImport(SimulImports.renderer_dll)]		public static extern void StaticTriggerAction(string name);
		#endregion
		#region API

		// These are for keyframe editing:

		public int GetNumSkyKeyframes()
		{
			return StaticRenderGetNumKeyframes(0);
		}
		public int GetNumCloudKeyframes()
		{
			return StaticRenderGetNumKeyframes(1);
		}
		public int GetNumCloud2DKeyframes()
		{
			return StaticRenderGetNumKeyframes(2);
		}

		public uint InsertSkyKeyframe(float t)
		{
			return StaticRenderInsertKeyframe(0,t);
		}
		public uint InsertCloudKeyframe(float t)
		{
			return StaticRenderInsertKeyframe(1,t);
		}
		public uint Insert2DCloudKeyframe(float t)
		{
			return StaticRenderInsertKeyframe(2,t);
		}
		public void DeleteKeyframe(uint uid)
		{
			StaticRenderDeleteKeyframe(uid);
		}
		
		public uint GetSkyKeyframeByIndex(int index)
		{
			return StaticRenderGetKeyframeByIndex(0,index);
		}

		public uint GetCloudKeyframeByIndex(int index)
		{
			return StaticRenderGetKeyframeByIndex(1,index);
		}

		public uint GetCloud2DKeyframeByIndex(int index)
		{
			return StaticRenderGetKeyframeByIndex(2,index);
		}

		// Getting and changing properties of keyframes.
		public void SetKeyframeValue(uint uid,string name,object value)
		{
			UnityEngine.Debug.Log("trueSKY.SetKeyframeValue "+uid+" "+name+" "+value);
			UnityEngine.Debug.Log("type is "+value.GetType());
			if(value.GetType()==typeof(double))
			{
				UnityEngine.Debug.Log("it's a double");
				double d=(double)value;
				StaticRenderKeyframeSetFloat(uid,name,(float)d);
			}
			else if(value.GetType()==typeof(float)||value.GetType()==typeof(double))
			{
				UnityEngine.Debug.Log("it's a float");
				StaticRenderKeyframeSetFloat(uid,name,(float)value);
			}
			else if(value.GetType()==typeof(int))
			{
				UnityEngine.Debug.Log("it's a int");
				StaticRenderKeyframeSetInt(uid,name,(int)value);
			}
			else if(value.GetType()==typeof(bool))
			{
				UnityEngine.Debug.Log("it's a bool");
				StaticRenderKeyframeSetBool(uid,name,(bool)value);
			}
		}
		public object GetKeyframeValue(uint uid,string name)
		{
			if(StaticRenderKeyframeHasFloat(uid,name))
				return StaticRenderKeyframeGetFloat(uid,name);
			if(StaticRenderKeyframeHasInt(uid,name))
				return StaticRenderKeyframeGetInt(uid,name);
			return 0;
		}
		
		#endregion
		bool _renderInEditMode = true;
		public bool RenderInEditMode
		{
			get
			{
				return _renderInEditMode;
			}
			set
			{
				if (_renderInEditMode != value) try
					{
						_renderInEditMode = value;
						StaticSetRenderBool("EnableRendering", Application.isPlaying || _renderInEditMode);
						//RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		static public void RepaintAll()
		{
		}
		[SerializeField]
		float _time;
		/// <summary>
		/// Time in the sequence, set from some external script, e.g. the sequence editor, or modified per-frame by the speed value.
		/// </summary>
		/// <param name="t"></param>
		public float time
		{
			get
			{
#if TRUESKY_LOGGING
				Debug.Log("trueSKY get _time " + _time);
#endif
				return _time;
			}
			set
			{
				if (_time != value)
				{
					try
					{
						_time = value;
#if TRUESKY_LOGGING
						Debug.Log("trueSKY time set from Unity as " + _time);
#endif
						StaticSetRenderFloat("Time", value);
						// What if, having changed this value, we now ask for a light colour before the next Update()?
						// so we force it:
						StaticTick(0.0f);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
				}
				else
				{
#if TRUESKY_LOGGING
				Debug.Log("_time is already "+_time);
#endif
				}
			}
		}
		[SerializeField]
		float _speed = 10.0F;
		/// <summary>
		/// Rate of time in the sequence.
		/// </summary>
		/// <param name="t"></param>
		public float speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
			}
		}
		static public bool advancedMode
		{
			get
			{
				string simul = Environment.GetEnvironmentVariable("SIMUL_BUILD");
				return (simul != null && simul.Length > 0);
			}
			set
			{
			}
		}
#if LICENSING
    public string LicenseKey
    {
        get
        {
            return _licenseKey;
        }
        set
        {
            if (_licenseKey != value) try
             {
                    _licenseKey=value;
				    StaticSetRenderString("LicenseKey",_licenseKey);
				    if(!Application.isPlaying)
					    RepaintAll();
                }
                catch (Exception exc)
                {
                    UnityEngine.Debug.Log(exc.ToString());
                }
        }
    }
    [SerializeField]
    string _licenseKey = "";
#endif
		public string GetLicenseExpiration()
		{
			StringBuilder str = new StringBuilder("", 20);
			try
			{
				StaticGetRenderString("LicenseExpiration", str, 16);
			}
			catch (Exception exc)
			{
				UnityEngine.Debug.Log(exc.ToString());
			}
			return str.ToString();
		}
		[SerializeField]
		bool _showDiagnostics = false;
		[SerializeField]
		float _cloudShadowing = 0.5F;
		[SerializeField]
		float _cloudShadowSharpness=0.05F;
		[SerializeField]
		bool _showCloudCrossSections = false;
		[SerializeField]
		bool _showCompositing = false;
		[SerializeField]
		bool _showFades = false;
		[SerializeField]
		bool _showCelestials = false;
		public float CloudShadowing
		{
			get
			{
				return _cloudShadowing;
			}
			set
			{
				if (_cloudShadowing != value) try
					{
						_cloudShadowing = value;
						StaticSetRenderFloat("SimpleCloudShadowing", _cloudShadowing);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public float CloudShadowSharpness
		{
			get
			{
				return _cloudShadowSharpness;
			}
			set
			{
				if(_cloudShadowSharpness!=value) try
					{
						_cloudShadowSharpness=value;
						StaticSetRenderFloat("SimpleCloudShadowSharpness",_cloudShadowSharpness);
						if(!Application.isPlaying)
							RepaintAll();
					}
					catch(Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public bool ShowDiagnostics
		{
			get
			{
				return _showDiagnostics;
			}
			set
			{
				if (_showDiagnostics != value) try
					{
						_showDiagnostics = value;
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public bool ShowCelestialDisplay
		{
			get
			{
				return _showCelestials;
			}
			set
			{
				bool v = value & _showDiagnostics;
				if (_showCelestials != v) try
					{
						_showCelestials = v;
						StaticSetRenderBool("ShowCelestialDisplay", _showCelestials);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public bool ShowAtmosphericTables
		{
			get
			{
				return _showFades;
			}
			set
			{
				bool v = value & _showDiagnostics;
				if (_showFades != v) try
					{
						_showFades = v;
						StaticSetRenderBool("ShowFades", _showFades);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public bool ShowCompositing
		{
			get
			{
				return _showCompositing;
			}
			set
			{
				bool v = value & _showDiagnostics;
				if (_showCompositing != v) try
					{
						_showCompositing = v;
						StaticSetRenderBool("ShowCompositing", _showCompositing);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public bool ShowCloudCrossSections
		{
			get
			{
				return _showCloudCrossSections;
			}
			set
			{
				bool v = value & _showDiagnostics;
				if (_showCloudCrossSections != v) try
					{
						_showCloudCrossSections = v;
						StaticSetRenderBool("ShowCloudCrossSections", _showCloudCrossSections);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		public static void RecompileShaders()
		{
			StaticTriggerAction("RecompileShaders");
			if (!Application.isPlaying)
				RepaintAll();
		}
		[SerializeField]
		Sequence _sequence;

		public Sequence sequence
		{
			get
			{
				return _sequence;
			}
			set
			{
				if (_sequence != value)
				{
					_sequence = value;
					Reload();
				}
			}
		}
		// The sequence .asset has changed, so we now reload the text in the asset.
		public void Reload()
		{
			if (_sequence == null)
				return;
			try
			{
				StaticSetSequenceTxt(_sequence.SequenceAsText);
			}
			catch (Exception exc)
			{
				UnityEngine.Debug.Log(exc.ToString());
			}
		}
		[SerializeField]
		int _CloudSteps = 200;
		public int CloudSteps
		{
			get
			{
				return _CloudSteps;
			}
			set
			{
				if (CloudSteps != value) try
					{
						_CloudSteps = value;
						StaticSetRenderInt("CloudSteps", _CloudSteps);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		[SerializeField]
		int _Downscale = 1;
		public int Downscale
		{
			get
			{
				return _Downscale;
			}
			set
			{
				if (_Downscale != value) try
					{
						_Downscale = value;
						StaticSetRenderInt("Downscale", 1 << _Downscale);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		[SerializeField]
		float _gamma = 1.0F;
		public float Gamma
		{
			get
			{
				return _gamma;
			}
			set
			{
				if (_gamma != value) try
					{
						_gamma = value;
						StaticSetRenderFloat("Gamma", _gamma);
						if (!Application.isPlaying)
							RepaintAll();
					}
					catch (Exception exc)
					{
						UnityEngine.Debug.Log(exc.ToString());
					}
			}
		}
		// Update is called once per frame
		void Update()
		{
			try
			{
				if (!_initialized)
					Init();
				_time += Time.deltaTime * _speed / (24.0F * 60.0F * 60.0F);
#if TRUESKY_LOGGING
			UnityEngine.Debug.Log("Update Set time " + _time);
#endif
				StaticSetRenderFloat("Time", _time);
				StaticTick(0.0f);
			}
			catch (Exception exc)
			{
				UnityEngine.Debug.Log(exc.ToString());
			}
		}
		bool _initialized = false;
		private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
		{
			/*  string libPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
						Path.DirectorySeparatorChar +
						"Lib" +
						Path.DirectorySeparatorChar;*/
			UnityEngine.Debug.Log("Resolving " + args.Name);
			return Assembly.Load("Assets\\Plugins\\x86\\dependencies\\" + args.Name);
		}
		/// <summary>
		/// Sun colour is given as a vector because Color class is clamped to [0,1] and irradiance can have arbitrary magnitude.
		/// </summary>
		/// <returns>Vector3</returns>
		public Vector3 getSunColour()
		{
			if (!_initialized)
				Init();
			Vector3 c = new Vector3(0, 0, 0);
			try
			{
				float r = StaticGetRenderFloat("SunIrradianceRed");
				float g = StaticGetRenderFloat("SunIrradianceGreen");
				float b = StaticGetRenderFloat("SunIrradianceBlue");
				c.x = r;
				c.y = g;
				c.z = b;
			}
			catch (Exception exc)
			{
				UnityEngine.Debug.Log(exc.ToString());
			}
			return c;
		}
		/// <summary>
		/// Returns the rotation of the sun as a Quaternion, for Directional Light objects.
		/// </summary>
		/// <returns></returns>
		public Quaternion getSunRotation()
		{
			float az = 0.0F, el = 0.0F;
			try
			{
				az = StaticGetRenderFloat("SunAzimuthDegrees");
				el = StaticGetRenderFloat("SunElevationDegrees");
			}
			catch (Exception exc)
			{
				_initialized = false;
				UnityEngine.Debug.Log("getSunRotation ");
				UnityEngine.Debug.Log(exc.ToString());
			}
			Quaternion q = Quaternion.Euler(el, 0.0F, az);
			return q;
		}
		void Awake()
		{
		}
		void Init()
		{
			try
			{
				if (_initialized)
					return;
				float savedTime = _time;
#if TRUESKY_LOGGING
			Debug.Log("trueSKY time restored from Unity scene as " + savedTime);
#endif
				SimulImports.Init();
#if TRUESKY_LOGGING
			StaticEnableLogging("trueSKYUnityRender.log");
#endif
				StaticInitInterface();
				Reload();
#if TRUESKY_LOGGING
			float t=StaticGetRenderFloat("time");
			Debug.Log("trueSKY initial time from sequence " + t);
			Debug.Log("savedTime " + savedTime);
#endif
				time = savedTime;
#if TRUESKY_LOGGING
			Debug.Log("Now time is " + time);
#endif
				_initialized = true;
				StaticSetRenderBool("RenderSky", true);
				StaticSetRenderBool("ReverseDepth", false);

				StaticSetRenderBool("EnableRendering", _renderInEditMode);
				StaticSetRenderBool("ShowFades", _showFades);
				StaticSetRenderBool("ShowCompositing", _showCompositing);
				StaticSetRenderBool("ShowCloudCrossSections", _showCloudCrossSections);
				StaticSetRenderInt("Downscale", 1 << _Downscale);
				StaticSetRenderInt("CloudSteps", _CloudSteps);
				StaticSetRenderFloat("SimpleCloudShadowing", _cloudShadowing);
				StaticSetRenderFloat("SimpleCloudShadowSharpness",_cloudShadowSharpness);
#if LICENSING
			StaticSetRenderString("LicenseKey",_licenseKey);
#endif
			}
			catch (Exception exc)
			{
				_initialized = false;
				print(exc.ToString());
			}
		}
	}
}
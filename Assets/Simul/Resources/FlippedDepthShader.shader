Shader "Custom/DepthShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		texelScale ("texelScale", Vector) = (0, 0,0) 
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	uniform sampler2D _CameraDepthTexture;
	struct v2f
	{
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
        float4 projPos : TEXCOORD1;
	};
	sampler2D _MainTex;
	float2 texelScale;
	inline void FixFlip(inout float x)
	{
		// Flip upside-down on DX-like platforms, if the buffer
		// we're rendering into is flipped as well.
		// FLIP_WORKAROUND_OFF check is only needed in pre 4.5 Unity, where _ProjectionParams.x has an incorrect value.
		// Can be safely removed in Unity 4.5.
		#if !defined(FLIP_WORKAROUND_ON)
			if (_ProjectionParams.x < 0)
				x *= -1.0;
		#endif
	}
	v2f vert( appdata_img v )
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv =  v.texcoord.xy;
		//FixFlip(o.pos.y);
        o.projPos = ComputeScreenPos(o.pos);
		
		o.projPos.y=1.0-o.projPos.y;
		return o;
	}
	float4 frag(v2f i) : COLOR
	{
		float4 ret;
		ret.a=1.0;
		//Linear01Depth linearizes...
		//float2 shadowTexCoord =  (i.projPos.xy / i.projPos.w + float2(0.0,0.0));
		//shadowTexCoord.x-=0.5*texelScale.x; 
		//shadowTexCoord.y=1.0-shadowTexCoord.y;
		//float depth =  (tex2D(_CameraDepthTexture,shadowTexCoord).r);
		float depth =  (tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.projPos)).r);
		//ret.rgba=100.0*(1.0-depth);
		//ret.rgb=Linear01Depth(depth);
		ret.rgb=depth;//1.0/(_ZBufferParams.x * depth + _ZBufferParams.y);
		
		// i.e. linear_depth = 1.0/_ZBufferParams.y when z=0 - i.e. depth=1, _ZBufferParams.y=1.0
		// and when z=1, linear_depth = 1.0/(_ZBufferParams.x + 1.0), which should be frustum near/far;
		// so n/f(x+1)=1.0
		//		(x+1) = f/n
		//			x = f/n -1
		//ret.r=1.0/(_ZBufferParams.x * depth + _ZBufferParams.y);
		//ret.gb=1.0/((1.0-1667.05920) * depth + 1667.05920);

		// suppose	x	=1.0 - m_FarClip / m_NearClip;
		// and		y	=m_FarClip / m_NearClip;
		// Then for r = 1.0/ (x depth + y), we get:
		//	depth=0 ->   r	= 1.0/y = near/far
		// depth=1	->	 r	= 1.0 / ( 1- far/near + far/near) 
		//					= 1.0 / 1.0 = 1.0
		return ret;//tex2D(_MainTex, i.uv) * _Intensity; 
	}
	ENDCG
	SubShader
	{
		Pass
		{
  			Blend Off
  
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }      

			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_nicest
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	} 
Fallback off


	
	




}

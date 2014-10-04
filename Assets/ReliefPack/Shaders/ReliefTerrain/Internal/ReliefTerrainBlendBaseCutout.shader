//
// Relief Terrain Shader
// Tomasz Stobierski 2014
//
// this is optimizing pass for sticked blend geometries
//
Shader "Relief Pack/ReliefTerrainBlendBaseCutout" {
Properties {
	RTP_DeferredAddPassSpec ("Gloss for deferred", Float) = 0.5
}
SubShader {
	Tags {
		"Queue" = "Geometry-101"
		"RenderType"="Transparent"
		"IgnoreProjector"="True"
	}
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }	
		Offset 2,2
		ColorMask 0
		Blend Zero Zero
		ZWrite On Cull Back
		Fog { Mode Off }
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct v2f {
			float4 pos : POSITION;
			fixed4 color : COLOR0;
		};

		v2f vert (appdata_full v) {
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;
			return o;
		}

		fixed4 frag(v2f input) : COLOR
		{
			clip(-input.color.a);
			return 0;
		}
		ENDCG			
	}

	Pass {
		Name "PREPASS"
		Tags { "LightMode" = "PrePassBase" }
		Blend Zero One, One Zero
		ZWrite On Cull Back
		Fog { Mode Off }
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		
		float RTP_DeferredAddPassSpec;
		
		struct v2f {
			float4 pos : POSITION;
			fixed4 color : COLOR0;
		};

		v2f vert (appdata_full v) {
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;
			return o;
		}

		fixed4 frag(v2f input) : COLOR
		{
			clip(-input.color.a);
			return fixed4(0,0,0,RTP_DeferredAddPassSpec);
		}
		ENDCG			
	}	
}	

Fallback Off
}

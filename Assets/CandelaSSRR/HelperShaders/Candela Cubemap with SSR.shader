Shader "CandelaSSRR/Cubemap Specular SSR" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_Reflectivity ("Reflectivity (A)", 2D) = "white" {}
	_SpecTex ("Specular(RGB) Roughness(A)", 2D) = "white" {}
	_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
}
SubShader {
	LOD 300
	Tags { "RenderType"="Opaque" }

CGPROGRAM
#pragma surface surf BlinnPhong

sampler2D _MainTex;
sampler2D _SpecTex;
sampler2D _Reflectivity;
samplerCUBE _Cube;

fixed4 _Color;
fixed4 _ReflectColor;
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float3 worldRefl;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	half4 SPG = tex2D(_SpecTex, IN.uv_MainTex);
	half4 REF = tex2D(_Reflectivity, IN.uv_MainTex);
	
	///ALL
	fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
	
	tex.a = REF.a;
	
	_SpecColor = _SpecColor*SPG;
	
	fixed4 c = tex * _Color;
	
	o.Albedo = c.rgb;
	o.Gloss = tex.a;
	o.Specular = _Shininess*SPG.a;
	
	
	reflcol *= tex.a;
	o.Emission = reflcol.rgb *_ReflectColor.rgb;
	o.Alpha =  _Color.a*tex.a;
}
ENDCG
}

FallBack "Reflective/VertexLit"
}
Shader "Glitch/Keeper" 
{
    Properties 
	{
        _NoiseTex ("Noise Texture (RG)", 2D) = "white" {}
        _Strength ("Distortion strength", Range(0.1, 1)) = 0.2
        _Transparency ("Transparency", Range(0.01, 0.1)) = 0.05
    }
     
    SubShader 
	{
		Tags { "Queue" = "Transparent+1" }
		
        GrabPass 
		{
            Name "BASE"
            Tags { "LightMode" = "Always" }
        }
       
        Pass 
		{
            Name "BASE"
            Tags { "LightMode" = "Always" }
            Lighting Off
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater 0
         
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2
			#include "UnityCG.cginc"
 
			sampler2D _GrabTexture : register(s0);
			float4 _NoiseTex_ST;
			sampler2D _NoiseTex;
			float _Strength;
			float _Transparency;
 
			struct data 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
 
			struct v2f 
			{
				float4 position : POSITION;
				float4 screenPos : TEXCOORD0;
				float2 uvmain : TEXCOORD2;
				float distortion : TEXCOORD3;
			};
			
			float4 Overlay (float4 a, float4 b)
			{
			    float4 r = float4(.0, .0, .0, b.a);
			    if (a.r > .5) r.r = 1.0 - (1.0 - 2.0 * (a.r - .5)) * (1.0 - b.r);
			    else r.r = (2.0 * a.r) * b.r;
			    if (a.g > .5) r.g = 1.0 - (1.0 - 2.0 * (a.g - .5)) * (1.0 - b.g);
			    else r.g = (2.0 * a.g) * b.g;
			    if (a.b > .5) r.b = 1.0 - (1.0 - 2.0 * (a.b - .5)) * (1.0 - b.b);
			    else r.b = (2.0 * a.b) * b.b;
			    return r;
			}
 
			v2f vert (data i) 
			{
				v2f o;
				o.position = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uvmain = TRANSFORM_TEX(i.texcoord, _NoiseTex);
				float viewAngle = dot(normalize(ObjSpaceViewDir(i.vertex)), i.normal);
				o.distortion = viewAngle * viewAngle; 
				float depth = -mul(UNITY_MATRIX_MV, i.vertex).z; 
				o.distortion /= 1 + depth / 15; 
				o.distortion *= _Strength; 
				o.screenPos = o.position;
				return o;
			}
 
			half4 frag(v2f i) : COLOR
			{  
				float2 screenPos = i.screenPos.xy / i.screenPos.w;
				screenPos.x = (screenPos.x + 1) * 0.5; 
				screenPos.y = (screenPos.y + 1) * 0.5; 
 
				// check if anti aliasing is used
				if (_ProjectionParams.x < 0)
					screenPos.y = 1 - screenPos.y;
   
				// get two offset values by looking up the noise texture shifted in different directions
				half4 offsetColor1 = tex2D(_NoiseTex, i.uvmain + _Time.xz / 50);
				half4 offsetColor2 = tex2D(_NoiseTex, i.uvmain - _Time.yx / 50);
   
				// use the r values from the noise texture lookups and combine them for x offset
				// use the g values from the noise texture lookups and combine them for y offset
				// use minus one to shift the texture back to the center
				// scale with distortion amount
				screenPos.x += ((offsetColor1.r + offsetColor2.r) - 1) * i.distortion;
				screenPos.y += ((offsetColor1.g + offsetColor2.g) - 1) * i.distortion;
   
				half4 col = tex2D(_GrabTexture, screenPos);
				col.a = i.distortion / _Transparency;
				
				float2 grabTexcoord = i.screenPos.xy / i.screenPos.w; 
				grabTexcoord.x = (grabTexcoord.x + 1.0) * .5;
				grabTexcoord.y = (grabTexcoord.y + 1.0) * .5; 
				#if UNITY_UV_STARTS_AT_TOP
				grabTexcoord.y = 1.0 - grabTexcoord.y;
				#endif
				
				half4 grabColor = tex2D(_GrabTexture, grabTexcoord); 
				
				return Overlay(grabColor, col);
			}
 
			ENDCG
        }
    }
}

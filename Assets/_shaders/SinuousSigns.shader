Shader "Glitch/Sinuous Sign"
{
	Properties
	{
		_TileCount ("Tile Count XY/Scanlines ZW",Vector) = (10.0, 10.0, 0.0, 0.0)
		_Color("Base Color",Color)=(1.0,1.0,1.0,1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Phase ("Phase",Range(0.0, 1.0)) = 0.5
		_PhaseStart ("Phase Start",Range(0.0, 0.5)) = 0.0
		_PhaseEnd ("Phase End",Range(0.5, 1.0)) = 1.0
		_IdleAmount ("Idle Amount",Range(0.0, 1.0)) = 0.5
		_IdleSpeed ("Idle Speed",Float) = 0.5
		_Noise ("Scatter Noise", 2D) = "grey" {}
		_Scatter ("Scatter",Range(0.0, 1.0)) = 0.5
		_Sharpness("Sharpness", Range(0.0, 1.0)) = 1.0
		_FromCenter("From Center", Range(0.0, 1.0)) = 0.0
		_Direction ("Direction",Range(0.0, 1.0)) = 0.0
		_Vertical ("Vertical",Range(0.0, 1.0)) = 0.0
		_Flash ("Flash",Range(0.0, 1.0)) = 0.5
		_ScaleRot("Scale XY/Rotate Speed Z/Angle Snap W", Vector) = (0.5, 0.5, 0.0, 0.0)
		_ScaleCenter("Scale Around Tile", Range(0.0, 1.0)) = 1.0
		[Toggle]_ClipScaling("Clip Scale", Float) = 1.0
		
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 400
		Blend SrcAlpha One
		Cull off
		ColorMask RGB
		
		CGPROGRAM
		#pragma surface surf SinuousUnlit
		#pragma target 3.0
		
		sampler2D _MainTex,_Noise;
		fixed4 _Color;
		float4 _TileCount,_ScaleRot;
		float _IdleSpeed;
		half _Phase,_IdleAmount,_PhaseStart,_PhaseEnd,_Scatter,_Sharpness,_FromCenter,_Direction,_Vertical,_Flash,_ScaleCenter,_ClipScaling;

		inline half4 LightingSinuousUnlit (SurfaceOutput s, half3 lightDir, half atten){return float4 (s.Emission,1.0);}
		//a custom bare-bones lighting model to ensure no silly lighting maths are being done unnecessarily 

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			float2 baseUv = IN.uv_MainTex;
			
			float scanLine = 1.0;
			
			if (_TileCount.z > 0.0 && _TileCount.w > 0.0)
			{
				scanLine = 1.0 - saturate(saturate(sin(IN.worldPos.y * _TileCount.z * 10.0 + _Time.y * 5.0)*0.5 + 0.5) * _TileCount.w);
			}
			
			
			float2 tileCount = min(2048.0, floor(_TileCount.xy));
			//no reason to accept tileCounts higher than the _Noise texture's resolution, which ideally won't be larger than 2048
			float2 tileUv = floor(baseUv * tileCount) / tileCount;
			half4 noise = tex2D (_Noise, tileUv);
			
			float tileWidth = 0.5/tileCount;
			//used later to correct the center of radial animation
			
			float idle =frac(_Time.y*0.5*_IdleSpeed);
			float phase = saturate((_Phase - _PhaseStart) / (_PhaseEnd - _PhaseStart));
			//stretch the phase input to fit in between PhaseStart and PhaseEnd
			//This allows several different materials to be animated in sequence using only one Phase value.
			
			float clearStart = saturate(1.0 + pow(2.0 * phase - 1.0, 1.0));
			float atEnds = saturate(1.0 - pow(2.0 * phase - 1.0, 4.0));
			float atEndsIdle = saturate(1.0 - pow(2.0 * idle - 1.0, 4.0));
			//helper variables for later.
			//clearStart equals zero at the beginning of _Phase
			//atEnds equals zero at the end and beginning of _Phase
			
			float scattering = atEnds * (noise.a - 0.5) * _Scatter;
			float2 scatteredTileUv = saturate(tileUv + scattering);
			//read the noise texture to see how much to offset our tile's uvs
			
			float scatteringIdle = atEndsIdle * saturate(noise.a - 0.5) * _Scatter;
			float2 scatteredTileUvIdle = saturate(tileUv + scatteringIdle);
			//same for idle
			
			float chosenPos = lerp(scatteredTileUv.x, scatteredTileUv.y, _Vertical);
			//determining which axis to use for the linear swipe can be mixed with any value, so we can get diagonal swipes if we want
			chosenPos = lerp (chosenPos, 1.0 - chosenPos, _Direction);
			//inverting the normalized position makes the animation play 'backwards'
			float effectl = (chosenPos - phase);
			//linear swipe
			
			float chosenPosIdle = lerp(scatteredTileUvIdle.x, scatteredTileUvIdle.y, _Vertical);
			chosenPosIdle = lerp (chosenPosIdle, 1.0 - chosenPosIdle, _Direction);
			float effectlIdle = (chosenPosIdle - idle);
			//same for idle
			
			float radialDist = distance(tileUv+tileWidth, 0.5) + scattering * 0.5;
			//since we're swiping two directions with radial swipe, we only scatter half as much
			float radialDir = lerp (radialDist, 1.0 - radialDist, _Direction);
			float effectr = radialDir - phase;
			//radial swipe
			
			float radialDistIdle = distance(tileUv+tileWidth, 0.5) + scatteringIdle * 0.5;
			float radialDirIdle = lerp (radialDistIdle, 1.0 - radialDistIdle, _Direction);
			float effectrIdle = radialDirIdle - idle;
			//same for idle
			
			float chosenEffect = lerp(effectl, effectr, _FromCenter);
			float chosenEffectIdle = lerp(effectlIdle, effectrIdle, _FromCenter);
			//choose linear vs radial swipe
			
			float tp = pow((_Sharpness + 0.3) * 8.0,2.0);
			//the transition power (how short the effect ripple is)
			
			float effect = atEnds * saturate(pow(1.0 - abs(chosenEffect), tp));
			effect += atEndsIdle * _IdleAmount * saturate(pow(1.0 - abs(chosenEffectIdle), tp));
			//add the idle animation effect
			
			float imageAmount = saturate(pow(1.0 - chosenEffect, 10.0 * tp)) * clearStart;
			//the effect amount is how much the tile is affected by the transition effects
			//the image amount is how much of the image is showing through the transition
			//we could round the imageAmount, but raising to a power gives us a smooth falloff
			
			float flash = 1.0 + (_Flash * 40.0 * effect);
			//additive flash effect
			
			float2 effectScaling = 1.0 - effect * _ScaleRot.xy;
			//the scaling to be applied
			
			float2 scaleCenter = lerp(0.5,tileUv + tileWidth,_ScaleCenter);
			//scale either around the whole image or the tile center
			
			float2 imageUvs = (baseUv - scaleCenter) * effectScaling + scaleCenter;
			//the scaled uvs
			
			float2 scaledTile = floor(imageUvs * tileCount) / tileCount;
			float2 keptTile = abs(scaledTile - tileUv);
			//this is where we check the scaled tile to clip it.
			//if the scaled pixel is no longer in the tile that is was originally, we don't want to display it.
			
			if (abs(_ScaleRot.z) > 0.0)
			{
				float rotdeg = _ScaleRot.z * _Time.y;
				if (_ScaleRot.w > 0.0)
					rotdeg = floor(rotdeg / _ScaleRot.w) * _ScaleRot.w;
				//perform angle snapping
				
				float rot = rotdeg * 0.01745329251;
				//convert from degrees to radians for the trig functions
				
				float2x2 rotMat = float2x2(cos(rot), -sin(rot), sin(rot), cos(rot));
				imageUvs = mul(imageUvs - 0.5, rotMat) + 0.5;
				//apply the rotation to the center of the scaled uvs
				//if we wanted to rotate each tile individually, the '0.5's here would be replaced by the scaleCenter from the scaling
			}
			
			half4 image = tex2D (_MainTex, imageUvs);
			//read the texture using the calculated uvs
			//the heart of this shader is distorted/offset uvs
			
			if ((keptTile.x > 0.01 || keptTile.y > 0.01) && _ClipScaling > 0.5)
				flash = 0.0;
			//recycling the flash multiplier to clip the edges of shrunken tiles if we need to
			//comparing ClipScaling > 0.5 is like comparing bool == true, since a toggle is either 0 (false) or 1 (true)
			
			o.Emission = scanLine * flash * (saturate(imageAmount * 4.0) * image.rgb * _Color.rgb * _Color.a);
			//return the final result
		}
		ENDCG
	} 
	FallBack "Diffuse"
	CustomEditor "SinuousSignsEditor"
}

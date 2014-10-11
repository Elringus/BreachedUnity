////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// you might want to tweak below defines by hand (commenting/uncommenting, but it's recommended to leave it for RTP_manager)
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// layer count switch (add pass - we can't undef this !)
#define _4LAYERS

// ATLASING in 4 layers mode to save 3 texture samplers
//#define RTP_USE_COLOR_ATLAS

// if you're using this shader on arbitrary mesh you can control splat coverage via vertices colors
// note that you won't be able to blend objects when VERTEX_COLOR_CONTROL is defined
// (add pass - we can't def this !)
//#define VERTEX_COLOR_CONTROL

// to compute far color basing only on global colormap
//#define SIMPLE_FAR

// uv blending
#define RTP_UV_BLEND
//#define RTP_DISTANCE_ONLY_UV_BLEND
//#define RTP_NORMALS_FOR_REPLACE_UV_BLEND
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// UV blend routing defines section
//
// DON'T touch defines below... (unless you know exactly what you're doing) - lines 30-52
#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
	#define UV_BLEND_SRC_0 (tex2Dlod(_SplatAtlasC, float4(uvSplat01M.xy, _MixMipActual.xx)).rgba)
	#define UV_BLEND_SRC_1 (tex2Dlod(_SplatAtlasC, float4(uvSplat01M.zw, _MixMipActual.yy)).rgba)
	#define UV_BLEND_SRC_2 (tex2Dlod(_SplatAtlasC, float4(uvSplat23M.xy, _MixMipActual.zz)).rgba)
	#define UV_BLEND_SRC_3 (tex2Dlod(_SplatAtlasC, float4(uvSplat23M.zw, _MixMipActual.ww)).rgba)
	#define UV_BLEND_SRC_4 (tex2Dlod(_SplatAtlasB, float4(uvSplat01M.xy, _MixMipActual.xx)).rgba)
	#define UV_BLEND_SRC_5 (tex2Dlod(_SplatAtlasB, float4(uvSplat01M.zw, _MixMipActual.yy)).rgba)
	#define UV_BLEND_SRC_6 (tex2Dlod(_SplatAtlasB, float4(uvSplat23M.xy, _MixMipActual.zz)).rgba)
	#define UV_BLEND_SRC_7 (tex2Dlod(_SplatAtlasB, float4(uvSplat23M.zw, _MixMipActual.ww)).rgba)
#else
	#define UV_BLEND_SRC_0 (tex2Dlod(_SplatC0, float4(uvSplat01M.xy, _MixMipActual.xx)).rgba)
	#define UV_BLEND_SRC_1 (tex2Dlod(_SplatC1, float4(uvSplat01M.zw, _MixMipActual.yy)).rgba)
	#define UV_BLEND_SRC_2 (tex2Dlod(_SplatC2, float4(uvSplat23M.xy, _MixMipActual.zz)).rgba)
	#define UV_BLEND_SRC_3 (tex2Dlod(_SplatC3, float4(uvSplat23M.zw, _MixMipActual.ww)).rgba)
#endif
#define UV_BLENDMIX_SRC_0 (_MixScale89AB.x)
#define UV_BLENDMIX_SRC_1 (_MixScale89AB.y)
#define UV_BLENDMIX_SRC_2 (_MixScale89AB.z)
#define UV_BLENDMIX_SRC_3 (_MixScale89AB.w)
#define UV_BLENDMIX_SRC_4 (_MixScale4567.x)
#define UV_BLENDMIX_SRC_5 (_MixScale4567.y)
#define UV_BLENDMIX_SRC_6 (_MixScale4567.z)
#define UV_BLENDMIX_SRC_7 (_MixScale4567.w)
// As we've got defined some shader parts, you can tweak things in following lines
////////////////////////////////////////////////////////////////////////

// for example, when you'd like layer 3 to be source for uv blend on layer 0 you'd set it like this:
//   #define UV_BLEND_ROUTE_LAYER_0 UV_BLEND_SRC_3
// HINT: routing one layer into all will boost performance as only 1 additional texture fetch will be performed in shader (instead of up to 8 texture fetches in default setup)
//
#define UV_BLEND_ROUTE_LAYER_0 UV_BLEND_SRC_0
#define UV_BLEND_ROUTE_LAYER_1 UV_BLEND_SRC_1
#define UV_BLEND_ROUTE_LAYER_2 UV_BLEND_SRC_2
#define UV_BLEND_ROUTE_LAYER_3 UV_BLEND_SRC_3
#define UV_BLEND_ROUTE_LAYER_4 UV_BLEND_SRC_4
#define UV_BLEND_ROUTE_LAYER_5 UV_BLEND_SRC_5
#define UV_BLEND_ROUTE_LAYER_6 UV_BLEND_SRC_6
#define UV_BLEND_ROUTE_LAYER_7 UV_BLEND_SRC_7
// below routing shiould be exactly the same as above
#define UV_BLENDMIX_ROUTE_LAYER_0 UV_BLENDMIX_SRC_0
#define UV_BLENDMIX_ROUTE_LAYER_1 UV_BLENDMIX_SRC_1
#define UV_BLENDMIX_ROUTE_LAYER_2 UV_BLENDMIX_SRC_2
#define UV_BLENDMIX_ROUTE_LAYER_3 UV_BLENDMIX_SRC_3
#define UV_BLENDMIX_ROUTE_LAYER_4 UV_BLENDMIX_SRC_4
#define UV_BLENDMIX_ROUTE_LAYER_5 UV_BLENDMIX_SRC_5
#define UV_BLENDMIX_ROUTE_LAYER_6 UV_BLENDMIX_SRC_6
#define UV_BLENDMIX_ROUTE_LAYER_7 UV_BLENDMIX_SRC_7
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//// comment below detail when not needed
#define RTP_SUPER_DETAIL
//#define RTP_SUPER_DTL_MULTS
// comment below if you don't use snow features
//#define RTP_SNOW
// layer number taken as snow normal for near distance (for deep snow cover)
//#define RTP_SNW_CHOOSEN_LAYER_NORM_0
// layer number taken as snow color/gloss for near distance
//#define RTP_SNW_CHOOSEN_LAYER_COLOR_0

// comment if you don't need global color map
#define COLOR_MAP
// if not defined global color map will be blended (lerp)
#define COLOR_MAP_BLEND_MULTIPLY
// advanced colormap blending per layer
//#define ADV_COLOR_MAP_BLENDING

// global normal map (and we will treat normals from mesh as flat (0,1,0))
//#define RTP_NORMALGLOBAL

// global trees/shadow map - used with Terrain Composer / World Composer by Nathaniel Doldersum
//#define RTP_TREESGLOBAL

// global ambient emissive map
//#define RTP_AMBIENT_EMISSIVE_MAP

// heightblend fake AO
#define RTP_HEIGHTBLEND_AO

//  layer emissiveness
//#define RTP_EMISSION
// when wetness is defined and fuild on surface is emissive we can mod its emisiveness by output normal (wrinkles of flowing "water")
// below define change the way we treat output normals (works fine for "lava" like emissive fuilds)
#define RTP_FUILD_EMISSION_WRAP
// with optional reafractive distortion to emulate hot air turbulence
//#define RTP_HOTAIR_EMISSION

// when defined you can see where layers 0-3 overlap layers 4-7 in 8 per pass mode. These areas costs higher
//  (note that when RTP_HARD_CROSSPASS is defined you won't see any overlapping areas)
//#define RTP_SHOW_OVERLAPPED
// when defined we don't calculate overlapping 0-3 vs 4-7 layers in 8 layers mode, but take "higher"
// it's recommended to use this define for significantly better performance
// undef it only when you really need smooth transitions between overlapping groups
//#define RTP_HARD_CROSSPASS

// define for harder heightblend edges
#define SHARPEN_HEIGHTBLEND_EDGES_PASS1
//#define SHARPEN_HEIGHTBLEND_EDGES_PASS2

// firstpass triplanar (handles PM only - no POM - in 8 layers mode triplanar only for first 4 layers)
//#define RTP_TRIPLANAR

// vertical texture
//#define RTP_VERTICAL_TEXTURE

// we use wet (can't be used with superdetail as globalnormal texture BA channels are shared)
//#define RTP_WETNESS
// water droplets
//#define RTP_WET_RIPPLE_TEXTURE
// if defined water won't handle flow nor refractions
//#define SIMPLE_WATER

//#define RTP_CAUSTICS
// when we use caustics and vertical texture - with below defined we will store vertical texture and caustics together (RGB - vertical texture, A - caustics) to save texture sampler
//#define RTP_VERTALPHA_CAUSTICS

// reflection map
//#define RTP_REFLECTION
#define RTP_ROTATE_REFLECTION

// if you don't use extrude reduction in layers properties (equals 0 everywhere)
// you can comment below - POM will run a bit faster
//#define USE_EXTRUDE_REDUCTION

// cutting holes functionality (make global colormap alpha channel completely black to cut)
//#define RTP_CUT_HOLES

//
// in 8 layers mode we can use simplier shading for not overplapped (RTP_HARD_CROSSPASS must be defined) 4-7 layers
// available options are:
// RTP_47SHADING_SIMPLE
// RTP_47SHADING_PM
// RTP_47SHADING_POM_LO (POM w/o shadows)
// RTP_47SHADING_POM_MED (POM with hard shadows)
// RTP_47SHADING_POM_HI (POM with soft shadows)
//

// RTP_FOG_EXP2, RTP_FOG_EXPONENTIAL, RTP_FOG_LINEAR, RTP_FOG_NONE
#define RTP_FOG_EXP2

// in presence of 2 passes we can do heightblend between passes
#define RTP_CROSSPASS_HEIGHTBLEND

// must be defined when we use 12 layers
//#define _12LAYERS

//
// below setting isn't reflected in LOD manager, it's only available here (and in RTP_ADDPBase.cginc)
// you can use it to control snow coverage from wet mask (special combined texture channel B)
//
//#define RTP_SNW_COVERAGE_FROM_WETNESS

// indepentent tiling switch (for multiple terrains that are of different size or we'd like to use different tiling on separate terrains)
//#define RTP_INDEPENDENT_TILING

// works only for 4 layers mode (w/o atlas), no triplanar
//#define FORCE_ANISO

// if defined we'll use world normal/reflection vectors in surf shader instead of those approximated in vert function (they don't take bumpmaping precisely)
// this is also forced when we use IBL (which needs more precise worldNormal / worldReflection vectors)
//#define RTP_RECONSTRUCT_WORLDNORMAL

// complementary lights
#define RTP_COMPLEMENTARY_LIGHTS
// complementary lights with spec (active only with above define)
//#define RTP_SPEC_COMPLEMENTARY_LIGHTS

// physically based shading - use fresnel (if you ask - in IBL we use fresnel by default)
// works fine in forward, in deferred it's calculated for one light only (this set via ReliefShader_applyLightForDeferred.cs)
#define RTP_PBL_FRESNEL
// physically based shading - visibility function (enhance a bit specularity)
//#define RTP_PBL_VISIBILITY_FUNCTION
// should be left defined unless you're using 3rd party PBL/PBS shading solutions that redefines hidden internal prepass shader for deferred
// (for example if you install Lux open source package - dig forum for more info - you should comment below define)
#define RTP_DEFERRED_PBL_NORMALISATION

// use IBL diffuse cubemap
//#define RTP_IBL_DIFFUSE
// use IBL specular cubemap
//#define RTP_IBL_SPEC

// we're working in LINEAR / GAMMA (used in IBL  fresnel , PBL fresnel and gloss calcs)
// if not defined we're rendering in GAMMA
#define RTP_COLORSPACE_LINEAR

// if defined we'll use cubemap defined by skyshop in its "Sky" GameObject
//#define RTP_SKYSHOP_SYNC
// define if you'd like to to sync RTP cubemap sampling with skyshop's rotated cubemaps
#define RTP_SKYSHOP_SKY_ROTATION

// helper for cross layer specularity / IBL / Refl bleeding
//#define NOSPEC_BLEED

// if not defined we will decode LDR cubemaps (RGB only)
#define IBL_HDR_RGBM
	
/////////////////////////////////////////////////////////////////////
//
// massive terrain - super simple mode
//
// if defined we're using very simple mode (4 layers only !)
// uses global color, global normal (optionaly), pixel trees / shadows (optionaly)
//#define SUPER_SIMPLE

// for super simple mode above:
// use grayscale detail colors (will be colorized by global colormap)
#define SS_GRAYSCALE_DETAIL_COLORS
// use bumpmapping
#define SS_USE_BUMPMAPS
// use perlin
#define SS_USE_PERLIN

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// don't touch below defines
//
// we're using mapped shadows for best performance (available only in 4 layers mode when using atlas)
// not implemented yet
//#define RTP_MAPPED_SHADOWS

#if defined(RTP_RECONSTRUCT_WORLDNORMAL) || defined(RTP_IBL_DIFFUSE)
	#define RECONSTRUCT_WORLDNORMAL
#endif

#ifdef _4LAYERS
	#ifdef RTP_HARD_CROSSPASS
		#undef RTP_HARD_CROSSPASS
	#endif
#endif

#if defined(COLOR_EARLY_EXIT) || !defined(APPROX_TANGENTS)
	#ifdef RTP_CUT_HOLES
		#undef RTP_CUT_HOLES
	#endif
#endif

#ifdef COLOR_MAP
	#define  COLOR_DAMP_VAL (global_color_value.a)
#else
	#define  COLOR_DAMP_VAL 1
#endif

#ifdef RTP_POM_SHADING_HI
	#define RTP_POM_SHADING
	#define RTP_SOFT_SHADOWS
#endif
#ifdef RTP_POM_SHADING_MED
	#define RTP_POM_SHADING
	#define RTP_HARD_SHADOWS
#endif
#ifdef RTP_POM_SHADING_LO
	#define RTP_POM_SHADING
	#define RTP_NO_SHADOWS
#endif

// dodefiniuj RTP_USE_COLOR_ATLAS dla triplanar w 8layers mode
#ifndef _4LAYERS
	#ifdef RTP_TRIPLANAR
		#ifndef RTP_USE_COLOR_ATLAS
			//#define RTP_USE_COLOR_ATLAS
		#endif
	#endif
#endif

#ifdef RTP_TRIPLANAR
	#ifdef RTP_POM_SHADING
		#define RTP_PM_SHADING
		#undef RTP_POM_SHADING
	#endif
#endif

// not used anymore in RTP3.1 due to new functionality that breaks it (replacement UV blend that depends on far distance)
// (przed poniższym define musi być przynajmniej jedna spacje aby LOD manager tego nie psuł)
//#ifdef RTP_SIMPLE_SHADING
//	#ifndef RTP_DISTANCE_ONLY_UV_BLEND
//         #define RTP_DISTANCE_ONLY_UV_BLEND
//	#endif
//#endif

#define GETrtp_snow_TEX rtp_snow_color_tex.rgb=csnow.rgb;rtp_snow_gloss=lerp(saturate(csnow.a*rtp_snow_gloss*2), 1, saturate((rtp_snow_gloss-0.5)*2));

// wyłącz kolor selction kiedy zabraknie samplerów
#if defined(_4LAYERS) && (!defined(RTP_USE_COLOR_ATLAS) || defined(RTP_MAPPED_SHADOWS))
#if defined(RTP_SNOW) && (defined(RTP_VERTICAL_TEXTURE) && !defined(RTP_VERTALPHA_CAUSTICS)) && defined(RTP_WETNESS) && defined(RTP_WET_RIPPLE_TEXTURE)
	#ifdef RTP_SNW_CHOOSEN_LAYER_COLOR_4
		#undef RTP_SNW_CHOOSEN_LAYER_COLOR_4
	#endif 
	#ifdef RTP_SNW_CHOOSEN_LAYER_COLOR_5
		#undef RTP_SNW_CHOOSEN_LAYER_COLOR_5
	#endif 
	#ifdef RTP_SNW_CHOOSEN_LAYER_COLOR_6
		#undef RTP_SNW_CHOOSEN_LAYER_COLOR_6
	#endif 
	#ifdef RTP_SNW_CHOOSEN_LAYER_COLOR_7
		#undef RTP_SNW_CHOOSEN_LAYER_COLOR_7
	#endif 
#endif
#endif
// wyłącz wet ripple kiedy mamy inne opcje wszystkie powlaczane
#if defined(_4LAYERS) && defined(RTP_MAPPED_SHADOWS) && (defined(RTP_VERTICAL_TEXTURE) && !defined(RTP_VERTALPHA_CAUSTICS)) && defined(RTP_WETNESS) && defined(RTP_WET_RIPPLE_TEXTURE)
	#undef RTP_WET_RIPPLE_TEXTURE
#endif
// potrzebujemy miejsca na dodatkowe samplery cieni
#if defined(RTP_MAPPED_SHADOWS)
	#if !defined(_4LAYERS)
		#undef RTP_MAPPED_SHADOWS
	#else
		//#define RTP_USE_COLOR_ATLAS
	#endif
#endif

#if defined(RTP_WETNESS) || defined(RTP_AMBIENT_EMISSIVE_MAP) || (defined(COLOR_MAP) && defined(ADV_COLOR_MAP_BLENDING))
	#define NEED_LOCALHEIGHT
#endif

#ifndef RTP_PBL_FRESNEL
	#define pbl_fresnel_term 1
#endif
#ifndef RTP_PBL_VISIBILITY_FUNCTION
	#define pbl_visibility_term 1
#endif
	
#if defined(SHADER_API_D3D11) 
CBUFFER_START(rtpConstants)
#endif

#ifdef TEX_SPLAT_REDEFINITION
	// texture samplers redefinitions (due to Unity bug that makes Unity4 terrain material unusable)
	#define _Control3 _Control
	#define _ColorMapGlobal _Splat0
	#define _NormalMapGlobal _Splat1
	#define _Control1 _Splat2
	#if defined(RTP_AMBIENT_EMISSIVE_MAP)
		#define _AmbientEmissiveMapGlobal _Splat2
	#else
		#define _TreesMapGlobal _Splat2
	#endif
	#define _BumpMapGlobal _Splat3
	sampler2D _Control;
#else
	sampler2D _Control, _Control3, _Control2, _Control1;
#endif

sampler2D _SplatAtlasC, _SplatAtlasB;
sampler2D _SplatC0, _SplatC1, _SplatC2, _SplatC3;
sampler2D _SplatB0, _SplatB1, _SplatB2, _SplatB3;
sampler2D _BumpMap89, _BumpMapAB, _BumpMap45, _BumpMap67;
sampler2D _ColorMapGlobal;

sampler2D _BumpMapGlobal;
sampler2D _SSColorCombinedB; 
float2 _terrain_size;
float _BumpMapGlobalScale;
float3 _GlobalColorMapBlendValues;
float _GlobalColorMapSaturation;
float _GlobalColorMapSaturationFar;
float _GlobalColorMapBrightness;
float _GlobalColorMapBrightnessFar;
float _GlobalColorMapNearMIP;
float _GlobalColorMapDistortByPerlin;
//float _GlobalColorMapSaturationByPerlin;
float EmissionRefractFiltering;
float EmissionRefractAnimSpeed;
float RTP_DeferredAddPassSpec;

float4 _MixScale89AB, _MixBlend89AB;
float4 _MixScale4567, _MixBlend4567;
float4 _GlobalColorPerLayer89AB, _GlobalColorPerLayer4567;
float4  _LayerBrightness89AB, _LayerSaturation89AB, _LayerBrightness2Spec89AB, _LayerAlbedo2SpecColor89AB;
float4  _LayerBrightness4567, _LayerSaturation4567, _LayerBrightness2Spec4567, _LayerAlbedo2SpecColor4567;
float4 _MixSaturation89AB, _MixBrightness89AB, _MixReplace89AB;
float4 _MixSaturation4567, _MixBrightness4567, _MixReplace4567;
float4 _LayerEmission89AB, _LayerEmission4567;
float4 _LayerEmissionRefractStrength89AB, _LayerEmissionRefractHBedge89AB;
float4 _LayerEmissionRefractStrength4567, _LayerEmissionRefractHBedge4567;
half4 _LayerEmissionColorR89AB;
half4 _LayerEmissionColorR4567;
half4 _LayerEmissionColorG89AB;
half4 _LayerEmissionColorG4567;
half4 _LayerEmissionColorB89AB;
half4 _LayerEmissionColorB4567;
half4 _LayerEmissionColorA89AB;
half4 _LayerEmissionColorA4567;
// adv global colormap blending
half4 _GlobalColorBottom89AB, _GlobalColorBottom4567;
half4 _GlobalColorTop89AB, _GlobalColorTop4567;
half4 _GlobalColorColormapLoSat89AB, _GlobalColorColormapLoSat4567;
half4 _GlobalColorColormapHiSat89AB, _GlobalColorColormapHiSat4567;
half4 _GlobalColorLayerLoSat89AB, _GlobalColorLayerLoSat4567;
half4 _GlobalColorLayerHiSat89AB, _GlobalColorLayerHiSat4567;
half4 _GlobalColorLoBlend89AB, _GlobalColorLoBlend4567;
half4 _GlobalColorHiBlend89AB, _GlobalColorHiBlend4567;

float4 _Spec89AB;
float4 _Spec4567;
float4 _FarSpecCorrection89AB;
float4 _FarSpecCorrection4567;
float4 _MIPmult89AB;
float4 _MIPmult4567;

sampler2D _TERRAIN_HeightMap3, _TERRAIN_HeightMap2, _TERRAIN_HeightMap;
float4 _TERRAIN_HeightMap3_TexelSize;
float4 _SplatAtlasC_TexelSize;
float4 _SplatAtlasB_TexelSize;
float4 _SplatC0_TexelSize;
float4 _BumpMapGlobal_TexelSize;
float4 _TERRAIN_ReliefTransform;
float _TERRAIN_ReliefTransformTriplanarZ;
float _TERRAIN_DIST_STEPS;
float _TERRAIN_WAVELENGTH;

float _blend_multiplier;

float _TERRAIN_ExtrudeHeight;
float _TERRAIN_LightmapShading;

float _TERRAIN_SHADOW_STEPS;
float _TERRAIN_WAVELENGTH_SHADOWS;
float _TERRAIN_SHADOW_SMOOTH_STEPS;
float _TERRAIN_SelfShadowStrength;
float _TERRAIN_ShadowSmoothing;

float rtp_mipoffset_color;
float rtp_mipoffset_bump;
float rtp_mipoffset_height;
float rtp_mipoffset_superdetail;
float rtp_mipoffset_flow;
float rtp_mipoffset_ripple;
float rtp_mipoffset_globalnorm;
float rtp_mipoffset_caustics;

// caustics
float TERRAIN_CausticsAnimSpeed;
half4 TERRAIN_CausticsColor;
float TERRAIN_CausticsWaterLevel;
float TERRAIN_CausticsWaterLevelByAngle;
float TERRAIN_CausticsWaterDeepFadeLength;
float TERRAIN_CausticsWaterShallowFadeLength;
float TERRAIN_CausticsTilingScale;
sampler2D TERRAIN_CausticsTex;

// for vert texture stored in alpha channel of caustics
float4 TERRAIN_CausticsTex_TexelSize;

///////////////////////////////////////////
//
// reflection
//
half4 TERRAIN_ReflColorA;
half4 TERRAIN_ReflColorB;
float TERRAIN_ReflectionRotSpeed; // 0-2, 0.3
float TERRAIN_ReflGlossAttenuation; // 0..1
half4 TERRAIN_ReflColorC;
float TERRAIN_ReflColorCenter; // 0.1 - 0.9

//
// water/wet
//
// global
float TERRAIN_GlobalWetness; // 0-1

sampler2D TERRAIN_RippleMap;
float4 TERRAIN_RippleMap_TexelSize;
float TERRAIN_RippleScale; // 4
float TERRAIN_FlowScale; // 1
float TERRAIN_FlowSpeed; // 0 - 3 (0.5)
float TERRAIN_FlowCycleScale; // 0.5 - 4 (1)
float TERRAIN_FlowMipOffset; // 0

float TERRAIN_RainIntensity; // 1
float TERRAIN_DropletsSpeed; // 10
float TERRAIN_WetDarkening;
float TERRAIN_WetDropletsStrength; // 0-1
float TERRAIN_WetHeight_Treshold;
float TERRAIN_WetHeight_Transition;

float TERRAIN_mipoffset_flowSpeed; // 0-5

// per layer
float4 TERRAIN_LayerWetStrength89AB; // 0 - 1 (1)
float4 TERRAIN_LayerWetStrength4567;

float4 TERRAIN_WaterLevel89AB; // 0 - 2 (0.5)
float4 TERRAIN_WaterLevel4567;
float4 TERRAIN_WaterLevelSlopeDamp89AB; // 0.25 - 32 (2)
float4 TERRAIN_WaterLevelSlopeDamp4567;
float4 TERRAIN_WaterEdge89AB; // 1 - 16 (2)
float4 TERRAIN_WaterEdge4567;

float4 TERRAIN_WaterOpacity89AB; // 0 - 1 (0.3)
float4 TERRAIN_WaterOpacity4567;
float4 TERRAIN_Refraction89AB; // 0 - 0.04 (0.01)
float4 TERRAIN_Refraction4567; 
float4 TERRAIN_WetRefraction89AB; // 0 - 1 (0.25)
float4 TERRAIN_WetRefraction4567; 
float4 TERRAIN_Flow89AB; // 0 - 1 (0.1)
float4 TERRAIN_Flow4567;
float4 TERRAIN_WetSpecularity89AB; // -1 - 1 (0)
float4 TERRAIN_WetSpecularity4567;

float4 TERRAIN_WetFlow89AB; // 0 - 1 (0.1)
float4 TERRAIN_WetFlow4567;
float4 TERRAIN_WetGloss89AB; // -1 - 1 (0)
float4 TERRAIN_WetGloss4567;
float4 TERRAIN_WaterSpecularity89AB; // -1 - 1 (0)
float4 TERRAIN_WaterSpecularity4567;
float4 TERRAIN_WaterGloss89AB; // -1 - 1 (0)
float4 TERRAIN_WaterGloss4567;
float4 TERRAIN_WaterGlossDamper89AB; // 0 - 1
float4 TERRAIN_WaterGlossDamper4567;
float4 TERRAIN_WaterEmission89AB; // 0-1 (0)
float4 TERRAIN_WaterEmission4567;
half4 TERRAIN_WaterColorR89AB;
half4 TERRAIN_WaterColorR4567;
half4 TERRAIN_WaterColorG89AB;
half4 TERRAIN_WaterColorG4567;
half4 TERRAIN_WaterColorB89AB;
half4 TERRAIN_WaterColorB4567;
half4 TERRAIN_WaterColorA89AB;
half4 TERRAIN_WaterColorA4567;

///////////////////////////////////////////

float _TERRAIN_distance_start;
float _TERRAIN_distance_transition;

float _TERRAIN_distance_start_bumpglobal;
float _TERRAIN_distance_transition_bumpglobal;
float rtp_perlin_start_val;
float4 _BumpMapGlobalStrength89AB;
float4 _BumpMapGlobalStrength4567;
float _FarNormalDamp;

sampler2D _NormalMapGlobal;
sampler2D _TreesMapGlobal;
sampler2D _AmbientEmissiveMapGlobal;
float _AmbientEmissiveMultiplier;
float _AmbientEmissiveRelief;
float4 _TERRAIN_trees_shadow_values;
float4 _TERRAIN_trees_pixel_values;

float _RTP_MIP_BIAS;

float4 PER_LAYER_HEIGHT_MODIFIER89AB;
float4 PER_LAYER_HEIGHT_MODIFIER4567;

float _SuperDetailTiling;
float4 _SuperDetailStrengthMultA89AB;
float4 _SuperDetailStrengthMultA4567;
float4 _SuperDetailStrengthMultB89AB;
float4 _SuperDetailStrengthMultB4567;
float4 _SuperDetailStrengthNormal89AB;
float4 _SuperDetailStrengthNormal4567;

float4 _SuperDetailStrengthMultASelfMaskNear89AB;
float4 _SuperDetailStrengthMultASelfMaskNear4567;
float4 _SuperDetailStrengthMultASelfMaskFar89AB;
float4 _SuperDetailStrengthMultASelfMaskFar4567;
float4 _SuperDetailStrengthMultBSelfMaskNear89AB;
float4 _SuperDetailStrengthMultBSelfMaskNear4567;
float4 _SuperDetailStrengthMultBSelfMaskFar89AB;
float4 _SuperDetailStrengthMultBSelfMaskFar4567;

float4 _VerticalTexture_TexelSize;
sampler2D _VerticalTexture;
float _VerticalTextureTiling;
float _VerticalTextureGlobalBumpInfluence;
float4 _VerticalTexture89AB;
float4 _VerticalTexture4567;

float rtp_global_color_brightness_to_snow;
float rtp_snow_slope_factor;
float rtp_snow_edge_definition;
float4 rtp_snow_strength_per_layer89AB;
float4 rtp_snow_strength_per_layer4567;
float rtp_snow_height_treshold;
float rtp_snow_height_transition;

float rtp_snow_fresnel;
float rtp_snow_diff_fresnel;
float rtp_snow_IBL_DiffuseStrength;
float rtp_snow_IBL_SpecStrength;

fixed4 rtp_snow_color;
float rtp_snow_gloss;
float rtp_snow_specular;
float rtp_snow_deep_factor;

///////////////////////////////////////////////////////////////////////////////////////////////
// lighting & IBL
///////////////////////////////////////////////////////////////////////////////////////////////
half3 rtp_customAmbientCorrection;

#define RTP_BackLightStrength RTP_LightDefVector.x
#define RTP_ReflexLightDiffuseSoftness RTP_LightDefVector.y
#define RTP_ReflexLightSpecSoftness RTP_LightDefVector.z
#define RTP_ReflexLightSpecularity RTP_LightDefVector.w
float4 RTP_LightDefVector;
half4 RTP_ReflexLightDiffuseColor1;
half4 RTP_ReflexLightDiffuseColor2;
half4 RTP_ReflexLightSpecColor;

#ifdef RTP_SKYSHOP_SYNC
	#define _CubemapSpec _SpecCubeIBL
	
	// SH IBL lighting taken under permission from Skyshop MarmosetCore.cginc
	uniform float3		_SH0;
	uniform float3		_SH1;
	uniform float3		_SH2;
	uniform float3		_SH3;
	uniform float3		_SH4;
	uniform float3		_SH5;
	uniform float3		_SH6;
	uniform float3		_SH7;
	uniform float3		_SH8;	
	float3 SHLookup(float3 dir) {
		//l = 0 band (constant)
		float3 result = _SH0.xyz;

		//l = 1 band
		result += _SH1.xyz * dir.y;
		result += _SH2.xyz * dir.z;
		result += _SH3.xyz * dir.x;

		//l = 2 band
		float3 swz = dir.yyz * dir.xzx;
		result += _SH4.xyz * swz.x;
		result += _SH5.xyz * swz.y;
		result += _SH7.xyz * swz.z;
		float3 sqr = dir * dir;
		result += _SH6.xyz * ( 3.0*sqr.z - 1.0 );
		result += _SH8.xyz * ( sqr.x - sqr.y );
		
		return abs(result);
	}	
#endif
samplerCUBE _CubemapDiff;
samplerCUBE _CubemapSpec;
float4x4	SkyMatrix;// set globaly by skyshop

// PBL / IBL
half4 RTP_gloss2mask89AB, RTP_gloss2mask4567;
half4 RTP_gloss_mult89AB, RTP_gloss_mult4567;
half4 RTP_gloss_shaping89AB, RTP_gloss_shaping4567;
half4 RTP_Fresnel89AB, RTP_Fresnel4567;
half4 RTP_FresnelAtten89AB, RTP_FresnelAtten4567;
half4 RTP_DiffFresnel89AB, RTP_DiffFresnel4567;
// IBL
half4 RTP_IBL_bump_smoothness89AB, RTP_IBL_bump_smoothness4567;
half4 RTP_IBL_DiffuseStrength89AB, RTP_IBL_DiffuseStrength4567;
half4 RTP_IBL_SpecStrength89AB, RTP_IBL_SpecStrength4567;
// used in deferred - add pass only
half4 _DeferredSpecDampAddPass89AB;

half4 TERRAIN_WaterIBL_SpecWetStrength89AB, TERRAIN_WaterIBL_SpecWetStrength4567;
half4 TERRAIN_WaterIBL_SpecWaterStrength89AB, TERRAIN_WaterIBL_SpecWaterStrength4567;

half TERRAIN_IBL_DiffAO_Damp;
half TERRAIN_IBLRefl_SpecAO_Damp;
//
///////////////////////////////////////////////////////////////////////////////////////////////

float RTP_AOamp;
float4 RTP_AO_89AB, RTP_AO_4567, RTP_AO_0123;
float RTP_AOsharpness;
#if defined(SHADER_API_D3D11) 
CBUFFER_END
#endif

float rtp_snow_strength;

#ifdef UNITY_PASS_PREPASSFINAL
uniform float4 _WorldSpaceLightPosCustom;
#endif

struct Input {
	float2 uv_Control : TEXCOORD0;
	float4 _uv_Relief;
	float4 _uv_Aux;
	float3 viewDir;
	float4 _viewDir;
	#if !defined(SUPER_SIMPLE)
	float4 lightDir;
	#endif
	
	// Geometry blend specific
	float4 color:COLOR;
};

// quick gamma to linear approx of pow(n,2.2) function
inline float FastToLinear(float t) {
		t *= t * (t * 0.305306011 + 0.682171111) + 0.012522878;
		return t;
}

half3 DecodeRGBM(float4 rgbm)
{
	#ifdef IBL_HDR_RGBM
		// gamma/linear RGBM decoding
		#if defined(RTP_COLORSPACE_LINEAR)
	    	return rgbm.rgb * FastToLinear(rgbm.a) * 8;
	    #else
	    	return rgbm.rgb * rgbm.a * 8;
	    #endif
	#else
    	return rgbm.rgb;
	#endif
}

struct RTPSurfaceOutput {
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half Specular;
	fixed Alpha;
	float2 RTP;
	half3 SpecColor;
};

fixed3 _FColor;
float _Fdensity;
float _Fstart,_Fend;
#if defined(RTP_FOG_LINEAR)
	void customFog (Input IN, RTPSurfaceOutput o, inout fixed4 color) {
		float f=saturate((_Fend-IN._uv_Relief.w)/(_Fend-_Fstart));
		#ifdef UNITY_PASS_FORWARDADD
			color.rgb*=f;
		#else					
			color.rgb=lerp(_FColor, color.rgb, f);
		#endif
	}
#else
	#if defined(RTP_FOG_EXP2)
		void customFog (Input IN, RTPSurfaceOutput o, inout fixed4 color) {
			float val=_Fdensity*IN._uv_Relief.w;
			float f=exp2(-val*val*1.442695);
			#ifdef UNITY_PASS_FORWARDADD
				color.rgb*=f;
			#else					
				color.rgb=lerp(_FColor, color.rgb, f);
			#endif
		}
	#else
		#if defined(RTP_FOG_EXPONENTIAL)
			void customFog (Input IN, RTPSurfaceOutput o, inout fixed4 color) {
				float g=log2(1-_Fdensity);
				float f=exp2(g*IN._uv_Relief.w);
				#ifdef UNITY_PASS_FORWARDADD
					color.rgb*=f;
				#else					
					color.rgb=lerp(_FColor, color.rgb, f);
				#endif
			}
		#else
			void customFog (Input IN, RTPSurfaceOutput o, inout fixed4 color) {
			}
		#endif
	#endif
#endif

inline fixed4 LightingCustomBlinnPhong_PrePass (in RTPSurfaceOutput s, half4 light)
{
	#if defined(RTP_DEFERRED_PBL_NORMALISATION)
		fixed spec = light.a;
	#else
		// we assume Lux is used with its compressed specular luminance term
		fixed spec = exp2(light.a) - 1;
	#endif
	
	fixed4 c;
	s.Albedo.rgb*=rtp_customAmbientCorrection*2+1;
	c.rgb = (s.Albedo * light.rgb + light.rgb * s.SpecColor * spec) *s.RTP.y + rtp_customAmbientCorrection*0.5;
	// alfa blend na kanale alfa - nie możemy polegać na zawartości bufora (spec - light.a)
	c.a = s.Alpha;// + spec * _SpecColor.a;
	return c;
}

inline fixed4 LightingCustomBlinnPhong (in RTPSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	fixed diff = dot (s.Normal, lightDir); // n_dot_l
	#if defined (RTP_COMPLEMENTARY_LIGHTS) && defined(DIRECTIONAL) && !defined(UNITY_PASS_FORWARDADD)
		float tmpdiff=diff+0.2;
		float diffBack = tmpdiff<0 ? tmpdiff*RTP_BackLightStrength : 0;
	#endif
	diff = saturate(diff);
	
	float n_dot_l=diff;
	float n_dot_h = saturate(dot (s.Normal, h));
	float h_dot_l = dot (h, lightDir);
	// hacking spec normalisation to get quiet a dark spec for max roughness (will be 0.25/16)
	float specular_power=exp2(10*s.Specular+1) - 1.75;
	float normalisation_term = specular_power / 16.0f; // /8.0f in equations, but we multiply (atten * 2) in lighting below
	float blinn_phong = pow( n_dot_h, specular_power );    // n_dot_h is the saturated dot product of the normal and half vectors
	float specular_term = normalisation_term * blinn_phong;
	
	#ifdef RTP_PBL_FRESNEL
		// fresnel
		//float exponential = pow( 1.0f - h_dot_l, 5.0f ); // Schlick's approx to fresnel
		// below pow 4 looks OK and is cheaper than pow() call
		float exponential = 1.0f - h_dot_l;
		exponential*=exponential;
		exponential*=exponential;
		// skyshop fit (I'd like people to get similar results in gamma / linear)
		#if defined(RTP_COLORSPACE_LINEAR)
			exponential=0.03+0.97*exponential;
		#else
			exponential=0.25+0.75*exponential;
		#endif
		float pbl_fresnel_term = lerp (1.0f, exponential,  s.RTP.x); // o.RTP.x - _Fresnel
	#endif
		
	#ifdef RTP_PBL_VISIBILITY_FUNCTION
		// visibility
		float n_dot_v = saturate(dot (s.Normal, viewDir));
		float alpha = 1.0f / ( sqrt( 3.1415/4 * specular_power + 3.1415/2 ) );
		float pbl_visibility_term = ( n_dot_l * ( 1.0f - alpha ) + alpha ) * ( n_dot_v * ( 1.0f - alpha ) + alpha ); // Both dot products should be saturated
		pbl_visibility_term = 1.0f / pbl_visibility_term;	
	#endif
	
	float spec = specular_term * pbl_fresnel_term * pbl_visibility_term * diff;
	
	fixed4 c;
	#if defined (RTP_COMPLEMENTARY_LIGHTS) && defined(DIRECTIONAL) && !defined(UNITY_PASS_FORWARDADD)
		c.rgb = 0;//s.Albedo * diffBack;
	#else
		c.rgb = 0;
	#endif
	s.Albedo.rgb*=rtp_customAmbientCorrection*2+1;
	#ifdef RTP_COLORSPACE_LINEAR
		// s.RTP.y - self - shadow atten from surf()
		c.rgb += (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec * s.SpecColor.rgb) * (min(atten, s.RTP.y) * 2)  + rtp_customAmbientCorrection*0.5;
	#else
		// shape ^2 spec golor (to fit IBL and not overbright spot for high glossy)
		// s.RTP.y - self - shadow atten from surf()
		c.rgb += (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec * s.SpecColor.rgb * s.SpecColor.rgb) * (min(atten, s.RTP.y) * 2)  + rtp_customAmbientCorrection*0.5;
	#endif
	c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;

	#if defined (RTP_COMPLEMENTARY_LIGHTS) && defined(DIRECTIONAL) && !defined(UNITY_PASS_FORWARDADD)
		//		
		// reflex lights
		//
		float3 normForDiffuse=lerp(s.Normal, float3(0,0,1), RTP_ReflexLightDiffuseSoftness);
		float3 normForSpec=s.Normal;//lerp(s.Normal, float3(0,0,1), RTP_ReflexLightSpecSoftness);
		//normForSpec=normalize(normForSpec);
		float sGloss=saturate(dot(s.SpecColor,0.3));
		float glossDiff1=(sGloss+1)*RTP_ReflexLightDiffuseColor1.a*saturate(1+diffBack);
		float glossDiff2=(sGloss+1)*RTP_ReflexLightDiffuseColor2.a*saturate(1+diffBack);
			
		// specularity from the opposite view direction
		#if defined (RTP_SPEC_COMPLEMENTARY_LIGHTS)
			viewDir.xy=-viewDir.xy;
			h = normalize ( lightDir + viewDir );
			float nh = abs(dot (normForSpec, h));
			specular_power=RTP_ReflexLightSpecularity;
			normalisation_term = ( specular_power - 1.75f ) / 8.0f;
			blinn_phong = pow( nh, specular_power );
			specular_term = normalisation_term * blinn_phong;		
			c.rgb += _LightColor0.rgb * RTP_ReflexLightSpecColor.rgb * specular_term * s.SpecColor * RTP_ReflexLightSpecColor.a;
		#endif
		
		fixed3 complColor=lerp(RTP_ReflexLightDiffuseColor1.rgb*glossDiff1, RTP_ReflexLightDiffuseColor2.rgb*glossDiff2, dot(normForDiffuse.xy, lightDir.xy)*0.5+0.5);
		c.rgb += s.Albedo * complColor * (abs(normForDiffuse.z)*0.6+0.4)*(-lightDir.z*0.3+0.7);
//		lightDir.y=-0.7; // 45 degrees
//		lightDir=normalize(lightDir);
//		
//		float3 lightDirRefl;
//		float3 refl_rot;
//		refl_rot.x=0.86602540378443864676372317075294;// = sin(+120deg);
//		refl_rot.y=-0.5; // = cos(+/-120deg);
//		refl_rot.z=-refl_rot.x;
//		
//		// 1st reflex
//		lightDirRefl.x=dot(lightDir.xz, refl_rot.yz);
//		lightDirRefl.y=lightDir.y;
//		lightDirRefl.z=dot(lightDir.xz, refl_rot.xy);	
//		diff = abs( dot (normForDiffuse, lightDirRefl) )*glossDiff1; 
//		float3 reflexRGB=RTP_ReflexLightDiffuseColor1.rgb * diff * diff;
//		c.rgb += s.Albedo * reflexRGB;
//		
//		// 2nd reflex
//		lightDirRefl.x=dot(lightDir.xz, refl_rot.yx);
//		lightDirRefl.z=dot(lightDir.xz, refl_rot.zy);	
//		diff = abs ( dot (normForDiffuse, lightDirRefl) )*glossDiff2;
//		reflexRGB=RTP_ReflexLightDiffuseColor2.rgb * diff * diff;
//		c.rgb += s.Albedo * reflexRGB;
	#endif
	
	return c;
}

inline half4 LightingCustomBlinnPhong_DirLightmap (RTPSurfaceOutput s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	
	color.rgb*=rtp_customAmbientCorrection*2+1;
	half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector) + rtp_customAmbientCorrection*0.5;
	
	half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);

	// PBL	
	half3 h = normalize (lightDir + viewDir);
	fixed diff = dot (s.Normal, lightDir); // n_dot_l
	diff = saturate(diff);
	
	float n_dot_l=diff;
	float n_dot_h = saturate(dot (s.Normal, h));
	float h_dot_l = dot (h, lightDir);
	// hacking spec normalisation to get quiet a dark spec for max roughness (will be 0.25/16)
	float specular_power=exp2(10*s.Specular+1) - 1.75;
	float normalisation_term = specular_power / 16.0f; // /8.0f in equations, but we multiply (atten * 2) in lighting below
	float blinn_phong = pow( n_dot_h, specular_power );    // n_dot_h is the saturated dot product of the normal and half vectors
	float specular_term = normalisation_term * blinn_phong;
	
	#ifdef RTP_PBL_FRESNEL
		// fresnel
		//float exponential = pow( 1.0f - h_dot_l, 5.0f ); // Schlick's approx to fresnel
		// below pow 4 looks OK and is cheaper than pow() call
		float exponential = 1.0f - h_dot_l;
		exponential*=exponential;
		exponential*=exponential;
		// skyshop fit (I'd like people to get similar results in gamma / linear)
		#if defined(RTP_COLORSPACE_LINEAR)
			exponential=0.03+0.97*exponential;
		#else
			exponential=0.25+0.75*exponential;
		#endif
		float pbl_fresnel_term = lerp (1.0f, exponential,  s.RTP.x); // o.RTP.x - _Fresnel
	#endif
		
	#ifdef RTP_PBL_VISIBILITY_FUNCTION
		// visibility
		float n_dot_v = saturate(dot (s.Normal, viewDir));
		float alpha = 1.0f / ( sqrt( 3.1415/4 * specular_power + 3.1415/2 ) );
		float pbl_visibility_term = ( n_dot_l * ( 1.0f - alpha ) + alpha ) * ( n_dot_v * ( 1.0f - alpha ) + alpha ); // Both dot products should be saturated
		pbl_visibility_term = 1.0f / pbl_visibility_term;	
	#endif
	
	float spec = specular_term * diff * pbl_fresnel_term * pbl_visibility_term;
	
	// specColor used outside in the forward path, compiled out in prepass
	#ifdef RTP_COLORSPACE_LINEAR
		specColor = lm * s.SpecColor.rgb * spec;
	#else
		// shape ^2 spec golor (to fit IBL and not overbright spot for high glossy)
		specColor = lm * s.SpecColor.rgb * s.SpecColor.rgb * spec;
	#endif

	#if defined(RTP_DEFERRED_PBL_NORMALISATION)
		// spec from the alpha component is used to calculate specular
		// in the Lighting*_Prepass function, it's not used in forward
		return half4(lm, spec)*s.RTP.y; // s.RTP.y - self - shadow atten from surf()
	#else
		// (part taken from Lux)
		// spec from the alpha component is used to calculate specular
		// in the Lighting*_Prepass function, it's not used in forward
		// we have to compress spec like we do in the "Intrenal-PrepassLighting" shader
		return half4(lm, log2(spec + 1))*s.RTP.y; // s.RTP.y - self - shadow atten from surf()
	#endif

}

inline float3 myObjSpaceLightDir( in float4 v )
{
	#ifdef UNITY_PASS_PREPASSFINAL
		float4 lpos=_WorldSpaceLightPosCustom;
	#else
		float4 lpos=_WorldSpaceLightPos0;
	#endif	
	float3 objSpaceLightPos = mul(_World2Object, lpos).xyz;
	#ifndef USING_LIGHT_MULTI_COMPILE
		return objSpaceLightPos.xyz - v.xyz * lpos.w;
	#else
		#ifndef USING_DIRECTIONAL_LIGHT
		return objSpaceLightPos.xyz * unity_Scale.w - v.xyz;
		#else
		return objSpaceLightPos.xyz;
		#endif
	#endif
}

half2 normalEncode (half3 n)
{
    half scale = 1.7777;
    half2 enc = n.xy / (n.z+1);
    enc /= scale;
    enc = enc*0.5+0.5;
    return enc;
}

half3 normalDecode(half2 enc)
{
    half scale = 1.7777;
    half3 nn =
        enc.xyy*half3(2*scale,2*scale,0) +
        half3(-scale,-scale,1);
    half g = 2.0 / dot(nn.xyz,nn.xyz);
    half3 n;
    n.xy = g*nn.xy;
    n.z = g-1;
    return n;
}

float4 _Splat0_ST;
float2 RTP_CustomTiling;

void vert (inout appdata_full v, out Input o) {
    #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
		UNITY_INITIALIZE_OUTPUT(Input, o);
	#endif
	
	#if defined(SUPER_SIMPLE)
		//
		// super simple mode
		//
		#if defined(RTP_INDEPENDENT_TILING)
			#ifdef TEX_SPLAT_REDEFINITION	
				o._uv_Relief.xy=(mul(_Object2World, v.vertex)).xz*_Splat0_ST.zw;
			#else
				o._uv_Relief.xy=(mul(_Object2World, v.vertex)).xz*RTP_CustomTiling.xy;				
			#endif
		#else
			o._uv_Relief.xy=v.texcoord.xy * _TERRAIN_ReliefTransform.xy + _TERRAIN_ReliefTransform.zw;
		#endif
		o._uv_Aux.xy=o._uv_Relief.xy*_BumpMapGlobalScale;		
		
		#ifdef APPROX_TANGENTS
			float3 _Dir=ObjSpaceViewDir(v.vertex)/unity_Scale.w;
			float _distance=length(_Dir);
			o._uv_Relief.w=_distance; // terrain isn't scaled
		#else
			o._uv_Relief.w=length(WorldSpaceViewDir(v.vertex)); // but custom geometry could be...
		#endif
		#ifdef APPROX_TANGENTS
			v.tangent.xyz = cross(v.normal, float3(0,0,1));
			v.tangent.w = -1;
		#endif		
		
		// need to get view dir for diffuse scatter (diffuse fresnel) in simple mode, too
		float3 binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
		float3x3 rotation = float3x3( v.tangent.xyz, binormal, v.normal.xyz );
		float3 EyeDirTan=mul(rotation, _Dir);
		EyeDirTan/=o._uv_Relief.w;
	 	o._viewDir.xy=-EyeDirTan.xy;
	 	
		#ifdef RTP_NORMALGLOBAL
			float far=saturate((o._uv_Relief.w - _TERRAIN_distance_start_bumpglobal) / _TERRAIN_distance_transition_bumpglobal);
			v.normal.xyz=lerp(lerp(float3(0,1,0), v.normal.xyz, _TERRAIN_trees_pixel_values.w), float3(0,1,0), far);
		#endif
	#else
		//
		// regular mode
		//
		float3 Wpos=mul(_Object2World, v.vertex).xyz;
		#if defined(RTP_INDEPENDENT_TILING)
			#ifdef TEX_SPLAT_REDEFINITION
				o._uv_Relief.xy=Wpos.xz*_Splat0_ST.zw;
				#if defined(RTP_TRIPLANAR)
					o._uv_Relief.z=Wpos.y*_Splat0_ST.z;
				#endif
			#else
				o._uv_Relief.xy=Wpos.xz*RTP_CustomTiling.xy;				
				#if defined(RTP_TRIPLANAR)
					o._uv_Relief.z=Wpos.y*RTP_CustomTiling.x;
				#endif
			#endif
		#else
			o._uv_Relief.xy=v.texcoord.xy * _TERRAIN_ReliefTransform.xy + _TERRAIN_ReliefTransform.zw;
			#if defined(RTP_TRIPLANAR)
				o._uv_Relief.z=Wpos.y/ _TERRAIN_ReliefTransformTriplanarZ;
			#endif
		#endif
		
		// przeniesione wyzej
		float3 _Dir=ObjSpaceViewDir(v.vertex)/unity_Scale.w;
		float _distance=length(_Dir);
		#if defined(RTP_REFLECTION) || (defined(RTP_IBL_SPEC) && !defined(RTP_RECONSTRUCT_WORLDNORMAL))
			float3 viewRefl = reflect (-_Dir, v.normal);
			float3 refl_vec = normalize(mul((float3x3)_Object2World, viewRefl)).xyz;
			float3 worldRefl;
			#ifdef RTP_ROTATE_REFLECTION
				float3 refl_rot;
				refl_rot.x=sin(_Time.x*TERRAIN_ReflectionRotSpeed);
				refl_rot.y=cos(_Time.x*TERRAIN_ReflectionRotSpeed);
				refl_rot.z=-refl_rot.x;
				worldRefl.x=dot(refl_vec.xz, refl_rot.yz);
				worldRefl.z=dot(refl_vec.xz, refl_rot.xy);
			#else
				worldRefl=refl_vec;
			#endif
			#if defined(RTP_TRIPLANAR)
				// reflection+triplanar - obl. ddx/ddy do pixel shadera
				o._uv_Aux.xy=normalEncode(worldRefl);
			#else
				o._uv_Aux.xy=o._uv_Relief.xy*1024*(1+_RTP_MIP_BIAS);
				o._uv_Aux.zw=normalEncode(worldRefl);
			#endif
		#else
			o._uv_Aux.xy=o._uv_Relief.xy*1024*(1+_RTP_MIP_BIAS);
		#endif
			
		#ifdef APPROX_TANGENTS
			o._uv_Relief.w=_distance; // terrain isn't scaled
		#else
			o._uv_Relief.w=length(WorldSpaceViewDir(v.vertex)); // but custom geometry could be...
		#endif
			
		#ifdef APPROX_TANGENTS
			#if defined(RTP_SNOW) || defined(RTP_TRIPLANAR)
				// ta aproksymacja lepiej trzyma "pion", w triplanar jest konieczna
				//v.tangent.xyz = normalize( cross(v.normal, float3(0, -v.normal.z, v.normal.y)) ); // uproszczony zapis klasycznej aproksymacji w oparciu o wektor (1,0,0)
				// dla v.normal.x=1 lub -1 tangent był nieoznaczony, poniższy tweak rozwiązuje problem
				v.tangent.xyz = normalize( cross(v.normal.xyz, cross(float3(1, 0, 0.0001), v.normal) ) );
				v.tangent.w = -1.0;
			#else
				// tak jest lepiej ze wzgl. na POM (i prościej)
			    v.tangent.xy = float2(1, 0) -  v.normal.x*v.normal.xy;
	//		   	//v.tangent.xy=normalize(v.tangent.xy); // z normalizacja roznica nieznaczna ...
			    v.tangent.z=0;
				v.tangent.w = -1;
			#endif	
		#endif
		#if defined(RTP_WETNESS) || defined(RTP_REFLECTION) || (defined(RTP_IBL_SPEC) && !defined(RTP_RECONSTRUCT_WORLDNORMAL)) || defined(RTP_SNOW) || defined(RTP_CAUSTICS) || defined(RTP_POM_SHADING) || defined(RTP_PM_SHADING) || (!defined(LIGHTMAP_OFF) && defined (DIRLIGHTMAP_OFF)) || defined(UNITY_PASS_PREPASSFINAL)  || defined(RTP_NORMALGLOBAL)
			float3 binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
			float3x3 rotation = float3x3( v.tangent.xyz, binormal, v.normal.xyz );
			
			o.lightDir.xyz = mul (rotation, myObjSpaceLightDir(v.vertex));
			o.lightDir.xyz=normalize(o.lightDir.xyz);
			//o.lightDir.z/=_TERRAIN_ExtrudeHeight;
		#endif
		
		#ifdef RTP_POM_SHADING
			float3 EyeDirTan=mul(rotation, _Dir);
			EyeDirTan/=_distance;
			
			//EyeDirTan.z/=_TERRAIN_ExtrudeHeight;
		 	o._viewDir.xy=-EyeDirTan.xy;
		 	//o._viewDir.xy*=_terrain_size;
	 	#else
			#if defined(RTP_PM_SHADING) || defined(RTP_REFLECTION) || defined(RTP_NORMALGLOBAL)
				float3 EyeDirTan=mul(rotation, _Dir);
				EyeDirTan/=_distance;
			 	o._viewDir.xy=EyeDirTan.xy;
			#endif
		#endif
	
		#if defined(RTP_SNOW) || defined(RTP_WETNESS) || defined(RTP_CAUSTICS)
			#if defined(APPROX_TANGENTS) && !defined(COLOR_EARLY_EXIT)
				o._viewDir.zw = normalEncode(( mul (rotation, float3(0,1,0)) ).xyz); // teren jest nierotowalny
			#else
				o._viewDir.zw = normalEncode(( mul (rotation, mul(_World2Object, float4(0,1,0,0)).xyz) ).xyz);
			#endif
		#endif
		
		#if defined(RTP_SNOW) || defined(RTP_VERTICAL_TEXTURE) || defined(RTP_CAUSTICS) || defined(RTP_WETNESS)
			#if defined(COLOR_EARLY_EXIT) || !defined(APPROX_TANGENTS)
				o.lightDir.w = Wpos.y;
			#else
				o.lightDir.w = v.vertex.y;
			#endif
		#endif
		
		#ifdef RTP_TRIPLANAR
			#if defined(COLOR_EARLY_EXIT)
				o._uv_Aux.zw=0;// zaszyte gotowe w kolorze
			#else
				#ifdef APPROX_TANGENTS
					o._uv_Aux.zw=v.normal.xz;
				#else
					o._uv_Aux.zw=mul(_Object2World, float4(v.normal,0)).xz;			
				#endif
			#endif
		#endif	
			
		#if defined(RTP_POM_SHADING_HI) || defined(RTP_POM_SHADING_MED) || defined( RTP_POM_SHADING_LO)
			float2 stretch_factor=v.normal.yy;
			stretch_factor /= float2(length(v.normal.xy), length(v.normal.zy));
			o._viewDir.xy*=stretch_factor.xy;
			o.lightDir.xy*=stretch_factor.xy;
		#endif
	
		#ifdef RECONSTRUCT_WORLDNORMAL
			v.color.xyz=v.normal.xyz;
		#endif	
					
		#ifdef RTP_NORMALGLOBAL
			float far=saturate((o._uv_Relief.w - _TERRAIN_distance_start_bumpglobal) / _TERRAIN_distance_transition_bumpglobal);
			v.normal.xyz=lerp(lerp(float3(0,1,0), v.normal.xyz, _TERRAIN_trees_pixel_values.w), float3(0,1,0), far);
		#else
			float far=saturate((o._uv_Relief.w - _TERRAIN_distance_start_bumpglobal) / _TERRAIN_distance_transition_bumpglobal);
			v.normal.xyz=lerp(v.normal.xyz, float3(0,1,0), far*_FarNormalDamp);
		#endif		
		
	#endif
	
}

void surf (Input IN, inout RTPSurfaceOutput o) {
	o.Normal=float3(0,0,1); o.Albedo=0;	o.Emission=0; o.Specular=RTP_DeferredAddPassSpec; o.Alpha=0;
	o.RTP.xy=float2(0,1); o.SpecColor=0;
	half o_Gloss=0;
	
	#if defined(SUPER_SIMPLE)
		//
		// super simple mode
		//
		#ifdef COLOR_EARLY_EXIT
			if (IN.color.a<0.002) return;
		#endif
		float4 splat_control = tex2D(_Control3, IN.uv_Control);		
		float total_coverage=dot(splat_control,1);
		splat_control/=total_coverage;
		#ifndef RTP_CUT_HOLES
		if (total_coverage<0.002) return;
		#endif
		
		float _uv_Relief_w=saturate((IN._uv_Relief.w - _TERRAIN_distance_start_bumpglobal) / _TERRAIN_distance_transition_bumpglobal);
		
		#if defined(SS_USE_PERLIN)
			float4 global_bump_val=tex2D(_BumpMapGlobal, IN._uv_Aux.xy*8)*0.4+tex2D(_BumpMapGlobal, IN._uv_Aux.xy)*0.6;
		#endif		
		
		#ifdef COLOR_MAP
			float global_color_blend=lerp( _GlobalColorMapBlendValues.x, _GlobalColorMapBlendValues.z, _uv_Relief_w);
			float4 global_color_value=tex2D(_ColorMapGlobal, IN.uv_Control);
			#ifdef RTP_CUT_HOLES
			clip(global_color_value.a-0.001f);
			if (total_coverage<0.002) return;
			#endif
			
			global_color_value.rgb=lerp(tex2Dlod(_ColorMapGlobal, float4(IN.uv_Control, _GlobalColorMapNearMIP.xx)).rgb, global_color_value.rgb, _uv_Relief_w);

//			#if defined(SS_USE_PERLIN)
//				float perlin2global_color=abs((global_bump_val.r-0.4)*5);
//				float GlobalColorMapSaturationByPerlin = saturate( lerp(_GlobalColorMapSaturation, _GlobalColorMapSaturationFar, _uv_Relief_w) -perlin2global_color*_GlobalColorMapSaturationByPerlin);
//			#else
//				float GlobalColorMapSaturationByPerlin = lerp(_GlobalColorMapSaturation, _GlobalColorMapSaturationFar, _uv_Relief_w);
//			#endif
			float GlobalColorMapSaturationByPerlin = lerp(_GlobalColorMapSaturation, _GlobalColorMapSaturationFar, _uv_Relief_w);
			global_color_value.rgb=lerp(dot(global_color_value.rgb,0.35).xxx, global_color_value.rgb, GlobalColorMapSaturationByPerlin);
			global_color_value.rgb*=lerp(_GlobalColorMapBrightness, _GlobalColorMapBrightnessFar, _uv_Relief_w);
		#endif
		
		#ifdef FAR_ONLY
		if (false) {
		#else
		if (_uv_Relief_w<1) {
		#endif
		 	fixed4 col;
		 	#if defined(SS_GRAYSCALE_DETAIL_COLORS)
				col.rgb = global_color_value.rgb*dot(splat_control, tex2D(_SSColorCombinedB, IN._uv_Relief.xy));
		 	#else
				col = splat_control.r * tex2D(_SplatC0, IN._uv_Relief.xy);
				col += splat_control.g * tex2D(_SplatC1, IN._uv_Relief.xy);
				col += splat_control.b * tex2D(_SplatC2, IN._uv_Relief.xy);
				col += splat_control.a * tex2D(_SplatC3, IN._uv_Relief.xy);
				
				float glcombined = col.a;
				#if defined(RTP_COLORSPACE_LINEAR)
				//glcombined=FastToLinear(glcombined);
				#endif
				float RTP_gloss2mask = dot(splat_control, RTP_gloss2mask89AB);
				float _Spec = dot(saturate(splat_control-float4(0.5,0.5,0.5,0.5))*2, _Spec89AB); // anti-bleed subtraction
				float RTP_gloss_mult = dot(splat_control, RTP_gloss_mult89AB);
				float RTP_gloss_shaping = dot(splat_control, RTP_gloss_shaping89AB);
				float gls = saturate(glcombined * RTP_gloss_mult);
				o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec;
				float2 gloss_shaped=float2(gls, 1-gls);
				gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
				gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
				o.Specular = saturate(gls);
				// gloss vs. fresnel dependency
				float fresnelAtten=dot(splat_control, RTP_FresnelAtten89AB);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
				o.RTP.x=dot(splat_control, RTP_Fresnel89AB);				
				o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);			
				#endif
				half colDesat=dot(col.rgb,0.33333);
				float brightness2Spec=dot(splat_control, _LayerBrightness2Spec89AB);
				o_Gloss*=lerp(1, colDesat, brightness2Spec);
				col.rgb=lerp(colDesat.xxx, col.rgb, dot(splat_control, _LayerSaturation89AB));	
				col.rgb*=dot(splat_control, _LayerBrightness89AB);  
		 	#endif
			o.Albedo=lerp(col.rgb, global_color_value.rgb, _uv_Relief_w);
			o_Gloss*=(1-_uv_Relief_w);
			o.Specular=lerp(o.Specular, 0.5, _uv_Relief_w);
			
			#if defined(SS_USE_BUMPMAPS)
				float3 n;
				float4 normals_combined;
				normals_combined = tex2D(_BumpMap89, IN._uv_Relief.xy).rgba*splat_control.rrgg;
				normals_combined+=tex2D(_BumpMapAB, IN._uv_Relief.xy).rgba*splat_control.bbaa;
				n.xy=(normals_combined.rg+normals_combined.ba)*2-1;
				n.xy*=1-_uv_Relief_w;
				n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
				o.Normal=n;
			#endif
		} else {
			o.Albedo=global_color_value.rgb;
		}
		float o_SpecularInvSquared = (1-o.Specular)*(1-o.Specular);

		#if defined(SS_USE_PERLIN)
			float3 norm_far;
			norm_far.xy = global_bump_val.rg*4-2;
			norm_far.z = sqrt(1 - saturate(dot(norm_far.xy, norm_far.xy)));		
			o.Normal+=norm_far*lerp(rtp_perlin_start_val,1, _uv_Relief_w)*dot(_BumpMapGlobalStrength89AB, splat_control);
		#endif
		
		global_color_blend *= dot(splat_control, _GlobalColorPerLayer89AB);
		#if defined(COLOR_MAP_BLEND_MULTIPLY) 
			o.Albedo=lerp(o.Albedo, o.Albedo*global_color_value.rgb*2, global_color_blend);
		#else
			o.Albedo=lerp(o.Albedo, global_color_value.rgb, global_color_blend);
		#endif
		
		#ifdef RTP_NORMALGLOBAL
			// tutaj musimy się zadowolić stretchowanym (w POM) viewDir, bo ten z surface shadera jest płaski (obl dla v.normal=(0,1,0))
			float3 IN_viewDir;
			IN_viewDir.xy=IN._viewDir.xy;
			IN_viewDir.z=sqrt(1 - saturate(dot(IN_viewDir.xy, IN_viewDir.xy)));				
			float diffFresnel = 1.0f - IN_viewDir.z;
		#else
			IN.viewDir=normalize(IN.viewDir);
			float diffFresnel = 1.0f - IN.viewDir.z; // unstretched version (i mamy wszystkie komponenty)
		#endif
		diffFresnel*=diffFresnel;
		diffFresnel*=diffFresnel;		
		// ^4 shaped diffuse fresnel term for soft surface layers (grass)
		float _DiffFresnel=dot(splat_control, RTP_DiffFresnel89AB);
		float diffuseScatteringFactor=1.0 + diffFresnel*_DiffFresnel;
		o.Albedo *= diffuseScatteringFactor;		
		
		#if defined(SS_USE_PERLIN)
			o.Normal=normalize(o.Normal);
		#endif
		
		#ifdef RTP_TREESGLOBAL	
			float4 pixel_trees_val=tex2D(_TreesMapGlobal, IN.uv_Control);
			float pixel_trees_blend_val=saturate((pixel_trees_val.r+pixel_trees_val.g+pixel_trees_val.b)*_TERRAIN_trees_pixel_values.z);
			pixel_trees_blend_val*=saturate((IN._uv_Relief.w - _TERRAIN_trees_pixel_values.x) / _TERRAIN_trees_pixel_values.y);
			o.Albedo=lerp(o.Albedo, pixel_trees_val.rgb, pixel_trees_blend_val);
			#if !defined(RTP_AMBIENT_EMISSIVE_MAP)
				float pixel_trees_shadow_val=saturate((IN._uv_Relief.w - _TERRAIN_trees_shadow_values.x) / _TERRAIN_trees_shadow_values.y);
				pixel_trees_shadow_val=lerp(1, pixel_trees_val.a, pixel_trees_shadow_val);
				o.RTP.y*=lerp(_TERRAIN_trees_shadow_values.z, 1, pixel_trees_shadow_val);
			#endif
		#endif
			
		#if defined(RTP_AMBIENT_EMISSIVE_MAP)
			float4 eMapVal=tex2D(_AmbientEmissiveMapGlobal, IN.uv_Control);
			o.Emission+=o.Albedo*eMapVal.rgb*_AmbientEmissiveMultiplier*lerp(1, saturate(o.Normal.z*o.Normal.z*2-1), _AmbientEmissiveRelief);
			float pixel_trees_shadow_val=saturate((IN._uv_Relief.w - _TERRAIN_trees_shadow_values.x) / _TERRAIN_trees_shadow_values.y);
			pixel_trees_shadow_val=lerp(1, eMapVal.a, pixel_trees_shadow_val);
			o.RTP.y*=lerp(_TERRAIN_trees_shadow_values.z, 1, pixel_trees_shadow_val);
		#endif			
		
		o.Alpha=total_coverage;		
		//
		// EOF super simple mode
		//
	#else
	//
	// regular mode
	//	
	float2 mip_selector;
	
	#if ((defined(RTP_REFLECTION)  || (defined(RTP_IBL_SPEC) && !defined(RTP_RECONSTRUCT_WORLDNORMAL))) && defined(RTP_TRIPLANAR))
		float2 IN_uv_Aux=IN._uv_Relief.xy*1024*(1+_RTP_MIP_BIAS);
		float2 dx = ddx( IN_uv_Aux.xy);
		float2 dy = ddy( IN_uv_Aux.xy);
	#endif
	#if defined(RTP_TRIPLANAR) 
		float2 mip_selectorTRIPLANAR;
		{
			float3 tmpUVZ=IN._uv_Relief.xyz*1024*(1+_RTP_MIP_BIAS);
			float2 dx = ddx( tmpUVZ.yz );
			float2 dy = ddy( tmpUVZ.yz );
			float d = max( dot( dx, dx ), dot( dy, dy ) );
			mip_selectorTRIPLANAR.x=0.5*log2(d);
			dx = ddx( tmpUVZ.xz );
			dy = ddy( tmpUVZ.xz );
			d = max( dot( dx, dx ), dot( dy, dy ) );
			mip_selectorTRIPLANAR.y=0.5*log2(d);
		}
	#endif
	
	#ifdef COLOR_EARLY_EXIT
		if (IN.color.a<0.002) return;
	#endif
	
	#ifdef VERTEX_COLOR_CONTROL
		float4 splat_controlA = IN.color;
	#else
		float4 splat_controlA = tex2D(_Control3, IN.uv_Control);
	#endif
 	float total_coverage=dot(splat_controlA, 1);
	#ifdef _4LAYERS
		//float4 splat_controlA_normalized=splat_controlA/total_coverage;
	#else
		float4 splat_controlB = tex2D(_Control2, IN.uv_Control);
	 	total_coverage+=dot(splat_controlB, 1);
		//float4 splat_controlA_normalized=splat_controlA/total_coverage;
		//float4 splat_controlB_normalized=splat_controlB/total_coverage;
	#endif
	
	float _uv_Relief_w=saturate((IN._uv_Relief.w - _TERRAIN_distance_start_bumpglobal) / _TERRAIN_distance_transition_bumpglobal);
	#ifdef FAR_ONLY
		#define _uv_Relief_z 0
		float _uv_Relief_wz_no_overlap=_uv_Relief_w;
	#else
		float _uv_Relief_z=saturate((IN._uv_Relief.w - _TERRAIN_distance_start) / _TERRAIN_distance_transition);
		float _uv_Relief_wz_no_overlap=_uv_Relief_w*_uv_Relief_z;
		_uv_Relief_z=1-_uv_Relief_z;
	#endif
	
	#if !( (defined(RTP_REFLECTION) || (defined(RTP_IBL_SPEC) && !defined(RTP_RECONSTRUCT_WORLDNORMAL))) && defined(RTP_TRIPLANAR) )
		float2 dx = ddx( IN._uv_Aux.xy);
		float2 dy = ddy( IN._uv_Aux.xy);
	#endif
	float d = max( dot( dx, dx ), dot( dy, dy ) );
	mip_selector=0.5*log2(d);
	
	#if !defined(RTP_TRIPLANAR) 
		float4 global_bump_val=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_BumpMapGlobalScale, mip_selector+rtp_mipoffset_globalnorm));
		global_bump_val.rg=global_bump_val.rg*0.6 + tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_BumpMapGlobalScale*8, mip_selector+rtp_mipoffset_globalnorm+3)).rg*0.4;
	#endif
	
	float3 IN_viewDir;
	IN_viewDir.xy=IN._viewDir.xy;
	IN_viewDir.z=sqrt(1 - saturate(dot(IN_viewDir.xy, IN_viewDir.xy)));	
	
	#if defined(RTP_TRIPLANAR) 
		float4 triplanar_xy_global_bumpval=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_BumpMapGlobalScale*8, mip_selector+rtp_mipoffset_globalnorm+3))*0.4;
		triplanar_xy_global_bumpval+=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_BumpMapGlobalScale, mip_selector+rtp_mipoffset_globalnorm))*0.6;
		
		fixed4 hMapVal_YZ = tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.yz);
		fixed4 hMapVal_XY = tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xy);
		fixed4 hMapVal_XZ = tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xz);	
		
		float3 triplanar_blend;
		#if defined(COLOR_EARLY_EXIT) 
			float2 _uv_Aux=IN.color.xz*2-1;
			triplanar_blend.xz = abs(_uv_Aux);
			bool2 triplanar_flip = _uv_Aux<0;
			triplanar_blend.y=abs(IN.color.y*2-1);
		#else
			triplanar_blend.xz = abs(IN._uv_Aux.zw);
			bool2 triplanar_flip = IN._uv_Aux.zw<0;
			triplanar_blend.y=sqrt(1 - saturate(dot(triplanar_blend.xz, triplanar_blend.xz)));
		#endif
		float triplanar_blend_y_uncomp=triplanar_blend.y;
				
		float3 triplanar_blend_tmp=triplanar_blend/dot(triplanar_blend,1);
		float4 global_bump_val=triplanar_blend_tmp.x*(tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.yz*_BumpMapGlobalScale, mip_selectorTRIPLANAR.xx+rtp_mipoffset_globalnorm))*0.4+tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.yz*_BumpMapGlobalScale*8, mip_selectorTRIPLANAR.xx+rtp_mipoffset_globalnorm+3))*0.6);
		global_bump_val+=triplanar_blend_tmp.y*triplanar_xy_global_bumpval;
		global_bump_val+=triplanar_blend_tmp.z*(tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xz*_BumpMapGlobalScale, mip_selectorTRIPLANAR.yy+rtp_mipoffset_globalnorm))*0.4+tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xz*_BumpMapGlobalScale*8, mip_selectorTRIPLANAR.yy+rtp_mipoffset_globalnorm+3))*0.6);
		
		#ifdef FAR_ONLY
			triplanar_blend = pow(abs(triplanar_blend), 128);
		#else
			triplanar_blend = pow(abs(triplanar_blend), lerp(128,32,_uv_Relief_z));
		#endif
		triplanar_blend/=dot(abs(triplanar_blend),1);		
		
		bool3 triplanar_blend_vector=triplanar_blend<0.98;
		bool triplanar_blend_flag=all(triplanar_blend_vector);
		float triplanar_blend_simple=max(max(triplanar_blend.x, triplanar_blend.y), triplanar_blend.z);
		#if defined(RTP_SUPER_DETAIL)
			float triplanar_blend_superdetail=saturate(triplanar_blend_simple-0.98)*20;
		#endif
		float3 uvTRI=float3(IN._uv_Relief.xy, mip_selector.x);
		float3 dirTRI=IN_viewDir.xyz;
		uvTRI=(triplanar_blend_simple==triplanar_blend.x) ? float3(IN._uv_Relief.yz, mip_selectorTRIPLANAR.x) : uvTRI;
		dirTRI.xy=(triplanar_blend_simple==triplanar_blend.x) ? IN_viewDir.yx : dirTRI.xy;
		if (triplanar_blend_simple==triplanar_blend.x) dirTRI.y=triplanar_flip.x ? dirTRI.y : -dirTRI.y;
		uvTRI=(triplanar_blend_simple==triplanar_blend.z) ? float3(IN._uv_Relief.xz, mip_selectorTRIPLANAR.y) : uvTRI;
		if (triplanar_blend_simple==triplanar_blend.z) dirTRI.y=triplanar_flip.y ? dirTRI.y : -dirTRI.y;
	#endif
		
	#ifdef COLOR_MAP
		float global_color_blend=lerp( lerp(_GlobalColorMapBlendValues.y, _GlobalColorMapBlendValues.x, _uv_Relief_z*_uv_Relief_z), _GlobalColorMapBlendValues.z, _uv_Relief_w);
		#if defined(RTP_SIMPLE_SHADING) || defined(COLOR_EARLY_EXIT)
			float4 global_color_value=tex2D(_ColorMapGlobal, IN.uv_Control);
		#else
			float4 global_color_value=tex2D(_ColorMapGlobal, IN.uv_Control+(global_bump_val.rg-float2(0.5f, 0.5f))*_GlobalColorMapDistortByPerlin);
		#endif
		#ifdef RTP_CUT_HOLES
		clip(global_color_value.a-0.001f);
		#endif
		
		// bez colormapy nie ma optymalizacji (ale najczesciej ją mamy)
		if (total_coverage<0.001) return;
				
		global_color_value.rgb=lerp(tex2Dlod(_ColorMapGlobal, float4(IN.uv_Control+(global_bump_val.rg-float2(0.5f, 0.5f))*_GlobalColorMapDistortByPerlin, _GlobalColorMapNearMIP.xx)).rgb, global_color_value.rgb, _uv_Relief_w);
		
		//float perlin2global_color=abs((global_bump_val.r-0.4)*5);
		//perlin2global_color*=perlin2global_color;
		//float GlobalColorMapSaturationByPerlin = saturate( lerp(_GlobalColorMapSaturation, _GlobalColorMapSaturationFar, _uv_Relief_w) -perlin2global_color*_GlobalColorMapSaturationByPerlin);
		float GlobalColorMapSaturationByPerlin = lerp(_GlobalColorMapSaturation, _GlobalColorMapSaturationFar, _uv_Relief_w);
		global_color_value.rgb=lerp(dot(global_color_value.rgb,0.35).xxx, global_color_value.rgb, GlobalColorMapSaturationByPerlin);
		global_color_value.rgb*=lerp(_GlobalColorMapBrightness, _GlobalColorMapBrightnessFar, _uv_Relief_w);
	#endif

	float4 tHA;
	float4 tHB=0;
	float4 splat_control1 = splat_controlA;
	#ifdef RTP_TRIPLANAR
		#ifdef USE_EXTRUDE_REDUCTION
			tHA=saturate(lerp((triplanar_blend.x*hMapVal_YZ + triplanar_blend.y*hMapVal_XY + triplanar_blend.z*hMapVal_XZ), 1, PER_LAYER_HEIGHT_MODIFIER89AB)+0.001);
		#else
			tHA=saturate((triplanar_blend.x*hMapVal_YZ + triplanar_blend.y*hMapVal_XY + triplanar_blend.z*hMapVal_XZ)+0.001);
		#endif	
	#else
		#ifdef USE_EXTRUDE_REDUCTION
			tHA=saturate(lerp(tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xy), 1, PER_LAYER_HEIGHT_MODIFIER89AB)+0.001);
		#else
			tHA=saturate(tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xy)+0.001);
		#endif	
	#endif
	splat_control1 *= tHA;
	
	#ifdef _4LAYERS
		float4 splat_control1_mid=splat_control1*splat_control1;
		splat_control1_mid/=dot(splat_control1_mid,1);
		float4 splat_control1_close=splat_control1_mid*splat_control1_mid;
		#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS1
			splat_control1_close*=splat_control1_close;
		#endif
		#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS2
			splat_control1_close*=splat_control1_close;
		#endif
		splat_control1_close/=dot(splat_control1_close,1);
		splat_control1=lerp(splat_control1_mid, splat_control1_close, _uv_Relief_z);
		#ifdef NOSPEC_BLEED		
			float4 splat_control1_nobleed=saturate(splat_control1-float4(0.5,0.5,0.5,0.5))*2;
		#else
			float4 splat_control1_nobleed=splat_control1;
		#endif
	#else
		float4 splat_control1_mid=splat_control1*splat_control1;
		float4 splat_control2 = splat_controlB;
		#ifdef USE_EXTRUDE_REDUCTION
			tHB=saturate(lerp(tex2D(_TERRAIN_HeightMap2, IN._uv_Relief.xy), 1, PER_LAYER_HEIGHT_MODIFIER4567)+0.001);
			splat_control2 *= tHB;
		#else
			tHB=saturate(tex2D(_TERRAIN_HeightMap2, IN._uv_Relief.xy)+0.001);
			splat_control2 *= tHB;
		#endif	
		float4 splat_control2_mid=splat_control2*splat_control2;
		float norm_sum=dot(splat_control1_mid,1) + dot(splat_control2_mid,1);
		splat_control1_mid/=norm_sum;
		splat_control2_mid/=norm_sum;
		
		float4 splat_control1_close=splat_control1_mid*splat_control1_mid;
		float4 splat_control2_close=splat_control2_mid*splat_control2_mid;
		#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS1
			splat_control1_close*=splat_control1_close;
			splat_control2_close*=splat_control2_close;
		#endif
		#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS2
			splat_control1_close*=splat_control1_close;
			splat_control2_close*=splat_control2_close;
		#endif
		norm_sum=dot(splat_control1_close,1) + dot(splat_control2_close,1);
		splat_control1_close/=norm_sum;
		splat_control2_close/=norm_sum;
		splat_control1=lerp(splat_control1_mid, splat_control1_close, _uv_Relief_z);
		splat_control2=lerp(splat_control2_mid, splat_control2_close, _uv_Relief_z);
		#ifdef NOSPEC_BLEED		
			float4 splat_control1_nobleed=saturate(splat_control1-float4(0.5,0.5,0.5,0.5))*2;
			float4 splat_control2_nobleed=saturate(splat_control2-float4(0.5,0.5,0.5,0.5))*2;
		#else
			float4 splat_control1_nobleed=splat_control1;
			float4 splat_control2_nobleed=splat_control2;
		#endif
	#endif
	
	float splat_controlA_coverage=dot(splat_control1, 1);
	#ifndef _4LAYERS
	float splat_controlB_coverage=dot(splat_control2, 1);
	#endif
	
	// layer emission - init step
	#ifdef RTP_EMISSION
		float emission_valA=dot(splat_control1_mid, _LayerEmission89AB);
		#ifndef _4LAYERS
			float emission_valB=dot(splat_control2_mid, _LayerEmission4567);
		#endif	
		float layer_emission = emission_valA;
		#ifndef _4LAYERS
			layer_emission += emission_valB;
		#endif		
		#if defined(_4LAYERS)
			half3 _LayerEmissionColor=half3( dot(splat_control1_mid, _LayerEmissionColorR89AB), dot(splat_control1_mid, _LayerEmissionColorG89AB), dot(splat_control1_mid, _LayerEmissionColorB89AB) );
		#else
			half3 _LayerEmissionColor=half3( dot(splat_control1_mid, _LayerEmissionColorR89AB)+dot(splat_control2_mid, _LayerEmissionColorR4567), dot(splat_control1_mid, _LayerEmissionColorG89AB)+dot(splat_control2_mid, _LayerEmissionColorG4567), dot(splat_control1_mid, _LayerEmissionColorB89AB)+dot(splat_control2_mid, _LayerEmissionColorB4567)  );
		#endif
	#endif
	
	#if defined(NEED_LOCALHEIGHT)
		float actH=dot(splat_control1, tHA);
		#ifndef _4LAYERS
			actH+=dot(splat_control2, tHB);
		#endif
	#endif
	
	#ifdef RTP_NORMALGLOBAL
		// tutaj musimy się zadowolić stretchowanym (w POM) viewDir, bo ten z surface shadera jest płaski (obl dla v.normal=(0,1,0))
		float diffFresnel = 1.0f - IN_viewDir.z;
	#else
		IN.viewDir=normalize(IN.viewDir);
		float diffFresnel = 1.0f - IN.viewDir.z; // unstretched version (i mamy wszystkie komponenty)
	#endif
	diffFresnel*=diffFresnel;
	diffFresnel*=diffFresnel;
	
	#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
	#ifdef _4LAYERS
		// gloss vs. fresnel dependency
		float fresnelAtten=dot(splat_control1, RTP_FresnelAtten89AB);
		o.RTP.x=dot(splat_control1, RTP_Fresnel89AB);
	#else
		float fresnelAtten=dot(splat_control1, RTP_FresnelAtten89AB)+dot(splat_control2, RTP_FresnelAtten4567);
		o.RTP.x=dot(splat_control1, RTP_Fresnel89AB)+dot(splat_control2, RTP_Fresnel4567);
	#endif	
	#endif
	
    #if defined(RTP_WETNESS) || defined(RTP_REFLECTION)
        float p = 0;
        float _WaterOpacity=0;
	#endif
	
    #if defined(RTP_WETNESS)
		float TERRAIN_LayerWetStrength=0;
		float TERRAIN_WetSpecularity=0;
		float TERRAIN_WetGloss=0;
		float TERRAIN_WaterSpecularity=0;
		float TERRAIN_WaterGloss=0;
		float TERRAIN_WaterGlossDamper=0;		
		half4 TERRAIN_WaterColor=half4(1,1,1,1); // A - fresnel
		float TERRAIN_WaterIBL_SpecWetStrength=0;
		float TERRAIN_WaterIBL_SpecWaterStrength=0;
    #endif		
	#if defined(RTP_WETNESS) || (defined(RTP_SNOW) && defined(RTP_SNW_COVERAGE_FROM_WETNESS))
		float mip_selector_tmp=saturate(IN._uv_Relief.w-1);// bug in compiler for forward pass, we have to specify mip level indirectly (can't be treated constant)
		float water_mask=tex2Dlod(_BumpMapGlobal, float4(IN.uv_Control*(1-2*_BumpMapGlobal_TexelSize.xx)+_BumpMapGlobal_TexelSize.xx, mip_selector_tmp.xx)).b;
    #endif		
		
	fixed3 col=0;
	fixed3 colAlbedo=0;
	
	float3 norm_far=float3(0,0,1);
	
	#if defined(RTP_SNOW) || defined(RTP_WETNESS)
		float perlinmask=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy/16, mip_selector+rtp_mipoffset_color-4+_uv_Relief_w*2)).r;
	#endif
	#if defined(RTP_SNOW) || defined(RTP_WETNESS) || defined(RTP_CAUSTICS)
		float3 flat_dir = normalDecode(IN._viewDir.zw);
		#if defined(RTP_WETNESS)
			float wetSlope=1-dot(norm_far, flat_dir.xyz);
		#endif
	#endif
	#ifdef RTP_WETNESS
		TERRAIN_WetHeight_Transition*=(perlinmask*0.5+0.5);
		float wet_height_fct=saturate((TERRAIN_WetHeight_Treshold - IN.lightDir.w - TERRAIN_WetHeight_Transition*0.5)/TERRAIN_WetHeight_Transition);
		wet_height_fct=wet_height_fct<0 ? 0 : wet_height_fct;
	#endif
	
	#ifdef RTP_CAUSTICS
	float damp_fct_caustics;
	#if defined(RTP_WETNESS)
		float damp_fct_caustics_inv;
	#endif		
	{
		float norm=saturate(1-flat_dir.z);
		norm*=norm;
		norm*=norm;  
		float CausticsWaterLevel=TERRAIN_CausticsWaterLevel+norm*TERRAIN_CausticsWaterLevelByAngle;
		damp_fct_caustics=saturate((IN.lightDir.w-CausticsWaterLevel+TERRAIN_CausticsWaterDeepFadeLength)/TERRAIN_CausticsWaterDeepFadeLength);
		float overwater=saturate(-(IN.lightDir.w-CausticsWaterLevel-TERRAIN_CausticsWaterShallowFadeLength)/TERRAIN_CausticsWaterShallowFadeLength);
		damp_fct_caustics*=overwater;
   		#if defined(RTP_WETNESS)
			damp_fct_caustics_inv=1-overwater;
		#endif		
		#ifdef RTP_TRIPLANAR
			damp_fct_caustics*=saturate(flat_dir.z+0.1)*0.9+0.1;
		#endif				
	}
	#endif	
		
	norm_far.xy = global_bump_val.rg*3-1.5;
	norm_far.z = sqrt(1 - saturate(dot(norm_far.xy, norm_far.xy)));
	
	#ifdef RTP_SNOW		
		float3 norm_for_snow=norm_far*0.3;
		norm_for_snow.z+=0.7;
	#endif
	
	float2 IN_uv_Relief_Offset;
	#ifdef RTP_SNOW
		float snow_const = 0.5*rtp_snow_strength-perlinmask;
		#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)
			snow_const -= water_mask*2;
		#endif
		float snow_height_fct=saturate((rtp_snow_height_treshold - IN.lightDir.w)/rtp_snow_height_transition)*4;
		snow_height_fct=snow_height_fct<0 ? 0 : snow_height_fct;
		snow_const -= snow_height_fct;
		
		#ifdef _4LAYERS
			float rtp_snow_layer_damp=dot(splat_control1, rtp_snow_strength_per_layer89AB);
		#else
			float rtp_snow_layer_damp=dot(splat_control1, rtp_snow_strength_per_layer89AB)+dot(splat_control2, rtp_snow_strength_per_layer4567);
		#endif	
		
		float snow_val;
		#ifdef COLOR_MAP
			snow_val = snow_const + rtp_snow_strength*dot(1-global_color_value.rgb, rtp_global_color_brightness_to_snow.xxx)+rtp_snow_strength*2;
		#else
			snow_val = snow_const + rtp_snow_strength*0.5*rtp_global_color_brightness_to_snow+rtp_snow_strength*2;
		#endif
		snow_val *= rtp_snow_layer_damp;
		snow_val -= rtp_snow_slope_factor*( 1 - dot(norm_for_snow, flat_dir.xyz) );

		float snow_depth=snow_val-1;
		//bool snow_MayBeNotFullyCovered_flag=(snow_val-rtp_snow_slope_factor)<3; //wyostrzamy warunek (oryginalnie 1) bo ta estymacja nie dziala gdy mamy ostre przejscia pomiędzy materialami i mamy łączenie pomiędzy materiałem ze zredukowanym śniegiem a innym
		snow_depth=snow_depth<0 ? 0:snow_depth*6; 
		
		float snow_depth_lerp=saturate(snow_depth-rtp_snow_deep_factor);

		fixed3 rtp_snow_color_tex=rtp_snow_color.rgb;
	#endif
	
	#ifdef RTP_UV_BLEND
		#ifdef RTP_DISTANCE_ONLY_UV_BLEND
			float blendVal=_uv_Relief_w;
		#else
			float blendVal=(1.0-_uv_Relief_z*0.3);
		#endif
		#ifdef _4LAYERS
			blendVal*=dot(_MixBlend89AB, splat_control1);
		#else
			blendVal*=dot(_MixBlend89AB, splat_control1)+dot(_MixBlend4567, splat_control2);
		#endif
		blendVal*=_blend_multiplier*saturate((global_bump_val.r*global_bump_val.g*2+0.3));
	#endif
	
	#ifdef RTP_POM_SHADING
		IN_viewDir.z=-IN_viewDir.z;
	#endif
	
	#if defined(RTP_UV_BLEND)
		float repl=0;
		float4 MixScaleRouted89AB=float4(UV_BLENDMIX_ROUTE_LAYER_0, UV_BLENDMIX_ROUTE_LAYER_1, UV_BLENDMIX_ROUTE_LAYER_2, UV_BLENDMIX_ROUTE_LAYER_3);
		#ifndef _4LAYERS
			float4 MixScaleRouted4567=float4(UV_BLENDMIX_ROUTE_LAYER_4, UV_BLENDMIX_ROUTE_LAYER_5, UV_BLENDMIX_ROUTE_LAYER_6, UV_BLENDMIX_ROUTE_LAYER_7);
		#endif
	#endif
	
	float4 MIPmult89AB=_MIPmult89AB*_uv_Relief_w;
	#ifndef _4LAYERS
		float4 MIPmult4567=_MIPmult4567*_uv_Relief_w;
	#endif
	
	#ifdef FORCE_ANISO				
		half3 col_aniso;
		half4 gloss_aniso;
		fixed4 c;
		c = tex2D(_SplatC0, IN._uv_Relief.xy); col_aniso = splat_control1.x * c.rgb; gloss_aniso.r = c.a;
		c = tex2D(_SplatC1, IN._uv_Relief.xy); col_aniso += splat_control1.y * c.rgb; gloss_aniso.g = c.a;
		c = tex2D(_SplatC2, IN._uv_Relief.xy); col_aniso += splat_control1.z * c.rgb; gloss_aniso.b = c.a;
		c = tex2D(_SplatC3, IN._uv_Relief.xy); col_aniso += splat_control1.w * c.rgb; gloss_aniso.a = c.a;			
		fixed4 normals_combined = tex2D(_BumpMap89, IN._uv_Relief.xy).rgba*splat_control1.rrgg;
		normals_combined+=tex2D(_BumpMapAB, IN._uv_Relief.xy).rgba*splat_control1.bbaa;
		float3 n_aniso;
		n_aniso.xy=(normals_combined.rg+normals_combined.ba)*2-1;
		n_aniso.xy*=_uv_Relief_z;
		n_aniso.z = sqrt(1 - saturate(dot(n_aniso.xy, n_aniso.xy)));
	#endif
	
	#ifdef FAR_ONLY
	if (false) {
	#else
	if (_uv_Relief_z>0) {
	#endif
 		//////////////////////////////////
 		//
 		// close
 		//
 		//////////////////////////////////
		#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
			float _off=16*_SplatAtlasC_TexelSize.x;
			float _mult=_off*-2+0.5;
			float4 _offMix=_off;
			float4 _multMix=_mult;
			#ifndef RTP_TRIPLANAR
				float hi_mip_adjust=(exp2(min(mip_selector.x+rtp_mipoffset_color,6)))*_SplatAtlasC_TexelSize.x; 
				_mult-=hi_mip_adjust;
				_off+=0.5*hi_mip_adjust;
			#endif
			float4 uvSplat01, uvSplat23;
			float4 uvSplat01M, uvSplat23M;
		#endif

	 	float4 rayPos = float4(IN._uv_Relief.xy, 1, clamp((mip_selector.x+rtp_mipoffset_height), 0, 6) );
	 	
	 	#ifdef RTP_POM_SHADING
		float3 EyeDirTan = IN_viewDir.xyz;
		float slopeF=1+IN_viewDir.z;
		slopeF*=slopeF;
		slopeF*=slopeF;
		EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL*_uv_Relief_z*(1-slopeF)); // damp bo kanale a colormapy, odleglosci i skompresowanym kacie obserwaci (poprawia widok zboczy)
		bool hit_flag=false;
		float delta=_TERRAIN_HeightMap3_TexelSize.x*exp2(rayPos.w)*_TERRAIN_WAVELENGTH/length(EyeDirTan.xy);
		EyeDirTan*=delta;
		bool height;
		
		float dh_prev=0;
		float h_prev=1.001;
		float _h;
		
		float shadow_atten=1;
		#endif
		
		#ifndef _4LAYERS 		
		#ifdef RTP_HARD_CROSSPASS
		 	if (false) {
	 	#else
		 	if (splat_controlA_coverage>0.01 && splat_controlB_coverage>0.01) {
	 	#endif
	 		//////////////////////////////////////////////
	 		//
	 		// splats 0-7 close combined
	 		//
	 		///////////////////////////////////////////////
	 		#ifdef RTP_SHOW_OVERLAPPED
	 		o.Emission.r=1;
	 		#endif

	 		#ifdef RTP_POM_SHADING
				if (COLOR_DAMP_VAL>0) {
				for(int i=0; i<_TERRAIN_DIST_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
			 		float4 tH1, tH2;
					#ifdef USE_EXTRUDE_REDUCTION
						tH1=lerp(tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER89AB);
						tH2=lerp(tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER4567);
					#else
						tH1=tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww);
						tH2=tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww);
					#endif
					_h=saturate(dot(splat_control1, tH1)+dot(splat_control2, tH2));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break; 
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				}
	
				if (hit_flag) {
					// secant search - 2 steps
					float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
					rayPos.xyz-=EyeDirTan*(1 - scl); // back
			 		float4 tH1, tH2;
					#ifdef USE_EXTRUDE_REDUCTION
						tH1=lerp(tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER89AB);
						tH2=lerp(tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER4567);
					#else
						tH1=tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww);
						tH2=tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww);
					#endif
					float _nh=saturate(dot(splat_control1, tH1)+dot(splat_control2, tH2));
					if (_nh >= rayPos.z) {
						EyeDirTan*=scl;
						scl=dh_prev / ((_nh-h_prev) - EyeDirTan.z);
						rayPos.xyz-=EyeDirTan*(1 - scl); // back
					} else {
						EyeDirTan*=(1-scl);
						dh_prev = rayPos.z - _nh;
						scl=dh_prev / ((_h-_nh) - EyeDirTan.z);
						rayPos.xyz+=EyeDirTan*scl; // forth
					}
				}
				#if defined(NEED_LOCALHEIGHT)
					actH=lerp(actH, rayPos.z, _uv_Relief_z);
				#endif
			#else
				#ifdef RTP_PM_SHADING
					rayPos.xy += ParallaxOffset(dot(splat_control1, tHA)+dot(splat_control2, tHB), _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
				#endif
				#if defined(NEED_LOCALHEIGHT)
					actH=lerp(actH, dot(splat_control1, tHA)+dot(splat_control2, tHB), _uv_Relief_z);
				#endif
			#endif
			
			//
			// hot air refraction
			#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				float2 emission_refract_offset;
				{
					emission_refract_offset = tex2Dlod(_BumpMapGlobal, float4(rayPos.xy+_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset += tex2Dlod(_BumpMapGlobal, float4(rayPos.xy-_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset = emission_refract_offset*2-1;
					
					float emission_refract_strength=dot(splat_control1_mid, _LayerEmissionRefractStrength89AB)+dot(splat_control2_mid, _LayerEmissionRefractStrength4567);	
					float emission_refract_edge_val=dot(splat_control1_mid, _LayerEmissionRefractHBedge89AB)+dot(splat_control2_mid, _LayerEmissionRefractHBedge4567);
					float heightblend_edge=dot(0.5-abs(splat_control1_mid-0.5), 1) + dot(0.5-abs(splat_control2_mid-0.5), 1);
					emission_refract_offset *= lerp(1, heightblend_edge, emission_refract_edge_val)*emission_refract_strength*_uv_Relief_z;
					// dla wody offset doliczamy później (aby była widoczna refrakcja na niej)
					#ifndef RTP_WETNESS
						rayPos.xy += emission_refract_offset*layer_emission;
					#endif
				}
			#endif
						
			////////////////////////////////
			// water
			//
			float4 water_splat_control1=splat_control1;
			float4 water_splat_control1_nobleed=splat_control1_nobleed;
			float4 water_splat_control2=splat_control2;
			float4 water_splat_control2_nobleed=splat_control2_nobleed;

			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control1, TERRAIN_LayerWetStrength89AB)+dot(water_splat_control2, TERRAIN_LayerWetStrength4567);
				float TERRAIN_WaterLevel=dot(water_splat_control1, TERRAIN_WaterLevel89AB)+dot(water_splat_control2, TERRAIN_WaterLevel4567);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control1, TERRAIN_WaterLevelSlopeDamp89AB)+dot(water_splat_control2, TERRAIN_WaterLevelSlopeDamp4567);
				float TERRAIN_Flow=dot(water_splat_control1, TERRAIN_Flow89AB)+dot(water_splat_control2, TERRAIN_Flow4567);
				float TERRAIN_WetFlow=dot(water_splat_control1, TERRAIN_WetFlow89AB)+dot(water_splat_control2, TERRAIN_WetFlow4567);
				float TERRAIN_WaterEdge=dot(water_splat_control1, TERRAIN_WaterEdge89AB)+dot(water_splat_control2, TERRAIN_WaterEdge4567);
				float TERRAIN_Refraction=dot(water_splat_control1, TERRAIN_Refraction89AB)+dot(water_splat_control2, TERRAIN_Refraction4567);
				float TERRAIN_WetRefraction=dot(water_splat_control1, TERRAIN_WetRefraction89AB)+dot(water_splat_control2, TERRAIN_WetRefraction4567);
				
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif
				float2 roff=0;
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control1_nobleed, TERRAIN_WetSpecularity89AB) + dot(water_splat_control2_nobleed, TERRAIN_WetSpecularity4567); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control1, TERRAIN_WetGloss89AB)+dot(water_splat_control2, TERRAIN_WetGloss4567);
					TERRAIN_WaterSpecularity = dot(water_splat_control1_nobleed, TERRAIN_WaterSpecularity89AB) + dot(water_splat_control2_nobleed, TERRAIN_WaterSpecularity4567); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control1, TERRAIN_WaterGloss89AB)+dot(water_splat_control2, TERRAIN_WaterGloss4567);
					TERRAIN_WaterGlossDamper=dot(water_splat_control1, TERRAIN_WaterGlossDamper89AB)+dot(water_splat_control2, TERRAIN_WaterGlossDamper4567);
					TERRAIN_WaterColor=half4( dot(water_splat_control1, TERRAIN_WaterColorR89AB)+dot(water_splat_control2, TERRAIN_WaterColorR4567), dot(water_splat_control1, TERRAIN_WaterColorG89AB)+dot(water_splat_control2, TERRAIN_WaterColorG4567), dot(water_splat_control1, TERRAIN_WaterColorB89AB)+dot(water_splat_control2, TERRAIN_WaterColorB4567), dot(water_splat_control1, TERRAIN_WaterColorA89AB)+dot(water_splat_control2, TERRAIN_WaterColorA4567) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control1, TERRAIN_WaterIBL_SpecWetStrength89AB)+dot(water_splat_control2, TERRAIN_WaterIBL_SpecWetStrength4567);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control1, TERRAIN_WaterIBL_SpecWaterStrength89AB)+dot(water_splat_control2, TERRAIN_WaterIBL_SpecWaterStrength4567);
					
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					float _RippleDamp=saturate(TERRAIN_LayerWetStrength*2-1)*saturate(1-wetSlope*4)*_uv_Relief_z;
					TERRAIN_RainIntensity*=_RippleDamp;
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);
					
					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=(dot(water_splat_control1, TERRAIN_WaterOpacity89AB) + dot(water_splat_control2, TERRAIN_WaterOpacity4567))*p;
					#if defined(RTP_EMISSION)
						float wEmission = (dot(water_splat_control1, TERRAIN_WaterEmission89AB) + dot(water_splat_control2, TERRAIN_WaterEmission4567))*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
						 #if defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
							rayPos.xy += emission_refract_offset*layer_emission;
						#endif
					#endif					
					#if !defined(RTP_SIMPLE_SHADING) && !defined(SIMPLE_WATER)
						float2 flowUV=lerp(IN._uv_Relief.xy, rayPos.xy, 1-p*0.5)*TERRAIN_FlowScale;
						float _Tim=frac(_Time.x*TERRAIN_FlowCycleScale)*2;
						float ft=abs(frac(_Tim)*2 - 1);
						float2 flowSpeed=clamp((flat_dir.xy+0.01)*4,-1,1)/TERRAIN_FlowCycleScale;
						flowSpeed*=TERRAIN_FlowSpeed*TERRAIN_FlowScale;
						float rtp_mipoffset_add = (1-saturate(dot(flowSpeed, flowSpeed)*TERRAIN_mipoffset_flowSpeed))*TERRAIN_mipoffset_flowSpeed;
						rtp_mipoffset_add+=(1-TERRAIN_LayerWetStrength)*8;
						float2 flowOffset=tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx)*flowSpeed, mip_selector+rtp_mipoffset_flow+rtp_mipoffset_add)).rg*2-1;
						flowOffset=lerp(flowOffset, tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx+0.5)*flowSpeed*1.25, mip_selector+rtp_mipoffset_add)).rg*2-1, ft);
						#ifdef RTP_SNOW
							flowOffset*=saturate(1-snow_val);
						#endif							
						// stały przepływ na płaskim
						//float slowMotionFct=dot(flowSpeed,flowSpeed);
						//slowMotionFct=saturate(slowMotionFct*50);
						//flowOffset=lerp(tex2Dlod(_BumpMapGlobal, float4(flowUV+float2(0,2*_Time.x*TERRAIN_FlowSpeed*TERRAIN_FlowScale), mip_selector+rtp_mipoffset_flow)).rg*2-1, flowOffset, slowMotionFct );
						//
						flowOffset*=lerp(TERRAIN_WetFlow, TERRAIN_Flow, p)*_uv_Relief_z*TERRAIN_LayerWetStrength;
					#else
						float2 flowOffset=0;
					#endif
					
					#if defined(RTP_WET_RIPPLE_TEXTURE) && !defined(RTP_SIMPLE_SHADING)
						float2 rippleUV = IN._uv_Relief.xy*TERRAIN_RippleScale + flowOffset*0.1*flowSpeed/TERRAIN_FlowScale;
					    float4 Ripple;
					  	{
					  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
						    Ripple.xy = Ripple.xy * 2 - 1;
						
						    float DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
						    float TimeFrac = DropFrac - 1.0f + Ripple.z;
						    float DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
						    float FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
						  	roff = Ripple.xy * FinalFactor * 0.35f;
						  	
						  	rippleUV+=float2(0.25,0.25);
					  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
						    Ripple.xy = Ripple.xy * 2 - 1;
						
						    DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
						    TimeFrac = DropFrac - 1.0f + Ripple.z;
						    DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
						    FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
						  	roff += Ripple.xy * FinalFactor * 0.35f;
					  	}
					  	roff*=4*_RippleDamp*lerp(TERRAIN_WetDropletsStrength, 1, p);
					  	#ifdef RTP_SNOW
					  		roff*=saturate(1-snow_val);
					  	#endif						  	
					  	roff+=flowOffset;
					#else
						roff = flowOffset;
					#endif
					
					#if !defined(RTP_SIMPLE_SHADING)
						rayPos.xy+=TERRAIN_Refraction*roff*max(p, TERRAIN_WetRefraction);
					#endif
				#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				} else {
					rayPos.xy += emission_refract_offset*layer_emission;
				#endif					
				}
			#endif
			// water
			////////////////////////////////			
									
			uvSplat01=frac(rayPos.xy).xyxy*_mult+_off;
			uvSplat01.zw+=float2(0.5,0);
			uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
			
//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif		 	

			fixed4 c;
			float4 gloss;
			float _MipActual=min(mip_selector.x+rtp_mipoffset_color,6);
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col = splat_control1.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += splat_control1.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += splat_control1.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += splat_control1.w * c.rgb; gloss.a = c.a;
			
			float glcombined = dot(gloss, splat_control1);
							
			#ifdef RTP_UV_BLEND
			#ifndef RTP_DISTANCE_ONLY_UV_BLEND
				float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult89AB + log2(MixScaleRouted89AB), float4(6,6,6,6));
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
				float4 _multMix89AB=_multMix-hi_mip_adjustMix; // nie chcemy nadpisać tego bo poniżej potrzeba nam tego dla kanałów 4-7
				float4 _offMix89AB=_offMix+0.5*hi_mip_adjustMix;
			
				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy)*_multMix89AB+_offMix89AB;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww)*_multMix89AB+_offMix89AB;
				uvSplat23M+=float4(0,0.5,0.5,0.5);			
				
				half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
				colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
				colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
				colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
			#endif
			#endif

			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, _MipActual.xx)); col += splat_control2.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, _MipActual.xx)); col += splat_control2.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, _MipActual.xx)); col += splat_control2.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, _MipActual.xx)); col += splat_control2.w * c.rgb; gloss.a = c.a;

			glcombined += dot(gloss, splat_control2);
						
			#ifdef RTP_UV_BLEND
			#ifndef RTP_DISTANCE_ONLY_UV_BLEND
				_MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult4567 + log2(MixScaleRouted4567), float4(6,6,6,6)); // na layerach 4-7 w trybie 8 layers nie możemy korzystać z UV blendu kanałów 4-7 (bo nie wiemy w define pobierającym teksturę poniżej czy to pierwszy czy drugi set layerów) dlatego używamy tutaj maski 89AB
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
				_multMix-=hi_mip_adjustMix;
				_offMix+=0.5*hi_mip_adjustMix;
			
				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.xxyy)*_multMix+_offMix;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.zzww)*_multMix+_offMix;
				uvSplat23M+=float4(0,0.5,0.5,0.5);			
				
				colBlend += splat_control2.x * UV_BLEND_ROUTE_LAYER_4;
				colBlend += splat_control2.y * UV_BLEND_ROUTE_LAYER_5;
				colBlend += splat_control2.z * UV_BLEND_ROUTE_LAYER_6;
				colBlend += splat_control2.w * UV_BLEND_ROUTE_LAYER_7;
			#endif
			#endif
			
			#if defined(RTP_UV_BLEND)
				#ifndef RTP_DISTANCE_ONLY_UV_BLEND			
					float3 colBlendDes=lerp((dot(colBlend.rgb,0.33333)).xxx, colBlend.rgb, dot(splat_control1, _MixSaturation89AB) + dot(splat_control2, _MixSaturation4567));
					repl=dot(splat_control1, _MixReplace89AB) + dot(splat_control2, _MixReplace4567);
					repl*= _uv_Relief_wz_no_overlap; // usuń ew. overlap near vs. far
					col=lerp(col, col*colBlendDes*(dot(splat_control1, _MixBrightness89AB) + dot(splat_control2, _MixBrightness4567)), lerp(blendVal, 1, repl));  
					col = lerp(col,colBlend.rgb, repl);
					// modify glcombined according to UV_BLEND gloss values
					glcombined=lerp(glcombined, colBlend.a, repl*0.5);					
				#endif
			#endif
			
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif	
			float RTP_gloss2mask = dot(splat_control1, RTP_gloss2mask89AB)+dot(splat_control2, RTP_gloss2mask4567)			;
			float _Spec = dot(splat_control1_nobleed, _Spec89AB) + dot(splat_control2_nobleed, _Spec4567); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control1, RTP_gloss_mult89AB)+dot(splat_control2, RTP_gloss_mult4567);
			float RTP_gloss_shaping = dot(splat_control1, RTP_gloss_shaping89AB)+dot(splat_control2, RTP_gloss_shaping4567);
			float gls = saturate(glcombined * RTP_gloss_mult);
			//o_Gloss *=  heightblend_AO*lerp(1, gl, RTP_gloss2mask) * _Spec; // przemnóż przez damping dla warstw 4567
			o_Gloss = lerp(1, gls, RTP_gloss2mask) * _Spec; // przemnóż przez damping dla warstw 4567
			
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);	
			#endif	
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control1, _LayerBrightness2Spec89AB) + dot(splat_control2, _LayerBrightness2Spec4567);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control1, _LayerSaturation89AB) + dot(splat_control2, _LayerSaturation4567));
			col*=dot(splat_control1, _LayerBrightness89AB) + dot(splat_control2, _LayerBrightness4567);  
			
			float3 n;
			float4 normals_combined;
			rayPos.w=mip_selector.x+rtp_mipoffset_bump;
			#ifdef RTP_SNOW
				rayPos.w += snow_depth;
			#endif				
			normals_combined = tex2Dlod(_BumpMap89, rayPos.xyww).rgba*splat_control1.rrgg;
			normals_combined+=tex2Dlod(_BumpMapAB, rayPos.xyww).rgba*splat_control1.bbaa;
			normals_combined+=tex2Dlod(_BumpMap45, rayPos.xyww).rgba*splat_control2.rrgg;
			normals_combined+=tex2Dlod(_BumpMap67, rayPos.xyww).rgba*splat_control2.bbaa;
			n.xy=(normals_combined.rg+normals_combined.ba)*2-1;
			n.xy*=_uv_Relief_z;
			n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
			o.Normal=n;
	        			
			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control1, _VerticalTexture89AB) + dot(splat_control2, _VerticalTexture4567));
			#endif
						
			////////////////////////////////
			// water
			//
	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif
		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=lerp( (1-TERRAIN_WaterGlossDamper), 1, min(_uv_Relief_z, 1-saturate((mip_selector.x+rtp_mipoffset_flow)/4))); // 4ty MIP level lub odległość>near daje całkowite tłumienie
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct;//  * saturate(_uv_Relief_z+0.2); // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color
		        
		        o.Normal = lerp(o.Normal, float3(0,0,1), max(p*0.7, _WaterOpacity));
		        o.Normal.xy+=roff;
		        //o.Normal=normalize(o.Normal);
		  		
	        #endif
			// water
			////////////////////////////////

			#if defined(RTP_SUPER_DETAIL) && !defined(RTP_SIMPLE_SHADING)
				float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(rayPos.xy*_SuperDetailTiling, mip_selector + rtp_mipoffset_superdetail));
				float3 super_detail_norm;
				super_detail_norm.xy = (super_detail.xy*4-2)*(dot(float3(0.8,0.8,0.8),col.rgb)+0.4)+o.Normal.xy;
				super_detail_norm.z = sqrt(1 - saturate(dot(super_detail_norm.xy, super_detail_norm.xy)));
				super_detail_norm=normalize(super_detail_norm);
				float sdVal=_uv_Relief_z*(dot(splat_control1, _SuperDetailStrengthNormal89AB)+dot(splat_control2, _SuperDetailStrengthNormal4567));
				#if defined(RTP_SNOW)
					sdVal*=saturate(1-snow_depth);
				#endif
				o.Normal=lerp(o.Normal, super_detail_norm, sdVal);
				#if defined(RTP_SUPER_DTL_MULTS) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
					float near_blend;
					float far_blend;
					float near_far_blend_dist=saturate(_uv_Relief_z-0.5)*2;
					near_blend=lerp(1, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskNear89AB) + dot(splat_control2, _SuperDetailStrengthMultASelfMaskNear4567));
					far_blend=lerp(0, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskFar89AB) + dot(splat_control2, _SuperDetailStrengthMultASelfMaskFar4567));
					col=lerp(col, col*super_detail.z*2, lerp(far_blend, near_blend, near_far_blend_dist)*(dot(splat_control1, _SuperDetailStrengthMultA89AB)+dot(splat_control2, _SuperDetailStrengthMultA4567)));
					near_blend=lerp(1, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskNear89AB) + dot(splat_control2, _SuperDetailStrengthMultBSelfMaskNear4567));
					far_blend=lerp(0, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskFar89AB) + dot(splat_control2, _SuperDetailStrengthMultBSelfMaskFar4567));
					col=lerp(col, col*super_detail.w*2, lerp(far_blend, near_blend, near_far_blend_dist)*(dot(splat_control1, _SuperDetailStrengthMultB89AB)+dot(splat_control2, _SuperDetailStrengthMultB4567)));
				#endif
			#endif

//#ifdef RTP_SNOW
//}
//#endif
			// snow color
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif				
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif				
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color, 6)));
				GETrtp_snow_TEX
			#endif		
			#endif
			// snow color

			IN_uv_Relief_Offset.xy=rayPos.xy;
			
		 	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		 	//
		 	// self shadowing 
		 	//
	 		#if defined(RTP_POM_SHADING) && !defined(RTP_TRIPLANAR)
	 		#if defined(RTP_SOFT_SHADOWS) || defined(RTP_HARD_SHADOWS)
	 			#ifdef RTP_SNOW
	 				rayPos.w=mip_selector.x+rtp_mipoffset_height+snow_depth;
	 			#endif

				EyeDirTan=IN.lightDir.xyz;
				EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
				delta=_TERRAIN_HeightMap3_TexelSize.x*exp2(rayPos.w)*_TERRAIN_WAVELENGTH_SHADOWS/length(EyeDirTan.xy);
				h_prev=rayPos.z;
				//rayPos.xyz+=EyeDirTan*_TERRAIN_HeightMap3_TexelSize.x*2;
				EyeDirTan*=delta;

				hit_flag=false;
				dh_prev=0;
				//_TERRAIN_SHADOW_STEPS=min(_TERRAIN_SHADOW_STEPS, ((EyeDirTan.z>0) ? (1-rayPos.z) : rayPos.z) / abs(EyeDirTan.z));
				for(int i=0; i<_TERRAIN_SHADOW_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
					_h=dot(splat_control1, tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww))+dot(splat_control2, tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break;
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				
				#ifdef RTP_SOFT_SHADOWS
					if (hit_flag) {
						// secant search
						float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
						rayPos.xyz-=EyeDirTan*(1 - scl); // back
						EyeDirTan=IN.lightDir.xyz*_TERRAIN_HeightMap3_TexelSize.x*2*_TERRAIN_WAVELENGTH_SHADOWS;
						EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
						float smooth_val=0;
						float break_val=_TERRAIN_ExtrudeHeight*_TERRAIN_ShadowSmoothing;
						for(int i=0; i<_TERRAIN_SHADOW_SMOOTH_STEPS; i++) {
							rayPos.xyz+=EyeDirTan;
							float d=dot(splat_control1, tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww))+dot(splat_control2, tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww)) - rayPos.z;
							smooth_val+=saturate(d);
							if (smooth_val>break_val) break;
						}
						shadow_atten=saturate(1-smooth_val/break_val);
					}
				#else
					shadow_atten=hit_flag ? 0 : shadow_atten;
				#endif
		
				shadow_atten=shadow_atten*_TERRAIN_SelfShadowStrength+(1-_TERRAIN_SelfShadowStrength);
				#ifdef RTP_SNOW
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL*(1-snow_depth_lerp));
				#else
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL);
				#endif
	        	#if defined(RTP_WETNESS)
	 				shadow_atten=lerp(shadow_atten, 1, _WaterOpacity);
				#endif
				
			#endif
			#endif		
			//
		 	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
		 	
		 	// end of splats 0-7 close combined			
	 	} else if (splat_controlA_coverage>splat_controlB_coverage)
	 	#endif // !_4LAYERS 		
	 	{
	 		//////////////////////////////////
	 		//
	 		// splats 0-3 close
	 		//
	 		//////////////////////////////////
	 		
			#ifdef RTP_HARD_CROSSPASS
				float hOffset=dot(splat_control1, tHA);
				splat_control1 /= dot(splat_control1, 1);
				#ifdef NOSPEC_BLEED		
					splat_control1_nobleed=saturate(splat_control1-float4(0.5,0.5,0.5,0.5))*2;
				#else
					splat_control1_nobleed=splat_control1;
				#endif
				splat_control2 = 0;
			#endif

	 		#if defined(RTP_POM_SHADING) && !defined(RTP_TRIPLANAR)
				if (COLOR_DAMP_VAL>0) {
				for(int i=0; i<_TERRAIN_DIST_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
			 		float4 tH;
					#ifdef USE_EXTRUDE_REDUCTION
						tH=lerp(tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER89AB);
					#else
						tH=tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww);
					#endif	
					_h=saturate(dot(splat_control1, tH));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break;
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				}
							
				if (hit_flag) {
					// secant search - 2 steps
					float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
					rayPos.xyz-=EyeDirTan*(1 - scl); // back
			 		float4 tH;
					#ifdef USE_EXTRUDE_REDUCTION
						tH=lerp(tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER89AB);
					#else
						tH=tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww);
					#endif	
					float _nh=saturate(dot(splat_control1, tH));
					if (_nh >= rayPos.z) {
						EyeDirTan*=scl;
						scl=dh_prev / ((_nh-h_prev) - EyeDirTan.z);
						rayPos.xyz-=EyeDirTan*(1 - scl); // back
					} else {
						EyeDirTan*=(1-scl);
						dh_prev = rayPos.z - _nh;
						scl=dh_prev / ((_h-_nh) - EyeDirTan.z);
						rayPos.xyz+=EyeDirTan*scl; // forth
					}
				}
				#if defined(NEED_LOCALHEIGHT)
					actH=lerp(actH, rayPos.z, _uv_Relief_z);
				#endif
			#else
		 		#ifdef RTP_TRIPLANAR
		 				float hgtXZ, hgtXY, hgtYZ, hgtTRI;
		 				hgtTRI=0; hgtXZ=0; hgtXY=0; hgtYZ=0; // to avoid "variable may used being uninitialised" warning
		 				if (triplanar_blend_flag) {
							#ifdef USE_EXTRUDE_REDUCTION
					 			hgtYZ = triplanar_blend.x * dot(splat_control1, lerp(hMapVal_YZ, 1, PER_LAYER_HEIGHT_MODIFIER89AB));
					 			hgtXY = triplanar_blend.y * dot(splat_control1, lerp(hMapVal_XY, 1, PER_LAYER_HEIGHT_MODIFIER89AB));
					 			hgtXZ = triplanar_blend.z * dot(splat_control1, lerp(hMapVal_XZ, 1, PER_LAYER_HEIGHT_MODIFIER89AB));
							#else
					 			hgtYZ = triplanar_blend.x * dot(splat_control1, hMapVal_YZ);
					 			hgtXY = triplanar_blend.y * dot(splat_control1, hMapVal_XY);
					 			hgtXZ = triplanar_blend.z * dot(splat_control1, hMapVal_XZ);
							#endif
							#if defined(NEED_LOCALHEIGHT)
								actH=lerp(actH, hgtYZ + hgtXY + hgtXZ, _uv_Relief_z) ;
							#endif
		 				} else {
		 					// no blend case
							#ifdef USE_EXTRUDE_REDUCTION
					 			hgtTRI = dot(splat_control1, lerp(tex2Dlod(_TERRAIN_HeightMap3, uvTRI.xyzz), 1, PER_LAYER_HEIGHT_MODIFIER89AB));
							#else
					 			hgtTRI = dot(splat_control1, tex2Dlod(_TERRAIN_HeightMap3, uvTRI.xyzz));
							#endif
			 				//hgtTRI*=triplanar_blend_simple;
							#if defined(NEED_LOCALHEIGHT)
								actH=lerp(actH, hgtTRI, _uv_Relief_z);
							#endif
		 				}
				#else
					#ifdef RTP_HARD_CROSSPASS
						#if defined(RTP_PM_SHADING)
							rayPos.xy += ParallaxOffset(hOffset, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
						#endif
						#if defined(NEED_LOCALHEIGHT)
							actH=lerp(actH, hOffset, _uv_Relief_z);
						#endif
					#else
						#if defined(RTP_PM_SHADING)
							rayPos.xy += ParallaxOffset(dot(splat_control1, tHA), _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
						#endif
						#if defined(NEED_LOCALHEIGHT)
							actH=lerp(actH, dot(splat_control1, tHA), _uv_Relief_z);
						#endif
					#endif
				#endif				
			#endif
			
			//
			// hot air refraction
			#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				float2 emission_refract_offset;
				{
					emission_refract_offset = tex2Dlod(_BumpMapGlobal, float4(rayPos.xy+_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset += tex2Dlod(_BumpMapGlobal, float4(rayPos.xy-_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset = emission_refract_offset*2-1;
					
					float emission_refract_strength=dot(splat_control1_mid, _LayerEmissionRefractStrength89AB);	
					float emission_refract_edge_val=dot(splat_control1_mid, _LayerEmissionRefractHBedge89AB);
					float heightblend_edge=dot(0.5-abs(splat_control1_mid-0.5), 1);
					emission_refract_offset *= lerp(1, heightblend_edge, emission_refract_edge_val)*emission_refract_strength*_uv_Relief_z;
					#ifdef RTP_TRIPLANAR
						emission_refract_offset *= triplanar_blend_y_uncomp;
					#endif					
					// dla wody offset doliczamy później (aby była widoczna refrakcja na niej)
					#ifndef RTP_WETNESS
						rayPos.xy += emission_refract_offset*layer_emission;
					#endif
				}
			#endif
			
			////////////////////////////////
			// water
			//
			float4 water_splat_control=splat_control1;
			float4 water_splat_control_nobleed=splat_control1_nobleed;
			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control, TERRAIN_LayerWetStrength89AB);
				float TERRAIN_WaterLevel=dot(water_splat_control, TERRAIN_WaterLevel89AB);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control, TERRAIN_WaterLevelSlopeDamp89AB);
				float TERRAIN_Flow=dot(water_splat_control, TERRAIN_Flow89AB);
				float TERRAIN_WetFlow=dot(water_splat_control, TERRAIN_WetFlow89AB);
				float TERRAIN_WaterEdge=dot(water_splat_control, TERRAIN_WaterEdge89AB);
				float TERRAIN_Refraction=dot(water_splat_control, TERRAIN_Refraction89AB);
				float TERRAIN_WetRefraction=dot(water_splat_control, TERRAIN_WetRefraction89AB);
				
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif
				float2 roff=0;
				float2 flowOffset	=0;
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control_nobleed, TERRAIN_WetSpecularity89AB); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control, TERRAIN_WetGloss89AB);
					TERRAIN_WaterSpecularity = dot(water_splat_control_nobleed, TERRAIN_WaterSpecularity89AB); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control, TERRAIN_WaterGloss89AB);
					TERRAIN_WaterGlossDamper=dot(water_splat_control, TERRAIN_WaterGlossDamper89AB);
					TERRAIN_WaterColor=half4( dot(water_splat_control, TERRAIN_WaterColorR89AB), dot(water_splat_control, TERRAIN_WaterColorG89AB), dot(water_splat_control, TERRAIN_WaterColorB89AB), dot(water_splat_control, TERRAIN_WaterColorA89AB) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWetStrength89AB);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWaterStrength89AB);
					
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					float _RippleDamp=saturate(TERRAIN_LayerWetStrength*2-1)*saturate(1-wetSlope*4)*_uv_Relief_z;
					TERRAIN_RainIntensity*=_RippleDamp;
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);

					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=dot(water_splat_control, TERRAIN_WaterOpacity89AB)*p;
					#if defined(RTP_EMISSION)
						float wEmission = dot(water_splat_control, TERRAIN_WaterEmission89AB)*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
						 #if defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
							rayPos.xy += emission_refract_offset*layer_emission;
							float hotFilter=abs(emission_refract_offset.x+emission_refract_offset.y)*50;
							hotFilter*=hotFilter;
							mip_selector += hotFilter*layer_emission*5;
						#endif
					#endif					
					#if !defined(RTP_SIMPLE_SHADING) && !defined(SIMPLE_WATER)
						float2 flowUV=lerp(IN._uv_Relief.xy, rayPos.xy, 1-p*0.5)*TERRAIN_FlowScale;
						float _Tim=frac(_Time.x*TERRAIN_FlowCycleScale)*2;
						float ft=abs(frac(_Tim)*2 - 1);
						float2 flowSpeed=clamp((flat_dir.xy+0.01)*4,-1,1)/TERRAIN_FlowCycleScale;
						flowSpeed*=TERRAIN_FlowSpeed*TERRAIN_FlowScale;
						float rtp_mipoffset_add = (1-saturate(dot(flowSpeed, flowSpeed)*TERRAIN_mipoffset_flowSpeed))*TERRAIN_mipoffset_flowSpeed;
						rtp_mipoffset_add+=(1-TERRAIN_LayerWetStrength)*8;
						flowOffset=tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx)*flowSpeed, mip_selector+rtp_mipoffset_flow+rtp_mipoffset_add)).rg*2-1;
						flowOffset=lerp(flowOffset, tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx+0.5)*flowSpeed*1.25, mip_selector+rtp_mipoffset_flow+rtp_mipoffset_add)).rg*2-1, ft);
						#ifdef RTP_SNOW
							flowOffset*=saturate(1-snow_val);
						#endif							
						// stały przepływ na płaskim
						//float slowMotionFct=dot(flowSpeed,flowSpeed);
						//slowMotionFct=saturate(slowMotionFct*50);
						//flowOffset=lerp(tex2Dlod(_BumpMapGlobal, float4(flowUV+float2(0,2*_Time.x*TERRAIN_FlowSpeed*TERRAIN_FlowScale), mip_selector+rtp_mipoffset_flow)).rg*2-1, flowOffset, slowMotionFct );
						//
						flowOffset*=lerp(TERRAIN_WetFlow, TERRAIN_Flow, p)*_uv_Relief_z*TERRAIN_LayerWetStrength;
					#endif
					
					#if defined(RTP_WET_RIPPLE_TEXTURE) && !defined(RTP_SIMPLE_SHADING)
						float2 rippleUV = IN._uv_Relief.xy*TERRAIN_RippleScale + flowOffset*0.1*flowSpeed/TERRAIN_FlowScale;
					    float4 Ripple;
					  	{
					  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
						    Ripple.xy = Ripple.xy * 2 - 1;
						
						    float DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
						    float TimeFrac = DropFrac - 1.0f + Ripple.z;
						    float DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
						    float FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
						  	roff = Ripple.xy * FinalFactor * 0.35f;
						  	
						  	rippleUV+=float2(0.25,0.25);
					  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
						    Ripple.xy = Ripple.xy * 2 - 1;
						
						    DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
						    TimeFrac = DropFrac - 1.0f + Ripple.z;
						    DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
						    FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
						  	roff += Ripple.xy * FinalFactor * 0.35f;
					  	}
					  	roff*=4*_RippleDamp*lerp(TERRAIN_WetDropletsStrength, 1, p);
					  	#ifdef RTP_SNOW
					  		roff*=saturate(1-snow_val);
					  	#endif						  	
					  	roff+=flowOffset;
					#else
						roff = flowOffset;
					#endif
					
					#if !defined(RTP_SIMPLE_SHADING)
						flowOffset=TERRAIN_Refraction*roff*max(p, TERRAIN_WetRefraction);
						#if !defined(RTP_TRIPLANAR)
							rayPos.xy+=flowOffset;
						#endif
					#endif
				#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				} else {
					rayPos.xy += emission_refract_offset*layer_emission;
				#endif					
				}
			#endif
			// water
			///////////////////////////////////////////
			
			#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
				#if !defined(RTP_TRIPLANAR)
					uvSplat01=frac(rayPos.xy).xyxy*_mult+_off;
					uvSplat01.zw+=float2(0.5,0);
					uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
				#endif
			#endif			
			
//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif		 			
	 		
			fixed4 c;
			float4 gloss=0;
			
			#ifdef RTP_TRIPLANAR
				//
				// triplanar
				//
				if (triplanar_blend_flag) {
					//
					// triplanar blend case
					//
					float4 normals_combined;
					float3 nA,nB,nC;
					nA=nB=nC=float3(0,0,1);
					#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
						float2 emission_refract_offset_tmp = emission_refract_offset*layer_emission;
					#else
						float2 emission_refract_offset_tmp=0;
					#endif
					#ifdef RTP_WETNESS
						float3 uvTRI1=float3(IN._uv_Relief.yz+flowOffset+emission_refract_offset_tmp, mip_selectorTRIPLANAR.x+rtp_mipoffset_color);
						float3 uvTRI2=float3(IN._uv_Relief.xy+flowOffset+emission_refract_offset_tmp, mip_selector.x+rtp_mipoffset_color);
						float3 uvTRI3=float3(IN._uv_Relief.xz+flowOffset+emission_refract_offset_tmp, mip_selectorTRIPLANAR.y+rtp_mipoffset_color);
					#else
						float3 uvTRI1=float3(IN._uv_Relief.yz+emission_refract_offset_tmp, mip_selectorTRIPLANAR.x+rtp_mipoffset_color);
						float3 uvTRI2=float3(IN._uv_Relief.xy+emission_refract_offset_tmp, mip_selector.x+rtp_mipoffset_color);
						float3 uvTRI3=float3(IN._uv_Relief.xz+emission_refract_offset_tmp, mip_selectorTRIPLANAR.y+rtp_mipoffset_color);
					#endif										
					
					if (triplanar_blend.x>0.02) {
						float4 _MixBlendtmp=splat_control1*triplanar_blend.x;
						float4 tmp_gloss;
						float3 dir=IN_viewDir.yxz;
						dir.y=triplanar_flip.x ? dir.y:-dir.y;
						#if defined(RTP_PM_SHADING)
							uvTRI1.xy+= ParallaxOffset(hgtYZ, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, dir);
						#endif
						#if defined(RTP_USE_COLOR_ATLAS)
							float _MipActual=min(uvTRI1.z,6);
							float hi_mip_adjust=(exp2(_MipActual))*_SplatAtlasC_TexelSize.x; 
							uvSplat01=frac(uvTRI1.xy).xyxy*(_mult-hi_mip_adjust)+_off+0.5*hi_mip_adjust;
							uvSplat01.zw+=float2(0.5,0);
							uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += _MixBlendtmp.y  * c.rgb; tmp_gloss.g = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a = c.a;
						#else
							c = tex2Dlod(_SplatC0, uvTRI1.xyzz); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r=c.a;
							c = tex2Dlod(_SplatC1, uvTRI1.xyzz); col += _MixBlendtmp.y * c.rgb; tmp_gloss.g=c.a;
							c = tex2Dlod(_SplatC2, uvTRI1.xyzz); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b=c.a;
							c = tex2Dlod(_SplatC3, uvTRI1.xyzz); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a=c.a;
						#endif				
						gloss+=triplanar_blend.x*tmp_gloss;
						
						#ifdef RTP_SNOW
							uvTRI1.z += snow_depth;
						#endif							
						
						normals_combined = tex2Dlod(_BumpMap89, uvTRI1.xyzz).grab*splat_control1.rrgg;  // x<-> y
						normals_combined+=tex2Dlod(_BumpMapAB, uvTRI1.xyzz).grab*splat_control1.bbaa;
						nA.xy=(normals_combined.rg+normals_combined.ba)*2-1;
						nA.x=triplanar_flip.x ? nA.x:-nA.x;
						nA.xy*=_uv_Relief_z;
						nA.z = sqrt(1 - saturate(dot(nA.xy, nA.xy)));
					}
					if (triplanar_blend.y>0.02) {
						float4 _MixBlendtmp=splat_control1*triplanar_blend.y;
						float4 tmp_gloss;
						#if defined(RTP_PM_SHADING)
							uvTRI2.xy+= ParallaxOffset(hgtXY, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
						#endif
						#if defined(RTP_USE_COLOR_ATLAS)
							float _MipActual=min(uvTRI2.z,6);
							float hi_mip_adjust=(exp2(_MipActual))*_SplatAtlasC_TexelSize.x; 
							uvSplat01=frac(uvTRI2.xy).xyxy*(_mult-hi_mip_adjust)+_off+0.5*hi_mip_adjust;
							uvSplat01.zw+=float2(0.5,0);
							uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += _MixBlendtmp.y  * c.rgb; tmp_gloss.g = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a = c.a;
						#else
							c = tex2Dlod(_SplatC0, uvTRI2.xyzz); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r=c.a;
							c = tex2Dlod(_SplatC1, uvTRI2.xyzz); col += _MixBlendtmp.y * c.rgb; tmp_gloss.g=c.a;
							c = tex2Dlod(_SplatC2, uvTRI2.xyzz); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b=c.a;
							c = tex2Dlod(_SplatC3, uvTRI2.xyzz); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a=c.a;
						#endif				
						gloss+=triplanar_blend.y*tmp_gloss;
						
						#ifdef RTP_SNOW
							uvTRI2.z += snow_depth;
						#endif							
						
						normals_combined = tex2Dlod(_BumpMap89, uvTRI2.xyzz).rgba*splat_control1.rrgg;
						normals_combined+=tex2Dlod(_BumpMapAB, uvTRI2.xyzz).rgba*splat_control1.bbaa;
						nB.xy=(normals_combined.rg+normals_combined.ba)*2-1;
						nB.xy*=_uv_Relief_z;
						nB.z = sqrt(1 - saturate(dot(nB.xy, nB.xy)));
					}
					if (triplanar_blend.z>0.02) {
						float4 _MixBlendtmp=splat_control1*triplanar_blend.z;
						float4 tmp_gloss;
						float3 dir=IN_viewDir.xyz;
						dir.y=triplanar_flip.y ? dir.y:-dir.y;
						#if defined(RTP_PM_SHADING)
							uvTRI3.xy+= ParallaxOffset(hgtXZ, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, dir);
						#endif
						#if defined(RTP_USE_COLOR_ATLAS)
							float _MipActual=min(uvTRI3.z,6);
							float hi_mip_adjust=(exp2(_MipActual))*_SplatAtlasC_TexelSize.x; 
							uvSplat01=frac(uvTRI3.xy).xyxy*(_mult-hi_mip_adjust)+_off+0.5*hi_mip_adjust;
							uvSplat01.zw+=float2(0.5,0);
							uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += _MixBlendtmp.y  * c.rgb; tmp_gloss.g = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b = c.a;
							c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a = c.a;
						#else
							c = tex2Dlod(_SplatC0, uvTRI3.xyzz); col += _MixBlendtmp.x * c.rgb; tmp_gloss.r=c.a;
							c = tex2Dlod(_SplatC1, uvTRI3.xyzz); col += _MixBlendtmp.y * c.rgb; tmp_gloss.g=c.a;
							c = tex2Dlod(_SplatC2, uvTRI3.xyzz); col += _MixBlendtmp.z * c.rgb; tmp_gloss.b=c.a;
							c = tex2Dlod(_SplatC3, uvTRI3.xyzz); col += _MixBlendtmp.w * c.rgb; tmp_gloss.a=c.a;
						#endif	
						gloss+=triplanar_blend.z*tmp_gloss;
						
						#ifdef RTP_SNOW
							uvTRI3.z += snow_depth;
						#endif			
										
						normals_combined = tex2Dlod(_BumpMap89, uvTRI3.xyzz).rgba*splat_control1.rrgg;
						normals_combined+=tex2Dlod(_BumpMapAB, uvTRI3.xyzz).rgba*splat_control1.bbaa;
						nC.xy=(normals_combined.rg+normals_combined.ba)*2-1;
						nC.y=triplanar_flip.y ? nC.y:-nC.y;
						nC.xy*=_uv_Relief_z;
						nC.z = sqrt(1 - saturate(dot(nC.xy, nC.xy)));
					}	
					float3 n=(triplanar_blend.x * nA + triplanar_blend.y * nB + triplanar_blend.z * nC);
					
					o.Normal=n;
				} else {
					//
					// triplanar no blend - simple case
					//
					#ifdef RTP_WETNESS
						uvTRI.xy+=flowOffset;
					#endif
					#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
						uvTRI.xy += emission_refract_offset*layer_emission;
					#endif
										
					#if defined(RTP_PM_SHADING)
						uvTRI.xy+= ParallaxOffset(hgtTRI, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, dirTRI);
					#endif
					float3 uvTRI_tmp=float3(uvTRI.xy, uvTRI.z+rtp_mipoffset_color);
					float4 tmp_gloss;
					#if defined(RTP_USE_COLOR_ATLAS)
						float _MipActual=min(uvTRI_tmp.z,6);
						float hi_mip_adjust=(exp2(_MipActual))*_SplatAtlasC_TexelSize.x; 
						uvSplat01=frac(uvTRI_tmp.xy).xyxy*(_mult-hi_mip_adjust)+_off+0.5*hi_mip_adjust;
						uvSplat01.zw+=float2(0.5,0);
						uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
						c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col += c.rgb*splat_control1.x; tmp_gloss.r = c.a;
						c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += c.rgb*splat_control1.y; tmp_gloss.g = c.a;
						c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += c.rgb*splat_control1.z; tmp_gloss.b = c.a;
						c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += c.rgb*splat_control1.w; tmp_gloss.a = c.a;
					#else
						c = tex2Dlod(_SplatC0, uvTRI_tmp.xyzz); col += c.rgb*splat_control1.x; tmp_gloss.r=c.a;
						c = tex2Dlod(_SplatC1, uvTRI_tmp.xyzz); col += c.rgb*splat_control1.y; tmp_gloss.g=c.a;
						c = tex2Dlod(_SplatC2, uvTRI_tmp.xyzz); col += c.rgb*splat_control1.z; tmp_gloss.b=c.a;
						c = tex2Dlod(_SplatC3, uvTRI_tmp.xyzz); col += c.rgb*splat_control1.w; tmp_gloss.a=c.a;
					#endif						
					gloss=tmp_gloss;
					
					uvTRI_tmp.z=uvTRI.z+rtp_mipoffset_bump;
					
					#ifdef RTP_SNOW
						uvTRI_tmp.z += snow_depth;
					#endif				
					
					float4 normA=tex2Dlod(_BumpMap89, uvTRI_tmp.xyzz).rgba;
					normA = (triplanar_blend.x>=0.95) ? normA.grab : normA;
					float4 normB=tex2Dlod(_BumpMapAB, uvTRI_tmp.xyzz).rgba;
					normB = (triplanar_blend.x>=0.95) ? normB.grab : normB;
					float4 normals_combined = normA*splat_control1.rrgg;
					normals_combined += normB*splat_control1.bbaa;
					float3 n;
					n.xy=(normals_combined.rg+normals_combined.ba)*2-1;
					if (triplanar_blend.x>=0.95) n.x= triplanar_flip.x ? n.x : -n.x;
					if (triplanar_blend.z>=0.95) 	n.y= triplanar_flip.y ? n.y : -n.y;
					n.xy*=_uv_Relief_z;
					n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
				
					o.Normal=n;
				}
				
				#if defined(RTP_UV_BLEND) && !defined(RTP_DISTANCE_ONLY_UV_BLEND)
					#if defined(RTP_USE_COLOR_ATLAS)
						float4 _MixMipActual=min(uvTRI.zzzz+rtp_mipoffset_color+MIPmult89AB+log2(MixScaleRouted89AB), float4(6,6,6,6));
						_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
						float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
						_multMix-=hi_mip_adjustMix;
						_offMix+=0.5*hi_mip_adjustMix;
						
						uvSplat01M=frac(uvTRI.xyxy*MixScaleRouted89AB.xxyy)*_multMix+_offMix;
						uvSplat01M.zw+=float2(0.5,0);
						uvSplat23M=frac(uvTRI.xyxy*MixScaleRouted89AB.zzww)*_multMix+_offMix;
						uvSplat23M+=float4(0,0.5,0.5,0.5);	
					#else								
						float4 _MixMipActual=uvTRI.zzzz+rtp_mipoffset_color+MIPmult89AB+log2(MixScaleRouted89AB);
						
						float4 uvSplat01M=uvTRI.xyxy*MixScaleRouted89AB.xxyy;
						float4 uvSplat23M=uvTRI.xyxy*MixScaleRouted89AB.zzww;
					#endif				
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;	
					//colBlend=lerp(half4(0.5, 0.5, 0.5, 0), colBlend, saturate((triplanar_blend_simple-0.75)*4));
					colBlend=lerp(half4(0.5, 0.5, 0.5, 0), colBlend, lerp(triplanar_blend_simple, 1, _uv_Relief_w));
				#endif				
				//
				// EOF triplanar
				//
			#else
				//
				// no triplanar
				//
				#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
					float _MipActual=min(mip_selector.x + rtp_mipoffset_color, 6);
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col = splat_control1.x * c.rgb; gloss.r = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.xx)); col += splat_control1.y * c.rgb; gloss.g = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.xx)); col += splat_control1.z * c.rgb; gloss.b = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.xx)); col += splat_control1.w * c.rgb; gloss.a = c.a;
				#else
					#if !defined(FORCE_ANISO) || !defined(RTP_SIMPLE_SHADING)
						rayPos.w=mip_selector.x+rtp_mipoffset_color;
						c = tex2Dlod(_SplatC0, rayPos.xyww); col = splat_control1.x * c.rgb; gloss.r = c.a;
						c = tex2Dlod(_SplatC1, rayPos.xyww); col += splat_control1.y * c.rgb; gloss.g = c.a;
						c = tex2Dlod(_SplatC2, rayPos.xyww); col += splat_control1.z * c.rgb; gloss.b = c.a;
						c = tex2Dlod(_SplatC3, rayPos.xyww); col += splat_control1.w * c.rgb; gloss.a = c.a;
					#endif
					#ifdef FORCE_ANISO				
						#if defined(RTP_SIMPLE_SHADING)
							col=col_aniso;
							gloss=gloss_aniso;
						#else
							col=lerp(col_aniso, col, _uv_Relief_z);
							gloss=lerp(gloss_aniso, gloss, _uv_Relief_z);
						#endif
					#endif
				#endif
				
				//
				// EOF no triplanar
				//
			#endif

			float glcombined = dot(gloss, splat_control1);
						
			#if defined(RTP_UV_BLEND) && !defined(RTP_TRIPLANAR)
			#ifndef RTP_DISTANCE_ONLY_UV_BLEND
				#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
					float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult89AB + log2(MixScaleRouted89AB), float4(6,6,6,6));
					_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
					float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
					_multMix-=hi_mip_adjustMix;
					_offMix+=0.5*hi_mip_adjustMix;
					
					uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy)*_multMix+_offMix;
					uvSplat01M.zw+=float2(0.5,0);
					uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww)*_multMix+_offMix;
					uvSplat23M+=float4(0,0.5,0.5,0.5);	
					
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
				#else
					float4 _MixMipActual=mip_selector.xxxx + rtp_mipoffset_color + MIPmult89AB + log2(MixScaleRouted89AB);
					
					float4 uvSplat01M=IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy;
					float4 uvSplat23M=IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww;
	
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
				#endif
			#endif
			#endif
			
			#if defined(RTP_UV_BLEND)
				#ifndef RTP_DISTANCE_ONLY_UV_BLEND			
					float3 colBlendDes=lerp((dot(colBlend.rgb, 0.33333)).xxx, colBlend.rgb, dot(splat_control1, _MixSaturation89AB));
					repl=dot(splat_control1, _MixReplace89AB);
					repl*= _uv_Relief_wz_no_overlap;
					col=lerp(col, col*colBlendDes*dot(splat_control1, _MixBrightness89AB), lerp(blendVal, 1, repl));  
					col = lerp( col, colBlend.rgb , repl );
					// modify glcombined according to UV_BLEND gloss values
					glcombined=lerp(glcombined, colBlend.a, repl*0.5);					
				#endif
			#endif
			
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif
			float RTP_gloss2mask = dot(splat_control1, RTP_gloss2mask89AB);
			float _Spec = dot(splat_control1_nobleed, _Spec89AB); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control1, RTP_gloss_mult89AB);
			float RTP_gloss_shaping = dot(splat_control1, RTP_gloss_shaping89AB);
			float gls = saturate(glcombined * RTP_gloss_mult);
			o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec;
			
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);
			#endif
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control1, _LayerBrightness2Spec89AB);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control1, _LayerSaturation89AB));
			col*=dot(splat_control1, _LayerBrightness89AB);  
			
			#if !defined(RTP_TRIPLANAR)
				float3 n;
				#if !defined(FORCE_ANISO) || !defined(RTP_SIMPLE_SHADING)
					float4 normals_combined;
					rayPos.w=mip_selector.x+rtp_mipoffset_bump;
					#ifdef RTP_SNOW
						rayPos.w += snow_depth;
					#endif				
					normals_combined = tex2Dlod(_BumpMap89, rayPos.xyww).rgba*splat_control1.rrgg;
					normals_combined+=tex2Dlod(_BumpMapAB, rayPos.xyww).rgba*splat_control1.bbaa;
					n.xy=(normals_combined.rg+normals_combined.ba)*2-1;
					n.xy*=_uv_Relief_z;
					n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
				#endif
				#ifdef FORCE_ANISO				
					#if defined(RTP_SIMPLE_SHADING)
						n=n_aniso;
					#else
						n=lerp(n_aniso, n, _uv_Relief_z);
					#endif
				#endif			
				o.Normal=n;
			#else
				// normalne wyliczone powyżej
			#endif
			
			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control1, _VerticalTexture89AB) );
			#endif
							
			////////////////////////////////
			// water
			//
	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif
		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=lerp( (1-TERRAIN_WaterGlossDamper), 1, min(_uv_Relief_z, 1-saturate((mip_selector.x+rtp_mipoffset_flow)/4))); // 4ty MIP level lub odległość>near daje całkowite tłumienie
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct;//  * saturate(_uv_Relief_z+0.2); // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color				
		        
		        o.Normal = lerp(o.Normal, float3(0,0,1), max(p*0.7, _WaterOpacity));
		        o.Normal.xy+=roff;
		        //o.Normal=normalize(o.Normal);
	  		
	        #endif
			// water
			////////////////////////////////
				
			#if defined(RTP_SUPER_DETAIL) && !defined(RTP_SIMPLE_SHADING)
				#ifdef RTP_TRIPLANAR
					float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(uvTRI.xy*_SuperDetailTiling, uvTRI.zz + rtp_mipoffset_superdetail));
					super_detail=lerp(float4(0.5,0.5,0,0), super_detail, triplanar_blend_superdetail);
				#else
					float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(rayPos.xy*_SuperDetailTiling, mip_selector + rtp_mipoffset_superdetail));
				#endif
				float3 super_detail_norm;				
				super_detail_norm.xy = (super_detail.xy*4-2)*(dot(float3(0.8,0.8,0.8),col.rgb)+0.4)+o.Normal.xy;
				super_detail_norm.z = sqrt(1 - saturate(dot(super_detail_norm.xy, super_detail_norm.xy)));
				super_detail_norm=normalize(super_detail_norm);
				float sdVal=_uv_Relief_z*dot(splat_control1, _SuperDetailStrengthNormal89AB);
				#if defined(RTP_SNOW)
					sdVal*=saturate(1-snow_depth);
				#endif
				o.Normal=lerp(o.Normal, super_detail_norm, sdVal);				
				#if defined(RTP_SUPER_DTL_MULTS) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
					float near_blend;
					float far_blend;
					float near_far_blend_dist=saturate(_uv_Relief_z-0.5)*2;
					near_blend=lerp(1, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskNear89AB));
					far_blend=lerp(0, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskFar89AB));
					col=lerp(col, col*super_detail.z*2, lerp(far_blend, near_blend, near_far_blend_dist)*dot(splat_control1, _SuperDetailStrengthMultA89AB));
					near_blend=lerp(1, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskNear89AB));
					far_blend=lerp(0, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskFar89AB));
					col=lerp(col, col*super_detail.w*2, lerp(far_blend, near_blend, near_far_blend_dist)*dot(splat_control1, _SuperDetailStrengthMultB89AB));
				#endif
			#endif
		
//#ifdef RTP_SNOW
//}
//#endif
			// snow color
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
			#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
			
				// ponownie przelicz uvSplat01 i uvSplat23 dla triplanar (nadpisane powyżej)
				#if defined(RTP_USE_COLOR_ATLAS) && defined(RTP_TRIPLANAR)
					float3 uvTRItmp=float3(IN._uv_Relief.xy, mip_selector.x+rtp_mipoffset_color);
					#if defined(RTP_PM_SHADING)
						#ifdef USE_EXTRUDE_REDUCTION
				 			hgtXY = triplanar_blend.y * dot(splat_control1, lerp(tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xy), 1, PER_LAYER_HEIGHT_MODIFIER89AB));
						#else
				 			hgtXY = triplanar_blend.y * dot(splat_control1, tex2D(_TERRAIN_HeightMap3, IN._uv_Relief.xy));
						#endif						
						uvTRItmp.xy+= ParallaxOffset(hgtXY, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
					#endif
					float hi_mip_adjustTMP=(exp2(min(uvTRItmp.z,6)))*_SplatAtlasC_TexelSize.x; 
					uvSplat01=frac(uvTRItmp.xy).xyxy*(_mult-hi_mip_adjustTMP)+_off+0.5*hi_mip_adjustTMP;
					uvSplat01.zw+=float2(0.5,0);
					uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
				#endif
							
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
			#else
				rayPos.w=mip_selector.x+rtp_mipoffset_color;
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatB0, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatC0, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatB1, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatC1, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatB2, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatC2, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatB3, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatC3, rayPos.xyww);
					GETrtp_snow_TEX
				#endif
			#endif	
			#endif
			// eof snow color
			
			IN_uv_Relief_Offset.xy=rayPos.xy;
			
		 	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		 	//
		 	// self shadowing 
		 	//
	 		#if defined(RTP_POM_SHADING) && !defined(RTP_TRIPLANAR)
	 		#if defined(RTP_SOFT_SHADOWS) || defined(RTP_HARD_SHADOWS)
	 			#ifdef RTP_SNOW
	 				rayPos.w=mip_selector.x+rtp_mipoffset_height+snow_depth;
	 			#endif
	 			
				EyeDirTan=IN.lightDir.xyz;
				EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
				delta=_TERRAIN_HeightMap3_TexelSize.x*exp2(rayPos.w)*_TERRAIN_WAVELENGTH_SHADOWS/length(EyeDirTan.xy);
				h_prev=rayPos.z;
				//rayPos.xyz+=EyeDirTan*_TERRAIN_HeightMap3_TexelSize.x*2;
				EyeDirTan*=delta;
		
				hit_flag=false;
				dh_prev=0;
				//_TERRAIN_SHADOW_STEPS=min(_TERRAIN_SHADOW_STEPS, ((EyeDirTan.z>0) ? (1-rayPos.z) : rayPos.z) / abs(EyeDirTan.z));
				for(int i=0; i<_TERRAIN_SHADOW_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
					_h=dot(splat_control1, tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break;
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				
				#ifdef RTP_SOFT_SHADOWS
					if (hit_flag) {
						// secant search
						float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
						rayPos.xyz-=EyeDirTan*(1 - scl); // back
						EyeDirTan=IN.lightDir.xyz*_TERRAIN_HeightMap3_TexelSize.x*exp2(rayPos.w)*_TERRAIN_WAVELENGTH_SHADOWS;
						EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
						float smooth_val=0;
						float break_val=_TERRAIN_ExtrudeHeight*_TERRAIN_ShadowSmoothing;
						for(int i=0; i<_TERRAIN_SHADOW_SMOOTH_STEPS; i++) {
							rayPos.xyz+=EyeDirTan;
							float d=dot(splat_control1, tex2Dlod(_TERRAIN_HeightMap3, rayPos.xyww)) - rayPos.z;
							smooth_val+=saturate(d);
							if (smooth_val>break_val) break;
						}
						shadow_atten=saturate(1-smooth_val/break_val);
					}
				#else
					shadow_atten=hit_flag ? 0 : shadow_atten;
				#endif
		
				shadow_atten=shadow_atten*_TERRAIN_SelfShadowStrength+(1-_TERRAIN_SelfShadowStrength);
				#ifdef RTP_SNOW
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL*(1-snow_depth_lerp));
				#else
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL);
				#endif
	        	#if defined(RTP_WETNESS)
	 				shadow_atten=lerp(shadow_atten, 1, _WaterOpacity);
				#endif
				
			#endif
			#endif
			//
		 	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
		 				
	 		// end of splats 0-3 close
	 	}
		#ifndef _4LAYERS 		
	 	else {
	 		//////////////////////////////////
	 		//
	 		// splats 4-7 close
	 		//
	 		//////////////////////////////////
	 		
			#ifdef RTP_HARD_CROSSPASS
				float hOffset=dot(splat_control2, tHB);
				splat_control2 /= dot(splat_control2, 1);
				#ifdef NOSPEC_BLEED		
					splat_control2_nobleed=saturate(splat_control2-float4(0.5,0.5,0.5,0.5))*2;
				#else
					splat_control2_nobleed=splat_control2;
				#endif
				splat_control1 = 0;
			#endif
	 		
	 		#if ( defined(RTP_POM_SHADING) && !defined(RTP_HARD_CROSSPASS) ) || ( defined(RTP_POM_SHADING) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO)) )
				if (COLOR_DAMP_VAL>0) {
				for(int i=0; i<_TERRAIN_DIST_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
			 		float4 tH;
					#ifdef USE_EXTRUDE_REDUCTION
						tH=lerp(tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER4567);
					#else
						tH=tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww);
					#endif						
					_h=saturate(dot(splat_control2, tH));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break; 
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				}
				
				if (hit_flag) {
					// secant search - 2 steps
					float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
					rayPos.xyz-=EyeDirTan*(1 - scl); // back
			 		float4 tH;
					#ifdef USE_EXTRUDE_REDUCTION
						tH=lerp(tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww), 1, PER_LAYER_HEIGHT_MODIFIER4567);
					#else
						tH=tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww);
					#endif						
					float _nh=saturate(dot(splat_control2, tH));
					if (_nh >= rayPos.z) {
						EyeDirTan*=scl;
						scl=dh_prev / ((_nh-h_prev) - EyeDirTan.z);
						rayPos.xyz-=EyeDirTan*(1 - scl); // back
					} else {
						EyeDirTan*=(1-scl);
						dh_prev = rayPos.z - _nh;
						scl=dh_prev / ((_h-_nh) - EyeDirTan.z);
						rayPos.xyz+=EyeDirTan*scl; // forth
					}
				}
				#if defined(NEED_LOCALHEIGHT)
					actH=lerp(actH, rayPos.z, _uv_Relief_z);
				#endif
			#else
				#ifdef RTP_HARD_CROSSPASS
			 		#if ( defined(RTP_PM_SHADING) && !defined(RTP_HARD_CROSSPASS) ) || ( defined(RTP_PM_SHADING) && defined(RTP_47SHADING_PM) )
						rayPos.xy += ParallaxOffset(hOffset, _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
					#endif
					#if defined(NEED_LOCALHEIGHT)
						actH=lerp(actH, hOffset, _uv_Relief_z);
					#endif
				#else
			 		#if ( defined(RTP_PM_SHADING) && !defined(RTP_HARD_CROSSPASS) ) || ( defined(RTP_PM_SHADING) && defined(RTP_47SHADING_PM) )
						rayPos.xy += ParallaxOffset(dot(splat_control2, tHB), _TERRAIN_ExtrudeHeight*_uv_Relief_z*COLOR_DAMP_VAL, IN_viewDir.xyz);
					#endif
					#if defined(NEED_LOCALHEIGHT)
						actH=lerp(actH, dot(splat_control2, tHB), _uv_Relief_z);
					#endif
				#endif
			#endif
			
			//
			// hot air refraction
			#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				float2 emission_refract_offset;
				{
					emission_refract_offset = tex2Dlod(_BumpMapGlobal, float4(rayPos.xy+_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset += tex2Dlod(_BumpMapGlobal, float4(rayPos.xy-_Time.xx*EmissionRefractAnimSpeed, mip_selector+rtp_mipoffset_color+EmissionRefractFiltering)).rg;
					emission_refract_offset = emission_refract_offset*2-1;
					
					float emission_refract_strength=dot(splat_control2_mid, _LayerEmissionRefractStrength4567);	
					float emission_refract_edge_val=dot(splat_control2_mid, _LayerEmissionRefractHBedge4567);
					float heightblend_edge=dot(0.5-abs(splat_control2_mid-0.5), 1);
					emission_refract_offset *= lerp(1, heightblend_edge, emission_refract_edge_val)*emission_refract_strength*_uv_Relief_z;
					// dla wody offset doliczamy później (aby była widoczna refrakcja na niej)
					#ifndef RTP_WETNESS
						rayPos.xy += emission_refract_offset*layer_emission;
					#endif
				}
			#endif
			
			////////////////////////////////
			// water
			//
			float4 water_splat_control=splat_control2;
			float4 water_splat_control_nobleed=splat_control2_nobleed;
			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control, TERRAIN_LayerWetStrength4567);
				float TERRAIN_WaterLevel=dot(water_splat_control, TERRAIN_WaterLevel4567);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control, TERRAIN_WaterLevelSlopeDamp4567);
				float TERRAIN_Flow=dot(water_splat_control, TERRAIN_Flow4567);
				float TERRAIN_WetFlow=dot(water_splat_control, TERRAIN_WetFlow4567);
				float TERRAIN_WaterEdge=dot(water_splat_control, TERRAIN_WaterEdge4567);
				float TERRAIN_Refraction=dot(water_splat_control, TERRAIN_Refraction4567);
				float TERRAIN_WetRefraction=dot(water_splat_control, TERRAIN_WetRefraction4567);
				
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif
				float2 roff=0;
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control_nobleed, TERRAIN_WetSpecularity4567); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control, TERRAIN_WetGloss4567);
					TERRAIN_WaterSpecularity = dot(water_splat_control_nobleed, TERRAIN_WaterSpecularity4567); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control, TERRAIN_WaterGloss4567);
					TERRAIN_WaterGlossDamper=dot(water_splat_control, TERRAIN_WaterGlossDamper4567);
					TERRAIN_WaterColor=half4( dot(water_splat_control, TERRAIN_WaterColorR4567), dot(water_splat_control, TERRAIN_WaterColorG4567), dot(water_splat_control, TERRAIN_WaterColorB4567), dot(water_splat_control, TERRAIN_WaterColorA4567) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWetStrength4567);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWaterStrength4567);
				
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					float _RippleDamp=saturate(TERRAIN_LayerWetStrength*2-1)*saturate(1-wetSlope*4)*_uv_Relief_z;
					TERRAIN_RainIntensity*=_RippleDamp;
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);
					
					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=dot(water_splat_control, TERRAIN_WaterOpacity4567)*p;
					#if defined(RTP_EMISSION)
						float wEmission = dot(water_splat_control, TERRAIN_WaterEmission4567)*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
						#if defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
							rayPos.xy += emission_refract_offset*layer_emission;
						#endif
					#endif
			 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
				 		#if !defined(RTP_SIMPLE_SHADING) && !defined(SIMPLE_WATER)
							float2 flowUV=lerp(IN._uv_Relief.xy, rayPos.xy, 1-p*0.5)*TERRAIN_FlowScale;
							float _Tim=frac(_Time.x*TERRAIN_FlowCycleScale)*2;
							float ft=abs(frac(_Tim)*2 - 1);
							float2 flowSpeed=clamp((flat_dir.xy+0.01)*4,-1,1)/TERRAIN_FlowCycleScale;
							flowSpeed*=TERRAIN_FlowSpeed*TERRAIN_FlowScale;
							float rtp_mipoffset_add = (1-saturate(dot(flowSpeed, flowSpeed)*TERRAIN_mipoffset_flowSpeed))*TERRAIN_mipoffset_flowSpeed;
							rtp_mipoffset_add+=(1-TERRAIN_LayerWetStrength)*8;
							float2 flowOffset=tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx)*flowSpeed, mip_selector+rtp_mipoffset_flow+rtp_mipoffset_add)).rg*2-1;
							flowOffset=lerp(flowOffset, tex2Dlod(_BumpMapGlobal, float4(flowUV+frac(_Tim.xx+0.5)*flowSpeed*1.25, mip_selector+rtp_mipoffset_flow+rtp_mipoffset_add)).rg*2-1, ft);
							#ifdef RTP_SNOW
								flowOffset*=saturate(1-snow_val);
							#endif							
							// stały przepływ na płaskim
							//float slowMotionFct=dot(flowSpeed,flowSpeed);
							//slowMotionFct=saturate(slowMotionFct*50);
							//flowOffset=lerp(tex2Dlod(_BumpMapGlobal, float4(flowUV+float2(0,2*_Time.x*TERRAIN_FlowSpeed*TERRAIN_FlowScale), mip_selector+rtp_mipoffset_flow)).rg*2-1, flowOffset, slowMotionFct );
							//
							flowOffset*=lerp(TERRAIN_WetFlow, TERRAIN_Flow, p)*_uv_Relief_z*TERRAIN_LayerWetStrength;
						#else
							float2 flowOffset=0;
						#endif
					#else
						float2 flowOffset=0;
					#endif
					
			 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
						#if defined(RTP_WET_RIPPLE_TEXTURE) && !defined(RTP_SIMPLE_SHADING)
							float2 rippleUV = IN._uv_Relief.xy*TERRAIN_RippleScale + flowOffset*0.1*flowSpeed/TERRAIN_FlowScale;
						    float4 Ripple;
						  	{
						  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
							    Ripple.xy = Ripple.xy * 2 - 1;
							
							    float DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
							    float TimeFrac = DropFrac - 1.0f + Ripple.z;
							    float DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
							    float FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
							  	roff = Ripple.xy * FinalFactor * 0.35f;
							  	
							  	rippleUV+=float2(0.25,0.25);
						  	 	Ripple = tex2Dlod(TERRAIN_RippleMap, float4(rippleUV, mip_selector + rtp_mipoffset_ripple));
							    Ripple.xy = Ripple.xy * 2 - 1;
							
							    DropFrac = frac(Ripple.w + _Time.x*TERRAIN_DropletsSpeed);
							    TimeFrac = DropFrac - 1.0f + Ripple.z;
							    DropFactor = saturate(0.2f + TERRAIN_RainIntensity * 0.8f - DropFrac);
							    FinalFactor = DropFactor * Ripple.z * sin( clamp(TimeFrac * 9.0f, 0.0f, 3.0f) * 3.1415);
							  	roff += Ripple.xy * FinalFactor * 0.35f;
						  	}
						  	roff*=4*_RippleDamp*lerp(TERRAIN_WetDropletsStrength, 1, p);
						  	#ifdef RTP_SNOW
						  		roff*=saturate(1-snow_val);
						  	#endif							  	
						  	roff+=flowOffset;
						#else
							roff = flowOffset;
						#endif
					#else
						roff = flowOffset;
					#endif
					
			 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
			 		#if !defined(RTP_SIMPLE_SHADING)
						rayPos.xy+=TERRAIN_Refraction*roff*max(p, TERRAIN_WetRefraction);
					#endif
					#endif
				#if defined(RTP_EMISSION) && defined(RTP_HOTAIR_EMISSION) && !defined(RTP_SIMPLE_SHADING)
				} else {
					rayPos.xy += emission_refract_offset*layer_emission;
				#endif					
				}
				
			#endif
			// water
			////////////////////////////////
		
			uvSplat01=frac(rayPos.xy).xyxy*_mult+_off;
			uvSplat01.zw+=float2(0.5,0);
			uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);

//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif	 	
			float4 c;
			float4 gloss;
			float _MipActual=min(mip_selector.x + rtp_mipoffset_color,6);
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, _MipActual.xx)); col = splat_control2.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, _MipActual.xx)); col += splat_control2.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, _MipActual.xx)); col += splat_control2.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, _MipActual.xx)); col += splat_control2.w * c.rgb; gloss.a = c.a;
			
			float glcombined = dot(gloss, splat_control2);
						
			#ifdef RTP_UV_BLEND
			#ifndef RTP_DISTANCE_ONLY_UV_BLEND
				float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult4567 + log2(MixScaleRouted4567), float4(6,6,6,6)); // na layerach 4-7 w trybie 8 layers nie możemy korzystać z UV blendu kanałów 4-7 (bo nie wiemy w define pobierającym teksturę poniżej czy to pierwszy czy drugi set layerów) dlatego używamy tutaj maski 89AB
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasB_TexelSize.x;
				_multMix-=hi_mip_adjustMix;
				_offMix+=0.5*hi_mip_adjustMix;

				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.xxyy)*_multMix+_offMix;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.zzww)*_multMix+_offMix;
				uvSplat23M+=float4(0,0.5,0.5,0.5);		
				
				half4 colBlend = splat_control2.x * UV_BLEND_ROUTE_LAYER_4;
				colBlend += splat_control2.y * UV_BLEND_ROUTE_LAYER_5;
				colBlend += splat_control2.z * UV_BLEND_ROUTE_LAYER_6;
				colBlend += splat_control2.w * UV_BLEND_ROUTE_LAYER_7;
			#endif
			#endif
			
			#if defined(RTP_UV_BLEND)
				#ifndef RTP_DISTANCE_ONLY_UV_BLEND			
					float3 colBlendDes=lerp((dot(colBlend.rgb,0.33333)).xxx, colBlend.rgb, dot(splat_control2, _MixSaturation4567));
					repl=dot(splat_control2, _MixReplace4567);
					repl*= _uv_Relief_wz_no_overlap;
					col=lerp(col, col*colBlendDes*dot(splat_control2, _MixBrightness4567), lerp(blendVal, 1, repl));  
					col = lerp(col,colBlend.rgb,repl);
					// modify glcombined according to UV_BLEND gloss values
					glcombined=lerp(glcombined, colBlend.a, repl*0.5);						
				#endif
			#endif
			
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif
			float RTP_gloss2mask = dot(splat_control2, RTP_gloss2mask4567);
			float _Spec = dot(splat_control2_nobleed, _Spec4567); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control2, RTP_gloss_mult4567);
			float RTP_gloss_shaping = dot(splat_control2, RTP_gloss_shaping4567);
			float gls = saturate(glcombined * RTP_gloss_mult);
			o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec;
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);			
			#endif
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control2, _LayerBrightness2Spec4567);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control2, _LayerSaturation4567));	
			col*=dot(splat_control2, _LayerBrightness4567);  
						
			float3 n;
			float4 normals_combined;
			rayPos.w=mip_selector.x+rtp_mipoffset_bump;
			#ifdef RTP_SNOW
				rayPos.w += snow_depth;
			#endif
			normals_combined = tex2Dlod(_BumpMap45, rayPos.xyww).rgba*splat_control2.rrgg;
			normals_combined+=tex2Dlod(_BumpMap67, rayPos.xyww).rgba*splat_control2.bbaa;
			n.xy=(normals_combined.rg+normals_combined.ba)*2-1;
			n.xy*=_uv_Relief_z;
			n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
			o.Normal=n;
			
			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control2, _VerticalTexture4567) );
			#endif
			
			////////////////////////////////
			// water
			//
	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif
		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=lerp( (1-TERRAIN_WaterGlossDamper), 1, min(_uv_Relief_z, 1-saturate((mip_selector.x+rtp_mipoffset_flow)/4))); // 4ty MIP level lub odległość>near daje całkowite tłumienie
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct;// * saturate(_uv_Relief_z+0.2); // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color
		        
		        o.Normal = lerp(o.Normal, float3(0,0,1), max(p*0.7, _WaterOpacity));
		        o.Normal.xy+=roff;
		        //o.Normal=normalize(o.Normal);
		  		
	        #endif
			// water
			////////////////////////////////
		        
	 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
			#if defined(RTP_SUPER_DETAIL)
				float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(rayPos.xy*_SuperDetailTiling, mip_selector+rtp_mipoffset_superdetail));
				float3 super_detail_norm;
				super_detail_norm.xy = (super_detail.xy*4-2)*(dot(float3(0.8,0.8,0.8),col.rgb)+0.4)+o.Normal.xy;
				super_detail_norm.z = sqrt(1 - saturate(dot(super_detail_norm.xy, super_detail_norm.xy)));
				super_detail_norm=normalize(super_detail_norm);
				float sdVal=_uv_Relief_z*dot(splat_control2, _SuperDetailStrengthNormal4567);
				#if defined(RTP_SNOW)
					sdVal*=saturate(1-snow_depth);
				#endif
				o.Normal=lerp(o.Normal, super_detail_norm, sdVal);		
				#if defined(RTP_SUPER_DTL_MULTS) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
					float near_blend;
					float far_blend;
					float near_far_blend_dist=saturate(_uv_Relief_z-0.5)*2;
					near_blend=lerp(1, global_bump_val.b, dot(splat_control2, _SuperDetailStrengthMultASelfMaskNear4567));
					far_blend=lerp(0, global_bump_val.b, dot(splat_control2, _SuperDetailStrengthMultASelfMaskFar4567));
					col=lerp(col, col*super_detail.z*2, lerp(far_blend, near_blend, near_far_blend_dist)*dot(splat_control2, _SuperDetailStrengthMultA4567));
					near_blend=lerp(1, global_bump_val.a, dot(splat_control2, _SuperDetailStrengthMultBSelfMaskNear4567));
					far_blend=lerp(0, global_bump_val.a, dot(splat_control2, _SuperDetailStrengthMultBSelfMaskFar4567));
					col=lerp(col, col*super_detail.w*2, lerp(far_blend, near_blend, near_far_blend_dist)*dot(splat_control2, _SuperDetailStrengthMultB4567));
				#endif
			#endif
			#endif
			
//#ifdef RTP_SNOW
//}
//#endif

			// snow color
	 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
			#endif
			#endif
			// eof snow color

		 	IN_uv_Relief_Offset.xy=rayPos.xy;
		 	
		 	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		 	//
		 	// self shadowing 
		 	//

	 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED)) )
	 		#if defined(RTP_POM_SHADING) && !defined(RTP_TRIPLANAR)
	 		#if defined(RTP_SOFT_SHADOWS) || defined(RTP_HARD_SHADOWS)
	 			#ifdef RTP_SNOW
	 				rayPos.w=mip_selector.x+rtp_mipoffset_height+snow_depth;
	 			#endif
	 		
				EyeDirTan=IN.lightDir.xyz;
				EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
				delta=_TERRAIN_HeightMap3_TexelSize.x*exp2(rayPos.w)*_TERRAIN_WAVELENGTH_SHADOWS/length(EyeDirTan.xy);
				h_prev=rayPos.z;
				//rayPos.xyz+=EyeDirTan*_TERRAIN_HeightMap3_TexelSize.x*2;
				EyeDirTan*=delta;
		
				hit_flag=false;
				dh_prev=0;
				//_TERRAIN_SHADOW_STEPS=min(_TERRAIN_SHADOW_STEPS, ((EyeDirTan.z>0) ? (1-rayPos.z) : rayPos.z) / abs(EyeDirTan.z));
				for(int i=0; i<_TERRAIN_SHADOW_STEPS; i++) {
					rayPos.xyz+=EyeDirTan;
					_h=dot(splat_control2, tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww));
					hit_flag=_h >= rayPos.z;
					if (hit_flag) break;
					h_prev=_h;
					dh_prev = rayPos.z - _h;
				}
				
		 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && defined(RTP_47SHADING_POM_HI) )
					#ifdef RTP_SOFT_SHADOWS
						if (hit_flag) {
							// secant search
							float scl=dh_prev / ((_h-h_prev) - EyeDirTan.z);
							rayPos.xyz-=EyeDirTan*(1 - scl); // back
							EyeDirTan=IN.lightDir.xyz*_TERRAIN_HeightMap3_TexelSize.x*2*_TERRAIN_WAVELENGTH_SHADOWS;
							EyeDirTan.z/=max(0.001, _TERRAIN_ExtrudeHeight*COLOR_DAMP_VAL);
							float smooth_val=0;
							float break_val=_TERRAIN_ExtrudeHeight*_TERRAIN_ShadowSmoothing;
							for(int i=0; i<_TERRAIN_SHADOW_SMOOTH_STEPS; i++) {
								rayPos.xyz+=EyeDirTan;
								float d=dot(splat_control2, tex2Dlod(_TERRAIN_HeightMap2, rayPos.xyww)) - rayPos.z;
								smooth_val+=saturate(d);
								if (smooth_val>break_val) break;
							}
							shadow_atten=saturate(1-smooth_val/break_val);
						}
					#else
						shadow_atten=hit_flag ? 0 : shadow_atten;
					#endif
				#else
					shadow_atten=hit_flag ? 0 : shadow_atten;
				#endif
		
				shadow_atten=shadow_atten*_TERRAIN_SelfShadowStrength+(1-_TERRAIN_SelfShadowStrength);
				#ifdef RTP_SNOW
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL*(1-snow_depth_lerp));
				#else
					shadow_atten=lerp(1, shadow_atten, saturate(_uv_Relief_z*4-1)*COLOR_DAMP_VAL);
				#endif
	        	#if defined(RTP_WETNESS)
	 				shadow_atten=lerp(shadow_atten, 1, _WaterOpacity);
				#endif
				
			#endif
			#endif
			#endif
			//
		 	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
		 	
	 		// end of splats 4-7 close		 	
	 	}
		#endif //!_4LAYERS 
		
 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO)) )
			#ifdef RTP_POM_SHADING
				o.RTP.y=shadow_atten;
			#endif		 			
		#endif
		
		#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			o_Gloss*=(1-_uv_Relief_w);
			o.Specular=lerp(o.Specular, 0, _uv_Relief_w);
		#endif			
			
	} else {
 		//////////////////////////////////
 		//
 		// far
 		//
 		//////////////////////////////////
				
		#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
		float _off=16*_SplatAtlasC_TexelSize.x;
		float _mult=_off*-2+0.5;
		float4 _offMix=_off;
		float4 _multMix=_mult;
		
		float4 uvSplat01, uvSplat23;
		#ifndef RTP_TRIPLANAR
			float hi_mip_adjust=(exp2(min(mip_selector.x+rtp_mipoffset_color,6)))*_SplatAtlasC_TexelSize.x;
			_mult-=hi_mip_adjust;
			_off+=0.5*hi_mip_adjust;
			
			uvSplat01=frac(IN._uv_Relief.xy).xyxy*_mult+_off;
			uvSplat01.zw+=float2(0.5,0);
			uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
		#endif		
		
		float4 uvSplat01M, uvSplat23M;
		#endif
		
		#if !defined(_4LAYERS) 
		#ifdef RTP_HARD_CROSSPASS
		 	if (false) {
	 	#else
		 	if (splat_controlA_coverage>0.01 && splat_controlB_coverage>0.01) {
	 	#endif
	 		//////////////////////////////////////////////
	 		//
	 		// splats 0-7 far combined
	 		//
	 		///////////////////////////////////////////////
	 		#ifdef RTP_SHOW_OVERLAPPED
	 		o.Emission.r=1;
	 		#endif
	 		
//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif			
#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			if (_uv_Relief_w<1) {
#endif
					
			float4 _MipActual=min(mip_selector.x + rtp_mipoffset_color+MIPmult89AB, 6);
			half4 c;
			float4 gloss;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MipActual.xx)); col = splat_control1.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MipActual.yy)); col += splat_control1.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MipActual.zz)); col += splat_control1.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MipActual.ww)); col += splat_control1.w * c.rgb; gloss.a = c.a;
			
			float glcombined = dot(gloss, splat_control1);
							
			#ifdef RTP_UV_BLEND
				float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult89AB + log2(MixScaleRouted89AB), float4(6,6,6,6));
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
				float4 _multMix89AB=_multMix - hi_mip_adjustMix; // _multMix / _offMix potrzebujemy poniżej dla kanałów 4-7
				float4 _offMix89AB=_offMix + 0.5*hi_mip_adjustMix;
							
				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy)*_multMix89AB+_offMix89AB;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww)*_multMix89AB+_offMix89AB;
				uvSplat23M+=float4(0,0.5,0.5,0.5);			
				
				half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
				colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
				colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
				colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
			#endif
						
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, _MipActual.xx)); col += splat_control2.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, _MipActual.yy)); col += splat_control2.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, _MipActual.zz)); col += splat_control2.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, _MipActual.ww)); col += splat_control2.w * c.rgb; gloss.a = c.a;

			glcombined += dot(gloss, splat_control2);
						
			#ifdef RTP_UV_BLEND
				_MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult4567 + log2(MixScaleRouted4567), float4(6,6,6,6)); // na layerach 4-7 w trybie 8 layers nie możemy korzystać z UV blendu kanałów 4-7 (bo nie wiemy w define pobierającym teksturę poniżej czy to pierwszy czy drugi set layerów) dlatego używamy tutaj maski 89AB
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
				_multMix-=hi_mip_adjustMix;
				_offMix+=0.5*hi_mip_adjustMix;
							
				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.xxyy)*_multMix+_offMix;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.zzww)*_multMix+_offMix;
				uvSplat23M+=float4(0,0.5,0.5,0.5);			
				
				colBlend += splat_control2.x * UV_BLEND_ROUTE_LAYER_4;
				colBlend += splat_control2.y * UV_BLEND_ROUTE_LAYER_5;
				colBlend += splat_control2.z * UV_BLEND_ROUTE_LAYER_6;
				colBlend += splat_control2.w * UV_BLEND_ROUTE_LAYER_7;
			#endif
			
			#if defined(RTP_UV_BLEND)
				float3 colBlendDes=lerp((dot(colBlend.rgb,0.33333)).xxx, colBlend.rgb, dot(splat_control1, _MixSaturation89AB) + dot(splat_control2, _MixSaturation4567));
				repl=dot(splat_control1, _MixReplace89AB) + dot(splat_control2, _MixReplace4567);
				repl*= _uv_Relief_wz_no_overlap;
				col=lerp(col, col*colBlendDes*(dot(splat_control1, _MixBrightness89AB) + dot(splat_control2, _MixBrightness4567)), lerp(blendVal, 1, repl));  
				col=lerp(col,colBlend.rgb,repl);
				// modify glcombined according to UV_BLEND gloss values
				glcombined=lerp(glcombined, colBlend.a, repl*0.5);			
			#endif
			
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif			
			float RTP_gloss2mask = dot(splat_control1, RTP_gloss2mask89AB)+dot(splat_control2, RTP_gloss2mask4567);
			float _Spec = dot(splat_control1_nobleed, _Spec89AB) + dot(splat_control2_nobleed, _Spec4567); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control1, RTP_gloss_mult89AB)+dot(splat_control2, RTP_gloss_mult4567);
			float RTP_gloss_shaping = dot(splat_control1, RTP_gloss_shaping89AB)+dot(splat_control2, RTP_gloss_shaping4567);
			float gls = saturate(glcombined * RTP_gloss_mult);
			o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec; // *heightblend_AO // przemnóż przez damping dla warstw 4567
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);
			#endif
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control1, _LayerBrightness2Spec89AB) + dot(splat_control2, _LayerBrightness2Spec4567);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control1, _LayerSaturation89AB) + dot(splat_control2, _LayerSaturation4567));
			col*=dot(splat_control1, _LayerBrightness89AB) + dot(splat_control2, _LayerBrightness4567);  
						
#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			o_Gloss*=(1-_uv_Relief_w);
			o.Specular=lerp(o.Specular, 0, _uv_Relief_w);
			}
#endif
			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control1, _VerticalTexture89AB) + dot(splat_control2, _VerticalTexture4567));
			#endif
						
//#ifdef RTP_SNOW
//}
//#endif	
			// snow color
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif				
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif			
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
				half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif				
			#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
				half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
				GETrtp_snow_TEX
			#endif		
			#endif
			// snow color
				
			#if defined(RTP_SUPER_DETAIL) && defined (RTP_SUPER_DTL_MULTS) && !defined(RTP_SIMPLE_SHADING) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
				float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_SuperDetailTiling, mip_selector+rtp_mipoffset_superdetail));

				float far_blend;
				far_blend=lerp(0, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskFar89AB) + dot(splat_control2, _SuperDetailStrengthMultASelfMaskFar4567));
				col=lerp(col, col*super_detail.z*2, far_blend*(dot(splat_control1, _SuperDetailStrengthMultA89AB)+dot(splat_control2, _SuperDetailStrengthMultA4567)));
				far_blend=lerp(0, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskFar89AB) + dot(splat_control2, _SuperDetailStrengthMultBSelfMaskFar4567));
				col=lerp(col, col*super_detail.w*2, far_blend*(dot(splat_control1, _SuperDetailStrengthMultB89AB)+dot(splat_control2, _SuperDetailStrengthMultB4567)));
			#endif
			
			////////////////////////////////
			// water
			//
			float4 water_splat_control1=splat_control1;
			float4 water_splat_control1_nobleed=splat_control1_nobleed;
			float4 water_splat_control2=splat_control2;
			float4 water_splat_control2_nobleed=splat_control2_nobleed;
			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control1, TERRAIN_LayerWetStrength89AB)+dot(water_splat_control2, TERRAIN_LayerWetStrength4567);
				float TERRAIN_WaterLevel=dot(water_splat_control1, TERRAIN_WaterLevel89AB)+dot(water_splat_control2, TERRAIN_WaterLevel4567);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control1, TERRAIN_WaterLevelSlopeDamp89AB)+dot(water_splat_control2, TERRAIN_WaterLevelSlopeDamp4567);
				//float TERRAIN_Flow=dot(water_splat_control1, TERRAIN_Flow89AB)+dot(water_splat_control2, TERRAIN_Flow4567);
				//float TERRAIN_WetFlow=dot(water_splat_control1, TERRAIN_WetFlow89AB)+dot(water_splat_control2, TERRAIN_WetFlow4567);
				float TERRAIN_WaterEdge=dot(water_splat_control1, TERRAIN_WaterEdge89AB)+dot(water_splat_control2, TERRAIN_WaterEdge4567);
				//float TERRAIN_Refraction=dot(water_splat_control1, TERRAIN_Refraction89AB)+dot(water_splat_control2, TERRAIN_Refraction4567);
				//float TERRAIN_WetRefraction=dot(water_splat_control1, TERRAIN_WetRefraction89AB)+dot(water_splat_control2, TERRAIN_WetRefraction4567);
				
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control1_nobleed, TERRAIN_WetSpecularity89AB) + dot(water_splat_control2_nobleed, TERRAIN_WetSpecularity4567); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control1, TERRAIN_WetGloss89AB)+dot(water_splat_control2, TERRAIN_WetGloss4567);
					TERRAIN_WaterSpecularity = dot(water_splat_control1_nobleed, TERRAIN_WaterSpecularity89AB) + dot(water_splat_control2_nobleed, TERRAIN_WaterSpecularity4567); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control1, TERRAIN_WaterGloss89AB)+dot(water_splat_control2, TERRAIN_WaterGloss4567);
					TERRAIN_WaterGlossDamper=dot(water_splat_control1, TERRAIN_WaterGlossDamper89AB)+dot(water_splat_control2, TERRAIN_WaterGlossDamper4567);
					TERRAIN_WaterColor=half4( dot(water_splat_control1, TERRAIN_WaterColorR89AB)+dot(water_splat_control2, TERRAIN_WaterColorR4567), dot(water_splat_control1, TERRAIN_WaterColorG89AB)+dot(water_splat_control2, TERRAIN_WaterColorG4567), dot(water_splat_control1, TERRAIN_WaterColorB89AB)+dot(water_splat_control2, TERRAIN_WaterColorB4567), dot(water_splat_control1, TERRAIN_WaterColorA89AB)+dot(water_splat_control2, TERRAIN_WaterColorA4567) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control1, TERRAIN_WaterIBL_SpecWetStrength89AB)+dot(water_splat_control2, TERRAIN_WaterIBL_SpecWetStrength4567);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control1, TERRAIN_WaterIBL_SpecWaterStrength89AB)+dot(water_splat_control2, TERRAIN_WaterIBL_SpecWaterStrength4567);
					
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);
					
					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=(dot(water_splat_control1, TERRAIN_WaterOpacity89AB) + dot(water_splat_control2, TERRAIN_WaterOpacity4567))*p;
					#if defined(RTP_EMISSION)
						float wEmission = (dot(water_splat_control1, TERRAIN_WaterEmission89AB) + dot(water_splat_control2, TERRAIN_WaterEmission4567))*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
					#endif
				}
			#endif

	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif

		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=(1-TERRAIN_WaterGlossDamper); // (_uv_Relief_z==0)
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct;// * 0.2; // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color
				
	        #endif
			// water
			////////////////////////////////
						
	 	} else if (splat_controlA_coverage>splat_controlB_coverage)
		#endif // !_4LAYERS
	 	{
	 		//////////////////////////////////////////////
	 		//
	 		// splats 0-3 far
	 		//
	 		///////////////////////////////////////////////
	 		
			#ifdef RTP_HARD_CROSSPASS
				splat_control1 /= dot(splat_control1, 1);
				#ifdef NOSPEC_BLEED		
					splat_control1_nobleed=saturate(splat_control1-float4(0.5,0.5,0.5,0.5))*2;
				#else
					splat_control1_nobleed=splat_control1;
				#endif
				splat_control2 = 0;
			#endif

//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif		
#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			if (_uv_Relief_w<1) {
#endif			

			half4 c;
			float4 gloss=0;			
			
			#ifdef RTP_TRIPLANAR
				//
				// triplanar no blend - simple case
				//
				float4 _MixMipActual=uvTRI.zzzz+rtp_mipoffset_color+MIPmult89AB;
				float4 tmp_gloss;
				#if defined(RTP_USE_COLOR_ATLAS)
					_MixMipActual=min( _MixMipActual, float4(6,6,6,6) );
					float4 hi_mip_adjustTMP1=(exp2(_MixMipActual))*_SplatAtlasC_TexelSize.x; 
					uvSplat01=frac(uvTRI.xy).xyxy*(_mult.xxxx-hi_mip_adjustTMP1)+_off.xxxx+0.5*hi_mip_adjustTMP1;
					uvSplat01.zw+=float2(0.5,0);
					uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MixMipActual.xx)); col += c.rgb*splat_control1.x; tmp_gloss.r = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MixMipActual.yy)); col += c.rgb*splat_control1.y; tmp_gloss.g = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MixMipActual.zz)); col += c.rgb*splat_control1.z; tmp_gloss.b = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MixMipActual.ww)); col += c.rgb*splat_control1.w; tmp_gloss.a = c.a;
				#else
					c = tex2Dlod(_SplatC0, float4(uvTRI.xy,_MixMipActual.xx)); col += c.rgb*splat_control1.x; tmp_gloss.r=c.a;
					c = tex2Dlod(_SplatC1, float4(uvTRI.xy,_MixMipActual.yy)); col += c.rgb*splat_control1.y; tmp_gloss.g=c.a;
					c = tex2Dlod(_SplatC2, float4(uvTRI.xy,_MixMipActual.zz)); col += c.rgb*splat_control1.z; tmp_gloss.b=c.a;
					c = tex2Dlod(_SplatC3, float4(uvTRI.xy,_MixMipActual.ww)); col += c.rgb*splat_control1.w; tmp_gloss.a=c.a;
				#endif						
				gloss=tmp_gloss;				
			
				#if defined(RTP_UV_BLEND) 
					_MixMipActual=uvTRI.zzzz+rtp_mipoffset_color+MIPmult89AB+log2(MixScaleRouted89AB);
					#if defined(RTP_USE_COLOR_ATLAS)
						_MixMipActual=min(_MixMipActual, float4(6,6,6,6));
						_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
						float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
						_multMix-=hi_mip_adjustMix;
						_offMix+=0.5*hi_mip_adjustMix;
						float4 uvSplat01M=frac(uvTRI.xyxy*MixScaleRouted89AB.xxyy)*_multMix+_offMix;
						uvSplat01M.zw+=float2(0.5,0);
						float4 uvSplat23M=frac(uvTRI.xyxy*MixScaleRouted89AB.zzww)*_multMix+_offMix;
						uvSplat23M+=float4(0,0.5,0.5,0.5);		
					#else
						float4 uvSplat01M=uvTRI.xyxy*MixScaleRouted89AB.xxyy;
						float4 uvSplat23M=uvTRI.xyxy*MixScaleRouted89AB.zzww;
					#endif
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
					//colBlend=lerp(half3(0.5, 0.5, 0.5, 0), colBlend, saturate((triplanar_blend_simple-0.75)*4));
				#endif

				//
				// EOF triplanar
				//
			#else
				//
				// no triplanar
				//
				#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
					float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color+MIPmult89AB, float4(6,6,6,6));
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, _MixMipActual.xx)); col = splat_control1.x * c.rgb; gloss.r = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, _MixMipActual.yy)); col += splat_control1.y * c.rgb; gloss.g = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, _MixMipActual.zz)); col += splat_control1.z * c.rgb; gloss.b = c.a;
					c = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, _MixMipActual.ww)); col += splat_control1.w * c.rgb; gloss.a = c.a;
				#else
					#ifdef FORCE_ANISO
						float4 _MixMipActual;
						col=col_aniso;
						gloss=gloss_aniso;
					#else
						float4 _MixMipActual=mip_selector.xxxx + rtp_mipoffset_color+MIPmult89AB;
						c = tex2Dlod(_SplatC0, float4(IN._uv_Relief.xy, _MixMipActual.xx)); col = splat_control1.x * c.rgb; gloss.r = c.a;
						c = tex2Dlod(_SplatC1, float4(IN._uv_Relief.xy, _MixMipActual.yy)); col += splat_control1.y * c.rgb; gloss.g = c.a;
						c = tex2Dlod(_SplatC2, float4(IN._uv_Relief.xy, _MixMipActual.zz)); col += splat_control1.z * c.rgb; gloss.b = c.a;
						c = tex2Dlod(_SplatC3, float4(IN._uv_Relief.xy, _MixMipActual.ww)); col += splat_control1.w * c.rgb; gloss.a = c.a;				
					#endif
				#endif
				
				//
				// EOF no triplanar
				//
			#endif

			#if defined(RTP_UV_BLEND) && !defined(RTP_TRIPLANAR)
				#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
					_MixMipActual=min(mip_selector.xxxx+rtp_mipoffset_color+ MIPmult89AB+log2(MixScaleRouted89AB), float4(6,6,6,6));
					_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
					float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
					_multMix-=hi_mip_adjustMix;
					_offMix+=0.5*hi_mip_adjustMix;
				
					uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy)*_multMix+_offMix;
					uvSplat01M.zw+=float2(0.5,0);
					uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww)*_multMix+_offMix;
					uvSplat23M+=float4(0,0.5,0.5,0.5);		
					
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
				#else
					_MixMipActual=mip_selector.xxxx+rtp_mipoffset_color+ MIPmult89AB+log2(MixScaleRouted89AB);
					
					float4 uvSplat01M=IN._uv_Relief.xyxy*MixScaleRouted89AB.xxyy;
					float4 uvSplat23M=IN._uv_Relief.xyxy*MixScaleRouted89AB.zzww;
					
					half4 colBlend = splat_control1.x * UV_BLEND_ROUTE_LAYER_0;
					colBlend += splat_control1.y * UV_BLEND_ROUTE_LAYER_1;
					colBlend += splat_control1.z * UV_BLEND_ROUTE_LAYER_2;
					colBlend += splat_control1.w * UV_BLEND_ROUTE_LAYER_3;
				#endif
			#endif
			
			float glcombined = dot(gloss, splat_control1);
			#if defined(RTP_UV_BLEND)
				float3 colBlendDes=lerp( (dot(colBlend.rgb,0.33333)).xxx, colBlend.rgb, dot(splat_control1, _MixSaturation89AB) );
				repl=dot(splat_control1, _MixReplace89AB);
				repl*= _uv_Relief_wz_no_overlap;
				col=lerp(col, col*colBlendDes*dot(splat_control1, _MixBrightness89AB), lerp(blendVal, 1, repl));  
				col=lerp(col,colBlend.rgb,repl);
				// modify glcombined according to UV_BLEND gloss values
				glcombined=lerp(glcombined, colBlend.a, repl*0.5);			
			#endif
			
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif
			float RTP_gloss2mask = dot(splat_control1, RTP_gloss2mask89AB);
			float _Spec = dot(splat_control1_nobleed, _Spec89AB); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control1, RTP_gloss_mult89AB);
			float RTP_gloss_shaping = dot(splat_control1, RTP_gloss_shaping89AB);
			float gls = saturate(glcombined * RTP_gloss_mult);
			o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec;
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);		
			#endif
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control1, _LayerBrightness2Spec89AB);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control1, _LayerSaturation89AB) );
			col*=dot(splat_control1, _LayerBrightness89AB);  
						
#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			o_Gloss*=(1-_uv_Relief_w);
			o.Specular=lerp(o.Specular, 0, _uv_Relief_w);
			}
#endif		
	
			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control1, _VerticalTexture89AB) );
			#endif
			
//#ifdef RTP_SNOW
//}
//#endif			

			// snow color
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
			#if !defined(_4LAYERS) || defined(RTP_USE_COLOR_ATLAS)
			
				// ponownie przelicz uvSplat01 i uvSplat23 dla triplanar (nadpisane powyżej)
				#if defined(RTP_USE_COLOR_ATLAS) && defined(RTP_TRIPLANAR)
					float3 uvTRItmp=float3(IN._uv_Relief.xy, mip_selector.x+rtp_mipoffset_color);
					float hi_mip_adjustTMP2=(exp2(min(uvTRItmp.z,6)))*_SplatAtlasC_TexelSize.x; 
					uvSplat01=frac(uvTRItmp.xy).xyxy*(_mult-hi_mip_adjustTMP2)+_off+0.5*hi_mip_adjustTMP2;
					uvSplat01.zw+=float2(0.5,0);
					uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
				#endif
			
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
			#else
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatB0, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatC0, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatB1, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatC1, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatB2, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatC2, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatB3, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatC3, float4(IN._uv_Relief.xy, mip_selector + rtp_mipoffset_color));
					GETrtp_snow_TEX
				#endif
			#endif	
			#endif
			// eof snow color
			
			#if defined(RTP_SUPER_DETAIL) && defined (RTP_SUPER_DTL_MULTS) && !defined(RTP_SIMPLE_SHADING) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
				float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_SuperDetailTiling, mip_selector+rtp_mipoffset_superdetail));

				float far_blend;
				far_blend=lerp(0, global_bump_val.b, dot(splat_control1, _SuperDetailStrengthMultASelfMaskFar89AB));
				col=lerp(col, col*super_detail.z*2, far_blend*dot(splat_control1, _SuperDetailStrengthMultA89AB));
				far_blend=lerp(0, global_bump_val.a, dot(splat_control1, _SuperDetailStrengthMultBSelfMaskFar89AB));
				col=lerp(col, col*super_detail.w*2, far_blend*dot(splat_control1, _SuperDetailStrengthMultB89AB));
			#endif
			
			////////////////////////////////
			// water
			//
			float4 water_splat_control=splat_control1;
			float4 water_splat_control_nobleed=splat_control1_nobleed;
			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control, TERRAIN_LayerWetStrength89AB);
				float TERRAIN_WaterLevel=dot(water_splat_control, TERRAIN_WaterLevel89AB);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control, TERRAIN_WaterLevelSlopeDamp89AB);
				//float TERRAIN_Flow=dot(water_splat_control, TERRAIN_Flow89AB);
				//float TERRAIN_WetFlow=dot(water_splat_control, TERRAIN_WetFlow89AB);
				float TERRAIN_WaterEdge=dot(water_splat_control, TERRAIN_WaterEdge89AB);
				//float TERRAIN_Refraction=dot(water_splat_control, TERRAIN_Refraction89AB);
				//float TERRAIN_WetRefraction=dot(water_splat_control, TERRAIN_WetRefraction89AB);
				
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control_nobleed, TERRAIN_WetSpecularity89AB); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control, TERRAIN_WetGloss89AB);
					TERRAIN_WaterSpecularity = dot(water_splat_control_nobleed, TERRAIN_WaterSpecularity89AB); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control, TERRAIN_WaterGloss89AB);
					TERRAIN_WaterGlossDamper=dot(water_splat_control, TERRAIN_WaterGlossDamper89AB);
					TERRAIN_WaterColor=half4( dot(water_splat_control, TERRAIN_WaterColorR89AB), dot(water_splat_control, TERRAIN_WaterColorG89AB), dot(water_splat_control, TERRAIN_WaterColorB89AB), dot(water_splat_control, TERRAIN_WaterColorA89AB) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWetStrength89AB);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWaterStrength89AB);
				
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);
					
					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=dot(water_splat_control, TERRAIN_WaterOpacity89AB)*p;
					#if defined(RTP_EMISSION)
						float wEmission = dot(water_splat_control, TERRAIN_WaterEmission89AB)*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
					#endif					
				}
			#endif

	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif

		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=(1-TERRAIN_WaterGlossDamper); // (_uv_Relief_z==0)
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct;//  * 0.2; // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color
		        
	        #endif
			// water
			////////////////////////////////
						
	 	}
	 	
	 	#ifndef _4LAYERS
	 	else {
	 		//////////////////////////////////////////////
	 		//
	 		// splats 4-7 far
	 		//
	 		///////////////////////////////////////////////
	 		
			#ifdef RTP_HARD_CROSSPASS
				splat_control2 /= dot(splat_control2, 1);
				#ifdef NOSPEC_BLEED		
					splat_control2_nobleed=saturate(splat_control2-float4(0.5,0.5,0.5,0.5))*2;
				#else
					splat_control2_nobleed=splat_control2;
				#endif
				splat_control1 = 0;
			#endif
						
//#ifdef RTP_SNOW
//if (snow_MayBeNotFullyCovered_flag) {
//#endif		

#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			if (_uv_Relief_w<1) {
#endif	
			float4 _MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color+MIPmult4567, float4(6,6,6,6));
			#if defined(RTP_TRIPLANAR)
				float4 hi_mip_adjustTMP1=(exp2(_MixMipActual))*_SplatAtlasB_TexelSize.x; 
				uvSplat01=frac(uvTRI.xy).xyxy*(_mult.xxxx-hi_mip_adjustTMP1)+_off.xxxx+0.5*hi_mip_adjustTMP1;
				uvSplat01.zw+=float2(0.5,0);
				uvSplat23=uvSplat01.xyxy+float4(0,0.5,0.5,0.5);
			#endif			
			half4 c;
			float4 gloss;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, _MixMipActual.xx)); col = splat_control2.x * c.rgb; gloss.r = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, _MixMipActual.yy)); col += splat_control2.y * c.rgb; gloss.g = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, _MixMipActual.zz)); col += splat_control2.z * c.rgb; gloss.b = c.a;
			c = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, _MixMipActual.ww)); col += splat_control2.w * c.rgb; gloss.a = c.a;
			
			#ifdef RTP_UV_BLEND
				_MixMipActual=min(mip_selector.xxxx + rtp_mipoffset_color + MIPmult4567 + log2(MixScaleRouted4567), float4(6,6,6,6)); // na layerach 4-7 w trybie 8 layers nie możemy korzystać z UV blendu kanałów 4-7 (bo nie wiemy w define pobierającym teksturę poniżej czy to pierwszy czy drugi set layerów) dlatego używamy tutaj maski 89AB
				_MixMipActual=max(float4(0,0,0,0),_MixMipActual); // nie może byc ujemny do obj hi_mip_adjustMix poniżej
				float4 hi_mip_adjustMix=exp2(_MixMipActual)*_SplatAtlasC_TexelSize.x;
				_multMix-=hi_mip_adjustMix;
				_offMix+=0.5*hi_mip_adjustMix;

				uvSplat01M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.xxyy)*_multMix+_offMix;
				uvSplat01M.zw+=float2(0.5,0);
				uvSplat23M=frac(IN._uv_Relief.xyxy*MixScaleRouted4567.zzww)*_multMix+_offMix;
				uvSplat23M+=float4(0,0.5,0.5,0.5);	

				half4 colBlend = splat_control2.x * UV_BLEND_ROUTE_LAYER_4;
				colBlend += splat_control2.y * UV_BLEND_ROUTE_LAYER_5;
				colBlend += splat_control2.z * UV_BLEND_ROUTE_LAYER_6;
				colBlend += splat_control2.w * UV_BLEND_ROUTE_LAYER_7;
			#endif
						
			float glcombined = dot(gloss, splat_control2);
			#if defined(RTP_UV_BLEND)
				float3 colBlendDes=lerp( (dot(colBlend.rgb,0.33333)).xxx, colBlend.rgb, dot(splat_control2, _MixSaturation4567) );
				repl=dot(splat_control2, _MixReplace4567);
				repl*= _uv_Relief_w*(1-_uv_Relief_z);
				col=lerp(col, col*colBlendDes*dot(splat_control2, _MixBrightness4567), lerp(blendVal, 1, repl));  
				col=lerp(col,colBlend.rgb,repl);
				// modify glcombined according to UV_BLEND gloss values
				glcombined=lerp(glcombined, colBlend.a, repl*0.5);
			#endif
			// calc gloss with UV_BLEND value taken into account
			#if defined(RTP_COLORSPACE_LINEAR)
			//glcombined=FastToLinear(glcombined);
			#endif	
			float RTP_gloss2mask = dot(splat_control2, RTP_gloss2mask4567);
			float _Spec = dot(splat_control2_nobleed, _Spec4567); // anti-bleed subtraction
			float RTP_gloss_mult = dot(splat_control2, RTP_gloss_mult4567);
			float RTP_gloss_shaping = dot(splat_control2, RTP_gloss_shaping4567);
			float gls = saturate(glcombined * RTP_gloss_mult);
			o_Gloss =  lerp(1, gls, RTP_gloss2mask) * _Spec;
			float2 gloss_shaped=float2(gls, 1-gls);
			gloss_shaped=gloss_shaped*gloss_shaped*gloss_shaped;
			gls=lerp(gloss_shaped.x, 1-gloss_shaped.y, RTP_gloss_shaping);
			o.Specular = saturate(gls);
			// gloss vs. fresnel dependency
			#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
			o.RTP.x*=lerp(1, 1-fresnelAtten, o.Specular*0.9+0.1);
			#endif
			// desaturation/brightness per layer + spec driven by resultant albedo color
			half colDesat=dot(col,0.33333);
			float brightness2Spec=dot(splat_control2, _LayerBrightness2Spec4567);
			o_Gloss*=lerp(1, colDesat, brightness2Spec);
			colAlbedo=col;
			col=lerp(colDesat.xxx, col, dot(splat_control2, _LayerSaturation4567) );
			col*=dot(splat_control2, _LayerBrightness4567);  
						
#if defined(SIMPLE_FAR) && defined(COLOR_MAP)
			o_Gloss*=(1-_uv_Relief_w);
			o.Specular=lerp(o.Specular, 0, _uv_Relief_w);
			}
#endif	

			#ifdef RTP_VERTICAL_TEXTURE
				float2 vert_tex_uv=float2(0, IN.lightDir.w/_VerticalTextureTiling) + _VerticalTextureGlobalBumpInfluence*global_bump_val.xy;
				#ifdef RTP_VERTALPHA_CAUSTICS
					half3 vert_tex=tex2Dlod(TERRAIN_CausticsTex, float4(vert_tex_uv, mip_selector-log2( TERRAIN_CausticsTex_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#else
					half3 vert_tex=tex2Dlod(_VerticalTexture, float4(vert_tex_uv, mip_selector-log2( _VerticalTexture_TexelSize.y/(_SplatC0_TexelSize.x*(_TERRAIN_ReliefTransformTriplanarZ/_VerticalTextureTiling)) ))).rgb;
				#endif
				col=lerp(col, col*vert_tex*2, dot(splat_control2, _VerticalTexture4567) );
			#endif			
			
//#ifdef RTP_SNOW
//}
//#endif		

			// snow color
	 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
			#if defined(RTP_SNOW) && !defined(RTP_SIMPLE_SHADING) && ( defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6) || defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7) )
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_0)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_4)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_1)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif			
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_5)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat01.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif			
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_2)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif				
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_6)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.xy, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif			
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_3)
					half4 csnow = tex2Dlod(_SplatAtlasC, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif				
				#if defined(RTP_SNW_CHOOSEN_LAYER_COLOR_7)
					half4 csnow = tex2Dlod(_SplatAtlasB, float4(uvSplat23.zw, min(mip_selector + rtp_mipoffset_color,6)));
					GETrtp_snow_TEX
				#endif		
			#endif
			#endif
			// eof snow color
			
	 		#if !defined(RTP_HARD_CROSSPASS) || ( defined(RTP_HARD_CROSSPASS) && (defined(RTP_47SHADING_POM_HI) || defined(RTP_47SHADING_POM_MED) || defined(RTP_47SHADING_POM_LO) || defined(RTP_47SHADING_PM)) )
			#if defined(RTP_SUPER_DETAIL) && defined (RTP_SUPER_DTL_MULTS) && !defined(RTP_SIMPLE_SHADING) && !defined(RTP_WETNESS) && !defined(RTP_REFLECTION)
				float4 super_detail=tex2Dlod(_BumpMapGlobal, float4(IN._uv_Relief.xy*_SuperDetailTiling, mip_selector+rtp_mipoffset_superdetail));
				
				float far_blend;
				far_blend=lerp(0, global_bump_val.b, dot(splat_control2, _SuperDetailStrengthMultASelfMaskFar4567));
				col=lerp(col, col*super_detail.z*2, far_blend*dot(splat_control2, _SuperDetailStrengthMultA4567));
				far_blend=lerp(0, global_bump_val.a, dot(splat_control2, _SuperDetailStrengthMultBSelfMaskFar4567));
				col=lerp(col, col*super_detail.w*2, far_blend*dot(splat_control2, _SuperDetailStrengthMultB4567));
			#endif
			#endif
						
			////////////////////////////////
			// water
			// 
			float4 water_splat_control=splat_control2;
			float4 water_splat_control_nobleed=splat_control2_nobleed;
			
			#ifdef RTP_WETNESS
				TERRAIN_LayerWetStrength=dot(water_splat_control, TERRAIN_LayerWetStrength4567);
				float TERRAIN_WaterLevel=dot(water_splat_control, TERRAIN_WaterLevel4567);
				TERRAIN_WaterLevel*=(1-wet_height_fct);
				#ifdef RTP_CAUSTICS
					TERRAIN_WaterLevel*=damp_fct_caustics_inv;
				#endif					
				float TERRAIN_WaterLevelSlopeDamp=dot(water_splat_control, TERRAIN_WaterLevelSlopeDamp4567);
				//float TERRAIN_Flow=dot(water_splat_control, TERRAIN_Flow4567);
				//float TERRAIN_WetFlow=dot(water_splat_control, TERRAIN_WetFlow4567);
				float TERRAIN_WaterEdge=dot(water_splat_control, TERRAIN_WaterEdge4567);
				//float TERRAIN_Refraction=dot(water_splat_control, TERRAIN_Refraction4567);
				//float TERRAIN_WetRefraction=dot(water_splat_control, TERRAIN_WetRefraction4567);
			
				#if defined(RTP_SNW_COVERAGE_FROM_WETNESS)				
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#else
					TERRAIN_LayerWetStrength*=saturate( 2*(1 - water_mask-perlinmask*(1-TERRAIN_LayerWetStrength*TERRAIN_GlobalWetness)) )*TERRAIN_GlobalWetness;
				#endif
				#ifdef RTP_TRIPLANAR
				// remove wetness from ceiling
				TERRAIN_LayerWetStrength*=saturate(saturate(flat_dir.z+0.1)*4);
				#endif
				#ifdef RTP_SNOW
				//TERRAIN_LayerWetStrength*=saturate(1-snow_val);
				#endif				
				if (TERRAIN_LayerWetStrength>0) {
					TERRAIN_WetSpecularity = dot(water_splat_control_nobleed, TERRAIN_WetSpecularity4567); // anti-bleed subtraction
					TERRAIN_WetGloss=dot(water_splat_control, TERRAIN_WetGloss4567);
					TERRAIN_WaterSpecularity = dot(water_splat_control_nobleed, TERRAIN_WaterSpecularity4567); // anti-bleed subtraction
					TERRAIN_WaterGloss=dot(water_splat_control, TERRAIN_WaterGloss4567);
					TERRAIN_WaterGlossDamper=dot(water_splat_control, TERRAIN_WaterGlossDamper4567);
					TERRAIN_WaterColor=half4( dot(water_splat_control, TERRAIN_WaterColorR4567), dot(water_splat_control, TERRAIN_WaterColorG4567), dot(water_splat_control, TERRAIN_WaterColorB4567), dot(water_splat_control, TERRAIN_WaterColorA4567) );
					TERRAIN_WaterIBL_SpecWetStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWetStrength4567);
					TERRAIN_WaterIBL_SpecWaterStrength=dot(water_splat_control, TERRAIN_WaterIBL_SpecWaterStrength4567);
					
					wetSlope=saturate(wetSlope*TERRAIN_WaterLevelSlopeDamp);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength*2);
					TERRAIN_WaterLevel=clamp(TERRAIN_WaterLevel + ((TERRAIN_LayerWetStrength - 1) - wetSlope)*2, 0, 2);
					TERRAIN_LayerWetStrength=saturate(TERRAIN_LayerWetStrength - (1-TERRAIN_LayerWetStrength)*actH*0.25);
					
					p = saturate((TERRAIN_WaterLevel - actH -(1-actH)*perlinmask*0.5)*TERRAIN_WaterEdge);
					p*=p;
			        _WaterOpacity=dot(water_splat_control, TERRAIN_WaterOpacity4567)*p;
					#if defined(RTP_EMISSION)
						float wEmission = dot(water_splat_control, TERRAIN_WaterEmission4567)*p;
						layer_emission = lerp( layer_emission, wEmission, _WaterOpacity);
						layer_emission = max( layer_emission, wEmission*(1-_WaterOpacity) );
					#endif
				}
			#endif

	        #if defined(RTP_WETNESS)
				#ifdef RTP_CAUSTICS
					TERRAIN_WetSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WetGloss*=damp_fct_caustics_inv;
					TERRAIN_WaterSpecularity*=damp_fct_caustics_inv;
					TERRAIN_WaterGloss*=damp_fct_caustics_inv;
				#endif
		  		float porosity = 1-saturate(o.Specular * 4 - 1);
				#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		        o.RTP.x = lerp(o.RTP.x, TERRAIN_WaterColor.a, TERRAIN_LayerWetStrength);
		        #endif
				float wet_fct = saturate(TERRAIN_LayerWetStrength*2-0.4);
				float glossDamper=(1-TERRAIN_WaterGlossDamper); // (_uv_Relief_z==0)
		        o.Specular += lerp(TERRAIN_WetGloss, TERRAIN_WaterGloss, p) * wet_fct * glossDamper; // glossiness boost
		        o.Specular=saturate(o.Specular);
		        o_Gloss += lerp(TERRAIN_WetSpecularity, TERRAIN_WaterSpecularity, p) * wet_fct ;// * 0.2; // spec boost
		        o_Gloss=max(0, o_Gloss);
		  		
		  		// col - saturation, brightness
		  		half3 col_sat=col.rgb*col.rgb; // saturation z utrzymaniem jasności
		  		col_sat*=dot(col.rgb,1)/dot(col_sat,1);
		  		wet_fct=saturate(TERRAIN_LayerWetStrength*(2-perlinmask));
		  		porosity*=0.5;
		  		col.rgb=lerp(col.rgb, col_sat, wet_fct*porosity);
				col.rgb*=1-wet_fct*TERRAIN_WetDarkening*(porosity+0.5);
						  		
		        // col - colorisation
		        col.rgb *= lerp(half3(1,1,1), TERRAIN_WaterColor.rgb, p*p);
		        
	 			// col - opacity
				col.rgb = lerp(col.rgb, TERRAIN_WaterColor.rgb, _WaterOpacity );
				colAlbedo=lerp(colAlbedo, col, _WaterOpacity); // potrzebne do spec color
		        
	        #endif
			// water
			////////////////////////////////	
							
	 	}
		#endif

	 	IN_uv_Relief_Offset.xy=IN._uv_Relief.xy;
	}
	
	float3 norm_snowCov=o.Normal;
	
	// far distance normals from uv blend scale
	#if defined(RTP_UV_BLEND)	&& defined(RTP_NORMALS_FOR_REPLACE_UV_BLEND)
	{
			float3 n;
			float2 normalsN;
			float4 mipN=mip_selector.xxxx+rtp_mipoffset_bump+MIPmult89AB+log2(MixScaleRouted89AB);
			#ifdef RTP_SNOW
				mipN += snow_depth;
			#endif
			n.xy = tex2Dlod(_BumpMap89, float4(IN._uv_Relief.xy*MixScaleRouted89AB.x,  mipN.xx)).rg*splat_control1.r;
			n.xy += tex2Dlod(_BumpMap89, float4(IN._uv_Relief.xy*MixScaleRouted89AB.y,  mipN.yy)).ba*splat_control1.g;
			n.xy += tex2Dlod(_BumpMapAB, float4(IN._uv_Relief.xy*MixScaleRouted89AB.z,  mipN.zz)).rg*splat_control1.b;
			n.xy += tex2Dlod(_BumpMapAB, float4(IN._uv_Relief.xy*MixScaleRouted89AB.w,  mipN.ww)).ba*splat_control1.a;
			#ifndef _4LAYERS
				mipN=mip_selector.xxxx+rtp_mipoffset_bump+MIPmult4567+log2(MixScaleRouted4567);
				#ifdef RTP_SNOW
					mipN += snow_depth;
				#endif
				n.xy += tex2Dlod(_BumpMap45, float4(IN._uv_Relief.xy*MixScaleRouted4567.x,  mipN.xx)).rg*splat_control2.r;
				n.xy += tex2Dlod(_BumpMap45, float4(IN._uv_Relief.xy*MixScaleRouted4567.y,  mipN.yy)).ba*splat_control2.g;
				n.xy += tex2Dlod(_BumpMap67, float4(IN._uv_Relief.xy*MixScaleRouted4567.z,  mipN.zz)).rg*splat_control2.b;
				n.xy += tex2Dlod(_BumpMap67, float4(IN._uv_Relief.xy*MixScaleRouted4567.w,  mipN.ww)).ba*splat_control2.a;
			#endif
			n.xy=n.xy*2-1;
			n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
			
			o.Normal = lerp( o.Normal, n, lerp(blendVal*0.5, 1, saturate(repl*2)) );
	}
	#endif

	float _BumpMapGlobalStrengthPerLayer=1;
	#ifdef _4LAYERS
		_BumpMapGlobalStrengthPerLayer=dot(_BumpMapGlobalStrength89AB, splat_control1);
	#else
		_BumpMapGlobalStrengthPerLayer=dot(_BumpMapGlobalStrength89AB, splat_control1)+dot(_BumpMapGlobalStrength4567, splat_control2);
	#endif
	#if !defined(RTP_SIMPLE_SHADING)
		{
		float3 tangentBase = float3(norm_far.z, 0, -norm_far.x); // normalize(cross(float3(0.0,1.0,0.0), norm_far));
		float3 binormalBase = cross(norm_far, tangentBase); //normalize(cross(norm_far, tangentBase));
		float3 combinedNormal = tangentBase * o.Normal.x + binormalBase * o.Normal.y + norm_far * o.Normal.z;
		o.Normal = lerp(o.Normal, combinedNormal, lerp(rtp_perlin_start_val,1, _uv_Relief_w)*_BumpMapGlobalStrengthPerLayer);
		}
	#else
		o.Normal+=norm_far*lerp(rtp_perlin_start_val,1, _uv_Relief_w)*_BumpMapGlobalStrengthPerLayer;	
	#endif	
	
	#ifdef _4LAYERS
	o_Gloss=lerp( saturate(o_Gloss+4*dot(splat_control1_nobleed, _FarSpecCorrection89AB)), o_Gloss, (1-_uv_Relief_w)*(1-_uv_Relief_w) );
	#else
	o_Gloss=lerp( saturate(o_Gloss+4*(dot(splat_control1_nobleed, _FarSpecCorrection89AB) + dot(splat_control2_nobleed, _FarSpecCorrection4567))), o_Gloss, (1-_uv_Relief_w)*(1-_uv_Relief_w) );
	#endif
	
	#ifdef COLOR_MAP
		float colBrightness=dot(col,1);
		#ifdef _4LAYERS
			global_color_blend *= dot(splat_control1, _GlobalColorPerLayer89AB);
		#else
			#ifdef RTP_HARD_CROSSPASS
				global_color_blend *= splat_controlA_coverage>splat_controlB_coverage ? dot(splat_control1, _GlobalColorPerLayer89AB) : dot(splat_control2, _GlobalColorPerLayer4567);
			#else
				global_color_blend *= dot(splat_control1, _GlobalColorPerLayer89AB) + dot(splat_control2, _GlobalColorPerLayer4567);
			#endif
		#endif
		#ifdef RTP_WETNESS
			global_color_blend*=(1-_WaterOpacity);
		#endif
		
		#ifdef ADV_COLOR_MAP_BLENDING
			// advanced global colormap blending
			#ifdef _4LAYERS
				half advGlobalColorMapBottomLevel = dot(splat_control1, _GlobalColorBottom89AB);
				half advGlobalColorMapTopLevel = dot(splat_control1, _GlobalColorTop89AB);
				half colorMapLoSat = dot(splat_control1, _GlobalColorColormapLoSat89AB);
				half colorMapHiSat = dot(splat_control1, _GlobalColorColormapHiSat89AB);
				half colLoSat = dot(splat_control1, _GlobalColorLayerLoSat89AB);
				half colHiSat = dot(splat_control1, _GlobalColorLayerHiSat89AB);
				half advGlobalColorMapLoBlend = dot(splat_control1, _GlobalColorLoBlend89AB);
				half advGlobalColorMapHiBlend = dot(splat_control1, _GlobalColorHiBlend89AB);
			#else
				half advGlobalColorMapBottomLevel = dot(splat_control1, _GlobalColorBottom89AB) + dot(splat_control2, _GlobalColorBottom4567);
				half advGlobalColorMapTopLevel = dot(splat_control1, _GlobalColorTop89AB) + dot(splat_control2, _GlobalColorTop4567);
				half colorMapLoSat = dot(splat_control1, _GlobalColorColormapLoSat89AB) + dot(splat_control2, _GlobalColorColormapLoSat4567);
				half colorMapHiSat = dot(splat_control1, _GlobalColorColormapHiSat89AB) + dot(splat_control2, _GlobalColorColormapHiSat4567);
				half colLoSat = dot(splat_control1, _GlobalColorLayerLoSat89AB) + dot(splat_control2, _GlobalColorLayerLoSat4567);
				half colHiSat = dot(splat_control1, _GlobalColorLayerHiSat89AB) + dot(splat_control2, _GlobalColorLayerHiSat4567);
				half advGlobalColorMapLoBlend = dot(splat_control1, _GlobalColorLoBlend89AB) + dot(splat_control2, _GlobalColorLoBlend4567);
				half advGlobalColorMapHiBlend = dot(splat_control1, _GlobalColorHiBlend89AB) + dot(splat_control2, _GlobalColorHiBlend4567);
			#endif
			
			half colorBlendValEnveloped = saturate( (actH - advGlobalColorMapBottomLevel) / (advGlobalColorMapTopLevel-advGlobalColorMapBottomLevel) );
			half3 globalColorValue = lerp(dot(global_color_value.rgb,0.3333).xxx, global_color_value.rgb, lerp(colorMapLoSat, colorMapHiSat, colorBlendValEnveloped) );
			half3 colValue = lerp(dot(col.rgb,0.3333).xxx, col.rgb, lerp(colLoSat, colHiSat, colorBlendValEnveloped) );
			col = lerp(col, colValue*globalColorValue.rgb*2, global_color_blend*lerp(advGlobalColorMapLoBlend, advGlobalColorMapHiBlend, colorBlendValEnveloped) );
		#else
			// basic global colormap blending
			#ifdef COLOR_MAP_BLEND_MULTIPLY
				col=lerp(col, col*global_color_value.rgb*2, global_color_blend);
			#else
				col=lerp(col, global_color_value.rgb, global_color_blend);
			#endif
		#endif
		#ifdef SIMPLE_FAR
			col=lerp(col, global_color_value.rgb, _uv_Relief_w);
		#endif		
		#ifdef RTP_IBL_DIFFUSE
			half3 colBrightnessNotAffectedByColormap=col*colBrightness/dot(col,1);
		#endif
	#endif
	
	#ifdef RTP_SNOW
		IN_uv_Relief_Offset.xy=lerp(IN_uv_Relief_Offset.xy, IN._uv_Relief.xy, snow_depth_lerp);
	
		#ifdef COLOR_MAP
			snow_val = snow_const + rtp_snow_strength*dot(1-global_color_value.rgb, rtp_global_color_brightness_to_snow.xxx)+rtp_snow_strength*2;
		#else
			snow_val = snow_const + rtp_snow_strength*0.5*rtp_global_color_brightness_to_snow+rtp_snow_strength*2;
		#endif
		
		snow_val*=rtp_snow_layer_damp;
		snow_val -= rtp_snow_slope_factor*saturate( 1 - dot( (norm_snowCov*0.5+norm_for_snow*0.5), flat_dir.xyz) - 0*dot( norm_for_snow, flat_dir.xyz));
		
		snow_val=saturate(snow_val);
		snow_val=pow(abs(snow_val), rtp_snow_edge_definition);
		rtp_snow_color_tex=lerp(rtp_snow_color.rgb, rtp_snow_color_tex, _uv_Relief_z);
		
		#ifdef COLOR_MAP
			half3 global_color_value_desaturated=dot(global_color_value.rgb, 0.37);//0.3333333); // będzie trochę jasniej
			#ifdef COLOR_MAP_BLEND_MULTIPLY
				rtp_snow_color_tex=lerp(rtp_snow_color_tex, rtp_snow_color_tex*global_color_value_desaturated.rgb*2, min(0.4,global_color_blend*0.7) );
			#else
				rtp_snow_color_tex=lerp(rtp_snow_color_tex, global_color_value_desaturated.rgb, min(0.4,global_color_blend*0.7) );
			#endif
		#endif

		col=lerp( col, rtp_snow_color_tex, snow_val );
		#ifdef RTP_IBL_DIFFUSE
			colBrightnessNotAffectedByColormap=lerp( colBrightnessNotAffectedByColormap, rtp_snow_color_tex, snow_val );
		#endif
		
		#if defined(RTP_SNW_CHOOSEN_LAYER_NORM_0) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_1) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_2) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_3) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_4) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_5) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_6) || defined(RTP_SNW_CHOOSEN_LAYER_NORM_7)
			float3 n;
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_0
				n.xy=tex2Dlod(_BumpMap89, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).rg*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_1
				n.xy=tex2Dlod(_BumpMap89, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).ba*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_2
				n.xy=tex2Dlod(_BumpMapAB, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).rg*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_3
				n.xy=tex2Dlod(_BumpMapAB, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).ba*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_4
				n.xy=tex2Dlod(_BumpMap45, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).rg*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_5
				n.xy=tex2Dlod(_BumpMap45, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).ba*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_6
				n.xy=tex2Dlod(_BumpMap67, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).rg*2-1;
			#endif
			#ifdef RTP_SNW_CHOOSEN_LAYER_NORM_7
				n.xy=tex2Dlod(_BumpMap67, float4(IN_uv_Relief_Offset.xy, mip_selector + rtp_mipoffset_bump)).ba*2-1;
			#endif
			n.xy*=_uv_Relief_z;
			n.z = sqrt(1 - saturate(dot(n.xy, n.xy)));
			float3 snow_normal=lerp(o.Normal, n, snow_depth_lerp );
		#else
			float3 snow_normal=o.Normal;
		#endif
				
		snow_normal=norm_for_snow + 2*snow_normal*(_uv_Relief_z*0.5+0.5);
		
		snow_normal=normalize(snow_normal);
		o.Normal=lerp(o.Normal, snow_normal, snow_val);
		float rtp_snow_specular_distAtten=rtp_snow_specular;
		o_Gloss=lerp(o_Gloss, rtp_snow_specular, snow_val);
		// przeniesione pod emisję (która zależy od specular materiału _pod_ śniegiem)
		//o.Specular=lerp(o.Specular, rtp_snow_gloss, snow_val);
		#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC) || defined(RTP_PBL_FRESNEL)
		o.RTP.x=lerp(o.RTP.x, rtp_snow_fresnel, snow_val);
		#endif
		float snow_damp=saturate(1-snow_val*2);
	#endif
	
	// emission of layer (inside)
	#ifdef RTP_EMISSION
		#ifdef RTP_SNOW
			layer_emission *= snow_damp*0.9+0.1; // delikatna emisja poprzez snieg
		#endif
		
		#if defined(RTP_WETNESS)
			layer_emission *= lerp(o.Specular, 1, p) * 2;
			// zróżnicowanie koloru na postawie animowanych normalnych
			#ifdef RTP_FUILD_EMISSION_WRAP
				float norm_fluid_val=lerp( 0.7, saturate(dot(o.Normal.xy*4, o.Normal.xy*4)), _uv_Relief_z);
				o.Emission += (col + _LayerEmissionColor.rgb ) * ( norm_fluid_val*p+0.15 ) * layer_emission * 4;
			#else
				float norm_fluid_val=lerp( 0.5, (o.Normal.x+o.Normal.y), _uv_Relief_z);
				o.Emission += (col + _LayerEmissionColor.rgb ) * ( saturate(norm_fluid_val*2*p)*1.2+0.15 ) * layer_emission * 4;
			#endif
		#else
			layer_emission *= o.Specular * 2;
			o.Emission += (col + _LayerEmissionColor.rgb*0.2 ) * layer_emission * 4;
		#endif
		layer_emission = max(0, 1 - layer_emission);
		o_Gloss *= layer_emission;
		o.Specular *= layer_emission;
		col *= layer_emission;
	#endif	
	
	// przeniesione pod emisję (która zależy od specular materiału _pod_ śniegiem)
	#ifdef RTP_SNOW
		o.Specular=lerp(o.Specular, rtp_snow_gloss, snow_val);
	#endif	
	
	o.Albedo=col;
	o.Normal=normalize(o.Normal);
	
	// heightblend AO
	#ifdef RTP_HEIGHTBLEND_AO
		#if defined(_4LAYERS)
			float heightblend_AO=saturate(dot(0.5-abs(lerp(splat_control1_mid, splat_control1_close, RTP_AOsharpness)-0.5), RTP_AO_89AB)*RTP_AOamp);
		#else
			float heightblend_AO=saturate(( dot(0.5-abs(lerp(splat_control1_mid, splat_control1_close, RTP_AOsharpness)-0.5), RTP_AO_89AB) + dot(0.5-abs(lerp(splat_control2_mid, splat_control2_close, RTP_AOsharpness)-0.5), RTP_AO_4567) )*RTP_AOamp);
		#endif	
		#ifdef RTP_SNOW
			heightblend_AO*=snow_damp;
		#endif
		//o.RTP.y*=1-heightblend_AO; // mnożone później (po emission glow, bo ten tłumi AO)
	#endif		
	
	// emission of layer (glow)
	#ifdef RTP_EMISSION
		#if defined(_4LAYERS)
			float emission_glow=saturate(dot(0.5-abs(splat_control1_mid-0.5), emission_valA.xxxx));
			emission_glow=saturate(emission_glow - 0.5*dot(splat_control1, emission_valA.xxxx));
		#else
			float emission_glow=saturate(( dot(0.5-abs(splat_control1_mid-0.5), emission_valA.xxxx) + dot(0.5-abs(splat_control2_mid-0.5), emission_valB.xxxx) ) );
			emission_glow=saturate(emission_glow - 0.5*(dot(splat_control1, emission_valA.xxxx) + dot(splat_control2, emission_valB.xxxx)) );
		#endif
		#ifdef RTP_SNOW
			emission_glow *= snow_damp*0.8+0.2; // delikatna emisja poprzez snieg
		#endif
		#ifdef RTP_HEIGHTBLEND_AO
			heightblend_AO*=(1-emission_glow);
		#endif
		o.Emission+=emission_glow*8*_LayerEmissionColor.rgb;
	#endif
	
	#ifdef RTP_HEIGHTBLEND_AO
		o.RTP.y*=1-heightblend_AO;
	#endif		
	
	#ifdef _4LAYERS
		float IBL_bump_smoothness=dot(splat_control1, RTP_IBL_bump_smoothness89AB);
		#ifdef RTP_IBL_DIFFUSE
			float RTP_IBL_DiffStrength=dot(splat_control1, RTP_IBL_DiffuseStrength89AB);
		#endif
		#if defined(RTP_IBL_SPEC) || defined(RTP_REFLECTION)
			// anti-bleed subtraction
			float RTP_IBL_SpecStrength=dot(splat_control1_nobleed, RTP_IBL_SpecStrength89AB);
		#endif
	#else
		float IBL_bump_smoothness=dot(splat_control1, RTP_IBL_bump_smoothness89AB)+dot(splat_control2, RTP_IBL_bump_smoothness4567);
		#ifdef RTP_IBL_DIFFUSE
			float RTP_IBL_DiffStrength=dot(splat_control1, RTP_IBL_DiffuseStrength89AB)+dot(splat_control2, RTP_IBL_DiffuseStrength4567);
		#endif
		#if defined(RTP_IBL_SPEC) || defined(RTP_REFLECTION)
			// anti-bleed subtraction
			float RTP_IBL_SpecStrength=dot(splat_control1_nobleed, RTP_IBL_SpecStrength89AB)+dot(splat_control2_nobleed, RTP_IBL_SpecStrength4567);
//			float RTP_IBL_SpecStrength=dot(splat_control1, RTP_IBL_SpecStrength89AB)+dot(splat_control2, RTP_IBL_SpecStrength4567);
		#endif
	#endif	
	// lerp IBL values with wet / snow
	#ifdef RTP_IBL_DIFFUSE
		#ifdef RTP_SNOW
			RTP_IBL_DiffStrength=lerp(RTP_IBL_DiffStrength, rtp_snow_IBL_DiffuseStrength, snow_val);
		#endif
	#endif
	#if defined(RTP_IBL_SPEC) || defined(RTP_REFLECTION)
		#ifdef RTP_WETNESS
			RTP_IBL_SpecStrength=lerp(RTP_IBL_SpecStrength, TERRAIN_WaterIBL_SpecWetStrength, TERRAIN_LayerWetStrength);
			RTP_IBL_SpecStrength=lerp(RTP_IBL_SpecStrength, TERRAIN_WaterIBL_SpecWaterStrength, p*p);
		#endif
		#ifdef RTP_SNOW
			RTP_IBL_SpecStrength=lerp(RTP_IBL_SpecStrength, rtp_snow_IBL_SpecStrength, snow_val);
		#endif
	#endif
	
	#if defined(RECONSTRUCT_WORLDNORMAL)
		float3 IBLNormal=lerp(o.Normal, float3(0,0,1), IBL_bump_smoothness);
		float3 normalW; // world normal z bumpmappinginem
		float3 reflVec; // world refl z bumpmappingiem
		{
		   	float3 wNormal = IN.color.xyz;
		    float3 oTangent = normalize( cross(IN.color.xyz, float3(0, -IN.color.z, IN.color.y)) );
		    float3 oBinormal = -cross(IN.color.xyz, oTangent);
		    normalW = (oTangent * IBLNormal.x) + (oBinormal * IBLNormal.y) + (wNormal * IBLNormal.z);
    		#if defined(RTP_REFLECTION) || defined(RTP_IBL_SPEC)
				#ifdef RTP_NORMALGLOBAL
					// tutaj musimy się zadowolić stretchowanym (w POM) viewDir, bo ten z surface shadera jest płaski (obl dla v.normal=(0,1,0))
					IN_viewDir=-IN_viewDir;
					#ifdef RTP_POM_SHADING
			    		float3 viewW = (oTangent * IN_viewDir.x) + (oBinormal * IN_viewDir.y) - (wNormal * IN_viewDir.z);
			    	#else
			    		float3 viewW = (oTangent * IN_viewDir.x) + (oBinormal * IN_viewDir.y) + (wNormal * IN_viewDir.z);
			    	#endif
				#else
			    	IN.viewDir=-IN.viewDir; // unstreched vewiDir - flip it for reconstruction
			    	float3 viewW = (oTangent * IN.viewDir.x) + (oBinormal * IN.viewDir.y) + (wNormal * IN.viewDir.z);
				#endif
				reflVec=reflect(normalize(viewW), normalW);
				#if defined(RTP_SKYSHOP_SYNC) && defined(RTP_SKYSHOP_SKY_ROTATION)
				 reflVec = SkyMatrix[0].xyz*reflVec.x + SkyMatrix[1].xyz*reflVec.y + SkyMatrix[2].xyz*reflVec.z;
				#endif		
			#endif
	   		#if defined(RTP_SKYSHOP_SYNC) && defined(RTP_SKYSHOP_SKY_ROTATION)
				normalW = SkyMatrix[0].xyz*normalW.x + SkyMatrix[1].xyz*normalW.y + SkyMatrix[2].xyz*normalW.z;
			#endif	
		}
	#endif
	
	// ^4 shaped diffuse fresnel term for soft surface layers (grass)
	float _DiffFresnel=0;
	#ifdef _4LAYERS
		_DiffFresnel=dot(splat_control1, RTP_DiffFresnel89AB);
	#else
		_DiffFresnel=dot(splat_control1, RTP_DiffFresnel89AB)+dot(splat_control2, RTP_DiffFresnel4567);
	#endif
	// diffuse fresnel term for snow
	#ifdef RTP_SNOW
		_DiffFresnel=lerp(_DiffFresnel, rtp_snow_diff_fresnel, snow_val);
	#endif
	float diffuseScatteringFactor=1.0 + diffFresnel*_DiffFresnel;
	o.Albedo *= diffuseScatteringFactor;
	#ifdef RTP_IBL_DIFFUSE
		colBrightnessNotAffectedByColormap *= diffuseScatteringFactor;
	#endif
	
	// spec color from albedo (metal tint)
	#ifdef _4LAYERS
		float Albedo2SpecColor=dot(splat_control1, _LayerAlbedo2SpecColor89AB);
	#else
		float Albedo2SpecColor=dot(splat_control1, _LayerAlbedo2SpecColor89AB) + dot(splat_control2, _LayerAlbedo2SpecColor4567);
	#endif
	#ifdef RTP_SNOW
		Albedo2SpecColor*=snow_damp;
	#endif
	#ifdef RTP_WETNESS
		colAlbedo=lerp(colAlbedo, o.Albedo, p);
	#endif
	// colAlbedo powinno być "normalizowane" (aby nie było za ciemne) jako tinta dla spec color
	float colAlbedoRGBmax=max(max(colAlbedo.r, colAlbedo.g), colAlbedo.b);
	colAlbedoRGBmax=max(colAlbedoRGBmax, 0.01);
	float3 colAlbedoNew=lerp(half3(1,1,1), colAlbedo.rgb/colAlbedoRGBmax.xxx, saturate(colAlbedoRGBmax*4)*Albedo2SpecColor);
	half3 SpecColor=_SpecColor.rgb*o_Gloss*colAlbedoNew*colAlbedoNew; // spec color for IBL/Refl
	o.SpecColor=SpecColor;

	#ifdef RTP_TREESGLOBAL	
		float4 pixel_trees_val=tex2D(_TreesMapGlobal, IN.uv_Control);
		float pixel_trees_blend_val=saturate((pixel_trees_val.r+pixel_trees_val.g+pixel_trees_val.b)*_TERRAIN_trees_pixel_values.z);
		pixel_trees_blend_val*=saturate((IN._uv_Relief.w - _TERRAIN_trees_pixel_values.x) / _TERRAIN_trees_pixel_values.y);
		o.Albedo=lerp(o.Albedo, pixel_trees_val.rgb, pixel_trees_blend_val);
		#if !defined(RTP_AMBIENT_EMISSIVE_MAP)
			float pixel_trees_shadow_val=saturate((IN._uv_Relief.w - _TERRAIN_trees_shadow_values.x) / _TERRAIN_trees_shadow_values.y);
			pixel_trees_shadow_val=lerp(1, pixel_trees_val.a, pixel_trees_shadow_val);
			float o_RTP_y_without_shadowmap_distancefade=o.RTP.y*lerp(pixel_trees_val.a, 1, _TERRAIN_trees_shadow_values.z);
			o.RTP.y*=lerp(_TERRAIN_trees_shadow_values.z, 1, pixel_trees_shadow_val);
		#endif
	#endif
		
	#if defined(RTP_AMBIENT_EMISSIVE_MAP)
		float4 eMapVal=tex2D(_AmbientEmissiveMapGlobal, IN.uv_Control);
		o.Emission+=o.Albedo*eMapVal.rgb*_AmbientEmissiveMultiplier*lerp(1, saturate(o.Normal.z*o.Normal.z-(1-actH)*(1-o.Normal.z*o.Normal.z)), _AmbientEmissiveRelief);
		float pixel_trees_shadow_val=saturate((IN._uv_Relief.w - _TERRAIN_trees_shadow_values.x) / _TERRAIN_trees_shadow_values.y);
		pixel_trees_shadow_val=lerp(1, eMapVal.a, pixel_trees_shadow_val);
		float o_RTP_y_without_shadowmap_distancefade=o.RTP.y*lerp(eMapVal.a, 1, _TERRAIN_trees_shadow_values.z);
		o.RTP.y*=lerp(_TERRAIN_trees_shadow_values.z, 1, pixel_trees_shadow_val);
	#endif	
	
	#if !defined(RTP_TREESGLOBAL) && !defined(RTP_AMBIENT_EMISSIVE_MAP)
		#define o_RTP_y_without_shadowmap_distancefade (o.RTP.y)
	#endif
	
	#ifdef RTP_IBL_DIFFUSE
		#ifdef RTP_SKYSHOP_SYNC
			half3 IBLDiffuseCol = SHLookup(normalW)*RTP_IBL_DiffStrength;
		#else
			half3 IBLDiffuseCol = DecodeRGBM(texCUBElod(_CubemapDiff, float4(normalW,0)))*RTP_IBL_DiffStrength;
		#endif
		IBLDiffuseCol*=colBrightnessNotAffectedByColormap * lerp(1, o_RTP_y_without_shadowmap_distancefade, TERRAIN_IBL_DiffAO_Damp);
		#ifndef RTP_IBL_SPEC
		o.Emission += IBLDiffuseCol.rgb;
		#else
		// dodamy za chwilę poprzez introplację, która zachowa energie
		#endif
	#endif

	// kompresuję odwrotnie mip blur (łatwiej osiągnąć "lustro")
	float o_SpecularInvSquared = (1-o.Specular)*(1-o.Specular);
	
	#ifdef RTP_POM_SHADING
		IN_viewDir.z=-IN_viewDir.z;
	#endif
	
	#ifdef RTP_IBL_SPEC
		#define RTP_IBLSPEC_BUMPED
		#ifdef RTP_IBLSPEC_BUMPED
			#ifndef RECONSTRUCT_WORLDNORMAL
				float3 IBLNormal=lerp(o.Normal, float3(0,0,1), IBL_bump_smoothness);
			#endif
			#ifdef RTP_NORMALGLOBAL
				// tutaj musimy się zadowolić stretchowanym (w POM) viewDir, bo ten z surface shadera jest płaski (obl dla v.normal=(0,1,0))
				IN_viewDir=-IN_viewDir;

				float n_dot_v = saturate(dot (IBLNormal, normalize(float3(IN_viewDir.xy, IN_viewDir.z))));
			#else
				float n_dot_v = saturate(dot (IBLNormal, normalize(-IN.viewDir.xyz)));
			#endif
			// float exponential = pow( 1.0f - n_dot_v, 5.0f ); // Schlick's approx to fresnel
			// below pow 4 looks OK and is cheaper than pow() call
			float exponential = 1.0f - n_dot_v;
			exponential*=exponential;
			exponential*=exponential;	
		#else
			float exponential=diffFresnel;
		#endif
		// skyshop fit (I'd like people to get similar results in gamma / linear)
		#if defined(RTP_COLORSPACE_LINEAR)
			exponential=0.03+0.97*exponential;
		#else
			exponential=0.25+0.75*exponential;
		#endif
		float spec_fresnel = lerp (1.0f, exponential, o.RTP.x);
		#if defined(RECONSTRUCT_WORLDNORMAL)
			half3 IBLSpecCol = DecodeRGBM(texCUBElod (_CubemapSpec, float4(reflVec, o_SpecularInvSquared*(6-exponential*o.RTP.x*3))))*RTP_IBL_SpecStrength;
		#else
			float3 reflVec;
			#if defined(RTP_TRIPLANAR)
				reflVec=normalDecode( IN._uv_Aux.xy + o.Normal.xy*(1-IBL_bump_smoothness)*0.1 );
			#else
				reflVec=normalDecode( IN._uv_Aux.zw + o.Normal.xy*(1-IBL_bump_smoothness)*0.1 );
			#endif		
			half3 IBLSpecCol = DecodeRGBM(texCUBElod (_CubemapSpec, float4(reflVec, o_SpecularInvSquared*(6-exponential*o.RTP.x*3))))*RTP_IBL_SpecStrength;
		#endif			
		IBLSpecCol.rgb*=spec_fresnel * SpecColor * lerp(1, o_RTP_y_without_shadowmap_distancefade, TERRAIN_IBLRefl_SpecAO_Damp);
		#ifdef RTP_IBL_DIFFUSE
			// link difuse and spec IBL together with energy conservation
			o.Emission += saturate(1-IBLSpecCol.rgb) * IBLDiffuseCol + IBLSpecCol.rgb;
		#else
			o.Emission+=IBLSpecCol.rgb;
		#endif
	#endif

		
    #if defined(RTP_REFLECTION) && !defined(RTP_SHOW_OVERLAPPED)
	#if defined(RTP_SIMPLE_SHADING)	
	
	#ifdef FAR_ONLY
	if (false) {
	#else
	if (_uv_Relief_z>0) {
	#endif

	#endif
		float2 mip_selectorRefl=o_SpecularInvSquared*(8-diffFresnel*o.RTP.x*4);
		#if defined(RECONSTRUCT_WORLDNORMAL)
			float t=tex2Dlod(_BumpMapGlobal, float4(reflVec.xz*0.5+0.5, mip_selectorRefl)).a;
		#else
			float3 reflVec;
			#if defined(RTP_TRIPLANAR)
				reflVec=normalDecode( IN._uv_Aux.xy + o.Normal.xy*(1-IBL_bump_smoothness)*0.1 );
			#else
				reflVec=normalDecode( IN._uv_Aux.zw + o.Normal.xy*(1-IBL_bump_smoothness)*0.1 );
			#endif			
			reflVec.xz=reflVec.xz*0.5+0.5;
			float t=tex2Dlod(_BumpMapGlobal, float4(reflVec.xz, mip_selectorRefl)).a;
		#endif		
		#ifdef RTP_IBL_SPEC
			half rim=spec_fresnel;
		#else
			#define RTP_REFLSPEC_BUMPED
			#ifdef RTP_REFLSPEC_BUMPED
				#ifndef RECONSTRUCT_WORLDNORMAL
					float3 IBLNormal=lerp(o.Normal, float3(0,0,1), IBL_bump_smoothness);
				#endif
				#ifdef RTP_NORMALGLOBAL
					// tutaj musimy się zadowolić stretchowanym (w POM) viewDir, bo ten z surface shadera jest płaski (obl dla v.normal=(0,1,0))
					IN_viewDir=-IN_viewDir;
					float n_dot_v = saturate(dot (IBLNormal, normalize(float3(IN_viewDir.xy, IN_viewDir.z))));
				#else
					float n_dot_v = saturate(dot (IBLNormal, normalize(IN.viewDir.xyz)));
				#endif
				// float exponential = pow( 1.0f - n_dot_v, 5.0f ); // Schlick's approx to fresnel
				// below pow 4 looks OK and is cheaper than pow() call
				float exponential = 1.0f - n_dot_v;
				exponential*=exponential;
				exponential*=exponential;	
				half rim= lerp(1, exponential, o.RTP.x);
			#else
				#if defined(RTP_COLORSPACE_LINEAR)
					diffFresnel=0.03+0.97*diffFresnel;
				#else
					diffFresnel=0.25+0.75*diffFresnel;
				#endif
		        half rim= lerp(1, diffFresnel, o.RTP.x);
			#endif
	    #endif
	    float downSideEnvelope=saturate(reflVec.y*3);
	    t *= downSideEnvelope;
	    rim *= downSideEnvelope*0.7+0.3;
		#if defined(RTP_SIMPLE_SHADING)
			rim*=RTP_IBL_SpecStrength*_uv_Relief_z;
		#else
			rim*=RTP_IBL_SpecStrength;
		#endif
		rim-=o_SpecularInvSquared*rim*TERRAIN_ReflGlossAttenuation; // attenuate low gloss
		
		half3 reflCol;
		reflCol=lerp(TERRAIN_ReflColorB.rgb, TERRAIN_ReflColorC.rgb, saturate(TERRAIN_ReflColorCenter-t) / TERRAIN_ReflColorCenter );
		reflCol=lerp(reflCol.rgb, TERRAIN_ReflColorA.rgb, saturate(t-TERRAIN_ReflColorCenter) / (1-TERRAIN_ReflColorCenter) );
		o.Emission += reflCol * SpecColor * lerp(1, o_RTP_y_without_shadowmap_distancefade, TERRAIN_IBLRefl_SpecAO_Damp) * rim * 2;
	#if defined(RTP_SIMPLE_SHADING)	
	}
	#endif
	#endif

	float diff;
	#if !defined(LIGHTMAP_OFF) && defined (DIRLIGHTMAP_OFF)
		//IN.lightDir.z*=_TERRAIN_ExtrudeHeight;
		diff = max (0, dot (o.Normal, IN.lightDir.xyz))*lerp(2,2-_BumpMapGlobalStrengthPerLayer,_uv_Relief_z);
		diff = lerp(diff, 1, _uv_Relief_w*0.75f); // w dużej odleglosci nakladamy tylko 25% diffa
		diff = diff*_TERRAIN_LightmapShading+(1-_TERRAIN_LightmapShading);
		#if defined(UNITY_PASS_PREPASSFINAL)
			diff=lerp(diff, 1, _uv_Relief_z);
		#endif
		o.RTP.y*=diff;
	#endif
	
	#ifdef RTP_CAUSTICS
	{
		if (damp_fct_caustics>0 && _uv_Relief_w<1) {
			float tim=_Time.x*TERRAIN_CausticsAnimSpeed;
			IN_uv_Relief_Offset.xy*=TERRAIN_CausticsTilingScale;
			#ifdef RTP_VERTALPHA_CAUSTICS
				float3 _Emission=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy+float2(tim, tim), mip_selector.xx+rtp_mipoffset_caustics.xx) ).aaa;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy+float2(-tim, -tim*0.873), mip_selector.xx+rtp_mipoffset_caustics.xx) ).aaa;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy*1.1+float2(tim, 0), mip_selector.xx+rtp_mipoffset_caustics.xx) ).aaa;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy*0.5+float2(0, tim*0.83), mip_selector.xx-1+rtp_mipoffset_caustics.xx) ).aaa;
			#else
				float3 _Emission=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy+float2(tim, tim), mip_selector.xx+rtp_mipoffset_caustics.xx) ).rgb;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy+float2(-tim, -tim*0.873), mip_selector.xx+rtp_mipoffset_caustics.xx) ).rgb;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy*1.1+float2(tim, 0), mip_selector.xx+rtp_mipoffset_caustics.xx) ).rgb;
				_Emission+=tex2Dlod(TERRAIN_CausticsTex, float4(IN_uv_Relief_Offset.xy*0.5+float2(0, tim*0.83), mip_selector.xx-1+rtp_mipoffset_caustics.xx) ).rgb;
			#endif
			_Emission=saturate(_Emission-1.55);
			_Emission*=_Emission;
			_Emission*=_Emission;
			_Emission*=TERRAIN_CausticsColor.rgb*8;
			_Emission*=damp_fct_caustics;
			_Emission*=(1-_uv_Relief_w);
			o.Emission+=_Emission;
		}
	} 
	#endif
	
	#if defined(RTP_CROSSPASS_HEIGHTBLEND)
		#if defined(_12LAYERS)
			// we've run out of samplers & heightblend would be very complex
			o.Alpha=total_coverage;
		#else
			splat_control1 = tHA*splat_controlA; // 2 (4-7)
			
			tHB=saturate(tex2D(_TERRAIN_HeightMap, IN._uv_Relief.xy)+0.001); // 1 (0-3)
			float4 splat_controlB = tex2D(_Control1, IN.uv_Control);
			float4 splat_control2 = tHB*splat_controlB;
			
			splat_control1_mid=splat_control1*splat_control1;
			float4 splat_control2_mid=splat_control2*splat_control2;
			
			float norm_sum=dot(splat_control1_mid,1) + dot(splat_control2_mid,1);
			splat_control1_mid/=norm_sum;
			splat_control2_mid/=norm_sum;
			
			splat_control1_close=splat_control1_mid*splat_control1_mid;
			float4 splat_control2_close=splat_control2_mid*splat_control2_mid;
			#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS1
				splat_control1_close*=splat_control1_close;
				splat_control2_close*=splat_control2_close;
			#endif
			#ifdef SHARPEN_HEIGHTBLEND_EDGES_PASS2
				splat_control1_close*=splat_control1_close;
				splat_control2_close*=splat_control2_close;
			#endif
			norm_sum=dot(splat_control1_close,1) + dot(splat_control2_close,1);
			splat_control1_close/=norm_sum;
			splat_control2_close/=norm_sum;
			splat_control1=lerp(splat_control1_mid, splat_control1_close, _uv_Relief_z);
			splat_control2=lerp(splat_control2_mid, splat_control2_close, _uv_Relief_z);			
			
			splat_control1_close=splat_control1_mid*splat_control1_mid;
			splat_control1=lerp(lerp(splat_control1_mid, splat_control1, _uv_Relief_w), splat_control1_close, _uv_Relief_z);
			splat_control2_close=splat_control2_mid*splat_control2_mid;
			splat_control2=lerp(lerp(splat_control2_mid, splat_control2, _uv_Relief_w), splat_control2_close, _uv_Relief_z);
			
			float normalize_sum=dot(splat_control1, 1)+dot(splat_control2, 1);
			splat_control1 /= normalize_sum;
			splat_control2 /= normalize_sum;
			o.Alpha=dot(splat_control1,1);
			
			#ifdef RTP_HEIGHTBLEND_AO
			// 0-3			
			float hbAOCrossPass=(dot(splat_control1,RTP_AO_0123)+dot(splat_control2,RTP_AO_89AB))*RTP_AOamp*_uv_Relief_z*2;
			heightblend_AO=1-saturate(1-dot(saturate(abs(splat_control2-0.5)*(2+RTP_AOsharpness)),0.25))*hbAOCrossPass;
			o.RTP.y*=heightblend_AO;
			
			// 4-7
//			heightblend_AO=1-saturate(1-dot(saturate(abs(splat_control1-0.5)*(2+RTP_AOsharpness)),0.25))*hbAOCrossPass;
//			o.RTP.y*=heightblend_AO;
						
//			heightblend_AO=1-saturate(1-saturate(abs(o.Alpha-0.5)*(2+RTP_AOsharpness)))*RTP_AOamp*_uv_Relief_z;
//			o.RTP.y*=heightblend_AO;
			#endif
		#endif	
	#else
		o.Alpha=total_coverage;
	#endif			
	
	// EOF regular mode
	#endif
	
	// przeniesione, norm_edge zamienione na o.Normal w obl. reflection
	#ifdef RTP_NORMALGLOBAL
		float3 global_norm;
		#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
			global_norm.xy=tex2D(_NormalMapGlobal, IN.uv_Control).xy * 2 - 1;
		#else
			global_norm.xy=tex2D(_NormalMapGlobal, IN.uv_Control).wy * 2 - 1;
		#endif
		global_norm.xy*=_TERRAIN_trees_shadow_values.w;
		global_norm.z=sqrt(1 - saturate(dot(global_norm.xy, global_norm.xy)));
		float3 tangentBase = normalize(float3(global_norm.z, 0, -global_norm.x)); //normalize(cross(float3(0.0,1.0,0.0), global_norm));
		float3 binormalBase = normalize(cross(global_norm, tangentBase));
		float3 combinedNormal = tangentBase * o.Normal.x + binormalBase * o.Normal.y + global_norm * o.Normal.z;
		o.Normal = lerp(o.Normal, combinedNormal, lerp(1, _uv_Relief_w, _TERRAIN_trees_pixel_values.w));
	#endif	
	
	#if ( !defined(UNITY_PASS_PREPASSBASE) && !defined(UNITY_PASS_PREPASSFINAL) ) || (!defined(RTP_DEFERRED_PBL_NORMALISATION))
		//o.Specular=1-o_SpecularInvSquared;
	#endif
	#if defined(UNITY_PASS_PREPASSBASE)	
		 #if defined(RTP_DEFERRED_PBL_NORMALISATION) && defined(RTP_COLORSPACE_LINEAR)
			o.Specular*=o.Specular;
		#endif
	#endif
	
	#if defined(UNITY_PASS_PREPASSBASE)	 || defined(UNITY_PASS_PREPASSFINAL)
		o.Specular=lerp(RTP_DeferredAddPassSpec, o.Specular, total_coverage);
		#if defined(COLOR_EARLY_EXIT)
		// chcemy mieć przejście do docelowego Speculara osiągnięte wolniej
		float cBiased=(1-IN.color.a);
		cBiased*=cBiased;
		cBiased*=cBiased;
		o.Specular=lerp(o.Specular, RTP_DeferredAddPassSpec, cBiased);
		#endif
		o.Specular=max(0.01, o.Specular);
	#endif
	
	#if defined(UNITY_PASS_PREPASSFINAL)	
		#if defined(RTP_DEFERRED_PBL_NORMALISATION)
			o.Specular=1-o.Specular;
			o.Specular*=o.Specular;
			o.Specular=1-o.Specular;
			// hacking spec normalisation to get quiet a dark spec for max roughness (will be 0.25/16)
			float specular_power=exp2(10*o.Specular+1) - 1.75;
			float normalisation_term = specular_power / (8.0f*60); // 60 - handpicked value - wizualnie daje najlepszy fit
			o.SpecColor*=normalisation_term;
		#endif
		#if defined(RTP_PBL_FRESNEL) && !defined(SUPER_SIMPLE)
			{
			half3 h = normalize (IN.lightDir.xyz + IN_viewDir.xyz);
			float n_dot_l = max(0, dot (o.Normal, IN.lightDir.xyz));
			float h_dot_l = dot (h, IN.lightDir.xyz);
			//float exponential = pow( 1.0f - h_dot_l, 5.0f ); // Schlick's approx to fresnel
			// below pow 4 looks OK and is cheaper than pow() call
			float exponential = 1.0f - h_dot_l;
			exponential*=exponential;
			exponential*=exponential;
			// skyshop fit (I'd like people to get similar results in gamma / linear)
			#if defined(RTP_COLORSPACE_LINEAR)
				exponential=0.02+0.98*exponential;
			#else
				exponential=0.1+0.9*exponential;
			#endif
			o.SpecColor *= lerp (1.0f, exponential,  o.RTP.x)*n_dot_l; // o.RTP.x - _Fresnel
			}
		#endif
	#endif
	#if defined(UNITY_PASS_PREPASSFINAL)
		o.SpecColor*=dot(splat_control1, _DeferredSpecDampAddPass89AB);
	#endif	
} 	
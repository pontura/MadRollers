﻿// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Curved World/One Directional Light/Transparent/Reflective"
{
	Properties 
	{
		//Tag
		[Tag]
		V_CW_TAG("", float) = 0

		//Default Options
		[DefaultOptions]
		V_CW_D_OPTIONS("", float) = 0

		_Color("Main Color", color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

		
		_ReflectColor("Reflection Color", color) = (1, 1, 1, 1)
		_Cube("Reflection Cube", Cube) = "_Skybox"{}

		//CurvedWorld Options
		[CurvedWorldOptions]
		V_CW_W_OPTIONS("", float) = 0


		[HideInInspector]
		_V_CW_Z_Bend_Size("", float) = 0
		[HideInInspector]
		_V_CW_Z_Bend_Bias("", float) = 0
		[HideInInspector]
		_V_CW_Y_Bend_Size("", float) = 0
		[HideInInspector]
		_V_CW_X_Bend_Size("", float) = 0
		[HideInInspector]
		_V_CW_Camera_Bend_Offset("", float) = 0
		 

		[HideInInspector]
		_V_CW_Rim_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Rim_Bias("", Range(-1, 1)) = 0.2

		[HideInInspector]
		_V_CW_Fog_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Fog_Density("", Range(0.0, 1.0)) = 1
		[HideInInspector]
		_V_CW_Fog_Start("", float) = 0
		[HideInInspector]
		_V_CW_Fog_End("", float) = 100
		
		[HideInInspector]
		_V_CW_IBL_Intensity("", float) = 1
		[HideInInspector] 
		_V_CW_IBL_Contrast("", float) = 1 
		[HideInInspector]   
		_V_CW_IBL_Cube("", cube ) = ""{}

		[HideInInspector]
		_V_CW_Emission_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Emission_Strength("", float) = 1

		[HideInInspector]
		_V_CW_Fresnel_Bias("", Range(0, 1)) = 1
	}

	
	SubShader 
	{
		Tags { "Queue"="Transparent+1" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "CurvedWorldTag"="One Directional Light/Transparent/Reflective" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 200
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		Fog{Mode Off}

		Pass
	    {
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" } 

			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest

			#define UNITY_PASS_FORWARDBASE  

			#pragma multi_compile V_CW_FRESNEL_OFF V_CW_FRESNEL_ON

			#pragma multi_compile V_CW_UNITY_VERTEXLIGHT_OFF V_CW_UNITY_VERTEXLIGHT_ON
			#pragma multi_compile V_CW_LIGHT_PER_VERTEX V_CW_LIGHT_PER_PIXEL
			#pragma multi_compile V_CW_SELF_ILLUMINATED_OFF V_CW_SELF_ILLUMINATED_ON
			#pragma multi_compile V_CW_RIM_OFF V_CW_RIM_ON
			#pragma multi_compile V_CW_FOG_OFF V_CW_FOG_ON
			#pragma multi_compile V_CW_VERTEX_COLOR_OFF V_CW_VERTEX_COLOR_ON
			#pragma multi_compile V_CW_IBL_OFF V_CW_IBL_ON

			#define V_NO_SHADOWS
			#define V_CW_REFLECTION

			
			#ifdef V_CW_UNITY_VERTEXLIGHT_ON    
				#pragma multi_compile_fwdbasealpha nodirlightmap  
			#else 
				#pragma multi_compile_fwdbasealpha nodirlightmap novertexlight
			#endif

			#pragma exclude_renderers d3d11_9x
			#include "UnityCG.cginc" 			 
			#include "../cginc/CurvedWorld.cginc"

			ENDCG

		}	//Pass

	}	//SubShader
	
	
	//Fallback - Unlit
	SubShader 
	{
		Tags { "Queue"="Transparent+1" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "CurvedWorldTag"="Unlit/Transparent/Reflective" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 150
		
		 
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		Fog{Mode Off}

		Pass 
	    {
			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment frag
			#define UNITY_PASS_UNLIT
			
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			#pragma multi_compile V_CW_FRESNEL_OFF V_CW_FRESNEL_ON

			#pragma multi_compile V_CW_SELF_ILLUMINATED_OFF V_CW_SELF_ILLUMINATED_ON
			#pragma multi_compile V_CW_RIM_OFF V_CW_RIM_ON
			#pragma multi_compile V_CW_FOG_OFF V_CW_FOG_ON
			#pragma multi_compile V_CW_VERTEX_COLOR_OFF V_CW_VERTEX_COLOR_ON
			#pragma multi_compile V_CW_IBL_OFF V_CW_IBL_ON

			#define V_CW_UNLIT_LIGHTMAP_ON
			#define V_CW_REFLECTION


			#pragma exclude_renderers d3d11_9x
			#include "../cginc/CurvedWorld.cginc" 

			ENDCG

		}	//Pass

	}	//SubShader
	 
	CustomEditor "CurvedWorldMaterial_Editor"

}	//Shader

﻿// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Curved World/Particles/Addative (Soft)"
{
	Properties 
	{
		//Tag
		[Tag]
		V_CW_TAG("", float) = 0

		//Default Options
		[DefaultOptions]
		V_CW_D_OPTIONS("", float) = 0

		[HideInInspector]
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_MainTex ("Particle Texture", 2D) = "white" {}

		[HideInInspector]
		_ReflectColor("Reflection Color", color) = (1, 1, 1, 1)
		[HideInInspector]
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
		_V_CW_Fog_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Fog_Density("", Range(0.0, 1.0)) = 1
		[HideInInspector]
		_V_CW_Fog_Start("", float) = 0
		[HideInInspector]
		_V_CW_Fog_End("", float) = 100

	}

	SubShader 
	{
		Tags { "Queue"="Transparent" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "CurvedWorldTag"="Particles/Addative (Soft)" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 100

		Blend One OneMinusSrcColor
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog{Mode Off}

		Pass
	    {
			CGPROGRAM
			#pragma vertex vert_particles
	    	#pragma fragment frag_particle
	    	#pragma multi_compile_particles

			 
			#pragma multi_compile V_CW_FOG_OFF V_CW_FOG_ON

			#define V_CW_VERTEX_COLOR_ON

			#pragma exclude_renderers d3d11_9x
			#include "../cginc/CurvedWorld.cginc" 

			fixed4 frag_particle(vOutput i) : SV_Target 
			{
				fixed4 retColor = i.color * tex2D(_MainTex, i.texcoord.xy);
				retColor.rgb *= retColor.a; 
				 
				#ifdef V_CW_FOG_ON
					retColor.rgb *= i.vfx.y;
				#endif

				return retColor;
			}   

			ENDCG

		}	//Pass
	}	//SubShader
	 
	CustomEditor "CurvedWorldMaterial_Editor"

}	//Shader

Shader "Custom/CurvedWorld_LitTexture_Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Curvature ("Curvature", Float) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _Curvature;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };

            v2f vert (appdata v)
            {
                // World position + curva
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float4 camSpace = mul(UNITY_MATRIX_V, worldPos);
                float z = camSpace.z;
                float curveY = _Curvature * z * z;
                v.vertex.y -= curveY;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float diff = max(0, dot(i.worldNormal, lightDir));
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                texColor.rgb *= _LightColor0.rgb * diff;

                UNITY_APPLY_FOG(i.fogCoord, texColor);
                return texColor;
            }
            ENDCG
        }
    }
}

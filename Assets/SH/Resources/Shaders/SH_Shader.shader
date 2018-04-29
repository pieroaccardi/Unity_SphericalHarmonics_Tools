Shader "SH/SH_Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
				float2 uv4 : TEXCOORD3;
				float4 color : COLOR0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 radiance : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				float4 sh_0 = v.color;
				float4 sh_1 = float4(v.uv2, v.uv3);
				float sh_2 = v.uv4.x;

				//i'm using the coefficients from the grace cubemap
				float r = (dot(sh_0, float4(0.9082, -0.1342, 0.0227, -0.0247)) + dot(sh_1, float4(-0.0384, -0.0158, -0.0641, -0.0485)) - sh_2 * 0.2564);
				float g = (dot(sh_0, float4(0.6772, -0.07, 0.0255, -0.0245)) + dot(sh_1, float4(-0.0391, 0.0143, -0.074, -0.0434)) - sh_2 * 0.02328);
				float b = (dot(sh_0, float4(0.5759, -0.0224, 0.0385, -0.0363)) + dot(sh_1, float4(-0.039, 0.0004, -0.0751, -0.0476)) - sh_2 * 0.2214);

				o.radiance = float4(r, b, g, 1) * 0.4;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.radiance;
			}
			ENDCG
		}
	}
}

Shader "SH/CoeffVisualizer"
{
	Properties
	{
		_Mode("Mode", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			struct appdata_t 
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 c0;
			float4 c1;
			float4 c2;
			float4 c3;
			float4 c4;
			float4 c5;
			float4 c6;
			float4 c7;
			float4 c8;

			samplerCUBE input;
			float _Mode;

			float Y0(float3 v)
			{
				return 0.2820947917f;
			}

			float Y1(float3 v)
			{
				return 0.4886025119f * v.y;
			}

			float Y2(float3 v)
			{
				return 0.4886025119f * v.z;
			}

			float Y3(float3 v)
			{
				return 0.4886025119f * v.x;
			}

			float Y4(float3 v)
			{
				return 1.0925484306f * v.x * v.y;
			}

			float Y5(float3 v)
			{
				return 1.0925484306f * v.y * v.z;
			}

			float Y6(float3 v)
			{
				return 0.3153915652f * (3.0f * v.z * v.z - 1.0f);
			}

			float Y7(float3 v)
			{
				return 1.0925484306f * v.x * v.z;
			}

			float Y8(float3 v)
			{
				return 0.5462742153f * (v.x * v.x - v.y * v.y);
			}

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.vertex.xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 v = i.texcoord.xyz;
				float4 original = texCUBE(input, i.texcoord);
				float4 approx = c0 * Y0(v) + c1 * Y1(v) + c2 * Y2(v) + c3 * Y3(v) + c4 * Y4(v) + c5 * Y5(v) + c6 * Y6(v) + c7 * Y7(v) + c8 * Y8(v);
				return lerp(original, approx, _Mode);
			}
			ENDCG
		}
	}
	
	Fallback Off
}

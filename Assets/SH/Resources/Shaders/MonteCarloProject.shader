Shader "SH/MonteCarloProject"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass  //y0
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(-0.6666, 0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y0(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
			ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y1
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0, 0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y1(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y2
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0.6666, 0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y2(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y3
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(-0.6666, 0, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y3(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y4
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0, 0, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y4(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y5
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0.6666, 0, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y5(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y6
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(-0.6666, -0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y6(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y7
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0, -0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y7(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}

//--------------------------------------------------------------------------------------------

		Pass  //y8
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SH_Utils.cginc"

			#define PI 3.1415927

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D random_samples;
			samplerCUBE input_cubemap;

			v2f vert(appdata_img v)
			{
				v2f_img o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = o.pos * float4(0.3333, 0.3333, 1, 1) + float4(0.6666, -0.6666, 0, 0);
				o.uv = o.pos.xy * 0.5 + 0.5;// MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 random = tex2D(random_samples, i.uv).xy;
				float phi = 2 * PI * random.x;
				float cos_theta = 2 * random.y - 1;
				float sin_theta = sqrt(1 - cos_theta * cos_theta);

				float3 v = float3(sin_theta * cos(phi), sin_theta * sin(phi), cos_theta);
				float sh = Y8(v);
				float4 proj = texCUBE(input_cubemap, v) * sh;
				proj.a = 1;
				return proj;
			}
				ENDCG
		}
	}
}

float AreaElement(float x, float y)
{
	return atan2(x * y, sqrt(x * x + y * y + 1));
}

float DifferentialSolidAngle(float textureSize, float2 uv)
{
	float inv = 1.0 / textureSize;
	float u = 2.0 * (uv.x + 0.5 * inv) - 1;
	float v = 2.0 * (uv.y + 0.5 * inv) - 1;
	float x0 = u - inv;
	float y0 = v - inv;
	float x1 = u + inv;
	float y1 = v + inv;
	return AreaElement(x0, y0) - AreaElement(x0, y1) - AreaElement(x1, y0) + AreaElement(x1, y1);
}

float rand(float2 n)
{
	return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
}

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

//GET TEXEL DIRECTION VECTOR FROM UV
float3 RfromUV(uint face, float u, float v)
{
	float3 dir;

	switch (face)
	{
	case 0: //+X
		dir.x = 1;
		dir.y = v * -2.0f + 1.0f;
		dir.z = u * -2.0f + 1.0f;
		break;

	case 1: //-X
		dir.x = -1;
		dir.y = v * -2.0f + 1.0f;
		dir.z = u * 2.0f - 1.0f;
		break;

	case 2: //+Y
		dir.x = u * 2.0f - 1.0f;
		dir.y = 1.0f;
		dir.z = v * 2.0f - 1.0f;
		break;

	case 3: //-Y
		dir.x = u * 2.0f - 1.0f;
		dir.y = -1.0f;
		dir.z = v * -2.0f + 1.0f;
		break;

	case 4: //+Z
		dir.x = u * 2.0f - 1.0f;
		dir.y = v * -2.0f + 1.0f;
		dir.z = 1;
		break;

	case 5: //-Z
		dir.x = u * -2.0f + 1.0f;
		dir.y = v * -2.0f + 1.0f;
		dir.z = -1;
		break;
	}

	return dir;
}
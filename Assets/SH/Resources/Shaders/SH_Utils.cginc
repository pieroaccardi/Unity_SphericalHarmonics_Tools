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
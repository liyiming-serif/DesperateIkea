// downsampling
// ===============================================
// grid1
// =============
float grid1(float val, float gx)
{
	return floor(val * gx) / gx;
}

float2 grid1(float2 val, float2 gx)
{
	return float2(grid1(val.x, gx.x), grid1(val.y, gx.y));
}

float3 grid1(float3 val, float3 gx)
{
	return float3(grid1(val.x, gx.x), grid1(val.y, gx.y), grid1(val.z, gx.z));
}

float4 grid1(float4 val, float4 gx)
{
	return float4(grid1(val.x, gx.x), grid1(val.y, gx.y), grid1(val.z, gx.z), grid1(val.w, gx.w));
}

// gridn
// =============
float gridn(float val, float gx)
{
	return floor(val / gx)*gx;
}

float2 gridn(float2 val, float2 gx)
{
	return float2(gridn(val.x, gx.x), gridn(val.y, gx.y));
}

float3 gridn(float3 val, float3 gx)
{
	return float3(gridn(val.x, gx.x), gridn(val.y, gx.y), gridn(val.z, gx.z));
}
// ===============================================

//polar stuff
// ===============================================
static const half PI = 3.14159;

float2 get_polar_uv(float2 uv)
{
	float2 pol = (uv - 0.5) * half2(1, 1);
	pol = float2(fmod(atan2(pol.y, pol.x), 2 * PI), length(pol));
	return pol;
}
// ===============================================

float remap (float value, float from1, float to1, float from2, float to2) {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}

float smoothedger(float pct, float th, float v)
{
	return smoothstep( pct-th, pct, v) - smoothstep( pct, pct+th, v);
}

float2x2 rotate2d(float _angle){
    return float2x2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}


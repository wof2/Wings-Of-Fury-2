
// Fx Map pixel shader

float4 ambientColor;
sampler normalMap;
sampler diffuseMap;
sampler fxMap;
/*
  Fx Map has the following data in the color channels
  RED: specular intensity
  GREEN: specular size (power exponent)
  BLUE: diffuse intensity
  ALPHA: ambient/emmisive intensity (optional)
*/

float diffuse(float3 LightDir, float3 N, float diffLevel)
{
  return max(dot(normalize(LightDir), N), 0.0) * diffLevel;
}

float specular(float3 HalfAngle, float3 N, float specLevel, float specPower)
{
  return specLevel * pow(max( dot( normalize(HalfAngle), N), 0.0), 128.0 * specPower ) ;
}


float4 main(float2 UV: TEXCOORD0, float3 LightDir : TEXCOORD1,
float3 HalfAngle : TEXCOORD2) : COLOR
{
  float4 texdiff = tex2D(diffuseMap, UV);
  // expand the normal vector from the normal map form 0..1 to -1..1
  float3 N = tex2D(normalMap, UV).xyz * 2 - 1;
  float4 fxVals = tex2D(fxMap, UV);

  return texdiff * (ambientColor * fxVals.w + diffuse(LightDir, N, fxVals.z)) + specular(HalfAngle, N, fxVals.x, fxVals.y);
} 
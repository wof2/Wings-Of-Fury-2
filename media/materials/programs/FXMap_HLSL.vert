// FxMap vertex shader

float4x4 worldViewProj_matrix;
float3 lightPosition;
float3 eyePosition;

struct VS_OUTPUT
{
  float4 Pos : POSITION;
  float2 UV : TEXCOORD0;
  float3 LightDir : TEXCOORD1;
  float3 HalfAngle : TEXCOORD2;
};

VS_OUTPUT main(float4 Pos : POSITION, float2 Tex : TEXCOORD0, float3 Normal : NORMAL, float3 Tangent : TEXCOORD1 )
{
  VS_OUTPUT Out = (VS_OUTPUT)0;
  // transform Position
  Out.Pos = mul(worldViewProj_matrix, Pos);

  Out.UV = Tex.xy;

  // calculate tangent space light vector
  // Get object space light direction
  float3 lightDir = lightPosition - Pos.xyz;
  float3 eyeDir = eyePosition - Pos.xyz;

  // compute the 3x3 tranform matrix
  // to transform from object space to tangent space
  float3x3 rotation;
  rotation[0] = Tangent;
  // calculate the binormal (NB we assume both normal and tangent are
  // already normalised)
  rotation[1] = cross(Tangent, Normal);
  rotation[2] = Normal;

  // rotate the light and eye vectors according to tangent space rotation matrix
  lightDir = normalize(mul(rotation, lightDir));
  eyeDir = normalize( mul(rotation, eyeDir) );

  Out.LightDir = lightDir;
  Out.HalfAngle = normalize(eyeDir + lightDir);

  return Out;
} 
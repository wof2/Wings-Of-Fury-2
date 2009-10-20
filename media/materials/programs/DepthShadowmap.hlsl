// Shadow caster vertex program.
void casterVP(
	float4 pos   			: POSITION,
		
	out float2 outDepth		: TEXCOORD0,
	out float4 outPos		: POSITION,

	uniform float4x4 worldViewProj,	
	uniform float4 depthRange
	)
{
	outPos = mul(worldViewProj, pos);

	// depth info for the fragment.
    outDepth.x = outPos.z;
    outDepth.y = outPos.w;
}


// Shadow caster fragment program for high-precision single-channel textures	
void casterFP(
	float2 depth			: TEXCOORD0,	
	out float4 result		: COLOR,
	uniform float4 pssmSplitPoints
	)
	
{
	
	float finalDepth = depth.x / depth.y;

	// just smear across all components 
	// therefore this one needs high individual channel precision
	result = float4(finalDepth, finalDepth, finalDepth, 1);
}





// Expand a range-compressed vector
float3 expand(float3 v)
{
	return (v - 0.5) * 2;
}


float test(sampler2D shadowMap, float4 shadowMapPos)
{	
		shadowMapPos = shadowMapPos / shadowMapPos.w;
		float2 uv = shadowMapPos.xy;		
		return (shadowMapPos.z - tex2D(shadowMap, uv.xy).r);
		
}



float shadowPCF(sampler2D shadowMap, float4 shadowMapPos, float2 offset)
{
	//return 1.0;
		shadowMapPos = shadowMapPos / shadowMapPos.w;
		float2 uv = shadowMapPos.xy;
		float3 o = float3(offset, -offset.x) * 0.3f;
	   
		// Note: We using 2x2 PCF. Good enough and is alot faster.
		float c =       (shadowMapPos.z <= tex2D(shadowMap, uv.xy - o.xy).r) ? 1 : 0; // top left
		c +=            (shadowMapPos.z <= tex2D(shadowMap, uv.xy + o.xy).r) ? 1 : 0; // bottom right
		c +=            (shadowMapPos.z <= tex2D(shadowMap, uv.xy + o.zy).r) ? 1 : 0; // bottom left
		c +=            (shadowMapPos.z <= tex2D(shadowMap, uv.xy - o.zy).r) ? 1 : 0; // top right
		
	   // c += 2.5;
		  
		return (c) / 4;
}

/* Normal mapping plus depth shadowmapping receiver programs
*/
void normalMapShadowReceiverVp(
             float4 position    : POSITION,
			 float3 normal		: NORMAL,
			 float2 uv			: TEXCOORD0,
			 float3 tangent     : TANGENT0,
			 
			 // outputs
			 out float4 outPos    	 : POSITION,
			
			 out float4 outShadowUV	 : TEXCOORD0,
			 out float3 oUv	 		 : TEXCOORD1,
			 out float3 oTSLightDir  : TEXCOORD2,			 
			 out float3 oLightData   : TEXCOORD3, // outDirectional, lightDist, attenuationDist
			
			 out float4 oLightPosition0   : TEXCOORD4,
			 out float4 oLightPosition1   : TEXCOORD5,
			 out float4 oLightPosition2   : TEXCOORD6,
			 out float4 outPosition 	  : TEXCOORD7,
			 // parameters
			 uniform float4 lightPosition, // object space
			 uniform float4x4 world,
			 uniform float4x4 worldViewProj,
			 uniform float4x4 texViewProj,
			 uniform float4 lightAttenuation,
			 
			 uniform float4x4 texWorldViewProjMatrix0,
			 uniform float4x4 texWorldViewProjMatrix1,
			 uniform float4x4 texWorldViewProjMatrix2
			 
			 
			 )
{
  
  
  float lightDist = distance(lightPosition, position);
  /*float xdist = abs(lightPosition.x - position.x);
  float ydist = abs(lightPosition.y - position.y);
  float zdist = abs(lightPosition.z - position.z);
  float lightDist =  sqrt(xdist*xdist + ydist*ydist + zdist*zdist);
  */
  
  if(lightPosition.w <= 0.5)
  {
     oLightData = float3(1, lightDist, lightAttenuation[0]);
  } else
  { 
     oLightData = float3(0, lightDist, lightAttenuation[0]);
  }
   
	float4 worldPos = mul(world, position);
	outPos = mul(worldViewProj, position);

	// calculate shadow map coords
	outShadowUV = mul(texViewProj, worldPos);
#if LINEAR_RANGE
	// adjust by fixed depth bias, rescale into range
	outShadowUV.z = (outShadowUV.z - shadowDepthRange.x) * shadowDepthRange.w;
#endif
	
	// pass the main uvs straight through unchanged
	oUv = float3(uv, outPos.z);

	// calculate tangent space light vector
	// Get object space light direction
	// Non-normalised since we'll do that in the fragment program anyway
	float3 lightDir = lightPosition.xyz -  (position * lightPosition.w);

	// Calculate the binormal (NB we assume both normal and tangent are
	// already normalised)
	// NB looks like nvidia cross params are BACKWARDS to what you'd expect
	// this equates to NxT, not TxN
	float3 binormal = cross(tangent, normal);
	
	// Form a rotation matrix out of the vectors
	float3x3 rotation = float3x3(tangent, binormal, normal);
	
	// Transform the light vector according to this matrix
	oTSLightDir = mul(rotation, lightDir);
	
	
	oLightPosition0 = mul(texWorldViewProjMatrix0, position);
    oLightPosition1 = mul(texWorldViewProjMatrix1, position);
    oLightPosition2 = mul(texWorldViewProjMatrix2, position);
    
    
	
}


void normalMapShadowReceiverFp(
			  float4 shadowUV	: TEXCOORD0,
			  float3 uv			: TEXCOORD1,
			  float3 TSlightDir : TEXCOORD2,			  
			  float3 lightData  : TEXCOORD3,
			 					  
			 //PSSM
			 float4 LightPosition0   : TEXCOORD4,
			 float4 LightPosition1   : TEXCOORD5,
			 float4 LightPosition2   : TEXCOORD6,
             float3  outPosition     : TEXCOORD7,
			  out float4 result	: COLOR,

			  uniform float4 lightColour,
			  uniform float fixedDepthBias,
					  
			  //PSSM
			  uniform float4 invShadowMapSize0,
			  uniform float4 invShadowMapSize1,
			  uniform float4 invShadowMapSize2,
			  uniform float4 shadow_scene_depth_range,
			  uniform float4 pssmSplitPoints,
			  
			  //Standard		  
			 
			  //PSSM
			  uniform sampler2D shadowMap0 : register(s0),
			  uniform sampler2D shadowMap1 : register(s1),
			  uniform sampler2D shadowMap2 : register(s2),   
			  
			 
			  uniform sampler2D   normalMap : register(s3),
			  uniform samplerCUBE normalCubeMap : register(s4))
{

 
    float3 lightVec;
	float3 bumpVec;
	float4 vertexColour;
	
	if(lightData[0] < 0.01) //  isDirectional  = lightData[0]
	{
		if(lightData[1] > lightData[2]) // lightDist > attenuationDist
		{
			result = float4(0,0,0,1);
			return;
		}

		// retrieve normalised light vector, expand from range-compressed
		lightVec = expand(texCUBE(normalCubeMap, TSlightDir).xyz);

		// get bump map vector, again expand from range-compressed
		bumpVec = expand(tex2D(normalMap, uv).xyz);

		// Calculate dot product
		vertexColour = lightColour * dot(bumpVec, lightVec);

		result = vertexColour;
		result *= (lightData[2] - lightData[1]) / lightData[2];
		return;

	}
  
 	// retrieve normalised light vector, expand from range-compressed
	lightVec = expand(texCUBE(normalCubeMap, TSlightDir).xyz);

	// get bump map vector, again expand from range-compressed
	bumpVec = expand(tex2D(normalMap, uv).xyz);


	// Calculate dot product
	vertexColour = lightColour * dot(bumpVec, lightVec);
    
  
	//Shadowing
	float shadowing = 1.0f;
	float ScreenDepth = uv.z;
		
	/*if (test(shadowMap0, LightPosition0) > 0.3)
	{
	    result = float4(1,1,0,1);	
	    return; 
	}*/
	
	// poza obszarem cienia
	if ( ScreenDepth  >  pssmSplitPoints.w)
	{   
	    // result = float4(1,1,0,1);	
		 result = vertexColour;		
		 return; // this is a fix for focused shadow camera setup
	}
	
	float adjust = 0.9975; // testing 		
	if (ScreenDepth <= pssmSplitPoints.y)
	{
		LightPosition0.z *=adjust;
		shadowing = shadowPCF(shadowMap0, LightPosition0, invShadowMapSize0.xy );
		 //result = float4(1,0,0,1);	
		// return;
	}
	else if (ScreenDepth <= pssmSplitPoints.z)
	{
		LightPosition1.z *=adjust;
	    shadowing = shadowPCF(shadowMap1, LightPosition1, invShadowMapSize1.xy );
	    // result = float4(0,1,0,1);	
	    // return;
	}
	else
	{
		LightPosition2.z *=adjust;	
	    shadowing = shadowPCF(shadowMap2, LightPosition2, invShadowMapSize2.xy);
	   // result = float4(0,0,1,1);		
		//return; 
	}
	
	/*float fogFar = 1000;
	float4 fogColour = float4(1,1,1,1);
    float dist = distance(float3(0,0,0), camera_pos);
    if(dist > fogFar) dist = fogFar;
    float fog = 1 - ((fogFar - dist) / fogFar);
    fog = 1.0;*/
         
  //  (depth-fogstart)/(fogend-fogstart)
	result = float4(vertexColour.xyz * shadowing , 1);
	//result = lerp(result, fogColour, fog);
	
}


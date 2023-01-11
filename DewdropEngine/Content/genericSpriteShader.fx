#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//	This is the first character texture that is applied to the shader
extern Texture2D img;

//	This is the second character texture that is applied to the shader
extern Texture2D pal;

//	This is the sampler for the first character texture.
sampler2D imageSampler = sampler_state
{
    Texture = <img>;
    Filter = POINT;
};

//	This is the sampler for the second character texture
sampler2D paletteSampler = sampler_state
{
    Texture = <pal>;
    Filter = POINT;
};

float palIndex;
float4 blend;
float blendMode;
float palSize;
//float delta;

float4 main(float2 texCoord : TEXCOORD) : COLOR
{
    float4 index = tex2D(imageSampler, texCoord);
    float4 baseColor = tex2D(paletteSampler, float2(((index.r * 255.0) + 0.5) / palSize, palIndex));
    float3 baseNoAlpha = baseColor.rgb;
    float3 finalColor = float3(0, 0, 0);
    //float3(
    //  ((-cos(delta * baseColor.r) / 2.0) + 0.5),
    //  ((-cos(delta * baseColor.r) / 2.0) + 0.5),
    //  ((-cos(delta * baseColor.r) / 2.0) + 0.5)
    //  );

    if (blendMode < 0.1)
    {
        finalColor = blend.rgb;
    }
    else if (blendMode < 1.1)
    {
        finalColor = baseNoAlpha * blend.rgb;
    }
    else if (blendMode < 2.1)
    {
        finalColor = 1.0 - (1.0 - blend.rgb) * (1.0 - baseNoAlpha);
    }

    return float4(finalColor, baseColor.a);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
};
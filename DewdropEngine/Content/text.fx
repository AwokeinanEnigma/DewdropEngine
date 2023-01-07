#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

float palIndex;
float4 blend;
float blendMode;
float palSize;
Texture2D SpriteTexture;

//	The sampler object we'll use to sample the texture
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

Texture2D TextureB;
sampler2D TextureBSampler = sampler_state
{
    Texture = <TextureB>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 index = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float2 paletteCoord = ((index.r * 255.0) + 0.5) / palSize;
    paletteCoord.y = palIndex;
    float4 baseColor = tex2D(TextureBSampler, paletteCoord);
    float3 baseNoAlpha = baseColor.rgb;
    float3 finalColor = 0;
    //float3 finalColor = ((-cos(delta * baseColor.r) / 2.0) + 0.5);

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

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float3 GetColorValues(UnityTexture2D Tex, float2 UV, UnitySamplerState SS, float Width, float Samples) {
    float3 max_col = 0;
    float factor = 6.28318530718 / Samples;

    for (int i = 0; i < Samples; i++) {
        float angle = i * factor; 
        float2 offset = float2(cos(angle), sin(angle)) * Width;
        float3 col = SAMPLE_TEXTURE2D(Tex, SS, saturate(UV + offset)).rgb;
        max_col = max(max_col, col);
    }

    float3 self_col = SAMPLE_TEXTURE2D(Tex, SS, UV).rgb;
    float3 is_inside = 1 - saturate(self_col * 100);
    return min(is_inside, max_col);
}

void SampleTexture_float(UnityTexture2D Tex, float2 UV, UnitySamplerState SS, float4 SceneColor, float4 Color1, float4 Color2, float4 Color3, float Width, float Samples, out float4 Out)
{
    float3 color_vals = GetColorValues(Tex, UV, SS, Width, Samples);
    float3 out_col = color_vals * float3(Color1.a, Color2.a, Color3.a);

    float4 red_color = out_col.r * Color1;
    float4 green_color = out_col.g * Color2;
    float4 blue_color = out_col.b * Color3;

    float4 mixed = (red_color + green_color + blue_color) * (1 / max(1, out_col.r + out_col.g + out_col.b));

    Out = lerp(SceneColor, mixed, mixed.a);
}
#endif //MYHLSLINCLUDE_INCLUDED
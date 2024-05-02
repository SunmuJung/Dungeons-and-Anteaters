Shader "Hidden/Custom/Grayscale"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Blend;
	float _red, _green, _blue;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		if(color.r < 0.5)
		{
		color.r *= 2;
		}
		else color.r *= 0.5;

		if(color.g < 0.5)
		{
		color.g *= 2;
		}
		else color.g *= 0.5;

		if(color.b < 0.5)
		{
		color.b *= 2;
		}
		else color.b *= 0.5;
		return color;
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}
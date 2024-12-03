Shader "UnityChan/Eye_URP"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0.8, 0.8, 1, 1)
        
        _MainTex ("Diffuse", 2D) = "white" {}
        _FalloffSampler ("Falloff Control", 2D) = "white" {}
        _RimLightSampler ("RimLight Control", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            // Uniform properties
            float4 _Color;
            float4 _ShadowColor;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_FalloffSampler);
            SAMPLER(sampler_FalloffSampler);
            TEXTURE2D(_RimLightSampler);
            SAMPLER(sampler_RimLightSampler);

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.worldNormal = TransformObjectToWorldNormal(v.normalOS);
                o.worldPos = TransformObjectToWorld(v.positionOS);
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float3 shadowColor = lerp(mainTex.rgb, _ShadowColor.rgb, 0.5);

                float3 viewDir = normalize(GetCameraPositionWS() - i.worldPos);
                float rim = pow(1.0 - saturate(dot(viewDir, normalize(i.worldNormal))), 2.0);
                float3 rimColor = SAMPLE_TEXTURE2D(_RimLightSampler, sampler_RimLightSampler, i.uv).rgb * rim;

                float3 finalColor = mainTex.rgb * shadowColor + rimColor;
                return float4(finalColor, mainTex.a);
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}


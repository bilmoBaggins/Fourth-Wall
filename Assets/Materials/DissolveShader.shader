Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Noise", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        sampler2D _DissolveTex;
        float _DissolveAmount;
        fixed4 _EdgeColor;
        fixed4 _Color;
        fixed4 _EmissionColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DissolveTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 color = _Color;
            mainColor.rgb = color.rgb;
            float noise = tex2D(_DissolveTex, IN.uv_DissolveTex).r;

            // Dissolve mask
            float dissolveMask = step(_DissolveAmount, noise);
            
            // Edge glow effect
            float edge = smoothstep(_DissolveAmount - 0.05, _DissolveAmount, noise);
            mainColor.rgb = lerp(_EdgeColor.rgb, mainColor.rgb, edge);

            mainColor.a *= dissolveMask; // Apply dissolve effect

            // Apply emission
            fixed4 emission = _EmissionColor; // Get emission color
            o.Emission = emission.rgb * mainColor.rgb; // Apply emission

            o.Albedo = mainColor.rgb;
            o.Alpha = mainColor.a;
        }
        ENDCG
    }
}

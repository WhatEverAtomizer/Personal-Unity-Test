Shader "Custom/BlackAndWhitePostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Intensity;

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Convert to grayscale
                float gray = dot(col.rgb, fixed3(0.3, 0.59, 0.11));

                // Apply intensity
                col.rgb = lerp(col.rgb, fixed3(gray, gray, gray), _Intensity);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

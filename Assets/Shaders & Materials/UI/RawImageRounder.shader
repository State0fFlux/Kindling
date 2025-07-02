Shader "UI/UIRoundedRawImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _CornerRadius ("Corner Radius", Range(0, 0.5)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _CornerRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float roundedAlpha(float2 uv, float radius)
            {
                float2 d = abs(uv * 2.0 - 1.0); // Map UV from [0,1] to [-1,1]
                d = max(d - 1.0 + radius * 2.0, 0.0);
                return 1.0 - saturate(dot(d, d) / (radius * radius));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float alphaMask = roundedAlpha(uv, _CornerRadius);
                fixed4 tex = tex2D(_MainTex, uv) * _Color;
                tex.a *= alphaMask;
                return tex;
            }
            ENDCG
        }
    }
}

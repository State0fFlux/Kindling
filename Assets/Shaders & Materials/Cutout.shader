Shader "UI/Cutout"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Overlay Color", Color) = (0,0,0,1)
        _CutoutPos ("Cutout Position (0-1)", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Normalized Radius (0-1)", Range(0,1)) = 0.5
        _ReferenceResolution ("Reference Resolution", Vector) = (1920, 1080, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _CutoutPos;
            float _Radius;
            float4 _ReferenceResolution;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Aspect-corrected UV distance from center
                float2 screenUV = i.uv;
                float2 aspect = float2(_ReferenceResolution.x / _ReferenceResolution.y, 1.0);

                float2 delta = (screenUV - _CutoutPos.xy) * aspect;
                float dist = length(delta);

                // Scale _Radius such that 1 = entire screen visible
                // √(aspect.x^2 + 1^2) is the diagonal of normalized screen with aspect applied
                float maxRadius = length(aspect) / 2;
                float scaledRadius = _Radius * maxRadius;

                if (dist < scaledRadius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}

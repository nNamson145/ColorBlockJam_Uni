Shader "Unlit/GridShader"
{
    Properties
    {
        _GridSize ("Grid Size", Float) = 1
        _LineThickness ("Line Thickness", Range(0.001, 0.1)) = 0.02
        _LineColor ("Line Color", Color) = (0, 0, 0, 1)
        _BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex : POSITION;
            };

            struct v2f 
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _GridSize;
            float _LineThickness;
            float4 _LineColor;
            float4 _BackgroundColor;

            v2f vert (appdata_t v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 gridPos = abs(frac(i.worldPos.xz * _GridSize) - 0.5);
                float gridLine = min(gridPos.x, gridPos.y) < _LineThickness ? 1.0 : 0.0;

                return lerp(_BackgroundColor, _LineColor, gridLine);
            }
            ENDCG
        }
    }
}

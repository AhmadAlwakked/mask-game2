Shader "Custom/HeatVision"
{
    Properties
    {
        _Color("Base Color", Color) = (1,1,1,1)
        _Heat("Heat", Range(0,1)) = 0
        _Enabled("Enabled", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 pos : SV_POSITION; };

            float _Heat;
            float _Enabled;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (_Enabled < 0.5)
                    return _Color;

                fixed4 heatCol;
                heatCol.r = _Heat;
                heatCol.g = 0;
                heatCol.b = 1.0 - _Heat;
                heatCol.a = 1;
                return heatCol;
            }
            ENDCG
        }
    }
}

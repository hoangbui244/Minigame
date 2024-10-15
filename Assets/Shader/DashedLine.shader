Shader "Custom/DashedLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Texture chính, nếu cần
        _Color ("Color", Color) = (1, 1, 1, 1) // Màu của nét vẽ
        _DashSize ("Dash Size", Float) = 1.0 // Kích thước của đoạn đứt
        _GapSize ("Gap Size", Float) = 1.0 // Kích thước của khoảng trống giữa các đoạn
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _DashSize; // Kích thước của nét đứt
            float _GapSize;  // Kích thước của khoảng trống

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Lấy UV từ đoạn line renderer
                float uvX = i.uv.x;

                // Tính toán xem tại điểm UV hiện tại là một phần của nét đứt hay khoảng trống
                float pattern = frac(uvX / (_DashSize + _GapSize)); // Tạo mẫu tuần hoàn

                // Nếu ở trong khoảng đứt (pattern < DashSize / (DashSize + GapSize)), hiển thị đoạn đứt
                if (pattern < _DashSize / (_DashSize + _GapSize))
                {
                    return tex2D(_MainTex, i.uv) * _Color; // Hiển thị màu của nét đứt
                }
                else
                {
                    return float4(0, 0, 0, 0); // Khoảng trống, không hiển thị
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/MultiSliceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutThickness ("Cut Thickness", Range(0, 0.1)) = 0.01 // Độ dày của vết cắt
        _CutColor ("Cut Color", Color) = (0, 0, 0, 1) // Màu của vết cắt
        _CutTransparency ("Cut Transparency", Range(0, 1)) = 0.5 // Độ trong suốt của vết cắt
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _CutThickness;
            fixed4 _CutColor;
            float _CutTransparency; // Tham số độ trong suốt cho vết cắt

            // Mảng chứa vị trí các vết cắt
            float _SlicePositionsX[200];  // Giới hạn tối đa 200 vết cắt
            int _SliceCount; // Số lượng vết cắt

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 c = tex2D(_MainTex, i.uv); // Lấy màu từ texture

                // Kiểm tra tất cả các vị trí cắt
                for (int j = 0; j < _SliceCount; j++)
                {
                    if (abs(i.worldPos.x - _SlicePositionsX[j]) < _CutThickness)
                    {
                        c.rgb = _CutColor.rgb; // Thay đổi màu sắc tại vết cắt
                        c.a = _CutTransparency; // Đặt alpha cho vết cắt
                    }
                }

                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

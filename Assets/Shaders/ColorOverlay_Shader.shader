Shader "Custom/ColorOverlay"
{
    Properties {
        _MainTex ("Main texture", 2D) = "white" {}
        [Toggle] _Overlay ("Overlay", float) = 0
    }
 
    SubShader {
 
        Tags { "Queue"="Transparent" }
 
        Pass {

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
           
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _MainTex_ST;
 
            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            {
                fixed4 c = tex2D(_MainTex, i.uv); // Colour of texture

                if(c[3] == 0)
                    return c;
               
                return fixed4(0.5, 0.5, 0.5, c[3]);
            }
 
            ENDCG
        }
 
    }
    FallBack "Diffuse"
}
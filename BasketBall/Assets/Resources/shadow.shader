// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Shadow" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _ColorTop ("Top Color", Color) = (1,1,1,1)
         _ColorMid ("Mid Color", Color) = (1,1,1,1)
         _ColorBot ("Bot Color", Color) = (1,1,1,1)
         _Middle ("Middle", Range(0.001, 0.999)) = 1
         _Color ("Main Color", Color) = (1,1,1,0.5)
    }
    SubShader {
    	Pass {
         CGPROGRAM
         #pragma vertex vert  
         #pragma fragment frag
         #include "UnityCG.cginc"
 
         fixed4 _ColorTop;
         fixed4 _ColorMid;
         fixed4 _ColorBot;
         float  _Middle;
 
         struct v2f {
             float4 pos : SV_POSITION;
             float4 texcoord : TEXCOORD0;
         };
 
         v2f vert (appdata_full v) {
             v2f o;
             o.pos = UnityObjectToClipPos (v.vertex);
             o.texcoord = v.texcoord;
             return o;
         }
 
         fixed4 frag (v2f i) : COLOR {
             fixed4 c = lerp(_ColorBot, _ColorMid, i.texcoord.y / _Middle) * step(i.texcoord.y, _Middle);
             c += lerp(_ColorMid, _ColorTop, (i.texcoord.y - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.y);
             c.a = 1;
             return c;
         }
         ENDCG
         }
        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        fixed4 _Color;
        sampler2D _MainTex;

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        float4 _MainTex_ST;

        v2f vert (appdata_base v)
        {
            v2f o;
            //o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            float4 vPosWorld = mul( unity_ObjectToWorld, v.vertex);
			//float4 lightDirection = -normalize(_WorldSpaceLightPos0);
			float4 lightDirection = float4(-0.2, -1, 0.1, 1);
			float opposite = vPosWorld.y - 0.1;//_PlaneHeight;
			float cosTheta = -lightDirection.y;	// = lightDirection dot (0,-1,0)
			float hypotenuse = opposite / cosTheta;
			float3 vPos = vPosWorld.xyz + ( lightDirection * hypotenuse );
			o.pos = mul (UNITY_MATRIX_VP, float4(vPos.x, 0.1, vPos.z ,1)); //_PlaneHeight, vPos.z ,1));  
			o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 texcol = tex2D (_MainTex, i.uv);
            return texcol * _Color;
        }
        ENDCG

        }
    }
}
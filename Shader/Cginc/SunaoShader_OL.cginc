//--------------------------------------------------------------
//              Sunao Shader Outline
//                      Copyright (c) 2020 揚茄子研究所
//--------------------------------------------------------------


	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	#include "Lighting.cginc"
	#include "SunaoShader_Function.cginc"

//-------------------------------------変数宣言

//----Main
	uniform sampler2D _MainTex;
	uniform float4    _MainTex_ST;
	uniform float4    _Color;
	uniform float     _Cutout;
	uniform float     _Alpha;
	uniform sampler2D _AlphaMask;
	uniform float     _AlphaMaskStrength;
	uniform float     _Bright;
	uniform bool      _VertexColor;

//----Lighting
	uniform float     _Unlit;
	uniform bool      _MonochromeLit;

//----Outline
	uniform bool      _OutLineEnable;
	uniform sampler2D _OutLineMask;
	uniform float4    _OutLineColor;
	uniform float     _OutLineSize;
	uniform sampler2D _OutLineTexture;
	uniform bool      _OutLineLighthing;
	uniform bool      _OutLineTexColor;
	uniform bool      _OutLineFixScale;

//----Other
	uniform float     _DirectionalLight;
	uniform float     _SHLight;
	uniform float     _PointLight;
	uniform bool      _LightLimitter;

	uniform bool      _EnableGammaFix;
	uniform float     _GammaR;
	uniform float     _GammaG;
	uniform float     _GammaB;

	uniform bool      _EnableBlightFix;
	uniform float     _BlightOutput;
	uniform float     _BlightOffset;

	uniform bool      _LimitterEnable;
	uniform float     _LimitterMax;


//-------------------------------------頂点シェーダ入力構造体

struct VIN {
	float4 vertex  : POSITION;
	float2 uv      : TEXCOORD;
	float3 normal  : NORMAL;
	float3 color   : COLOR;
};


//-------------------------------------頂点シェーダ出力構造体

struct VOUT {
	float4 pos     : SV_POSITION;
	float2 uv      : TEXCOORD0;
	float3 color   : TEXCOORD1;
	float  mask    : TEXCOORD2;

	LIGHTING_COORDS(4 , 5)
	UNITY_FOG_COORDS(6)
};


//-------------------------------------頂点シェーダ

VOUT vert (VIN v) {

	VOUT o;

//----UV
	o.uv    = TRANSFORM_TEX(v.uv, _MainTex);

//----アウトラインマスク
	o.mask  = MonoColor(tex2Dlod(_OutLineMask , float4(o.uv , 0.0f , 0.0f)).rgb);

//----頂点座標変換
	float4 outv = (float4)0.0f;
	if (_OutLineEnable) {

		float4 fixscale;
		fixscale.x  = length(float3(unity_ObjectToWorld[0].x , unity_ObjectToWorld[1].x , unity_ObjectToWorld[2].x));
		fixscale.y  = length(float3(unity_ObjectToWorld[0].y , unity_ObjectToWorld[1].y , unity_ObjectToWorld[2].y));
		fixscale.z  = length(float3(unity_ObjectToWorld[0].z , unity_ObjectToWorld[1].z , unity_ObjectToWorld[2].z));
		fixscale.w  = 1.0f;
		fixscale    = 0.01f / fixscale;

		if (_OutLineFixScale) fixscale *= 10.0f;

		outv  = v.vertex;
		outv += float4(v.normal , 0) * fixscale * _OutLineSize * o.mask;
	}
	o.pos   = UnityObjectToClipPos(outv);
	float3 PosW = mul(unity_ObjectToWorld , v.vertex).xyz;

//----カラー & ライティング
	o.color = _OutLineColor.rgb;

	if (_OutLineTexColor) {
		if (_VertexColor) o.color *= v.color;
		                  o.color *= _Bright;
	}

	if (_OutLineLighthing) {
		float3 Lighting = (float3)0.0f;

		#ifdef PASS_OL_FB
			Lighting  =                ShadeSH9(float4(-1.0f ,  0.0f ,  0.0f , 1.0f));
			Lighting  = max(Lighting , ShadeSH9(float4( 1.0f ,  0.0f ,  0.0f , 1.0f)));
			Lighting  = max(Lighting , ShadeSH9(float4( 0.0f , -1.0f ,  0.0f , 1.0f)));
			Lighting  = max(Lighting , ShadeSH9(float4( 0.0f ,  1.0f ,  0.0f , 1.0f)));
			Lighting  = max(Lighting , ShadeSH9(float4( 0.0f ,  0.0f , -1.0f , 1.0f)));
			Lighting  = max(Lighting , ShadeSH9(float4( 0.0f ,  0.0f ,  1.0f , 1.0f)));
			Lighting *= _SHLight;

			#if VERTEXLIGHT_ON

				float4 VLDirX = unity_4LightPosX0 - PosW.x;
				float4 VLDirY = unity_4LightPosY0 - PosW.y;
				float4 VLDirZ = unity_4LightPosZ0 - PosW.z;

				float4 VLLength = VLightLength(VLDirX , VLDirY , VLDirZ);

				float4 VLAtten  = VLightAtten(VLLength) * _PointLight * 0.9f;
				Lighting += unity_LightColor[0].rgb * VLAtten.x;
				Lighting += unity_LightColor[1].rgb * VLAtten.y;
				Lighting += unity_LightColor[2].rgb * VLAtten.z;
				Lighting += unity_LightColor[3].rgb * VLAtten.w;

			#endif
		#endif

		#ifdef PASS_OL_FB
			Lighting += _LightColor0 * _DirectionalLight;
		#endif
		#ifdef PASS_OL_FA
			Lighting += _LightColor0 * _PointLight * 0.9f;
		#endif

		if (_LightLimitter) Lighting = saturate(Lighting);
		if (_MonochromeLit) Lighting = MonoColor(Lighting);

		o.color *=  Lighting;
	}

//----ポイントライト
	TRANSFER_VERTEX_TO_FRAGMENT(o);

//----フォグ
	UNITY_TRANSFER_FOG(o,o.pos);


	return o;
}


//-------------------------------------フラグメントシェーダ

float4 frag (VOUT IN) : COLOR {

//----カラー計算
	float4 OUT          = float4(0.0f , 0.0f , 0.0f , 1.0f);

	OUT.rgb = tex2D(_OutLineTexture , IN.uv) * IN.color;

	#ifdef PASS_OL_FA
		if (_OutLineLighthing) OUT.rgb *= LIGHT_ATTENUATION(IN);
	#endif

	if (_OutLineTexColor) {
	       OUT.rgb     *= tex2D(_MainTex , IN.uv);
	}

	#if defined(TRANSPARENT) || defined(CUTOUT)
		OUT.a     = saturate(tex2D(_MainTex  , IN.uv).a * _Color.a * _Alpha);
		OUT.a    *= lerp(1.0f , MonoColor(tex2D(_AlphaMask  , IN.uv).rgb) , _AlphaMaskStrength);
	#endif

//----カットアウト

	#ifdef CUTOUT
		clip(OUT.a - _Cutout);
		OUT.a = 1.0f;
	#endif

	clip(IN.mask - 0.2f);

//----ガンマ修正
	if (_EnableGammaFix) {
		_GammaR = max(_GammaR , 0.00001f);
		_GammaG = max(_GammaG , 0.00001f);
		_GammaB = max(_GammaB , 0.00001f);

	       OUT.r        = pow(OUT.r , 1.0f / (1.0f / _GammaR));
	       OUT.g        = pow(OUT.g , 1.0f / (1.0f / _GammaG));
	       OUT.b        = pow(OUT.b , 1.0f / (1.0f / _GammaB));
	}

//----明度修正
	if (_EnableBlightFix) {
	       OUT.rgb     *= _BlightOutput;
	       OUT.rgb      = max(OUT.rgb + _BlightOffset , 0.0f);
	}

//----出力リミッタ
	if (_LimitterEnable) {
	       OUT.rgb      = min(OUT.rgb , _LimitterMax);
	}

//----フォグ
	UNITY_APPLY_FOG(IN.fogCoord, OUT);


	return OUT;
}

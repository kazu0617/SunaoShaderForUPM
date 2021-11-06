//--------------------------------------------------------------
//              Sunao Shader GUI
//                      Copyright (c) 2020 揚茄子研究所
//
// This software is released under the MIT License.
// see LICENSE or http://sunao.orz.hm/agenasulab/ss/LICENSE
//--------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SunaoShader {

	public class GUI : ShaderGUI {

		MaterialProperty MainTex;
		MaterialProperty Color;
		MaterialProperty Alpha;
		MaterialProperty Cutout;
		MaterialProperty BumpMap;
		MaterialProperty OcclusionMap;
		MaterialProperty AlphaMask;
		MaterialProperty Bright;
		MaterialProperty BumpScale;
		MaterialProperty OcclusionStrength;
		MaterialProperty OcclusionMode;
		MaterialProperty AlphaMaskStrength;
		MaterialProperty VertexColor;

		MaterialProperty ShadeMask;
		MaterialProperty Shade;
		MaterialProperty ShadeWidth;
		MaterialProperty ShadeGradient;
		MaterialProperty ShadeColor;
		MaterialProperty CustomShadeColor;
		MaterialProperty ToonEnable;
		MaterialProperty Toon;
		MaterialProperty ToonSharpness;
		MaterialProperty LightMask;
		MaterialProperty LightBoost;
		MaterialProperty Unlit;
		MaterialProperty MonochromeLit;

		MaterialProperty OutLineEnable;
		MaterialProperty OutLineMask;
		MaterialProperty OutLineColor;
		MaterialProperty OutLineSize;
		MaterialProperty OutLineLighting;
		MaterialProperty OutLineTexColor;
		MaterialProperty OutLineTexture;
		MaterialProperty OutLineFixScale;

		MaterialProperty EmissionEnable;
		MaterialProperty EmissionMap;
		MaterialProperty EmissionColor;
		MaterialProperty Emission;
		MaterialProperty EmissionMap2;
		MaterialProperty EmissionMode;
		MaterialProperty EmissionBlink;
		MaterialProperty EmissionFrequency;
		MaterialProperty EmissionWaveform;
		MaterialProperty EmissionScrX;
		MaterialProperty EmissionScrY;
		MaterialProperty EmissionLighting;
		MaterialProperty IgnoreTexAlphaE;
		MaterialProperty EmissionInTheDark;

		MaterialProperty ParallaxEnable;
		MaterialProperty ParallaxMap;
		MaterialProperty ParallaxColor;
		MaterialProperty ParallaxEmission;
		MaterialProperty ParallaxDepth;
		MaterialProperty ParallaxDepthMap;
		MaterialProperty ParallaxMap2;
		MaterialProperty ParallaxMode;
		MaterialProperty ParallaxBlink;
		MaterialProperty ParallaxFrequency;
		MaterialProperty ParallaxWaveform;
		MaterialProperty ParallaxPhaseOfs;
		MaterialProperty ParallaxScrX;
		MaterialProperty ParallaxScrY;
		MaterialProperty ParallaxLighting;
		MaterialProperty IgnoreTexAlphaPE;
		MaterialProperty ParallaxInTheDark;

		MaterialProperty ReflectionEnable;
		MaterialProperty MetallicGlossMap;
		MaterialProperty MatCap;
		MaterialProperty Specular;
		MaterialProperty Metallic;
		MaterialProperty MatCapStrength;
		MaterialProperty GlossMapScale;
		MaterialProperty SpecularTexColor;
		MaterialProperty MetallicTexColor;
		MaterialProperty MatCapTexColor;
		MaterialProperty SpecularSH;
		MaterialProperty ReflectLit;
		MaterialProperty MatCapLit;
		MaterialProperty IgnoreTexAlphaR;

		MaterialProperty RimLitEnable;
		MaterialProperty RimLitMask;
		MaterialProperty RimLitColor;
		MaterialProperty RimLit;
		MaterialProperty RimLitGradient;
		MaterialProperty RimLitLighting;
		MaterialProperty RimLitTexColor;
		MaterialProperty RimLitMode;
		MaterialProperty IgnoreTexAlphaRL;

		MaterialProperty Culling;
		MaterialProperty EnableZWrite;
		MaterialProperty DirectionalLight;
		MaterialProperty SHLight;
		MaterialProperty PointLight;
		MaterialProperty LightLimitter;
		MaterialProperty EnableGammaFix;
		MaterialProperty GammaR;
		MaterialProperty GammaG;
		MaterialProperty GammaB;
		MaterialProperty EnableBlightFix;
		MaterialProperty BlightOutput;
		MaterialProperty BlightOffset;
		MaterialProperty LimitterEnable;
		MaterialProperty LimitterMax;


		bool    MainFoldout       = false;
		bool    ShadingFoldout    = false;
		bool    OutlineFoldout    = false;
		bool    EmissionFoldout   = false;
		bool    ParallaxFoldout   = false;
		bool    ReflectionFoldout = false;
		bool    RimLightFoldout   = false;
		bool    OtherFoldout      = false;

		bool    OnceRun           = true;

		int     Version_H         = 1;
		int     Version_M         = 2;
		int     Version_L         = 2;

		int     VersionC          = 0;
		int     VersionM          = 0;

		public override void OnGUI(MaterialEditor ME , MaterialProperty[] Prop) {

			var mat = (Material)ME.target;

			bool Shader_Cutout      = mat.shader.name.Contains("Cutout");
			bool Shader_Transparent = mat.shader.name.Contains("Transparent");


			MainTex           = FindProperty("_MainTex"           , Prop , false);
			Color             = FindProperty("_Color"             , Prop , false);
			Alpha             = FindProperty("_Alpha"             , Prop , false);
			Cutout            = FindProperty("_Cutout"            , Prop , false);
			BumpMap           = FindProperty("_BumpMap"           , Prop , false);
			OcclusionMap      = FindProperty("_OcclusionMap"      , Prop , false);
			AlphaMask         = FindProperty("_AlphaMask"         , Prop , false);
			Bright            = FindProperty("_Bright"            , Prop , false);
			BumpScale         = FindProperty("_BumpScale"         , Prop , false);
			OcclusionStrength = FindProperty("_OcclusionStrength" , Prop , false);
			OcclusionMode     = FindProperty("_OcclusionMode"     , Prop , false);
			AlphaMaskStrength = FindProperty("_AlphaMaskStrength" , Prop , false);
			VertexColor       = FindProperty("_VertexColor"       , Prop , false);

			ShadeMask         = FindProperty("_ShadeMask"         , Prop , false);
			Shade             = FindProperty("_Shade"             , Prop , false);
			ShadeWidth        = FindProperty("_ShadeWidth"        , Prop , false);
			ShadeGradient     = FindProperty("_ShadeGradient"     , Prop , false);
			ShadeColor        = FindProperty("_ShadeColor"        , Prop , false);
			CustomShadeColor  = FindProperty("_CustomShadeColor"  , Prop , false);

			ToonEnable        = FindProperty("_ToonEnable"        , Prop , false);
			Toon              = FindProperty("_Toon"              , Prop , false);
			ToonSharpness     = FindProperty("_ToonSharpness"     , Prop , false);

			LightMask         = FindProperty("_LightMask"         , Prop , false);
			LightBoost        = FindProperty("_LightBoost"        , Prop , false);
			Unlit             = FindProperty("_Unlit"             , Prop , false);
			MonochromeLit     = FindProperty("_MonochromeLit"     , Prop , false);

			OutLineEnable     = FindProperty("_OutLineEnable"     , Prop , false);
			OutLineMask       = FindProperty("_OutLineMask"       , Prop , false);
			OutLineColor      = FindProperty("_OutLineColor"      , Prop , false);
			OutLineSize       = FindProperty("_OutLineSize"       , Prop , false);
			OutLineLighting   = FindProperty("_OutLineLighthing"  , Prop , false);
			OutLineTexColor   = FindProperty("_OutLineTexColor"   , Prop , false);
			OutLineTexture    = FindProperty("_OutLineTexture"    , Prop , false);
			OutLineFixScale   = FindProperty("_OutLineFixScale"   , Prop , false);

			EmissionEnable    = FindProperty("_EmissionEnable"    , Prop , false);
			EmissionMap       = FindProperty("_EmissionMap"       , Prop , false);
			EmissionColor     = FindProperty("_EmissionColor"     , Prop , false);
			Emission          = FindProperty("_Emission"          , Prop , false);
			EmissionMap2      = FindProperty("_EmissionMap2"      , Prop , false);
			EmissionMode      = FindProperty("_EmissionMode"      , Prop , false);
			EmissionBlink     = FindProperty("_EmissionBlink"     , Prop , false);
			EmissionFrequency = FindProperty("_EmissionFrequency" , Prop , false);
			EmissionWaveform  = FindProperty("_EmissionWaveform"  , Prop , false);
			EmissionScrX      = FindProperty("_EmissionScrX"      , Prop , false);
			EmissionScrY      = FindProperty("_EmissionScrY"      , Prop , false);
			EmissionLighting  = FindProperty("_EmissionLighting"  , Prop , false);
			IgnoreTexAlphaE   = FindProperty("_IgnoreTexAlphaE"   , Prop , false);
			EmissionInTheDark = FindProperty("_EmissionInTheDark" , Prop , false);

			ParallaxEnable    = FindProperty("_ParallaxEnable"    , Prop , false);
			ParallaxMap       = FindProperty("_ParallaxMap"       , Prop , false);
			ParallaxColor     = FindProperty("_ParallaxColor"     , Prop , false);
			ParallaxEmission  = FindProperty("_ParallaxEmission"  , Prop , false);
			ParallaxDepth     = FindProperty("_ParallaxDepth"     , Prop , false);
			ParallaxDepthMap  = FindProperty("_ParallaxDepthMap"  , Prop , false);
			ParallaxMap2      = FindProperty("_ParallaxMap2"      , Prop , false);
			ParallaxMode      = FindProperty("_ParallaxMode"      , Prop , false);
			ParallaxBlink     = FindProperty("_ParallaxBlink"     , Prop , false);
			ParallaxFrequency = FindProperty("_ParallaxFrequency" , Prop , false);
			ParallaxWaveform  = FindProperty("_ParallaxWaveform"  , Prop , false);
			ParallaxPhaseOfs  = FindProperty("_ParallaxPhaseOfs"  , Prop , false);
			ParallaxScrX      = FindProperty("_ParallaxScrX"      , Prop , false);
			ParallaxScrY      = FindProperty("_ParallaxScrY"      , Prop , false);
			ParallaxLighting  = FindProperty("_ParallaxLighting"  , Prop , false);
			IgnoreTexAlphaPE  = FindProperty("_IgnoreTexAlphaPE"  , Prop , false);
			ParallaxInTheDark = FindProperty("_ParallaxInTheDark" , Prop , false);

			ReflectionEnable  = FindProperty("_ReflectionEnable"  , Prop , false);
			MetallicGlossMap  = FindProperty("_MetallicGlossMap"  , Prop , false);
			MatCap            = FindProperty("_MatCap"            , Prop , false);
			Specular          = FindProperty("_Specular"          , Prop , false);
			Metallic          = FindProperty("_Metallic"          , Prop , false);
			MatCapStrength    = FindProperty("_MatCapStrength"    , Prop , false);
			GlossMapScale     = FindProperty("_GlossMapScale"     , Prop , false);
			SpecularTexColor  = FindProperty("_SpecularTexColor"  , Prop , false);
			MetallicTexColor  = FindProperty("_MetallicTexColor"  , Prop , false);
			MatCapTexColor    = FindProperty("_MatCapTexColor"    , Prop , false);
			SpecularSH        = FindProperty("_SpecularSH"        , Prop , false);
			ReflectLit        = FindProperty("_ReflectLit"        , Prop , false);
			MatCapLit         = FindProperty("_MatCapLit"         , Prop , false);
			IgnoreTexAlphaR   = FindProperty("_IgnoreTexAlphaR"   , Prop , false);

			RimLitEnable      = FindProperty("_RimLitEnable"      , Prop , false);
			RimLitMask        = FindProperty("_RimLitMask"        , Prop , false);
			RimLitColor       = FindProperty("_RimLitColor"       , Prop , false);
			RimLit            = FindProperty("_RimLit"            , Prop , false);
			RimLitGradient    = FindProperty("_RimLitGradient"    , Prop , false);
			RimLitLighting    = FindProperty("_RimLitLighthing"   , Prop , false);
			RimLitTexColor    = FindProperty("_RimLitTexColor"    , Prop , false);
			RimLitMode        = FindProperty("_RimLitMode"        , Prop , false);
			IgnoreTexAlphaRL  = FindProperty("_IgnoreTexAlphaRL"  , Prop , false);

			Culling           = FindProperty("_Culling"           , Prop , false);
			EnableZWrite      = FindProperty("_EnableZWrite"      , Prop , false);
			DirectionalLight  = FindProperty("_DirectionalLight"  , Prop , false);
			SHLight           = FindProperty("_SHLight"           , Prop , false);
			PointLight        = FindProperty("_PointLight"        , Prop , false);
			LightLimitter     = FindProperty("_LightLimitter"     , Prop , false);
			EnableGammaFix    = FindProperty("_EnableGammaFix"    , Prop , false);
			GammaR            = FindProperty("_GammaR"            , Prop , false);
			GammaG            = FindProperty("_GammaG"            , Prop , false);
			GammaB            = FindProperty("_GammaB"            , Prop , false);
			EnableBlightFix   = FindProperty("_EnableBlightFix"   , Prop , false);
			BlightOutput      = FindProperty("_BlightOutput"      , Prop , false);
			BlightOffset      = FindProperty("_BlightOffset"      , Prop , false);
			LimitterEnable    = FindProperty("_LimitterEnable"    , Prop , false);
			LimitterMax       = FindProperty("_LimitterMax"       , Prop , false);


			if (OnceRun) {
				OnceRun = false;

				VersionC = Version_H               * 10000 + Version_M               * 100 + Version_L;
				VersionM = mat.GetInt("_VersionH") * 10000 + mat.GetInt("_VersionM") * 100 + mat.GetInt("_VersionL");

				if (VersionC > VersionM) {
					mat.SetInt("_VersionH" , Version_H);
					mat.SetInt("_VersionM" , Version_M);
					mat.SetInt("_VersionL" , Version_L);
					VersionM = VersionC;
				}

				var Keyword = new List<string>(mat.shaderKeywords);
				foreach (string Key in Keyword) {
					mat.DisableKeyword(Key);
				}
			}

			if (VersionC < VersionM) {
				using (new EditorGUILayout.VerticalScope("box")) {
					EditorGUILayout.HelpBox(
						"このマテリアルは現在お使いのSunao Shaderよりも新しいバージョン(" + mat.GetInt("_VersionH") + "." + mat.GetInt("_VersionM") + "." + mat.GetInt("_VersionL") + ")で作られています。\n" +
						"そのため一部表現が正しくなかったり設定値に互換がない可能性があります。\n" +
						"新しいバージョンのSunao Shaderが公開されている場合はアップデートをおすすめします。\n" +
						"現在お使いのSunao Shaderのバージョンは " + Version_H + "." + Version_M + "." + Version_L + " です。" ,
						MessageType.Warning
					);
					if (GUILayout.Button("無視する")) {
						mat.SetInt("_VersionH" , Version_H);
						mat.SetInt("_VersionM" , Version_M);
						mat.SetInt("_VersionL" , Version_L);
						VersionM = VersionC;
					}
				}
			}


			GUILayout.Label("Main", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				using (new EditorGUILayout.VerticalScope("box")) {

					GUILayout.Label("Main Color", EditorStyles.boldLabel);

					ME.TexturePropertySingleLine (new GUIContent("Main Texture") , MainTex , Color);
					ME.TextureScaleOffsetProperty(MainTex);

					if (Shader_Cutout     ) ME.ShaderProperty(Cutout , new GUIContent("Cutout"));
					if (Shader_Transparent) ME.ShaderProperty(Alpha  , new GUIContent("Alpha" ));

					
				}

				using (new EditorGUILayout.VerticalScope("box")) {

					GUILayout.Label("Texture Maps", EditorStyles.boldLabel);

					ME.TexturePropertySingleLine(new GUIContent("Normal Map") , BumpMap     );
					ME.TexturePropertySingleLine(new GUIContent("Occlusion" ) , OcclusionMap);
					if (Shader_Cutout || Shader_Transparent) ME.TexturePropertySingleLine(new GUIContent("Alpha Mask") , AlphaMask);
				}

				EditorGUI.indentLevel ++;

				if (mat.GetInt("_MainFO") == 1) MainFoldout = true;
				MainFoldout = EditorGUILayout.Foldout(MainFoldout , "Advanced Settings" , EditorStyles.boldFont);

				if (MainFoldout) {
					mat.SetInt("_MainFO" , 1);

					ME.ShaderProperty(Bright , new GUIContent("Brightness"));

					if (BumpMap.textureValue      != null) {
						ME.ShaderProperty(BumpScale         , new GUIContent("Normal Map Scale"  ));
					}
					if (OcclusionMap.textureValue != null) {
						ME.ShaderProperty(OcclusionStrength , new GUIContent("Occlusion Strength"));
						ME.ShaderProperty(OcclusionMode     , new GUIContent("Occlusion Mode"    ));
					}
					if ((AlphaMask.textureValue   != null) && (Shader_Cutout || Shader_Transparent)) {
						ME.ShaderProperty(AlphaMaskStrength , new GUIContent("Alpha Mask Strength"));
					}

					ME.ShaderProperty(VertexColor , new GUIContent("Use Vertex Color"  ));
				} else {
					mat.SetInt("_MainFO" , 0);
				}

				EditorGUI.indentLevel --;

			}


			GUILayout.Label("Shading & Lighting", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				using (new EditorGUILayout.VerticalScope("box")) {

					GUILayout.Label("Shading", EditorStyles.boldLabel);

					ME.TexturePropertySingleLine(new GUIContent("Shade Mask") , ShadeMask);
					ME.ShaderProperty(Shade , new GUIContent("Shade Strength"));

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_ShadingFO") == 1) ShadingFoldout = true;
					ShadingFoldout = EditorGUILayout.Foldout(ShadingFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (ShadingFoldout) {
						mat.SetInt("_ShadingFO" , 1);

						ME.ShaderProperty(ShadeWidth       , new GUIContent("Shade Width"       ));
						ME.ShaderProperty(ShadeGradient    , new GUIContent("Shade Gradient"    ));
						ME.ShaderProperty(ShadeColor       , new GUIContent("Shade Color"       ));
						ME.ShaderProperty(CustomShadeColor , new GUIContent("Custom Shade Color"));
					} else {
						mat.SetInt("_ShadingFO" , 0);
					}

					EditorGUI.indentLevel --;

				}

				using (new EditorGUILayout.VerticalScope("box")) {

					GUILayout.Label("Toon Shading", EditorStyles.boldLabel);

					ME.ShaderProperty(ToonEnable , new GUIContent("Enable Toon Shading"));
					if (mat.GetInt("_ToonEnable") == 1) {
						ME.ShaderProperty(Toon           , new GUIContent("Toon"           ));
						ME.ShaderProperty(ToonSharpness  , new GUIContent("Toon Sharpness" ));
					}

				}

				using (new EditorGUILayout.VerticalScope("box")) {

					GUILayout.Label("Lighting", EditorStyles.boldLabel);

					ME.TexturePropertySingleLine(new GUIContent("Lighting Boost Mask") , LightMask);
					if (LightMask.textureValue != null) {
						ME.ShaderProperty(LightBoost , new GUIContent("Lighting Boost"));
					}

					ME.ShaderProperty(Unlit , new GUIContent("Unlighting"));
					ME.ShaderProperty(MonochromeLit , new GUIContent("Monochrome Lighting"));

				}
			}


			GUILayout.Label("Outline", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				ME.ShaderProperty(OutLineEnable , new GUIContent("Enable Outline"));

				if (mat.GetInt("_OutLineEnable") == 1) {
					ME.TexturePropertySingleLine(new GUIContent("Outline Mask") , OutLineMask);
					ME.ShaderProperty(OutLineColor , new GUIContent("Outline Color"));
					ME.ShaderProperty(OutLineSize  , new GUIContent("Outline Scale" ));

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_OutlineFO") == 1) OutlineFoldout = true;
					OutlineFoldout = EditorGUILayout.Foldout(OutlineFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (OutlineFoldout) {
						mat.SetInt("_OutlineFO" , 1);

						ME.ShaderProperty(OutLineLighting  , new GUIContent("Use Light Color" ));
						ME.ShaderProperty(OutLineTexColor  , new GUIContent("Use Main Texture"));
						ME.TexturePropertySingleLine(new GUIContent("Outline Texture") , OutLineTexture);
						ME.ShaderProperty(OutLineFixScale  , new GUIContent("x10 Scale"       ));
					} else {
						mat.SetInt("_OutlineFO" , 0);
					}

					EditorGUI.indentLevel --;

				}
			}


			GUILayout.Label("Emission", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				ME.ShaderProperty(EmissionEnable , new GUIContent("Enable Emission"));

				if (mat.GetInt("_EmissionEnable") == 1) {

					ME.TexturePropertySingleLine(new GUIContent("Emission Mask") , EmissionMap);
					if (EmissionMap.textureValue != null) {
						ME.TextureScaleOffsetProperty(EmissionMap);
					}

					ME.ShaderProperty(EmissionColor , new GUIContent("Emission Color"    ));
					ME.ShaderProperty(Emission      , new GUIContent("Emission Intensity"));

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_EmissionFO") == 1) EmissionFoldout = true;
					EmissionFoldout = EditorGUILayout.Foldout(EmissionFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (EmissionFoldout) {
						mat.SetInt("_EmissionFO" , 1);

						ME.TexturePropertySingleLine(new GUIContent("2nd Emission Mask") , EmissionMap2);
						if (EmissionMap2.textureValue != null) {
							ME.TextureScaleOffsetProperty(EmissionMap2);
						}

						ME.ShaderProperty(EmissionMode      , new GUIContent("Emission Mode"   ));

						ME.ShaderProperty(EmissionBlink     , new GUIContent("Blink"           ));
						if (mat.GetFloat("_EmissionBlink") > 0) {
							ME.ShaderProperty(EmissionFrequency , new GUIContent("Frequency"   ));
							ME.ShaderProperty(EmissionWaveform  , new GUIContent("Waveform"    ));
						}

						ME.ShaderProperty(EmissionScrX      , new GUIContent("Scroll X"        ));
						ME.ShaderProperty(EmissionScrY      , new GUIContent("Scroll Y"        ));

						ME.ShaderProperty(EmissionLighting , new GUIContent("Use Lighting"     ));

						if (Shader_Transparent) {
							ME.ShaderProperty(IgnoreTexAlphaE , new GUIContent("Ignore Main Texture Alpha"));
						}

						ME.ShaderProperty(EmissionInTheDark , new GUIContent("Only in the Dark"));
					} else {
						mat.SetInt("_EmissionFO" , 0);
					}

					EditorGUI.indentLevel --;

				}
			}


			GUILayout.Label("Parallax Emission", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				ME.ShaderProperty(ParallaxEnable , new GUIContent("Enable Parallax Emission"));

				if (mat.GetInt("_ParallaxEnable") == 1) {

					ME.TexturePropertySingleLine(new GUIContent("Parallax Emission Mask") , ParallaxMap);
					if (ParallaxMap.textureValue != null) {
						ME.TextureScaleOffsetProperty(ParallaxMap);
					}

					ME.ShaderProperty(ParallaxColor    , new GUIContent("Emission Color"    ));
					ME.ShaderProperty(ParallaxEmission , new GUIContent("Emission Intensity"));
					ME.ShaderProperty(ParallaxDepth    , new GUIContent("Parallax Depth"));

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_ParallaxFO") == 1) ParallaxFoldout = true;
					ParallaxFoldout = EditorGUILayout.Foldout(ParallaxFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (ParallaxFoldout) {
						mat.SetInt("_ParallaxFO" , 1);

						ME.TexturePropertySingleLine(new GUIContent("Parallax Depth Mask"       ) , ParallaxDepthMap);
						if (ParallaxDepthMap.textureValue != null) {
							ME.TextureScaleOffsetProperty(ParallaxDepthMap);
						}

						ME.TexturePropertySingleLine(new GUIContent("2nd Parallax Emission Mask") , ParallaxMap2    );
						if (ParallaxMap2.textureValue != null) {
							ME.TextureScaleOffsetProperty(ParallaxMap2);
						}

						ME.ShaderProperty(ParallaxMode      , new GUIContent("Emission Mode"   ));

						ME.ShaderProperty(ParallaxBlink     , new GUIContent("Blink"           ));
						if (mat.GetFloat("_ParallaxBlink") > 0) {
							ME.ShaderProperty(ParallaxFrequency , new GUIContent("Frequency"   ));
							ME.ShaderProperty(ParallaxWaveform  , new GUIContent("Waveform"    ));
							ME.ShaderProperty(ParallaxPhaseOfs  , new GUIContent("Phase Offset"));
						}

						ME.ShaderProperty(ParallaxScrX      , new GUIContent("Scroll X"        ));
						ME.ShaderProperty(ParallaxScrY      , new GUIContent("Scroll Y"        ));

						ME.ShaderProperty(ParallaxLighting , new GUIContent("Use Lighting"     ));

						if (Shader_Transparent) {
							ME.ShaderProperty(IgnoreTexAlphaPE , new GUIContent("Ignore Main Texture Alpha"));
						}

						ME.ShaderProperty(ParallaxInTheDark , new GUIContent("Only in the Dark"));
					} else {
						mat.SetInt("_ParallaxFO" , 0);
					}

					EditorGUI.indentLevel --;

				}
			}


			GUILayout.Label("Reflection", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				ME.ShaderProperty(ReflectionEnable , new GUIContent("Enable Reflection"));

				if (mat.GetInt("_ReflectionEnable") == 1) {
					ME.TexturePropertySingleLine(new GUIContent("Reflection Mask") , MetallicGlossMap);

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Reflection Environment", EditorStyles.boldLabel);

						ME.ShaderProperty(Specular      , new GUIContent("Specular Intensity"));
						ME.ShaderProperty(Metallic      , new GUIContent("Metallic"          ));
						ME.ShaderProperty(GlossMapScale , new GUIContent("Smoothness"        ));

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Material Capture", EditorStyles.boldLabel);

						ME.TexturePropertySingleLine(new GUIContent("Material Capture") , MatCap);
						if (MatCap.textureValue != null) {
							ME.ShaderProperty(MatCapStrength , new GUIContent("MatCap Strength"));
						}

					}

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_ReflectionFO") == 1) ReflectionFoldout = true;
					ReflectionFoldout = EditorGUILayout.Foldout(ReflectionFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (ReflectionFoldout) {
						mat.SetInt("_ReflectionFO" , 1);

						ME.ShaderProperty(SpecularTexColor , new GUIContent("Use Main Texture for Specular"));
						ME.ShaderProperty(MetallicTexColor , new GUIContent("Use Main Texture for Metallic"));
						ME.ShaderProperty(MatCapTexColor   , new GUIContent("Use Main Texture for MatCap"  ));
						ME.ShaderProperty(SpecularSH       , new GUIContent("SH Light Specular"            ));
						ME.ShaderProperty(ReflectLit       , new GUIContent("Use Light Color for Metallic" ));
						ME.ShaderProperty(MatCapLit        , new GUIContent("Use Light Color for MatCap"   ));

						if (Shader_Transparent) {
							ME.ShaderProperty(IgnoreTexAlphaR  , new GUIContent("Ignore Main Texture Alpha"));
						}

					} else {
						mat.SetInt("_ReflectionFO" , 0);
					}

					EditorGUI.indentLevel --;

				}
			}


			GUILayout.Label("Rim Lighting", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				ME.ShaderProperty(RimLitEnable , new GUIContent("Enable Rim Lighting"));

				if (mat.GetInt("_RimLitEnable") == 1) {

					ME.TexturePropertySingleLine(new GUIContent("Rim Light Mask") , RimLitMask);
					ME.ShaderProperty(RimLitColor , new GUIContent("Rim Light Color"));
					ME.ShaderProperty(RimLit      , new GUIContent("Rim Lighting"   ));

					EditorGUI.indentLevel ++;

					if (mat.GetInt("_RimLightingFO") == 1) RimLightFoldout = true;
					RimLightFoldout = EditorGUILayout.Foldout(RimLightFoldout , "Advanced Settings" , EditorStyles.boldFont);

					if (RimLightFoldout) {
						mat.SetInt("_RimLightingFO" , 1);

						ME.ShaderProperty(RimLitGradient  , new GUIContent("Rim Light Gradient"));
						ME.ShaderProperty(RimLitLighting  , new GUIContent("Use Light Color"   ));
						ME.ShaderProperty(RimLitTexColor  , new GUIContent("Use Main Texture"  ));
						ME.ShaderProperty(RimLitMode      , new GUIContent("Rim Light Mode"    ));

						if (Shader_Transparent) {
							ME.ShaderProperty(IgnoreTexAlphaRL , new GUIContent("Ignore Main Texture Alpha"));
						}

					} else {
						mat.SetInt("_RimLightingFO" , 0);
					}

					EditorGUI.indentLevel --;
				}
			}


			GUILayout.Label("Other", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope("box")) {

				if (mat.GetInt("_OtherSettingsFO") == 1) OtherFoldout = true;

					EditorGUI.indentLevel ++;

					if (OtherFoldout) {
						OtherFoldout = EditorGUILayout.Foldout(OtherFoldout , ""              , EditorStyles.boldFont);
					} else {
						OtherFoldout = EditorGUILayout.Foldout(OtherFoldout , "Show Settings" , EditorStyles.boldFont);
					}

					EditorGUI.indentLevel --;

				if (OtherFoldout) {
					mat.SetInt("_OtherSettingsFO" , 1);

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Culling Mode" , EditorStyles.boldLabel);

						ME.ShaderProperty(Culling , new GUIContent("Culling Mode"));

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Z Write"      , EditorStyles.boldLabel);

						ME.ShaderProperty(EnableZWrite , new GUIContent("Enable Z Write"));

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Lights"       , EditorStyles.boldLabel);

						ME.ShaderProperty(DirectionalLight , new GUIContent("Directional Light Intensity"));
						ME.ShaderProperty(SHLight          , new GUIContent("SH Light Intensity"         ));
						ME.ShaderProperty(PointLight       , new GUIContent("Point/Spot Light Intensity" ));
						ME.ShaderProperty(LightLimitter    , new GUIContent("Light Intensity Limitter"   ));

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Gamma Fix"    , EditorStyles.boldLabel);

						ME.ShaderProperty(EnableGammaFix   , new GUIContent("Enable Gamma Fix"));
						if (mat.GetInt("_EnableGammaFix") == 1) {
							ME.ShaderProperty(GammaR , new GUIContent("Gamma R"));
							ME.ShaderProperty(GammaG , new GUIContent("Gamma G"));
							ME.ShaderProperty(GammaB , new GUIContent("Gamma B"));
						}

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Brightness Fix" , EditorStyles.boldLabel);

						ME.ShaderProperty(EnableBlightFix  , new GUIContent("Enable Brightness Fix"));
						if (mat.GetInt("_EnableBlightFix") == 1) {
							ME.ShaderProperty(BlightOutput  , new GUIContent("Output Blightness"));
							ME.ShaderProperty(BlightOffset  , new GUIContent("Blightness Offset"));
						}

					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Output Limitter" , EditorStyles.boldLabel);

						ME.ShaderProperty(LimitterEnable    , new GUIContent("Enable Output Limitter"));
						if (mat.GetInt("_LimitterEnable") == 1) {
							ME.ShaderProperty(LimitterMax    , new GUIContent("Limitter Max"));
						}
					}

					using (new EditorGUILayout.VerticalScope("box")) {

						GUILayout.Label("Render Queue" , EditorStyles.boldLabel);

						ME.RenderQueueField();
					
					}

				} else {
					mat.SetInt("_OtherSettingsFO" , 0);
				}
			}


			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Sunao Shader " + Version_H + "." + Version_M + "." + Version_L , EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();

		}
	}

	public class SToggleDrawer : MaterialPropertyDrawer {

		public override void OnGUI(Rect Pos, MaterialProperty Prop, GUIContent Label, MaterialEditor ME) {

			bool IN  = false;
			if (Prop.floatValue >= 0.5f) IN = true;

			var  OUT = EditorGUI.Toggle(Pos, Label, IN);
			

			if (OUT) {
				Prop.floatValue = 1.0f;
			} else {
				Prop.floatValue = 0.0f;
			}
		}
	}

}

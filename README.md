# Sunao Shader for UPM

このパッケージは、MIT License のもと公開されているSunaoShaderをUPMで使用できるよう、kazu0617がGitHubに上げなおしたものになります。
UPMを活用して使用する場合は、 https://docs.unity3d.com/ja/current/Manual/upm-ui-giturl.html を参考にしつつgiturlを指定してください。

このバージョンを指定する場合は https://github.com/kazu0617/SunaoShaderForUPM#1.3.0 となります。

以下、公式のReadme.txtを記載します。

```txt
//-----------------------------------------------------------------------------
//              Sunao Shader    Ver 1.3.0
//
//                      Copyright (c) 2020 揚茄子研究所
//                              Twitter : @SUNAO_VRC
//                              VRChat  : SUNAO_
//-----------------------------------------------------------------------------
```

Sunao Shaderは、VRChatでの使用を想定して作られたUnity用シェーダーです。
VRChatにおける多種多様なライティング環境に極力対応するよう設計されていますので、
ワールドの雰囲気にマッチした自然な表現を行うことができます。 

Sunao Shaderは以下の機能を持っています。

- カスタマイズできる陰影
- トゥーンシェーディング
- ライティングの調整
- アウトライン
- 時間変化可能なエミッション
- 視差エミッション
- 詳細設定が可能なリフレクション(スペキュラ反射 / 環境マッピング / MatCap)
- リムライティング
- ノーマルマップ
- 各種テクスチャマスク
- 両面描画
- ShadowCaster(他のオブジェクトに影を落とす機能)
- 頂点カラー
- VRChatのセーフティ発動時にテクスチャや一部パラメータを維持
- 頂点ライト(重要でないライト)を含めた全ライティングに対応
- リアルタイムライトが無い場合でもライト方向を疑似的に計算
- etc ...

また、シェーダーキーワードを一切使用していませんので、VRChat上で何体もの
アバターが表示されても描画がおかしくなることがありません。

解説書は http://sunao.orz.hm/agenasulab/ss/ にあります。

本シェーダーはMIT Licenseにて公開されています。
改変、再配布、アセットやモデルデータへの同梱（有償、無償問わず）等は
MIT Licenseの範囲内で自由に行うことができます。

詳しくは同梱のLICENSE及び http://sunao.orz.hm/agenasulab/ss/LICENSE を参照して
ください。
(日本語訳: https://ja.osdn.net/projects/opensource/wiki/licenses%2FMIT_license )

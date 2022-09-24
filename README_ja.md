# KatamariFix

言語: [English](README.md)

塊魂アンコール (Steam 版のみ) のバグを修正します。

このプロジェクトは進行中 (work in progress) です。

## インストール方法

1. [BepInEx](https://github.com/BepInEx/BepInEx) をインストールする。
2. [リリースページ](https://github.com/KashEight/KatamariFix/releases) からこのプラグインをダウンロードする。
3. `KatamariFix.dll` を `BepInEx/plugins` フォルダーに展開する。

**注意**: このプラグインは BepInEx 6 が必要です！
また、依存関係のライブラリのバグのため、修正済みの [MonoMod.Common](https://github.com/BepInEx/BepInEx/files/9275584/MMTestFix.zip) をダウンロードして、`BepInEx/core` に展開する作業が必要です！ (ref: https://github.com/BepInEx/BepInEx/issues/458)

## 修正したバグ

- オーディオバグ
  - ゲームの BGM/SE (ゲームムービーも含む) のデフォルトが大きすぎる

## 計画中

- コレクションバグ
  - 集めていない名前が記録される

## プラグインのバグ / 新しいバグ

プラグインがうまく動作しないまたは, 何か問題がある場合, Issue か Pull Request をくれればありがたいです。
本体のバグを見つけた場合も同様にお願いします！

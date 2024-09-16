# Script API をはじめよう (Windows 向け環境構築)

# 使用する環境
- Visual Studio Code 
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.8.0 (npmパッケージ)
- Minecraft 統合版もしくは教育版

# できるようになること
- Script API がなにかがわかる
- Script API を実装するための環境構築
- Manifest ファイルへの記載方法がわかる
- TypeScript を使用したスクリプト生成方法がわかる

# Script API とは何か
プログラム (JavaScript) を使用して、特定の条件でコマンドを動かしたり、自動化したり、ワールドの設定を変更したりと様々な仕組みをつくることのできる機能です。mcfunction ファイルを作ることでビヘイビアパックから任意のコマンドを実行することもできますが、プログラミング特有の機能 (変数、反復処理、条件分岐、配列など)を使えるので、コマンドを組むより効率的な開発が可能です。


# エディタとトランスパイラの準備
エディタとは、プログラムを書くのに便利なアプリです。例えばキーワードに色づけしたり、開業やタブを自動で入れたりと様々な機能があります。今回は Visual Studio Code (以降 VSCode )を使用して、ここでプログラムを作成するための準備をしていきます。

## Visual Studio Code の準備
まずは公式サイトからダウンロード＆インストールしてください。

https://code.visualstudio.com/download

以上です。

・・・と、言いたいところですが、より便利にするために拡張機能の導入をおすすめします。(なくても書けます)

- [JavaScript (ES6) code snipetts](https://marketplace.visualstudio.com/items?itemName=xabikos.JavaScriptSnippets) - JavaScript のキーワードに色付け (シンタックスハイライト) する機能
- [IntelliCode](https://marketplace.visualstudio.com/items?itemName=VisualStudioExptTeam.vscodeintellicode) - キーワードの補完や提案をしてくれる便利ツール
- [Japanese Language Pack for VSCode](https://marketplace.visualstudio.com/items?itemName=MS-CEINTL.vscode-language-pack-ja) - 日本語化パッケージ
- [Material Icon Theme](https://marketplace.visualstudio.com/items?itemName=PKief.material-icon-theme) - ファイルアイコンが表示される機能


# トランスパイラ (TypeScript) の準備
Script API そのものは JavaScript (以降 JS) で書きますが、公式のドキュメントを見ると TypeScript (以降 TS) で書かれています。おそらくですが、公式も TS を推奨しているようですので、ここでもその環境を整えていきます。

まずは npm パッケージと JS を扱えるように Node.js をインストールします。

https://nodejs.org/en/

インストールできたかどうかはバージョンを確認するコマンドで確認できます。コマンドプロンプトを開くか、VSCode上で `Cttl` + `Shift` + `@` でコマンドパレットを開いて実行します。

```
npm -v
```

このようにバージョンが表示されていればOKです。

```
9.2.0
```

次に TS の準備をしていきます。npm コマンドを用いて以下のコマンドを実行します。

```
npm install -g typescript
```

できたらバージョンの確認をしていきましょう。下記コマンドを実行して、

```
tsc -v
```

バージョンが表示されていればOKです。

```
Version 5.6.2
```

ここまでで JS と TS を扱う準備はできました。

# ビヘイビアパックの作成
プログラミング環境は整いましたが、問題は **どこにどのファイルを置くのか** ですね。使用するOSによって配置場所が少し異なるので、順番に確認していきます。

ビヘイビアパック (アドオン) は開発用と本番用で置く場所が異なります。今回はワールドを作成したうえで、そのワールドに適用させるためのアドオンを作っていきます。まずは統合版マイクラフトを開いて新しいワールドを作っておいてください。

## ファイルの作成場所
アドオンを作成するためのファイルディレクトリはここです。

```
C:\Users\<UserName>\AppData\Local\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang
```

この中に、さらにディレクトリがあり、開発用途では `development_` の接頭語が書かれている中に作成します。今回は本番環境 (ワールドデータ) に設置するため、`minecraftWorlds` ディレクトリの中に入ります。ワールド名は完全にランダムなので、更新日順に並べ替えるとわかりやすいです。一番最新のフォルダを見てみましょう。

![](https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/001-introduction/media/01.jpg)

何がなんだか・・・という感じではありますが、この中の `behavior_packs` に JS のファイルを配置していきます。ここからは VSCode をつかってファイルを見ていきますので、このフォルダまるごと VSCode から開いてみましょう。例えばこんな開き方があります。

- そのディレクトリ内で右クリックしてコマンドプロンプトをひらき、`code .` コマンドを実行
- 1つ上のディレクトリへ戻り、フォルダまるごと VSCode にドラッグアンドドロップして開く

VSCode のファイルエクスプローラーがこのようになれば準備完了です。

![](https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/001-introduction/media/02.jpg)

## Manifest ファイルの作成
Manifest (マニフェスト) ファイルはアドオンの情報を記載したファイルです。これがないとマインクラフトからアドオンを読み込むことができませんので、 **必ず作成**してください。

で、どうやってつくるかというと、公式サイトに書いてあります。

https://learn.microsoft.com/ja-jp/minecraft/creator/reference/content/addonsreference/examples/addonmanifest?view=minecraft-bedrock-stable

が、ここまで丁寧に書く必要はありませんので、下記を参考に作ってみましょう。テンプレートを用意しておきますのでコピペしてOKです。

`behavior_packs` の中に適当なディレクトリ (例えば `script_api_bp`) をつくり、その中に `manifest.json` を作成します。作成したら下記のようにコードを書いて保存します。

```json
{
    "format_version": 2,
    "header": {
        "description": "Script API サンプル",
        "name": "ScriptAPI-behavior",
        "uuid": "152f05cf-4f71-ff99-dab5-406cf33bef16",
        "version": [1, 0, 0],
        "min_engine_version": [1, 20, 0]
    },
    "modules": [
        {
            "type": "script",
            "language": "javascript",
            "uuid": "ee45cc17-4765-2198-9210-d283b45d0407",
            "entry": "scripts/main.js",
            "version": [1,0,0]
        }
    ],
    "dependencies": [
        {
            "module_name": "@minecraft/server",
            "version": "1.13.0"
        }
    ]
}
```

ここまでのファイル構成はこんな感じです。`behavior_packs` から先にファイルが存在していればOKです。

```
│  level.dat
│  level.dat_old
│  levelname.txt
│  world_behavior_packs.json
│  world_icon.jpeg
│  world_resource_packs.json
│
├─behavior_packs
│  └─script_api_bp
│          manifest.json
├─db
└─resource_packs
```

マニフェストファイルの解説はまたの機会に。

ここまでできたらマイクラを再起動してワールド設定を開き、ビヘイビアパックを有効にしてみてください。

![](https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/001-introduction/media/03.jpg)

## プロジェクトの作成
プログラムを書くために必要な設定、パッケージのインストールなどを行います。
まずはワールドデータのディレクトリ上でコマンドパレットを開きます (`Ctrl` + `Shift` + `@`)。下記コマンドを実行して、プロジェクト用のディレクトリ作成、パッケージを導入します。

```
mkdir script_sample
cd script_sample
npm init -y
```

次に、下記コマンドを使って必要なパッケージを入れていきます。

```
npm install typescript @minecraft/server
```

次に、TSの設定ファイルを作成します。自動で生成すると色々面倒なので、手動で作成します。`script_sample`の中に `tsconfig.json` を作成して、下記の内容を記載します。

```json
{
  "compilerOptions": {
    "module": "ES2022",
    "target": "ES2023",
    "moduleResolution": "Node",
    "allowSyntheticDefaultImports": true,
    "rootDir": "./src",
    "outDir": "../behavior_packs/script_api_bp/scripts"
  },
  "exclude": [ "node_modules" ],
  "include": [ "src" ]
}
```

ここも細かい説明は省きますが、大事なのは `outDir` です。マインクラフトでの Script API は基本的にビヘイビアパックの `scripts` ディレクトリ内に作成された JS のファイルを読みに行くため、そこにファイル生成する必要があります。

次に、プログラムのコードを設置します。基本はプロジェクトのフォルダ `script_sample` の中に `src` フォルダを作成して、そこにプログラムを書いていきます。作るファイル名は `main.ts` としてください。

ここまでのファイル構成はこんな感じです。

```
├─behavior_packs
│  └─script_api_bp
│         manifest.json
│
├─resource_packs
└─script_sample
    │  package-lock.json
    │  package.json
    │  tsconfig.json
    │
    ├─node_modules
    │  │
    │  ├─@minecraft
    │  │  ├─common
    │  │  └─server
    │  │
    │  └─typescript
    └─src
            main.ts
```

## はじめての Script API
さて、ようやく準備が整いましたので、プログラムを書いてみましょう。書き方やお作法なんかは後回しにして、まずは動かしてみましょう。先ほど作成した `main.ts` に書き込みます。

```ts
import { world } from "@minecraft/server";

world.afterEvents.worldInitialize.subscribe(() => {
    world.sendMessage("ワールドが初期化されたよ！");
});
```

できたらコマンドパレットを開いてトランスパイルします。

```
tsc
```

たったこれだけで、`behavior_packs/script_api_bp` の中に自動で `scripts` ディレクトリが作成されて、その中に本体となる `main.js` が生成されます。

```
├─behavior_packs
│  └─script_api_bp
│      │  manifest.json
│      │
│      └─scripts
│              main.js
│
├─resource_packs
└─script_sample
    │  package-lock.json
    │  package.json
    │  tsconfig.json
    │
    ├─node_modules
    │  │
    │  ├─@minecraft
    │  │  ├─common
    │  │  └─server
    │  │
    │  └─typescript
    └─src
            main.ts
```

こんな感じになっていれば大丈夫です。色々とファイルが増えて大変かもしれませんが、ここまで準備できればあとは `src` ディレクトリの中をいじって `tsc` でトランスパイルするだけです！

ではお待ちかね、マイクラで動きを確認してみましょう。ビヘイビアパックを有効化しているので、そのまま読み込むことができます。そのままゲームをプレイしてみてください。

![](https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/001-introduction/media/04.jpg)

画像のように、プレイヤーが参加した瞬間にメッセージが表示されていればOKです！

## プログラムの更新と反映
では最後に、プログラムを変更してマイクラに反映してみましょう。`main.ts` を下記のように変更します。

```ts
import { system, world } from "@minecraft/server";

let time: number = 0;

system.runInterval(() => {
    time++;
    world.sendMessage(`${time}秒経過`);
}, 20);
```

コマンドパレットにて `tsc` コマンドを実行して `main.js` を更新します。更新できたらマイクラで `/reload` コマンドを実行してスクリプトファイルを再度読み込みます。すると、画像のように1秒ごとに経過時間をお知らせしてくれます。

![](https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/001-introduction/media/05.jpg)


ということで最も躓きやすいポイント、環境構築を終えました。別のコンテンツでは Script API を活用したプログラムの作成方法についても記載していきます。

# おまけ：Minecraft Education
統合版だけでなく、教育版にも Script API の導入はできます。ただし、Betaバージョンの Script API の使用はできません。これは統合版における実験機能が搭載されていないためです。

安定版でしたら同じ手順で `manifest.json` に依存関係を記載し、利用することができます。
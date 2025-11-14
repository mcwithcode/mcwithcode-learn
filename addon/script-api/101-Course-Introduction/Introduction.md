**お品書き**
1. アドオン開発環境構築
2. Script API 環境構築
3. はじめてのプログラム
4. おまけ

**想定環境**
- Windows 11 24H2
- Minecraft BE 1.21.123
- VS Code
- Node.js
- TypeScript

※すでに [VSCode](https://code.visualstudio.com/) は導入済みを想定しています。まだ導入できていない方はダウンロードしてください。

# アドオン開発環境構築
## プロジェクトの作成
まずは [Minecraft Creator Tools](https://mctools.dev/) にアクセスし、開発用テンプレートを準備します。いくつかテンプレートが用意されていますが、今回はシンプルに Add-On Starter テンプレートを使用します。

![](https://storage.googleapis.com/zenn-user-upload/1fb12d642ac7-20251114.png)

必要情報を記入していき、プロジェクトの保存先を "Use device storage" にします。このとき、Webブラウザからのアクセスを許可してあげてください。

![](https://storage.googleapis.com/zenn-user-upload/d5f319d91136-20251114.png)

プロジェクトの保存先はわかりやすい場所で大丈夫です。筆者の環境ではデスクトップにプロジェクトフォルダを作成し、そこを選択しています。準備ができましたらOKを押しましょう。

プロジェクトが作成された後はWebブラウザは使用しませんので閉じてください。デスクトップにもどり、プロジェクトフォルダを丸ごと VS  Code から開いてみましょう。下記のようにファイルが生成されていればOKです。

![](https://storage.googleapis.com/zenn-user-upload/a797290778c0-20251114.png)

## アドオンの出力先の設定
`.env` ファイルを開いてみましょう。

```.env
PROJECT_NAME="mc_script"
MINECRAFT_PRODUCT="BedrockUWP"
CUSTOM_DEPLOYMENT_PATH=""
```

`MINECRAFT_PRODUCT` の部分がデフォルトでは "BedrockUWP" となっています。これはアドオンの出力先を自動で設定してくれており、このままパッケージングすると、デフォルトでそれぞれのフォルダにアドオンが出力されます。

ただし、これはマイクラのバージョンが 1.21.120 よりも前であればこのままで良いのですが、1.21.120 以降の場合はディレクトリが変わっています。筆者と同じ環境でやる場合は以下の変更が必要です。
※<user-name> は皆さんのPC環境に合わせてください。

```.env
PROJECT_NAME="mc_script"
MINECRAFT_PRODUCT="Custom"
CUSTOM_DEPLOYMENT_PATH="C:\Users\<user-name>\AppData\Roaming\Minecraft Bedrock\Users\Shares\games\com.mojang"
```

ちょっとお話が逸れますが、もし教育版で作成する場合は、"Custom" に変更し、教育版の com.mojang フォルダを指定します。このパスの先にそれぞれ "development" フォルダがあり、自動的にそこへアドオンが追加されるようになります。

```.env
PROJECT_NAME="mc_script"
MINECRAFT_PRODUCT="Custom"
CUSTOM_DEPLOYMENT_PATH="C:\Users\<user-name>\AppData\Local\Packages\Microsoft.MinecraftEducationEdition_8wekyb3d8bbwe\LocalState\games\com.mojang"
```

## マニフェストファイルの確認
次に "behavior_packs" フォルダと "resource_packs" フォルダの中を見てみましょう。先程プロジェクト作成する際に "short_name" で指定した名前のフォルダが作成されており、この中にはアドオン本体を定義するためのファイル `manifest.json` があります。

プロジェクト作成時に決めた "title" がアドオン名になります。アドオン名を変更したい場合は "name" 属性を変更します。

```json
"header": {
    "name": "script",
    "description": "This is my first script API!",
    "uuid": "e019c516-8c21-4b3a-b71b-7e6a85d7b251",
    "version": [1, 0, 0],
    "min_engine_version": [1, 21, 120]
  }
```

ざっとアドオン周りはこんな感じです。今回は Script API にフォーカスするので、manifest.json やらフォルダ構成やらの話はしません！

# Scripit API 環境構築
Script API は JavaScript で動作しますが、公式のリファレンスを見るとサンプルコードなどは TypeScript で書かれています。公式に合わせて TypeScript で書かれたコードを JavaScript に変換（トランスパイル）するための環境を整えていきましょう。
※JavaScript を生で書くより、TypeScript を使うほうが補完機能が使えてとても便利です。

</br>

まずは Node.js を[インストール](https://nodejs.org/ja)します。そのままインストーラーをダウンロードできますので、手順どおり進めてください。

</br>

次に、VS Code のターミナルを開いて必要なパッケージをインストールしていきます。プロジェクト作成時に必要なパッケージを定義した "package.json" も準備されていますので、まとめて環境を整えることができます。下記のコマンドを実行してください。

```txt
npm install
```

しばらくするとインストールが完了し、環境構築が完了します！簡単ですね！！

後はオプションですが、アドオン開発する際に入れておくと便利な拡張機能も紹介しておきます。
- [Material Icon Theme](https://marketplace.visualstudio.com/items?itemName=PKief.material-icon-theme) - ファイルアイコンが表示される機能
- [Blockception's Minecraft Bedrock Development](https://marketplace.visualstudio.com/items?itemName=BlockceptionLtd.blockceptionvscodeminecraftbedrockdevelopmentextension) - mcfunction, json などのシンタックスハイライト機能
- [Bedrock Definitions](https://marketplace.visualstudio.com/items?itemName=destruc7i0n.vscode-bedrock-definitions) - IDやコマンドなどの自動入力サポート

# はじめてのプログラム
プログラムを書く場所についてですが `scripts` フォルダの中に TypeScript のファイルを設置していきます。デフォルトで `main.ts` というファイルがありますが、これがメインのプログラムになります。

ここをこんな感じに書いてみましょう。

```ts
import { world, system } from "@minecraft/server";

function main() {
  world.sendMessage(`やっほー！`);
}

system.run(main);
```

書けたらターミナルを開き、以下のコマンドを実行してアドオンを出力します。

```txt
npm run local-deploy
```

統合版マインクラフトを起動し、新しいワールドを作成します。その際に、作成したアドオンを追加するのを忘れないでください。

![](https://storage.googleapis.com/zenn-user-upload/38afbe3ab679-20251115.png)

マイクラに入り、以下のコマンドを実行してみてください。

```txt
/reload
```

スクリプトが再読み込みされて、「やっほー！」と表示されます。

![](https://storage.googleapis.com/zenn-user-upload/fb5b73703b8e-20251115.png)

## プチ解説
import は道具箱です。マイクラサーバーの機能のどんな機能を使うかを { } で囲み、今回は `world` と `system` を使用しています。

それぞれ、ワールドとシステムに関する操作をするのですが、例えばワールドに対する操作として「ワールド全体にメッセージを送る」という機能があります。それが `sendMessage()` というもので、関数ともいいます。

また、メッセージを送るためのシステム設定として `run(main)` を使用しています。run の () の中には「どんな関数を動かすか」を書きます。今回は「やっほー！」と表示される機能を持った main 関数を指定しています。

ざっくりこんな感じです。関数と言われて難しく感じるかもしれませんが、「何かが起こる機能」だと思ってOKです。それをワールドに入った瞬間にシステムが動かしている（関数を呼び出している）ということです。なので、このプログラムは実質1回しか動きません。

# おまけ
何回もプログラムを動かしたいなぁーと思ったら、自分自身でもう一度関数を呼んであげれば良いのです。

```ts
import { world, system } from "@minecraft/server";

function main() {
  world.sendMessage(`経過時間 : ${system.currentTick}`);
  system.run(main);
}

system.run(main);
```

できたら、VS Code のターミナルで下記コマンドを実行して

```txt
npm run local-deploy
```

マイクラに戻って、下記のコマンドを打ってみてください。

```txt
/reload
```

動かすと、経過時間が Tick ごとに表示されていきます。

![](https://storage.googleapis.com/zenn-user-upload/4caa9a826692-20251115.png)

## プチ解説
一番最後の行に `run(main)` があり、ワールドに入った後に1度だけ呼び出しています。

main 関数の中には経過時間を表示するための `currentTick` というものがあります。これはシステムが起動してからの現在時刻の情報を持っています。それを `sendMessage()` で表示しているので、マイクラが起動してからどれくらい経過したかをメッセージで表示されるわけです。

前回と違うのは、main 関数の中にさらに `run(main)` が入っていることです。これはメッセージを表示した後に、もう一度 main を呼び出す事になっているので、実質無限にメッセージを表示することになります。

関数の中から自分の関数を呼び出すことを再帰呼び出しと言ったりしますが、マイクラの世界ではこんなことをせずとも、「ある条件をきっかけに関数（機能）を呼び出す」ということができるのですが、それはまた次回にやっていきましょう✨️
# Script API の基本構文
Script API は統合版 (教育版) 向けのアドオンの一種で、JavaScript (TypeScript) を用いてマイクラの機能やゲームシステムの仕組みを開発することができるツールです。今回は Script API を記述するための基本的な構文を扱います。

# 実行環境
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.8.0 (npmパッケージ)
- Minecraft BE 1.21

# できるようになること
- プログラムの基本構文 (変数、関数、反復処理、条件分岐) がわかる
- Script API で用意されている関数を使えるようになる
- ドキュメントを読みながら必要な関数を組み合わせられるようになる
- カウントダウンタイマーのアドオンがつくれるようになる

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/002-api_methods/media/timer.gif?token=GHSAT0AAAAAACWOUDUGTYYRZKG5FBXRMWWGZXJCLEQ" vspace="10">

# プログラムの基本構文
TypeScript入門というわけではありませんが、マイクラ上で Script API を動かすのにあった方が良い前提知識をいくつか掲載します。

## 変数と型
変数はプログラムで扱う上で、数値や文字列などのようなデータを保存しておくことができます。

書き方
```txt
let 変数名: 型;
const 変数名: 型;
```

型はデータの種類のことで、数値を扱うのか、文字列を扱うのかといった種類を決めることができます。

|型|意味|
|--|--|
|number|数値|
|string|文字列|
|boolean|ブール値 (真偽値)|

`let` と `const` は値を書き換えることができるのか、そうでないのかです。今は深く考えずに使い分けていきましょう。

|構文|用途|
|--|--|
|let|何度も値を書き換えるときに使う|
|const|1回値を書き込んだらそれ以降は変更しない時に使う|

```ts
let num1: number; // 数字を保存するための変数を用意
let num2: number = 1; // 数値 1 をで初期化
const num_const: number = 2; // 定数 2 で初期化 (変更不可)

let str1: string; // 文字列を保存するための変数を用意
let str2: string = "minecraft"; // 文字列 "minecraft" で初期化
const str_const: string = "zombie" // 定数 "zombie" で初期化 (変更不可)

let bool1: boolean; // ブール値を保存するための変数を用意
let bool2: boolean = true; // ブール値 true で初期化
const bool3: boolean = false; // 定数 false で初期化  (変更不可、あまりこういう使い方はしない)
```

## 配列
配列は同じ変数名で複数の値を入れることのできる機能です。基本的に変数と使い方は同じですが、`[]` を使って宣言します。配列に入っている値を取り出す場合は `[]` に対応する値 (番号) を入れます。配列は0番目から始まることに注意してください。

```ts
let num: number[] = [0, 1, 2, 3, 4];
let str: string[] = ["fox", "cat", "wolf"];

// 3 の数値を取り出すとき
num[3];
// "fox" の文字列をとりだすとき
str[0];
```

## 関数
関数は機能のかたまりです。必要な情報を与えたら、あとはいい感じに処理してくれる仕組みを作ることができます。

書き方
```txt
function 関数名 (引数名: 引数の型): 戻り値の型 {
    return 戻り値;
}
```

|構文|用途|
|--|--|
|引数|関数を動かすときに必要になる情報を受け取るために使う|
|戻り値|関数で処理した内容を渡すときに使う|

例えば、入力した数値を2倍にして返す関数を作ることができます。

```ts
// 関数を作るとき
function times (num: number): number {
    return num * 2;
}

// 関数を使うとき
let ans = times(2);

// 結果を受け取る必要がないときは関数だけ書く
times(3);
```

上記の場合だと、関数 `times()` に2という値を渡して変数 `ans` でその計算結果を受け取っていますね。Script API では引数に渡すデータがいろいろあるので、関数の作り方というよりも使い方をメインに習得していきましょう。

## 反復処理
反復処理は文字通り、何度も同じ動きを繰り返すときに使います。`for` や `while` といった書き方がありますが、今は `for` の使い方だけ習得できていればOKです。

書き方

```txt
for(初期化式; 条件式; 更新式) {

}
```

|構文|用途|
|--|--|
|初期化式|反復処理の始まりの値を決める|
|条件式|反復した際に条件に合っていればくり返しを続ける|
|更新式|反復するたびに変数を更新する|

さて、これだけ見てもよくわからんと思いますが、簡単に言えば「今、何回目のループが行われたかを変数を使って判断する」ためにいろいろ書く必要があるわけです。例えばこんな例を見てみましょう。

```ts
for(let i = 0; i < 3; i++) {
    // ここに繰り返したいプログラムを書く 
}
```

どのように動いているかというと・・・

1. `i = 0` で初期化
2. `i < 3` で比較して、条件に合うなら繰り返す
3. `for`の中を処理したあとに `i++` で `i` の値を増やす
4. 手順2～3を繰り返す
5. 条件に合わなくなった場合、ループから抜け出す

となります。`i` の値を追っていくともっとわかりやすく、

|ループ回数|`i`の値|条件式|解説|
|--|--|--|--|
|1回目|i = 0| 0 < 3 |0は3より小さい → 成り立つ → くり返す|
|2回目|i = 1| 1 < 3 |1は3より小さい → 成り立つ → くり返す|
|3回目|i = 2| 2 < 3 |2は3より小さい → 成り立つ → くり返す|
|4回目|i = 3| 3 < 3 |3は3より小さい → 成り立たない → くり返さない|

とこんな感じです。4回目は繰り返さないので、3回繰り返して終わるプログラムになります。

## 条件分岐
条件分岐は値や文字列などを比較し、条件によって処理を分ける仕組みです。

書き方
```ts
// if 文
if(条件式)　{
    // ある条件のときだけ実行
}

// ある条件によって複数分岐する
if (条件式1) {
    // 条件式1に当てはまる場合にこの中を実行
} else if (条件式2) {
    // 条件式2に当てはまる場合にこの中を実行
} else {
    // 条件式1, 2 に当てはまらない場合にこの中を実行
}

// ある条件によって複数分岐する
switch (変数) {
    case 比較したい値1:
        // 変数と比較したい値1が当てはまる場合にこの中を実行
        break;
    case 比較したい値2:
        // 変数と比較したい値2が当てはまる場合にこの中を実行
        break;
    case 比較したい値3:
        // 変数と比較したい値3が当てはまる場合にこの中を実行
        break;
    default:
        // どれにも当てはまらない場合にこの中を実行
        break;
}
```

このようにいろいろありますが、特定の条件だけなら `if`, 2分岐なら `if-else`, 3分岐なら `else-if`, 4分岐以上なら `switch` を使うといいかもしれません。例えば、反復処理との相性はすごく良いです。

```ts
let i: number = 0;

for(i = 0; i < 10; i++) {
    if (i < 5) {
        // iが5より小さいときに動く
    } else {
        // iが5以上のときに動く
    }
}
```
# ドキュメントを読んでみよう
ここまで基礎をやって何が嬉しいかというと、ドキュメントが少し読めるようになります。まずはこれを見てみましょう。

https://learn.microsoft.com/en-us/minecraft/creator/scriptapi/minecraft/server/world?view=minecraft-bedrock-stable

このリンクは `World` クラスのドキュメントです。急に「クラス」とか言われて良くわからんかもしれませんが、「関数などが集まった便利な道具箱」だと思ってください。この中に入っている関数 (機能) を組み合わせていろいろ作ります。ちょっと下へスクロールすると Methods という項目があります。これが関数です。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/002-api_methods/media/01.jpg?token=GHSAT0AAAAAACWOUDUGC5Q27FUA4PQ7QBZCZXJCJTQ" vspace="10">

では `sendMessage` を見てみましょう。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/002-api_methods/media/02.jpg?token=GHSAT0AAAAAACWOUDUHY5QNOE2WAL6YLVDQZXJCKVQ" vspace="10">

いろいろ書いてありますが重要な部分だけ抜き出すなら Parameters の部分です。パラメータは関数に代入すべき引数のことを指します。この例だと

```
message: (RawMessage | string)[] | RawMessage | string
```

と書いてありますね。一見難しく見えますがもう少し分解してみるとこんな意味になります。
- RawMessage: string[]
- RawMessage: string

もっとわかりやすくするならこうですね。

```ts
function sendMessage (RawMessage: string[]) {

}

function sendMessage (RawMessage: string) {

}
```

公式のほうで関数を準備してくれているので、使い方さえわかっていれば誰でも簡単に機能を使うことができます。これがパッケージ (ライブラリ) の良さです。関数のセクションで使い方は説明していますので、使い方は大丈夫だと思います。

```ts
sendMessage("メッセージ");
```

ただし、この `sendMessage()` という関数は `World` クラスの中に入っているため、道具箱から持ってこないといけません。ということで、最終的にこのようになります。1行目は公式が準備してくれているパッケージを読み込んで、world の道具箱を使うことを示しています。

```ts
import { world } from "@minecraft/server";

world.sendMessage("メッセージ");
```

さて、ドキュメントの読み方はなんとなくわかったでしょうか？
実は、これだけではまだ不十分なのです。なぜなら **どのタイミングでプログラムを動かすか** を作れていないためです。マイクラを起動してすぐに動くのか、何かをきっかけに動くのか。このような発火条件のことを **トリガー** や **イベント** と言ったりしますが、機能をつくりながら練習してみましょう。

# カウントダウンタイマーをつくろう
さて、なんとなくプログラムの書き方がわかった＆ドキュメントの書き方がわかった と思いますので、書く練習をしてみましょう。
まずは必要な機能を考えてみましょう。

- タイマーの値を保存しておく変数
- プレイヤーがワールドに入ったかどうかをトリガーにする
- 画面上に残り何秒か表示する

必要になる関数 (機能) は以下です

- [System クラス](https://learn.microsoft.com/en-us/minecraft/creator/scriptapi/minecraft/server/system?view=minecraft-bedrock-stable)
- [World クラス](https://learn.microsoft.com/en-us/minecraft/creator/scriptapi/minecraft/server/world?view=minecraft-bedrock-stable)
- [runInterval() 関数](https://learn.microsoft.com/en-us/minecraft/creator/scriptapi/minecraft/server/system?view=minecraft-bedrock-stable#runinterval)

これらを用いて `main.ts` へ書いてみましょう。

```ts
import { system, world } from "@minecraft/server";

let time: number = 10;

system.runInterval(() => {
    world.sendMessage(`${time}`);
    
    if(time < 0) {
        world.sendMessage("時間です！");
        return 0;
    }
    
    time--;
}, 20);
```
できたら `tsc` でトランスパイル＆マイクラで `/reload` コマンドを実行して動かしてみてください。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/main/addon/script-api/002-api_methods/media/timer.gif?token=GHSAT0AAAAAACWOUDUGTYYRZKG5FBXRMWWGZXJCLEQ" vspace="10">

ちなみに、以下は文字列補完という書き方で、文字列の中に変数を埋め込むことができます。

```ts
world.sendMessage(`${time}`);
```

あとは `runInterval()` 関数ですが、パラメータは2つあって `callback()` と `tickIntercal` がありましたね。前者は関数、後者は何チック (tick) ごとに実行するかを書きます。20[tick] で1秒です。

ということで、プログラムの書き方、ドキュメントの読み方、カウントダウンタイマーのサンプルプログラムを作ってみました。なんとなく書き方がわかってきたら嬉しいです。
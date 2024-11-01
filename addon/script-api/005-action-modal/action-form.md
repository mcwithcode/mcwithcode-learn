# Script API による GUI の実装 (ActionForm 編)
Sctipt API と server-ui パッケージを使って、マインクラフトにモーダルメニューを作成します。モーダルにはテキストボックス、スライダー、ドロップダウンメニューなどを設置することができ、簡単な入力フォームを作成することができます。

# 実行環境
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.13.0 (npmパッケージ)
- @minecraft/server-ui 1.3.0 (npmパッケージ)
- Minecraft BE 1.21

# できるようになること
- ModalForm を開くことができる。
- ModalForm を定義してUIを構成できる。
- フォーム内の UI を操作して処理を実行できる。

↓こんなのをつくります。


# server-ui パッケージの導入
モーダルフォームを作成するには `@minecraft/server-ui` パッケージの導入が必要です。

```txt
npm install @minecraft/server-ui
```

また、`manifest.json` にも依存関係の宣言が必要です。

```json
// ここより上はヘッダーやモジュールなどの宣言
"dependencies": [
    {
        "module_name": "@minecraft/server",
        "version": "1.13.0"
    },
    {
        "module_name": "@minecraft/server-ui",
        "version": "1.3.0"
    }
]
```

ドキュメントを参照する場合はこちら。
https://learn.microsoft.com/ja-jp/minecraft/creator/scriptapi/minecraft/server-ui/minecraft-server-ui?view=minecraft-bedrock-stable

# モーダルフォームの作り方
モーダルフォームは `ModalFormData` クラスのインスタンスを生成することで作成可能です。しかし、ただ作成しただけでは表示されません。必ず `show()` メソッドを使用して、表示させる対象となるプレイヤーを引数に代入する必要があります。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ModalFormData } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showModalForm(player);
    } 
});

function showModalForm(player: Player) {
    const modal = new ModalFormData();
    modal.title("モーダルの例");
    modal.textField("タイトル", "");
    modal.show(player);
}
```

モーダルフォームを表示する場合、必ず1つ以上の要素が必要です。要素とは、モーダル上に表示される UI のことで、以下のいずれかになります。

- `textField`
- `dropdown`
- `slider`
- `toggle`

どれも書かなかった場合、以下のようなエラーが表示されます。

```txt
無効な json 形式を受信しました。エラー::要求されたプロパティ 'content' を検出することができませんでした。
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/script-api/005-modal-form/media/01.jpg" vspace="10">

公式のドキュメントはこちら
https://learn.microsoft.com/ja-jp/minecraft/creator/scriptapi/minecraft/server-ui/modalformdata?view=minecraft-bedrock-stable

## title
フォームのタイトルを定義する場合は `title()` メソッドを使用します。引数には文字列型を入れますが、セクションを使用したカラーコードの指定も可能です。

```ts
modal.title("タイトルだよー");
```

```ts
modal.title("§6タイトルだよ！");
```

## textField
テキスト入力を受け付ける UI で `textField()` メソッドを使用します。`defaultValue` は省略可能です。

|引数|型|説明|
|--|--|--|
|label|string|テキストフィールド上部の説明ラベル|
|placeholderText|string|テキストフィールド内に仮に入力しておく文字|
|defaultValue|string|あらかじめ入力されている文字|

プレースホルダーありの場合

```ts
modal.textField("名前", "スティーブ");
```

デフォルト値が設定されている場合

```ts
modal.textField("名前", "", "アレックス");
```


## toggle
オン・オフの入力を受け付ける UI で `toggle()` メソッドを使用します。`defaultValue` は省略可能です。

|引数|型|説明|
|--|--|--|
|label|string|トグル上部の説明ラベル|
|defaultValue|boolean|オンオフ状態の初期値|

初期値の指定がない場合

```ts
modal.toggle("トグルの設定");
```

最初からオンの状態にしておく場合

```ts
modal.toggle("トグルの設定", true);
```



## slider
整数値の入力を受け付ける UI で `slider()` メソッドを使用します。`defaultValue` は省略可能です。

|引数|型|説明|
|--|--|--|
|label|string|スライダー上部の説明ラベル|
|minimumValue|number|スライダーの最小値|
|maximumValue|number|スライダーの最大値|
|valueStep|number|目盛りのこまかさ (1目盛りあたりの値の大きさ)|
|defaultValue|number|スライダーの初期値|

最小値 0, 最大値 64 で目盛りの細かさが 1 のスライダー

```ts
modal.slider("1番目の例", 0, 64, 1);
```

最小値 0, 最大値 100, 初期値が 10 で目盛りの細かさが 5 のスライダー
 
```ts
modal.slider("2番目の例", 0, 100, 5, 10);
```



## dropdown
複数の選択肢から選べる UI で `dropdown()` メソッドを使用します。

|引数|型|説明|
|--|--|--|
|label|string|トグル上部の説明ラベル|
|options|string|選択肢|
|defaultValue|number|ドロップダウンの初期値 (何番目か)|

初期値が設定されていない例

```ts
let foodList = ["ごはん", "パン", "ラーメン", "じゃがいも"];
modal.dropdown("ドロップダウンの例1", foodList);
```

初期値が設定されている例 (配列の要素は 0 番目から始まるので、1を選択すると緑茶になる。)

```ts
let drinkList = ["水", "緑茶", "紅茶", "コーヒー", "炭酸水"];
modal.dropdown("ドロップダウンの例2", drinkList, 1);
```


## submitButton
データを送信する際のボタンを変更する場合は `submitButton()` メソッドを使用します。が、モーダル生成時にボタンもデフォルトで設定されており、「送信」というボタンになっているので、基本あまり使いません。ボタンの文字を変更したい場合に使います。

```ts
modal.submitButton("実行する");
```

# 取得できる値
アクションフォームでは、ボタンの選択によって遷移していたため、レスポンスは何番目のボタンが選択されたかになります。

一方、モーダルフォームでは、複数の入力項目に対して「何が選択されたか」をレスポンスで受け取ります。例えば、以下の例を見てみましょう。

```ts
function showModalForm(player: Player) {
    const modal = new ModalFormData();
    let effectList = ["night_vision", "speed", "regeneration"];
    let selectorList = ["@a", "@p", "@s"];

    modal.title("エフェクトの例")
    modal.dropdown("エフェクト", effectList);
    modal.dropdown("対象", selectorList);
    modal.slider("持続時間", 1, 120, 10);
    modal.slider("効果レベル", 1, 3, 1);
    modal.textField("オプション", "", "効果を発動しました。");
    modal.submitButton("実行する");

    modal.show(player).then((response: ModalFormResponse) => {
        world.sendMessage(JSON.stringify(response.formValues));
    });
}
```

このように UI が並んでいたとします。



このとき、値としては以下のように取得できます。

```
[0,0,10,1,"効果を発動しました。"]
```

ということで、レスポンスとしては配列で **UIの順番どおりに値を取得** することができます。

|配列|値|説明|
|--|--|--|
|0番目|0|effectList の 0番目 (night_vision) が選択されている|
|1番目|0|selectorList の 0番目 (@a) が選択されている|
|2番目|10|持続時間が 10|
|3番目|1|レベルが 1|
|4番目|"効果を発動しました。"|オプションで入力されたテキスト|

各UI要素について、配列を扱うものは配列の選択されている要素、スライダーであればその値、テキストフィールドであれば文字列そのものを受け取ることができます。また、ここにはありませんでしたが、トグルは `true` か `false` かの値が得られます。ただ、注意が必要なのは取得した値は型が決まっていませんので、TypeScriptを使用している場合は型アサーションなどが必要になります。

それぞれ値を取り出す場合はまとめて取得するか、配列要素を指定するかになります。

```ts
modal.show(player).then((response: ModalFormResponse) => {
    const selector = selectorList[response.formValues[0] as number];
    const effect = effectList[response.formValues[1] as number];
    const time = response.formValues[2];
    const level = response.formValues[3];
    const option = response.formValues[4];

    world.sendMessage(`/effect ${selector} ${effect} ${time} ${level}\n${option}`);
});
```

これを実行すると、以下のようなメッセージを取得できます。

```
/effect @a night_vision 1 1
効果を発動しました。
```

これを応用することで、例えばプレイヤーにエフェクトを付与することができますね。


# サンプルコード

## プレイヤーにエフェクトを付与する例
エフェクトのコマンドを動かす際のサブコマンドやオプションが、モーダルと相性が良いので練習がてら作ってみましょう。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ModalFormData, ModalFormResponse } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showModalForm(player);
    } 
});

function showModalForm(player: Player) {
    const modal = new ModalFormData();
    const effectList: Record<string, string>[] = [
        {"移動速度上昇":"speed"},
        {"採掘速度上昇":"haste"},
        {"ジャンプ力上昇":"jump_boost"},
        {"回復":"regeneration"},
    ];
    let selectorList: Record<string, string>[] = [
        {"すべてのプレイヤー":"@a"},
        {"近くのプレイヤー":"@p"},
        {"すべてのエンティティ":"@e"},
        {"自分":"@s"},
    ];

    modal.title("エフェクトの付与")
    modal.dropdown("エフェクト", effectList.flatMap(i => Object.keys(i)));
    modal.dropdown("対象", selectorList.flatMap(i => Object.keys(i)));
    modal.slider("持続時間", 1, 120, 1);
    modal.slider("効果レベル", 1, 3, 1);
    modal.toggle("パーティクルの表示", true);
    modal.submitButton("実行する");

    modal.show(player).then((response: ModalFormResponse) => {

        if(response.canceled) {
            return 0;
        }

        const effect = Object.values(effectList[response.formValues[0] as number]);
        const selector = Object.values(selectorList[response.formValues[1] as number]);
        const time = response.formValues[2];
        const level = response.formValues[3];
        const particle = response.formValues[4];

        world.sendMessage(`/effect ${selector} ${effect} ${time} ${level} ${!particle}`);
        player.runCommand(`/effect ${selector} ${effect} ${time} ${level} ${!particle}`);
    });
}
```

## パーティクルずかんの例

パーティクルをコマンドで動かすとき、Tabキーによる補完機能がなくて不便ですよね。そこで、あらかじめプログラムからパーティクルを呼び出せるようにして、どんな演出だったかを確認できるような機能をつくってみます。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player, Vector3, system } from "@minecraft/server";
import { ModalFormData, ModalFormResponse } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showModalForm(player);
    } 
});

function showModalForm(player: Player) {
    const modal = new ModalFormData();
    const particleList: string[] = [
        "minecraft:mobflame_single",
        "minecraft:white_smoke_particle",
        "minecraft:weaving_emitter",
        "minecraft:large_explosion",
        "minecraft:mobspell_emitter"
    ]
    const location: Vector3 = player.location;

    modal.title("パーティクルずかん")
    modal.dropdown("パーティクル", particleList);
    modal.textField("x 座標", "", Math.round(location.x).toString());
    modal.textField("y 座標", "", Math.round(location.y).toString());
    modal.textField("z 座標", "", Math.round(location.z).toString());
    modal.slider("実行時間 [tick]", 1, 100, 1);
    modal.slider("遅延 [tick]", 0, 20, 1);
    modal.submitButton("実行する");

    modal.show(player).then((response: ModalFormResponse) => {
        if(response.canceled) {
            return 0;
        }

        const particle = particleList[response.formValues[0] as number];
        const x = response.formValues[1];
        const y = response.formValues[2];
        const z = response.formValues[3];
        let time = response.formValues[4] as number;
        const delay = response.formValues[5] as number + 1;

        let timeCount: number = 0;

        system.runInterval(() => {
            if(timeCount > (time / delay)) {
                return 0;
            }
            player.runCommand(`titleraw @s actionbar {
                "rawtext":[{"text":"${particle} を表示中\n残り : ${((time / delay) - timeCount).toFixed(2)}秒"}]
            }`);
            player.runCommand(`particle ${particle} ${x} ${y} ${z}`);
            timeCount++;
        }, delay);
    });
}
```
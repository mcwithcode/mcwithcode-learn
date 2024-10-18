# Script API による GUI の実装 (ActionForm 編)
Sctipt API と server-ui を使って、マインクラフトにオリジナルのメニューを作成することができます。

# 実行環境
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.13.0 (npmパッケージ)
- @minecraft/server-ui 1.3.0 (npmパッケージ)
- Minecraft BE 1.21

# できるようになること
- ActionForm を開くことができる。
- ActionForm を定義してUIを構成できる。
- フォーム内の UI を操作して処理を実行できる。


# server-ui パッケージの導入
アクションフォームを作成するには `@minecraft/server-ui` パッケージの導入が必要です。

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

# ActionForm の作り方
アクションフォームは `ActionFormData` クラスのインスタンスを生成することで作成可能です。しかし、ただ作成しただけでは表示されません。必ず `show()` メソッドを使用して、表示させる対象となるプレイヤーを引数に代入する必要があります。

例えば、特定のアイテムを右クリックしたらメニュー画面を表示する場合はこのように書けます。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ActionFormData } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        const form = new ActionFormData();
        form.title("タイトル");
        form.body("本文");
        form.button("ボタン名");
        form.show(player);
    } 
});
```

ですが、モーダルはモーダルとしての機能 (function) として実装したいと思いますので、関数として宣言しておくのがおすすめです。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ActionFormData } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showActionForm(player);
    } 
});

function showActionForm(player: Player) {
    const form = new ActionFormData();
    form.title("タイトル");
    form.body("本文");
    form.button("ボタン名");
    form.show(player);
}
```

また、フォームのUIを構成する最低限の要素として、ボタン要素の初期化が必要です。初期化されていない状態で `show()` メソッドを実行すると、下記の様なエラーが表示されます。

```txt
無効な json 形式を受信しました。エラー::要求されたプロパティ 'buttons' を検出することができませんでした。
```



マイクラのUIフォームは json (JSON UI) で定義されており、タイトル、ボディー、ボタン要素が代入されていないと正しくUIが構成されません。 

## title
フォームのタイトルを定義する場合は `title()` メソッドを使用します。引数には文字列型を入れますが、セクションを使用したカラーコードの指定も可能です。

```ts
form.title("タイトルだよー");
```

```ts
form.title("§6タイトルだよ！");
```

## body
フォームのボディに文字列を記載する場合は `body()` メソッドを使用します。引数には文字列（色付する場合はセクション）を書きます。

```ts
form.body("本文をここに");
```

改行する場合は `\n` を使用します。これは通常の構文と同じです。

```ts
form.body("§61行目§r\n2行目\n3行目");
```

## button
フォームにボタンを設置する場合は `button()` メソッドを使用します。アイコンを表示する場合は第ニ引数にアイコン（画像ファイル）のパスを記載します。例えば、通常のアイテムはこれらのディレクトリにあります。

- アイテム `textures/items/<アイテムID>`
- ブロック `textures/blocks/<ブロックID>`

また、セクション記号を使用して色付けも可能です。

```ts
form.button("ボタン名");
form.button("ボタン1行目\nボタン2行目");
form.button("めっちゃ長い文字列をこんなふうに記載すると文字数オーバーになってしまうということもなく、しっかり改行されるみたい。");
form.button("アイコンも置けるよ", "textures/items/apple");
```



## show
`show()` は対象のプレイヤーにGUIを表示させるメソッドです。必ず記載しないとGUIが表示されません。
このメソッドは `Promise` にて生成された非同期なメソッドで、GUIが表示されている間もユーザーからの入力を受け付けることで、どのボタンが押されたかを検知します。

基本的にはどのボタンが押されたかを `ActionFormResponse` 型として受け取り、その値を使用します。

```ts
form.show(player).then((response: ActionFormResponse) => {
    player.sendMessage(`${response.selection}`);
});
```

上記のようにすると、選択されたボタンが **何番目のボタンか** を取得できます。例えば button セクションで4つのボタンを作成しましたが、一番上のボタンを選ぶと `0` という値が取得できます。以降、2番目のボタンを押すと `1` となり、3番目を押すと `2` となります。

したがって、ボタンを押したときに特定の処理をする場合は条件分岐が利用出来ます。

```ts
form.show(player).then((response: ActionFormResponse) => {
    switch(response.selection) {
        case 0:
            player.sendMessage(`${response.selection}番目のボタンが押されたよ！`);
            break;
        case 1:
            player.sendMessage(`${response.selection}番目のボタンが押されたよ！`);
            break;
        case 2:
            player.sendMessage(`${response.selection}番目のボタンが押されたよ！`);
            break;
        default:
            player.sendMessage(`${response.selection}番目のボタンが押されたみたい～`);
            break;
    }
});
```

これでボタンが押されたときの処理は作れるようになりました。が、このまま画面を閉じると `undefined` となってしまいます。こんなときはキャンセル処理を実装することで、安全にダイアログを閉じることができます。

## キャンセル処理とエラー処理
キャンセル操作により、ダイアログを閉じたときの処理は `canceled` プロパティを使用します。

```ts
form.show(player).then((response: ActionFormResponse) => {
    if(response.canceled) {
        player.sendMessage(`キャンセルされたで`);
        return 0;
    }
    // switch syntax
}
```

また、予期しないエラーが発生した場合 `catch` に流すこともできます。キャンセル理由は `cancelationReason` プロパティで取得できますが、プレイヤーが意図的にキャンセルした場合は `PlayerClosed` と表示されます。

```ts
form.show(player).then((response: ActionFormResponse) => {
    if(response.canceled) {
        player.sendMessage(`キャンセルされたで`);
        return 0;
    }

    // switch syntax

}).catch((response: ActionFormResponse) => {
    player.sendMessage(`エラー: ${response.cancelationReason}`);
    return -1;
});
```

# サンプルコード

## Mobを召喚する例
選択したボタンに応じて、召喚する生き物を変えたり、自分以外の生き物を kill する機能をつくってみます。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ActionFormData, ActionFormResponse } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showActionForm(player);
    } 
});

function showActionForm(player: Player) {
    const form = new ActionFormData();
    form.title("生き物コントロールパネル");
    form.body("生き物の召喚と削除ができるメニュー");
    form.button("にわとりを召喚する");
    form.button("ウシを召喚する");
    form.button("ヒツジを召喚する");
    form.button("§cプレイヤー以外を消去する§r", "textures/blocks/barrier");
    form.show(player).then((response: ActionFormResponse) => {
        if(response.canceled) {
            return 0;
        }
        switch(response.selection) {
            case 0:
                player.runCommandAsync("/summon chicken");
                break;
            case 1:
                player.runCommandAsync("/summon cow");
                break;
            case 2:
                player.runCommandAsync("/summon sheep");
                break;
            default:
                player.runCommandAsync("/kill @e[type=!player]");
                break;
        }
    }).catch((response: ActionFormResponse) => {
        player.sendMessage(`エラー: ${response.cancelationReason}`);
        return -1;
    });
}
```

## アイテムを付与する例
選択したボタンに応じて、装備一式を与える機能をつくります。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";
import { ActionFormData, ActionFormResponse } from "@minecraft/server-ui";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:stick") {
        showActionForm(player);
    } 
});

function showActionForm(player: Player) {
    const form = new ActionFormData();
    form.title("装備品コントロールパネル");
    form.body("装備品一式を付与します。");
    form.button("鉄装備を付与する", "textures/items/iron_sword");
    form.button("金装備を付与する", "textures/items/gold_sword");
    form.button("ダイヤモンド装備を付与する", "textures/items/diamond_sword");
    form.button("ネザライト装備を付与する", "textures/items/netherite_sword");
    form.button("§cアイテムを削除する§r", "textures/blocks/barrier");
    form.show(player).then((response: ActionFormResponse) => {
        if(response.canceled) {
            return 0;
        }
        switch(response.selection) {
            case 0:
                giveEquipments("iron", player);
                break;
            case 1:
                giveEquipments("golden", player);
                break;
            case 2:
                giveEquipments("diamond", player);
                break;
            case 3:
                giveEquipments("netherite", player);
                break;
            default:
                player.runCommandAsync("/clear @s");
                player.runCommandAsync("/give @s stick"); // メニュー用の棒を与える
                break;
        }
    }).catch((response: ActionFormResponse) => {
        player.sendMessage(`エラー: ${response.cancelationReason}`);
        return -1;
    });
}

function giveEquipments(material: string, player: Player) {
    player.runCommandAsync(`/give @s ${material}_sword`);
    player.runCommandAsync(`/give @s ${material}_helmet`);
    player.runCommandAsync(`/give @s ${material}_chestplate`);
    player.runCommandAsync(`/give @s ${material}_leggings`);
    player.runCommandAsync(`/give @s ${material}_boots`);
}
```
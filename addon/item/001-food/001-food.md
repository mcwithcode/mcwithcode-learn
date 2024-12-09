# 食べ物アイテムを追加する

# 使用する環境
- Visual Studio Code 
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.8.0 (npmパッケージ)
- Minecraft BE 1.21.50 / Education 1.21.06

# できるようになること
- 食べ物の役割をもつアイテムのビヘイビアパックの作れる
- アイテムのテクスチャが作れる
- リソースパックへのテクスチャの反映ができる

↓こんなのをつくります

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/item/001-food/media/example.gif" vspace="10">

# 食べ物としての役割を追加する
まずはビヘイビアパックにて、アイテムを定義していきます。`behavior_packs` ディレクトリの中に適当な名前 (例えば`bp`) のディレクトリを作成し、必要なファイルを追加していきます。アイテムを追加する場合は必ず `items` というディレクトリが必要です。

- `bp/manifest.json`
- `bp/items/green_apple.json`

まずはマニフェストファイルです。

```json
{
    "format_version": 2,
    "header": {
        "description": "mcwithcode",
        "name": "mcwithcode-behavior",
        "uuid": "875bb317-80fd-4ded-b7a1-5abdaa4a8c54",
        "version": [1,0,0],
        "min_engine_version": [1,21,0]
    },
    "modules": [
        {
            "description": "mcwithcode",
            "type": "data",
            "uuid": "82a83e3f-5e8d-4613-94e5-52ecc5de9721",
            "version": [1,0,0]
        }
    ],
    "dependencies": [
        {
            "uuid": "ea4855b9-6874-4fbf-9ff3-ed70f791d10c",
            "version": [3,0,0]
        }
    ]
}
```

続いて、アイテムの定義 `green_apple.json` です。

```json
{
    "format_version": "1.20.50",
    "minecraft:item": {
        "description": {
            "identifier": "mcwithcode:green_apple",
            "menu_category": {
                "category": "nature",
                "group": "itemGroup.name.miscFood"
            }
        },
        "components": {
            "minecraft:icon": "green_apple",
            "minecraft:display_name": {
                "value": "item.mcwithcode:green_apple.name"
            },
            "minecraft:food": {
                "nutrition": 5,
                "saturation_modifier": 0.2
            },
            "minecraft:use_animation": "eat",
            "minecraft:use_modifiers": {
                "use_duration": 1.3,
                "movement_modifier": 0.33
            }
        }
    }
}
```

## description - アイテムの情報

アイテムを定義するには `minecraft:item` 構造体で宣言します。`description` にはアイテムの基本情報を記載していきます。

```json
"description": {
    "identifier": "mcwithcode:green_apple",
    "menu_category": {
        "category": "nature",
        "group": "itemGroup.name.miscFood"
    }
}
```

|キーワード|意味|
|--|--|
|identifier|アイテムID|
|menu_category|クリエイティブタブでの配置設定|
|category|アイテムカテゴリー（タブ）|
|group|アイテムグループ（＋ボタンで展開する部分）|


アイテムIDをつける際には名前空間を記載します。名前空間とはIDを識別するための接頭語のようなもので、上記の `mcwithcode:` に相当します。

## components - アイテムの機能
定義したアイテムに機能を追加することができます。

```json
"components": {
    "minecraft:icon": "green_apple",
    "minecraft:display_name": {
        "value": "item.mcwithcode:green_apple.name"
    },
    "minecraft:food": {
        "nutrition": 5,
        "saturation_modifier": 0.2
    },
    "minecraft:use_animation": "eat",
    "minecraft:use_modifiers": {
        "use_duration": 1.6,
        "movement_modifier": 0.33
    }
}
```

|キーワード|意味|
|--|--|
|minecraft:icon|アイテムスロットに表示される画像|
|minecraft:display_name|アイテムの表示名（langファイルでも定義可能）|
|minecraft:food|食べ物としての機能|
|nutrition|空腹ゲージの回復量（1あたり肉ゲージ半分）|
|saturation_modifier|隠し満腹度の倍率（デフォルトでは 0.6）|
|minecraft:use_animation|アイテムを使用したときの動作|
|minecraft_use_modifiers|アイテムを使用するのにかかる時間 (minecraft:foodと組み合わせて使う)|
|use_duration|アイテムを使用するのにかかる時間 (デフォルトで 1.6 秒)|
|movement_modifier|アイテムを使用している間のプレイヤーの移動速度倍率 (0～1の間)|

隠し満腹度は「腹持ち状態」とも言われており、空腹度ゲージとは別のゲージが存在します。隠しゲージのため見えませんが、この値が 0 より大きい場合、食べ物を食べるとこのゲージも回復します。空腹ゲージよりも優先してこちらのゲージが減るため、食べた後に少し時間が経ってから空腹ゲージの値が減り始めます。隠し満腹度は `満腹度 × 倍率` によって決まります。


## アイテムを試す

アイテムがコマンドから取り出せますので、試してみましょう。

```
/give @s mcwithcode:green_apple
```

ビヘイビアパックだけではテクスチャがありませんので、透明なアイテムとして存在しています。テクスチャを貼るには次セクションのリソースパックの追加が必要です。

# アイテムテクスチャの適用
リソースパックを追加しますので、`resource_packs` のディレクトリに適当な名前のディレクトリ (例えば`rp`) を作成します。必要になるファイルは下記の通りです。

- `rp/manifest.json`
- `rp/textures/items/green_apple.png`
- `rp/textures/item_texture.json`
- `rp/texts/languages.json`
- `rp/texts/ja_JP.lang`

まずはマニフェストファイルです。

```json
{
    "format_version": 2,
    "header": {
        "description": "mcwithcode",
        "name": "mcwithcode-rp",
        "uuid": "ea4855b9-6874-4fbf-9ff3-ed70f791d10c",
        "version":[1,0,0],
        "min_engine_version": [1,21,0]
    },
    "modules": [
        {
            "type":"resources",
            "uuid": "0f759494-507c-4675-adb7-266e04030dea",
            "version": [1,0,0]
        }
    ]
}
```

続いて `item_texture.json`

```json
{
    "resource_pack_name": "mcwithcode-rp",
    "texture_name": "atlas.items",
    "texture_data": {
        "green_apple": {
            "textures": "textures/items/green_apple"
        }
    }
}
```

続いて `languages.json` です。

```json
[
    "ja_JP"
]
```

続いて `ja_JP.lang` です。

```lang
item.mcwithcode:green_apple.name=青りんご
```

そして画像を `rp/textures/items/` のディレクトリに入れます。今回は `green_apple.png` と名付けました。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/item/001-food/media/green_apple.png" vspace="10" width="64px" height="64px">


## 解説

`item_texture.json` は画像データをパック内で使用できるテクスチャとして使用できるようになります。例えば上記の例では `green_apple` というテクスチャに画像パスを設定することで、このキーワードを使用すると設定された画像パスから画像を読み込むことができるようになります。今回はビヘイビアパックの `minecraft:icon` に使用しています。


`languages.json` は言語ファイルを宣言するためのファイルです。ここに書かれた言語は `lang` ファイルを参照して、名称を割り当てることができるようになります。


`ja_JP.lang` は言語ファイルそのもので、日本語での表示名を定義できます。ビヘイビアパックの `minecraft:display_name` に使われています。


画像ファイル自体は jpeg でも png でも問題ありません。が、必ず 16 × 16 のサイズで作る必要があります。また、背景を透過しないと背景色までアイテムとして適用されてしまいます。例えば Windows を使用している方はペイントアプリにて、アイテムをデザインできます。16 × 16 サイズで設定し、最後に「レイヤー」から「背景の非表示」を選ぶことで、透過することができます。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/item/001-food/media/paint.jpg" vspace="10">


## アイテムの完成
ビヘイビアパックとリソースパックを適用することで、このようなアイテムを作ることができます。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/item/001-food/media/item.png" vspace="10">

# エフェクトの追加
食べ物を食べた時にエフェクトをつけるような機能は Script API で実装できます。例えば、青リンゴを食べたら体力回復するような効果を付ける場合は、このように書きます。

```ts
world.afterEvents.itemCompleteUse.subscribe((event: ItemCompleteUseAfterEvent) => {
    const item = event.itemStack.typeId;
    const player = event.source as Player;

    if(item === "mcwithcode:green_apple") {
        player.runCommand("effect @s regeneration 5 1");
    }
})
```

食べ終わった後、つまりアイテムを使用しきった後に発動しなければならないので `itemCompleteUse` を使用します。あとは使用者自身にエフェクトを付与するような処理をつくります。

Script API を使うためには準備が必要ですので、詳細はこちらの記事を参考にしてください。
https://www.mcwithcode.com/learn/script-api-beginning-for-windows


このようにして、アイテムを追加したり、そのアイテムを使った時の動きを作ることができます。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/item/001-food/media/example2.gif" vspace="10">
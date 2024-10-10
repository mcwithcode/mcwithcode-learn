# Script API によるアイテムトリガーの実装
アイテムトリガーとは、特定のアイテムを使用したときにプログラムを実行するための発火点のことです。例えば「鉄の剣」を右クリックしたら「前方にテレポートするプログラム」を実行するようなことが出来ます。アイテムトリガーを扱えるようになることで、アイテムの右クリック検知が実装できるようになります。

# 実行環境
- TypeScript 5.6.2
- npm 9.2.0
- @minecraft/server 1.13.0 (npmパッケージ)
- Minecraft BE 1.21

# できるようになること
- アイテムの右クリック検知ができる。
- アイテムを使用したときに特定のプログラムを実行することができる。
- アイテムの種類だけでなく、名前を条件にしてトリガーできる。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/script-api/003-item_trigger/media/01.gif" vspace="10">

# アイテムを使用したときを検知する
アイテムを検知する場合は `world` クラスの `afterEvents`プロパティ内にある `itemUse` プロパティ内にある `subscribe()` メソッドを使用します。

https://learn.microsoft.com/en-us/minecraft/creator/scriptapi/minecraft/server/worldafterevents?view=minecraft-bedrock-stable#itemuse

```ts
import { ItemUseAfterEvent, world } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {

});
```

このメソッドはプレイヤーがアイテムを使用する度に発火し、内部の処理が実行されます。その際に、どのアイテムがトリガーとなったか（使用されたか）のイベントが引数に代入されます。

代入された `event` には `itemStack` プロパティが含まれており、その中にアイテムの種類や名前を取得するためのプロパティが含まれています。これを使うことで、どのアイテムが使用されたかを特定できるのですが、型が合いません。そこで `as` を使用することで `ItemStack` 型へ上書きします。（これを型アサーションといいます。）

さらに `itemStack` からアイテムIDを取得するために `typeId` プロパティを使って比較を行うことで、「どのアイテムを使用したらプログラムを実行するのか」を実装することができます。ここまでを踏まえて最終的にはこのようになります。

```ts
import { ItemStack, ItemUseAfterEvent, world } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword") {
        // ここに処理を書く
    };
});
```

上記の例では鉄の剣を使ったら条件分岐の中が実行されます。

## アイテムを使用したときにメッセージを送信する
メッセージを表示する方法は `Player` クラスと `World` クラスを使う方法がありますが、ここでは後者を使ってみます。

例えば、鉄の剣を使ったときにメッセージを出す場合はこのようになります。

```ts
import { ItemStack, ItemUseAfterEvent, world } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword") {
        world.sendMessage("鉄の剣が使われたよ！");
    }
});
```

もう1つ分岐させて、ダイヤモンドの剣を使ったときの処理を作ることも出来ます。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword") {
        world.sendMessage("鉄の剣が使われたよ！");
    } else if (itemStack.typeId === "minecraft:diamond_sword") {
        world.sendMessage("これはダイヤモンドの剣だ！");
    }
});
```

## アイテムを使用したときにテレポートする
テレポートする場合は `Player` クラスの `teleport()` メソッドを使用する方法と、`runCommandAsync()` を使用する方法とあります。`teleport()` メソッドを使用する場合は引数に `Vector3` 型を入れないといけないので、このようになります。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player, Vector3 } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    const posA: Vector3 = { x: -10, y: 63, z: 345 };
    const posB: Vector3 = { x: -13, y: 63, z: 344 };

    if(itemStack.typeId === "minecraft:iron_sword") {
        player.teleport(posA);
    } else if (itemStack.typeId === "minecraft:diamond_sword") {
        player.teleport(posB);
    }
});
```

この方法では絶対座標でしか指定ができないため、相対座標や視点相対座標を使用する場合は `runCommandAsync()` で生のコマンドを記述するのをおすすめします。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword") {
        // 相対座標（高さ方向へ3ブロックテレポート）
        player.runCommandAsync("tp @s ~ ~3 ~");
    } else if (itemStack.typeId === "minecraft:diamond_sword") {
        // 視点相対座標（前方5ブロックへテレポート）
        player.runCommandAsync("tp @s ^ ^ ^5");
    }
});
```

## アイテムを使用したときにブロックを設置する
ブロックを設置するには `World` クラスの `setBlockType` メソッドを使用するか、`runCommandAsync()` メソッドで生のコマンドを記述するかになります。簡単なのは後者で、このように書けます。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword") {
        player.runCommandAsync("setblock -10 62 345 iron_block");
    } else if (itemStack.typeId === "minecraft:diamond_sword") {
        player.runCommandAsync("setblock -10 62 345 diamond_block");
    }
});
```

この状態で剣を持ち替えて右クリックすると、該当する座標のブロックが変わります。

続いて `setBlockType()` メソッドでの実装方法です。少し面倒なのが世界の種類（現世、ネザー、エンドなど）を `getDimension()` メソッドで指定する必要があるのと、ブロックを設置するために `Vector3` 型を宣言しないといけないことです。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    const pos: Vector3 = { x: -10, y: 62, z: 345 };

    if(itemStack.typeId === "minecraft:iron_sword") {
        world.getDimension("overworld").setBlockType(pos, "minecraft:iron_block");
    } else if (itemStack.typeId === "minecraft:diamond_sword") {
        world.getDimension("overworld").setBlockType(pos, "minecraft:diamond_block");
    }
});
```

# アイテム名を絞りこんで検知する
アイテム名を指定する場合は `nameTag` プロパティを参照して比較します。これは `ItemStack` クラスにて定義されているので、アイテムIDとあわせて条件式に記述することで、「鉄の剣かつ『新しい剣』という名前がついているアイテムをつかったとき」のようにトリガー条件を絞ることができます。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.typeId === "minecraft:iron_sword" && itemStack.nameTag === "新しい剣") {
        //「鉄の剣」かつ「新しい剣」というネームタグがついていれば実行
    } else if (itemStack.typeId === "minecraft:iron_sword" && itemStack.nameTag === "古い剣") {
        //「鉄の剣」かつ「古い剣」というネームタグがついていれば実行
    }
});
```

もちろん、アイテムIDの指定なしでネームタグだけでの判定もできます。

```ts
world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.nameTag === "新しい剣") {
        //「新しい剣」というネームタグがついていれば実行（アイテムの種類は問わない）
    } else if (itemStack.nameTag === "古い剣") {
        //「古い剣」というネームタグがついていれば実行（アイテムの種類は問わない）
    }
});
```


## アイテム名ごとに表示するメッセージを変更する
例えばアイテムにつけたネームタグをそのまま取得して、どちらのアイテムが使用されているかを判定することができます。

```ts
import { ItemStack, ItemUseAfterEvent, world } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.nameTag === "新しい剣") {
        world.sendMessage(`これは §6${itemStack.nameTag} だ！`);
    } else if (itemStack.nameTag === "古い剣") {
        world.sendMessage(`これは §a${itemStack.nameTag} だ！`);
    }
});
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/script-api/003-item_trigger/media/01.gif" vspace="10">

## アイテム名ごとにテレポートする方向を変える
このプログラムは既出ですので、条件式をネームタグに変えただけで実装できます。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.nameTag === "新しい剣") {
        player.runCommandAsync("tp @s ~ ~3 ~");
    } else if (itemStack.nameTag === "古い剣") {
        player.runCommandAsync("tp @s ^ ^ ^5");
    }
});
```

## アイテム名ごとに設置するブロックの種類を変える
このプログラムも既出ですので、条件式をネームタグに変えるだけで実装可能です。

```ts
import { ItemStack, ItemUseAfterEvent, world, Player } from "@minecraft/server";

world.afterEvents.itemUse.subscribe((event: ItemUseAfterEvent) => {
    const player = event.source as Player;
    const itemStack = event.itemStack as ItemStack;

    if(itemStack.nameTag === "新しい剣") {
        player.runCommandAsync("setblock -10 62 345 iron_block");
    } else if (itemStack.nameTag === "古い剣") {
        player.runCommandAsync("setblock -10 62 345 diamond_block");
    }
});
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/script-api/003-item_trigger/media/02.gif" vspace="10">

# おまけ - MakeCode
教育版マインクラフトを使用している場合、ビジュアルプログラミングでも同様のことができます。ただし、アイテム名を検知することは出来ませんので、アイテムごとでの発火が可能です。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/script-api/003-item_trigger/media/03.jpg" vspace="10">
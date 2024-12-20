# 【統合版/Java版】setblock コマンド
指定した座標に任意のブロックを設置することができるコマンドです。

# setblock の使い方

|キーワード|説明|
|--|--|
|x|x座標|
|y|y座標|
|z|z座標|
|ブロックID|石や草ブロックなどのID (stone, grass など)|
|ブロック状態|block state ともいう。ブロックの上側下側、向きなど|
|置き換え方法|設置する際に置き換え、破壊、残すか選択できる|

```txt
/setblock <x> <y> <z> <ブロックID> [ブロック状態] 置き換え方法
```

ブロック状態と置き換え方法については省略できます。ブロックの置き換え方法については3種類から選べます。何も指定しない場合は replace となります。

|置き換え方法|説明|
|--|--|
|destroy|ブロックを破壊してから設置します。破壊されたブロックはアイテムとしてドロップします。|
|replace|ブロックを置き換えます。元のブロックはドロップしません。|
|keep|設置先の座標が空気ブロックのときのみ設置します。|

# setblock の使用例

`10 20 10` の座標に石ブロックを設置する。

```
/setblock 10 20 10 stone 
```

自分の座標にレッドストーンブロックを設置する。

```
/setblock ~ ~ ~ redstone_block
```

自分から5ブロック東にガラスブロックを設置する。

```
/setblock ~5 ~ ~ glass
```

自分から3ブロック北、2ブロック上にガラスブロックを設置する。

```
/setblock ~ ~2 ~-3 glass
```

自分の見ている方向の10ブロック先に砂を設置する。

```
/seblock ^ ^ ^10 sand
```

自分の見ている方向の6ブロック先にオークの木材を設置し、元のブロックは破壊する。

```
/seblock ^ ^ ^6 oak_planks destroy
```

# ハーフブロックの設置方法

## 統合版での書き方
統合版ではブロックステートとして `"vertical_half"` に値を指定します。</br>
`10 20 10` の座標にオークのハーフブロックを下側に設置する場合

```
/setblock 10 20 10 oak_slab["vertical_half"="bottom"]
```

上側に設置する場合

```
/setblock 10 20 10 oak_slab["vertical_half"="top"]
```

## Java版での書き方
Java版ではブロックステートとして `type` に値を指定します。</br>
`10 20 10` の座標にオークのハーフブロックを下側に設置する場合

```txt
/setblock 10 20 10 minecraft:oak_slab[type=bottom]
```

オークのハーフブロックを上側に設置する場合

```txt
/setblock 10 20 10 minecraft:oak_slab[type=top]
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/command/common/001-setblock/media/half.jpg" vspace="10">

# ブロック（彩釉テラコッタ）の向きを変えて設置する方法

## 統合版での書き方
統合版ではブロックステートとして `"facing_direction"` に値を指定します。</br>

|ブロック状態の値|説明|
|--|--|
|2|南|
|3|北|
|4|東|
|5|西|

`10 20 10` の座標に青の彩釉テラコッタを東向きに設置する場合

```txt
/setblock 10 20 10 minecraft:blue_glazed_terracotta["facing_direction"=4]
```

## Java版での書き方
Java版ではブロックステートとして `facing` に値を指定します。</br>

|ブロック状態の値|説明|
|--|--|
|north|北|
|south|南|
|west|西|
|east|東|

`10 20 10` の座標に青の彩釉テラコッタを東向きに設置する場合

```txt
/setblock 10 20 10 minecraft:blue_glazed_terracotta[facing=east]
```

<br/>

基本的な使い方はここまでです。block state（ブロックの状態）の書き方は上記以外にもありますが、統合版とJava版で異なります。
# アニメーションつき画像を作成する
紙芝居のように画像を切り替えられる gif アニメーション画像を作成します。マインクラフトでは gif 形式のファイルを読み込むことはできませんので、複数の画像を組み合わせたものを1つのファイルとして、順番に読み込んでいきます。

# 使用する環境
- Visual Studio Code 
- Blockbench
- Minecraft BE 1.21.50 / Education 1.21.10
  
# できるようになること
- 時間ごとに切り替わる画像を作成できる
- アニメーションに必要な画像ファイルを作成できる
- materialファイル、geometryファイル、render_controllerファイルを作成できる
- リソースパックにて material, geometry, render_controller を適用できる

↓こんなのをつくります

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/img_anim03.gif" vspace="10">

# 画像ファイルの準備
画像自体は特に指定はありませんが、今回は16:9で作成していきます。

マイクラ内で風景の良さそうなところに行って、適当に撮影してきました。（もし一緒に使いたい場合はダウンロードしていただいてOKです）

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/sc01.jpg" vspace="10">

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/sc02.jpg" vspace="10">

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/sc02.jpg" vspace="10">

画像を1枚だけ表示する場合は、これ単体でもいいのですが、アニメーションを作る場合はこれらの画像を縦につなげる必要があります。

今回は[こちら](https://photocombine.net/cb/)の画像結合ツールのサイトをお借りして、3枚の画像を1枚にします。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/img_save.webp" vspace="10">

このようにできたら「保存」をクリックし、PNG形式（またはJPEG）で保存します。

# 画像エンティティの作成

## geometry の作成
geometry は画像を構成するための骨組みみたいなものです。今回は Blockbench というアプリを使って作成していきます。

Bedrock Entity を選択し、Create New Model をクリックします。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/bb01.webp" vspace="10">

プロジェクトの設定にて Texture Size を画像比率と同じにします。今回の場合はは 16 と 9 ですね。

できたら TEXTURES に画像を入れます。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/bb02.webp" vspace="10">

OUTLINER で Cube を追加します。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/bb03.webp" vspace="10">

Cube の Position を `-8, 0, 0` にして、Size を `16, 9, 0` にします。画像の位置がずれている場合は UV のところから枠をクリックして、合わせてください。Position が -8 になっているのはオフセットです。幅が 16 なので、その中心にしています。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/bb04.webp" vspace="10">

File から Export → Export Bedrock Geometry を選択し、geometry ファイルを出力します。わかりやすい名前でいいと思いますが、今回は `anim_img.geo.json` としました。これでジオメトリファイルの完成です。

## geometry ファイルの配置
ここからはマインクラフトのアドオンフォルダ内での作業になります。すでにマニフェストファイルを作成済みで、ワールドにアドオンが適用されている前提で進めます。

ビヘイビアパックのルートディレクトリは `bp`, リソースパックのルートディレクトリは `rp` と記載します。皆さんの環境に合わせて調整してください。

作成したジオメトリファイルを、以下のディレクトリに入れてください。

`/rp/models/entity/anim_img.geo.json`

ファイルの中を開いて、`"identifier": "geometry.unknown"` になっている部分を、ファイル名と同じにしておきます。例えばこんな感じです。`"identifier": "geometry.anim_img",`

## 画像ファイルの配置
画像はリソースパック内の `textures` に入れておきます。

`rp/textures/anim_img.png`

## material の作成
リソースパックフォルダの中に、マテリアルファイルを追加します。

※ビヘイビアパックのルートディレクトリは `bp`, リソースパックのルートディレクトリは `rp` と記載します。皆さんの環境に合わせて調整してください。

`rp/materials/entity.material`

```json
{
	"materials": {
		"version": "1.0.0",
		"animated_img_16x9:entity": {
            "+defines": ["USE_UV_ANIM"]
        }
	}
}
```

今回は `animated_img_16x9` という名前をつけていますが、ここもわかりやすい名前でOKです。（名前を変更した場合は、この後の記述方法も変わるのでご注意ください）

## render_contoller の作成
次に、描画制御用ファイルを作成します。これは描画ルールになり、実際に画像をどのタイミングで切り替えるかなどの設定にも使用します。

`rp/render_controllers/anim_img.render_controllers.json`

```json
{
	"format_version": "1.10.0",
	"render_controllers": {
		"controller.render.anim_img": {
			"geometry": "Geometry.default",
			"materials": [{"*": "Material.default"}],
			"textures": ["Texture.default"],
			"uv_anim": {
				"offset": [0, "math.mod(math.floor(q.life_time * 1), 3) / 3"],
				"scale": [1, "1/3"]
			}
		}
	}
}
```

### 計算式の変数

```json
"uv_anim": {
    "offset": [0, "math.mod(math.floor(q.life_time * 1秒あたりに切り替える枚数), 画像の枚数) / 画像の枚数"],
    "scale": [1, "1 / 画像の枚数"]
}
```

## ビヘイビアの定義
画像をコマンドやスポーンエッグから呼べるように、エンティティ情報を登録します。

`bp/entities/anim_img.json`

```json
{
    "format_version":"1.19.20",
    "minecraft:entity":{
        "description":{
            "identifier": "mcwithcode:anim_img",
            "is_spawnable":true,
            "is_summonable":true
        },
        "components": {
            "minecraft:damage_sensor": {
                "triggers": [
                    {
                        "deals_damage": false
                    }
                ]
            }
        }
    }
}
```

## リソースの定義
ここまでで作成した material, render_controller, geometry をすべて参照して、画像のテクスチャを貼り付けます。

`rp/entity/anim_img.json`

```json
{
	"format_version": "1.17.0",
	"minecraft:client_entity": {
		"description": {
			"identifier": "mcwithcode:anim_img",
			"render_controllers": [
				"controller.render.anim_img"
			],
			"geometry": {
				"default": "geometry.anim_img"
			},
			"textures": {
				"default": "textures/anim_img"
			},
			"materials": {
				"default": "anim_img"
			}
		}
	}
}
```

# 動かしてみよう
実際にマイクラのワールドで動かしてみましょう。

作成した画像をコマンドから呼び出します。今回の例では

```txt
/summon mcwithcode:anim_img
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/img_anim01.gif" vspace="10">

あれ？小さくね？？と思ったそこのあなた。大丈夫です、大きくできます。

# 付録

## 画像サイズの拡大
やり方は2つあります。簡単なのはビヘイビアにて `scale` 属性を付与することです。コンポーネント属性に scale を追加して、何倍にするかを指定します。今回、Bolockbenchにてスケールを1で作成しているので、例えば5倍すると幅が5ブロック分の大きさになります。

`bp/entities/anim_img.json`

```json
"components": {
    "minecraft:scale": {
        "value": 5
    }
}
```

もう一つのやり方は、ジオメトリファイルを変更する方法です。bones属性の `size` を変更します。このとき、縦横比を維持する場合は同じ拡大率で調整する必要があります。

`rp/models/entity/anim_img_geo.json`

```json
"bones": [
    {
        "name": "bb_main",
        "pivot": [0, 0, 0],
        "cubes": [
            {"origin": [-8, 0, 0], "size": [16, 9, 0], "uv": [0, 0]}
        ]
    }
]
```

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/img_anim03.gif" vspace="10">

## 早く切り替える
render_controller の offset にて変更できます。

```json
// 1秒に1枚
"offset": [0, "math.mod(math.floor(q.life_time * 1), 3) / 3"]

// 1秒に2枚
"offset": [0, "math.mod(math.floor(q.life_time * 2), 3) / 3"]

// 2秒に1枚
"offset": [0, "math.mod(math.floor(q.life_time * 0.5), 3) / 3"]
```

この部分の `q.life_time` にかかっている数字を変えることで切り替えスピードが変わります。例えば1にすると1秒ごとに変化します。2にすると0.5秒ごとに変化しますので、1秒あたりに何枚の画像を切り替えるかになります。

0.5にすれば1秒あたりに0.5枚となりますが、2秒で1枚と考えれば、2秒ごとに画像が切り替わるという意味になります。

<img src="https://raw.githubusercontent.com/mcwithcode/mcwithcode-learn/refs/heads/main/addon/entity/001-img-anim/media/img_anim02.gif" vspace="10">

## 画像の枚数がかわったとき
`rp/textures/anim_img.png` にて、画像を更新します。

次に、描画制御にて計算式を一部変更します。主に 3 と書いてあった部分を変更していきます。

`rp/render_controllers/anim_img.render_controllers.json`

```json
// 画像が2枚
"offset": [0, "math.mod(math.floor(q.life_time * 1), 2) / 2"],
"scale": [1, "1 / 2"]

// 画像が5枚
"offset": [0, "math.mod(math.floor(q.life_time * 1), 5) / 5"],
"scale": [1, "1 / 5"]

// 画像が10枚
"offset": [0, "math.mod(math.floor(q.life_time * 1), 10) / 10"],
"scale": [1, "1 / 10"]
```

# 参考
Bedrock Wiki / Entity Texture Animation

https://wiki.bedrock.dev/visuals/animated-entity-texture
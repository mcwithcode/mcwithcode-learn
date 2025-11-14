# 【C#】マインクラフトで花火を打ち上げる
MinecraftConnection を使用して、Java版マインクラフトにて花火を打ち上げる方法について紹介します。

# 使用する環境
- Visual Studio Code
- MinecraftConnection 2.1.0 
- Minecraft Java Edition 1.21.8
- Minecraft Server 1.21.8

すでに RCON が許可されたマインクラフトサーバーを起動できる前提で進めます。サーバーの準備、MinecraftConnectionの導入については[ドキュメント](https://www.mcwithcode.com/docs)をご覧ください。

# 花火を打ち上げるコマンド
マインクラフトにて花火を打ち上げるためには以下のコマンドを実行します。

```
/summon minecraft:firework_rocket <x> <y> <z>
```

しかし、このままでは空の花火となるため色や形が表現できません。つまりはシケ花火となるわけです。

そこで、NBTを使用して色情報や形状、爆発するまでの時間を記述します。



これで花火を打ち上げることが可能になりました。が、毎回このコマンドを打つのは大変ですね。特にコマンドの記述はミスタイプがつきもので、チャットに打ち込むにしてもカギ括弧の付け間違い、スペルミス、JSON形式の書き方の不備が一箇所でもあると正しく実行出来ません。

この問題を解決してくれるのが MinecraftConnection パッケージです。

# MinecraftConnection による花火の作成
すでに .NET の環境と VSCode が使える前提で進めます。まずはターミナルを開いてプロジェクトを作りましょう。

```txt
dotnet new console -o <プロジェクト名>
```

MinecraftConnection を導入します。（2025/9/18現在、最新版は 2.1.0 です）
```txt
dotnet add package MinecraftConnection --version 2.1.0
```
using System.Security.Cryptography;
using MinecraftConnection;
using MinecraftConnection.Entity;

string address = "127.0.0.1";
ushort port = 25575;
string pass = "minecraft";

var command = new MinecraftCommands(address, port, pass);
var pos = new Position(65, 71, 600);
var fireworks = new Fireworks()
{
    LifeTime = 10,
    Type = FireworkType.SmallBall,
    Colors = new List<FireworkColors> { FireworkColors.YELLOW },
};

command.SendCommand("time set day");
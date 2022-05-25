# asteroids

Here is a draft of an "Asteroids" game. It provides basic mechanics of the original game, but also alters gameplay a little bit.

The base idea was to split core logic into managers system. ManagersHub.cs contains references to all of the services and also provides a Awake/Update/Destroy loop for each of them to avoid too many MonoBehaviours. Also, this approach guarantees single instance of each service.

The logic of enemies, player, and asteroids is based on an approach close to ECS, but not use it full potential due it is only a prototype.

Game settings are based on scriptable objects (Assets/Resources/Data). They provide an opportunity of easy applying custom settings while tuning the game.

To start the game open the "Game" scene and press start button.

Controls: W - move, A,D - rotate, E - main/alternative weapon switch, Spacebar - Fire.

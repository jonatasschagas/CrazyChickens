namespace java com.jonatas.chagas.games.thrift.server

struct PlayerTO {
  1: i32 playerId;
  2: i32 playerType;
}

struct GameStateTO {
  1: bool gameStarted
  2: list<i32> maze
  3: list<PlayerTO> players
}

service GameService {
  GameStateTO getState(1: i32 playerId)
  bool movePlayer(1: i32 playerId, 2: i32 playerDirection)
  i32 registerPlayer(1: i32 playerType)
}
using UnityEngine;
using System.Collections;

public enum TILE_TYPE {
	GROUND,
	WALL
}

public class Coord {
	public int x;	
	public int y;
	public TILE_TYPE tileType;
	public Transform tileObject;

	public Coord(int _x, int _y, TILE_TYPE _tileType, Transform _tileObject) {
		x = _x;
		y = _y;
		tileType = _tileType;
		tileObject = _tileObject;
	}

	public Coord(int _x, int _y) {
		x = _x;
		y = _y;
	}
}
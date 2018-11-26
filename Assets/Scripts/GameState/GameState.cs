using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public MainCharacter mainCharacterPrefab;
    private MainCharacter mainCharacter;

    private bool blocked;
    public bool IsBlocked
    {
        get
        {
            return blocked;
        }
        set
        {
            blocked = value;
            if (blocked)
            {
                FindObjectOfType<CursorManager>().SetCursor(CursorManager.CursorType.Arrow);
            }
            // else sanchous, we should generate mouse move event somehow
        }
    }
    public uint MovePoints() { return mainCharacter.MovePoints; }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private MainCharacter MainChar()
    {
        if (!mainCharacter)
        {
            mainCharacter = Instantiate(mainCharacterPrefab);
        }

        return mainCharacter;
    }

    // Instant move
    public void MovePlayerToCell(HexCell cellToMoveTo)
    {
        MainChar().Cell = cellToMoveTo;
    }

    // Ingame player movement
    public bool MovePlayerThroughPath(List<HexCell> path)
    {
        return MainChar().MoveThroughPath(path);
    }
}

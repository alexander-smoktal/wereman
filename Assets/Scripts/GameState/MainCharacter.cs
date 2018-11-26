using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move functionality should be moved to Creatures interface
public class MainCharacter : MonoBehaviour {
    private static float sMoveSpeed = 1.5f;

    // Child components
    private Rigidbody2D rb2D;
    private Animator animator;

    private HexCell activeCell;
    private HexCell cellToMoveTo;
    private List<HexCell> path;
    private GameState gameState;

    private uint movePoints;
    public HexCell Cell
    {
        get
        {
            return activeCell;
        }
        set
        {
            activeCell = value;
            transform.position = value.transform.position;
        }
    }

    // This ,ust be calculated on stats update
    public uint MovePoints { get { return movePoints; } }

	// Use this for initialization
	void Start () {
        movePoints = 10;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        path = new List<HexCell>();
        gameState = FindObjectOfType<GameState>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cellToMoveTo)
        {
            if (Vector2.Distance(transform.position, cellToMoveTo.transform.position) < 1)
            {
                if (path.Count == 0)
                {
                    Stop();
                }
                else
                {
                    MoveToNextCellFromPath();
                }
                
                return;
            }

            Vector2 newPosition = Vector2.MoveTowards(transform.position, cellToMoveTo.transform.position, sMoveSpeed);
            rb2D.MovePosition(newPosition);
        }
    }

    private void Stop()
    {
        cellToMoveTo = null;
        animator.SetTrigger("Stop");
        gameState.IsBlocked = false;
    }

    private void MoveToNextCellFromPath()
    {
        if (path.Count == 0)
        {
            Debug.LogError("Cannot move throug empty path! Try later.");
            return;
        }

        HexCell nextCell = path[0];
        path.RemoveAt(0);

        MoveToCell(nextCell);
    }

    private void MoveToCell(HexCell cellToMoveTo)
    {
        this.cellToMoveTo = cellToMoveTo;

        // Rotate
        float angle = Mathf.Atan2(cellToMoveTo.transform.position.y - transform.position.y, cellToMoveTo.transform.position.x - transform.position.x) * 180 / Mathf.PI;
        rb2D.MoveRotation(angle);
    }

    public bool MoveThroughPath(List<HexCell> path)
    {
        if (this.path.Count != 0)
        {
            Debug.Log("Trying to move player while he's moving");
            return false;
        }

        gameState.IsBlocked = true;
        animator.SetTrigger("Walk");

        this.path = path;
        MoveToNextCellFromPath();
        return true;
    }
}

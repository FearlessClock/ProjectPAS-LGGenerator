using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public struct NewStep
{
    public Vector2 position;
    public Vector2 lastPosition;
}
public class FullWalker : MonoBehaviour
{

    public Tilemap floor;
    public GameObject marker;
    private Stack<bool> coroutineStack;
    // Use this for initialization
    void Start()
    {
        coroutineStack = new Stack<bool>();
        Vector2 position2D = new Vector2(this.transform.position.x, this.transform.position.y);
        NewStep newStep = new NewStep();
        newStep.position = position2D;
        newStep.lastPosition = position2D;
        StartCoroutine("SlowStep", newStep);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(coroutineStack.Count);
        if(coroutineStack.Count == 0)
        {
            Debug.Log("It is finished");
        }
    }
    private Vector2[] SurroundingTilesStateNotVisited(Vector2 position, Vector2 lastPosition)
    {
        List<Vector2> availibilities = new List<Vector2>();
        Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 newPos = position + directions[i];
            if (IsEmpty(newPos) && !lastPosition.Equals(newPos))
            {
                availibilities.Add(directions[i]);
            }
        }
        return availibilities.ToArray();
    }

    private bool IsEmpty(Vector2 pos)
    {
        Collider2D col = Physics2D.OverlapPoint(pos);
        if (col != null && col.CompareTag("Marker"))
        {
            return false;
        }
        return col;
    }

    public IEnumerator SlowStep(NewStep newStep)
    {
        coroutineStack.Push(true);
        Vector2 lastPos = newStep.lastPosition;
        bool walkable = true;
        GameObject token = Instantiate<GameObject>(marker);
        token.transform.position = newStep.position;
        token.transform.parent = this.transform;
        while (walkable)
        {
            yield return new WaitForSeconds(0.1f);
            //token.GetComponent<SpriteRenderer>().enabled = false;
            token = Instantiate<GameObject>(marker);
            Vector2[] available = SurroundingTilesStateNotVisited(newStep.position, lastPos);
            lastPos = newStep.position;
            walkable = WalkInAllDirections(available, ref newStep.position);
            token.transform.position = newStep.position;
        }

        coroutineStack.Pop();
    }
    
    public bool WalkInAllDirections(Vector2[] direction, ref Vector2 position)
    {
        // Carry on walking in this direction
        if(direction.Length == 1)
        {
            WalkerCentraliser.AddDungeonSize();
            position += direction[0];
            return true;
        }
        else
        {
            WalkerCentraliser.AddCrossRoads();
            for (int i = 0; i < direction.Length; i++)
            {
                WalkerCentraliser.AddDungeonSize();
                NewStep newStep = new NewStep();
                newStep.position = position + direction[i];
                newStep.lastPosition = position;
                StartCoroutine("SlowStep", newStep);
            }
            return false;
        }
    }
}

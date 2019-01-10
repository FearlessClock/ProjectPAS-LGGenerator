using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomWalker : MonoBehaviour {
    public Tilemap floor;
    public Vector2 lastPosition;
    private bool stepping = false;
	// Use this for initialization
	void Start ()
    {
        lastPosition = this.transform.position;
    }

    private Vector2[] SurroundingTilesStateNotVisited(Vector2 position)
    {
        List<Vector2> availibilities = new List<Vector2>();
        Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 newPos = position + directions[i];
            if(IsEmpty(newPos) && !lastPosition.Equals(newPos))
            {
                availibilities.Add(directions[i]);
            }
        }
        return availibilities.ToArray();
    }

    private bool IsEmpty(Vector2 pos)
    {
        return Physics2D.OverlapPoint(pos);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!stepping)
        {
            StartCoroutine("SlowStep");
        }
    }

    public IEnumerator SlowStep()
    {
        stepping = true;
        yield return new WaitForSeconds(1);
        Vector2[] available = SurroundingTilesStateNotVisited(this.transform.position);
        lastPosition = this.transform.position;
        this.transform.position += RandomDirection(available);
        stepping = false;
    }

    private Vector3 RandomDirection(Vector2[] availableDirections)
    {
        if(availableDirections.Length > 0)
        {
            return availableDirections[Random.Range(0, availableDirections.Length)];
        }
        return Vector3.zero;
    }
}

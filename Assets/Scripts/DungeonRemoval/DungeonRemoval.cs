using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DungeonRemovalRules;

public class DungeonRemoval : MonoBehaviour {
    public string axiom;
    private Vector3 startPosition;
    private Quaternion startRotation;
    //Tilemap that the dungeon will be generated in
    public Tilemap tilemap;
    public TileBase filledTile;

    public AbstractDigRules[] rules;
    //Position of the generator at different points in the generation
    private Stack<Vector3> positionStack;
    private Stack<Quaternion> rotationStack;

    public void Dig(Vector3 pos, DigSpaceRule space)
    {
        int offsetX = 0;// (space.sizeX / 2);
        int offsetY = (space.sizeY / 2);

        for(int i = 0; i < space.sizeY; i++)
        {
            for(int j = 0; j < space.sizeX; j++)
            {
                if(space.GetAtPosition(j, i))
                {
                    Vector3 marker = this.transform.up * (j - offsetX);
                    marker += this.transform.right * (i - offsetY);
                    
                    tilemap.SetTile(Vector3Int.FloorToInt(marker + this.transform.position), space.replacementTile);
                }
            }
        }
    }

    public void DigLetter(char letter)
    {
        if (IsStackLetter(letter))
        {
            if (letter.Equals('['))
            {
                positionStack.Push(this.transform.position);
                rotationStack.Push(this.transform.rotation);
            }
            else if (letter.Equals(']'))
            {
                if (positionStack.Count > 0 && rotationStack.Count > 0)
                {
                    this.transform.SetPositionAndRotation(positionStack.Pop(), rotationStack.Pop());
                }
            }
        }
        else
        {
            foreach (AbstractDigRules rule in rules)
            {
                if (rule.GetReplacementLetter().Equals(letter))
                {
                    if(rule.GetReplacementSpaceRule() != null)
                    {
                        Dig(this.transform.position, rule.GetReplacementSpaceRule());
                    }
                    if(rule.GetDungeonMovement() != null)
                    {
                        rule.GetDungeonMovement().DungeonStep(this.transform);
                    }
                }
            }
        }
    }

    private bool IsStackLetter(char letter)
    {
        if (letter.Equals('[') || letter.Equals(']'))
        {
            return true;
        }
        return false;
    }

    // Use this for initialization
    void Awake ()
    {
        positionStack = new Stack<Vector3>();
        rotationStack = new Stack<Quaternion>();
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
    }

    IEnumerator DigCoroutine()
    {
        int step = 0;
        foreach (char letter in axiom)
        {
            DigLetter(letter);
            step++;
            if(step > 10)
            {
                step = 0;
                yield return 1;
            }
        }
    }

    Coroutine fullGenerateCoroutine;
    public void GenerateFull()
    {
        //ResetDungeon();
        if(fullGenerateCoroutine != null)
        {
            StopCoroutine(fullGenerateCoroutine);
        } 
        positionStack = new Stack<Vector3>();
        rotationStack = new Stack<Quaternion>();
        this.transform.SetPositionAndRotation(startPosition, startRotation);
        fullGenerateCoroutine = StartCoroutine("DigCoroutine");
    }
    private int generationStep = 0;
    public void StepGenerate()
    {
        if(generationStep < axiom.Length)
        {
            DigLetter(axiom[generationStep]);
            generationStep++;
        }
    }

    public void SetAxiomFromLsystem(Lsystem lsystem)
    {
        axiom = lsystem.axiom;
    }

    public void ResetDungeon()
    {
        if (fullGenerateCoroutine != null)
        {
            StopCoroutine(fullGenerateCoroutine);
        }
        generationStep = 0;
        this.transform.SetPositionAndRotation(startPosition, startRotation);
        tilemap.ClearAllTiles();
    }

    // Update is called once per frame
    void Update () {
		
	}
}

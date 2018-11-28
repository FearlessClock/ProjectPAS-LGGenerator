using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGenerator;

public class StringGenerator : MonoBehaviour {

    public string axiom;
    public DungeonPieces[] dungeonPieces;
    private Dictionary<char, List<DungeonPieces>> dungeonPiecesDict;

    private Stack<Vector3> positionStack;
    private Stack<Quaternion> rotationStack;

    private int stepCounter;

    public Transform parent;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public void SetAxiomFromLsystem(Lsystem lsystem)
    {
        axiom = lsystem.axiom;
    }

	// Use this for initialization
	void Awake () {
        SetupDungeonPieces();
        positionStack = new Stack<Vector3>();
        rotationStack = new Stack<Quaternion>();
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
    }

    public void RenderFullAxiom()
    {
        ClearDungeon();
        StartCoroutine("RenderCoRoutine");
    }

    private IEnumerator RenderCoRoutine()
    {
        for (int i = 0; i < axiom.Length; i++)
        {
            RenderLetter(i);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void ClearDungeon()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        this.transform.SetPositionAndRotation(startPosition, startRotation);
    }

    private void RenderLetter(int index)
    {
        char letter = axiom[index];
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
        else if (dungeonPiecesDict.ContainsKey(letter))
        {
            //Get the different char for the rule
            char current;
            char preLetter;
            char postLetter;
            GetLetters(out current, out preLetter, out postLetter, index, axiom.Length);

            //Get all the pieces for the dungeon for the corresponding letter
            List<DungeonPieces> pieces = new List<DungeonPieces>();
            dungeonPiecesDict.TryGetValue(letter, out pieces);

            if (pieces != null)
            {
                foreach (DungeonPieces piece in pieces)
                {
                    if (piece.CanApplyRule(current, preLetter, postLetter))
                    {
                        //Spawn the piece
                        if(piece.room != null)
                        {
                            GameObject pieceObj = Instantiate<GameObject>(piece.room, this.transform.position, this.transform.rotation);
                            pieceObj.transform.parent = parent;
                        }
                        if(piece.movement != null)
                        {
                            piece.movement.DungeonStep(this.transform);
                        }
                    }
                }
            }
        }
        Debug.Log("New position: " + this.transform.position + " new rotation: " + this.transform.rotation.eulerAngles);
    }

    private bool IsStackLetter(char letter)
    {
        if(letter.Equals('[') || letter.Equals(']'))
        {
            return true;
        }
        return false;
    }

    private void GetLetters(out char preDec, out char preLet, out char postLet, int index, int axiomLength)
    {
        postLet = ' ';
        if (index + 1 <= axiomLength - 1)
        {
            postLet = axiom[index + 1];
        }

        preLet = ' ';
        if (index - 1 >= 0)
        {
            preLet = axiom[index - 1];
        }

        preDec = axiom[index];
    }

    /// <summary>
    /// Setup the dictionary with the dungeon pieces
    /// </summary>
    public void SetupDungeonPieces()
    {
        dungeonPiecesDict = new Dictionary<char, List<DungeonPieces>>();
        //For each production rule that has been defined
        foreach (DungeonPieces piece in dungeonPieces)
        {
            //Check if the rule exists, get the old list, add the new rule to it, remove the old rule list and add the new one
            char replacementLetter = piece.ReplacementLetter;
            Debug.Log(replacementLetter);
            if (dungeonPiecesDict.ContainsKey(replacementLetter))
            {
                List<DungeonPieces> newPieces = new List<DungeonPieces>();
                dungeonPiecesDict.TryGetValue(replacementLetter, out newPieces);
                newPieces.Add(piece);
                dungeonPiecesDict.Remove(replacementLetter);
                dungeonPiecesDict.Add(replacementLetter, newPieces);
            }
            else
            {
                List<DungeonPieces> rules = new List<DungeonPieces>();
                rules.Add(piece);
                dungeonPiecesDict.Add(replacementLetter, rules);
            }
        }
    }
	public void StepThrough()
    {
        if(stepCounter < axiom.Length)
        {
            RenderLetter(stepCounter);
            stepCounter++;
        }
    }
	// Update is called once per frame
	void Update () {
    }
}

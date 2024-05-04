using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PawnManager : ItemManager
{
    protected override string ManagerName => "Orch";
    protected override string ItemCategory => "Pawn";

    public UnityEvent OnMove = new();

    public void MovePawn(Vector3 cords)
    {
        Debug.Log($"Orch: Pawn ${Selected.GetInstanceID()} moving to {cords}");
        Selected.transform.position = _orch.Terrain.WorldToCell(
            _orch.Terrain.GetCellCenterWorld(Vector3Int.FloorToInt(cords))
        );
    }

    public void HighlightPawn()
    {
        if (Selected.TryGetComponent<Pawn>(out Pawn selected))
        {
            selected.Highlight();
        }
        Debug.Log($"{ManagerName}: Highlighting {ItemCategory} {Selected.GetInstanceID()}");
    }
}

public class TileManager : ItemManager
{
    protected override string ManagerName => "Orch";
    protected override string ItemCategory => "Tile";
}

public class CardManager : ItemManager
{
    protected override string ManagerName => "Orch";
    protected override string ItemCategory => "Card";
}

public class Orch : MonoBehaviour
{
    // GameObject Refs
    public Camera MainCam { get; private set; }
    public HighlightGridController HighlightGrid { get; private set; }
    public Tilemap Terrain { get; private set; }

    // ScriptableObjects
    public PawnManager PawnManager;
    public TileManager TileManager;
    public CardManager CardManager;

    void Start()
    {
        MainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        HighlightGrid = GameObject
            .FindWithTag("HighlightGrid")
            .GetComponent<HighlightGridController>();

        Terrain = GameObject.FindWithTag("Terrain").GetComponent<Tilemap>();

        PawnManager = ScriptableObject.CreateInstance<PawnManager>();
    }

    // Instead of having a clusterf of selection behavior, put a button in the UI that puts you in a SelectionState
    // Then choose behavior based on that SelectionState
    // Naming is based on Command Pattern states
    public enum CommandState
    {
        Continue,
        Success,
        Failure,
    }
}

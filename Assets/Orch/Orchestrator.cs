using UnityEngine;

// WTFAILA
// This is a partial class, its a super handy feature I found in C#
// Basically it lets you divide up a class across files
// I have all my 'modular' pieces stored in the "Parts" folder and I've got all my bigger picture things in this file
// When you code in each section, it'll let you access stuff from other parts as if it were in this file bc its just one big class

// It may seem intimidating at first, but partial classes are the same as normal classes, nothing changes functionally

// Note 1:
// This is my design philosophy, but you should only make a class when you need a class, not to organize
// That isn't a hard-and-fast rule, but its a pretty good one that I picked up from more experienced programmers

// Note 2:
// I'm still figuring out good naming conventions and stuff, but CLASSNAME_[Section Name] seems good for the 'modular' pieces

// Note 3:
// I wrote 'modular' above because Modules are an actual thing that exists
// The files in the "Parts" folder are not Modules
// They are just sets of properties and methods that together are kind of self-contained
// There is nothing special about the CLASSNAME file, its just another piece of the blob

// Note 4:
// Unity will see all the pieces but only attach this file because its the only one where the class name and filename match

// Note 5:
// You only declare what a partial class extends in one file, which in this case is "MonoBehaviour"
// I could have done that in any of the files and it would work the same

public partial class Orchestrator : MonoBehaviour
{
    // GameObject Refs
    private HighlightGridController _highlightGrid;
    public Camera MainCam { get; private set; }

    // Instead of having a clusterf of selection behavior, put a button in the UI that puts you in a SelectionState
    // Then choose behavior based on that SelectionState
    // Naming is based on Command Pattern states
    public enum CommandState
    {
        Continue,
        Success,
        Failure,
    }

    void Start()
    {
        _highlightGrid = GameObject
            .FindWithTag("HighlightGrid")
            .GetComponent<HighlightGridController>();
        MainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PawnMoved.AddListener(DeselectPawn);
    }

    // Apparently its important to destroy event listeners when you're done with a thing, prevents memory leaks
    // It may be irrelevant here bc the orch shouldn't be destroyed, but maybe between scenes and stuff it is, idk
    void OnDestroy()
    {
        PawnSelected.RemoveAllListeners();
    }
}

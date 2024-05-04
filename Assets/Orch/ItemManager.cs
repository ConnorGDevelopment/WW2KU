using UnityEngine;
using UnityEngine.Events;

public abstract class NeedsItemManager : ScriptableObject
{
    private GameObject _selected;

    public virtual GameObject Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }
}

public abstract class ItemManager : NeedsItemManager
{
    public UnityEvent OnSelect = new();
    public UnityEvent OnDeselect = new();

    protected virtual string ManagerName { get; }
    protected virtual string ItemCategory { get; }

    private void SelectCalls()
    {
        OnSelect.Invoke();
    }

    protected virtual void SelectMain(GameObject newSelection)
    {
        if (newSelection.gameObject.GetInstanceID() != Selected.gameObject.GetInstanceID())
        {
            Selected = newSelection;
            OnSelect.Invoke();
        }
        else
        {
            Deselect();
        }
    }

    protected virtual void SelectLogging(GameObject newSelection)
    {
        Debug.Log(
            $"{ManagerName}: ${ItemCategory} ${newSelection.gameObject.GetInstanceID()} selected"
        );
    }

    public void Select(GameObject newSelection)
    {
        SelectMain(newSelection);
        SelectCalls();
        SelectLogging(newSelection);
    }

    private void DeselectCalls()
    {
        OnDeselect.Invoke();
    }

    protected virtual void DeselectMain()
    {
        Selected = null;
    }

    protected virtual void DeselectLogging()
    {
        Debug.Log($"{ManagerName}: ${ItemCategory} deselected");
    }

    public void Deselect()
    {
        DeselectMain();
        DeselectCalls();
        DeselectLogging();
    }

    protected Orch _orch { get; private set; }

    public void Awake()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orch>();
    }
}

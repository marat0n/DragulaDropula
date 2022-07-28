using Microsoft.AspNetCore.Components;

namespace DragulaDropula;

public class DropTargetModel : ComponentBase
{
    private bool _isWaitDropping;
    
    [Inject] protected DragNDropController DragDrop { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Method for validating dropped item. If validation is successful then <c>OnAccept</c> will be invoked.
    /// </summary>
    [Parameter] public Func<object, bool>? ValidateItem { get; set; }

    /// <summary>
    /// Simple method for getting any dropped item.
    /// </summary>
    [Parameter] public Action<DraggableModel>? OnDrop { get; set; }
    
    /// <summary>
    /// <c>OnAccept</c> action will be invoked only if <c>ValidateItem</c> method returns <c>true</c>.
    /// </summary>
    [Parameter] public Action<object>? OnAccept { get; set; }
    
    /// <summary>
    /// <c>OnReject</c> action will be invoked only if <c>ValidateItem</c> method returns <c>false</c>.
    /// </summary>
    [Parameter] public Action<object>? OnReject { get; set; }

    private void InvokeOnDrop(DraggableModel model)
    {
        if (ValidateItem?.Invoke(model.ItemToDrop) ?? false)
            OnAccept?.Invoke(model.ItemToDrop);
        else OnReject?.Invoke(model.ItemToDrop);
        
        OnDrop?.Invoke(model);
    }

    protected void StartWaitDropping()
    {
        if (_isWaitDropping) return;
        
        DragDrop.DropToTarget += InvokeOnDrop;
        _isWaitDropping = true;
    }

    protected void EndWaitDropping()
    {
        DragDrop.DropToTarget -= InvokeOnDrop;
        _isWaitDropping = false;
    }
}
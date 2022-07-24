using Microsoft.AspNetCore.Components;

namespace DragulaDropula;

public class DropTargetModel : ComponentBase
{
    private bool _isWaitDropping;
    
    [Inject] protected DragNDropController DragDrop { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public Action<DraggableModel>? OnDrop { get; set; }

    private void InvokeOnDrop(DraggableModel model) => OnDrop?.Invoke(model);
    
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
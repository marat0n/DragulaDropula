using Microsoft.AspNetCore.Components;

namespace DragulaDropula;

public class DropTargetModel : ComponentBase
{
    [Inject] protected DragNDropController DragDrop { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public Action<DraggableModel>? OnDrop { get; set; }

    private bool _isWaitDropping;
    
    protected void StartWaitDropping()
    {
        if (_isWaitDropping) return;
        if (OnDrop is not null) DragDrop.DropToTarget += OnDrop.Invoke;
        _isWaitDropping = true;
    }

    protected void EndWaitDropping()
    {
        if (OnDrop is not null) DragDrop.DropToTarget -= OnDrop.Invoke;
        _isWaitDropping = false;
    }
}
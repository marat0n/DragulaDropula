using Microsoft.AspNetCore.Components;

namespace DragulaDropula;

public class DropTargetModel : ComponentBase
{
    [Inject] protected DragNDropController DragDrop { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public Action<DraggableModel>? OnDrop { get; set; }

    protected void StartWaitDropping()
    {
        if (OnDrop is not null) DragDrop.DropToTarget += OnDrop.Invoke;
    }

    protected void EndWaitDropping()
    {
        if (OnDrop is not null) DragDrop.DropToTarget -= OnDrop.Invoke;
    }
}
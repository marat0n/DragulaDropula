using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public class DragNDropController
{
    public delegate void DraggingEventHandler(MouseEventArgs e);
    public delegate void DroppingEventHandler(out DraggableModel droppedModel);
    public delegate void DroppingToTargetEventHandler(DraggableModel droppedModel);
    
    public event DraggingEventHandler? OnMove;
    public event DroppingEventHandler? OnDrop;
    public event DroppingToTargetEventHandler? DropToTarget;

    public void FireMove(MouseEventArgs evt) => OnMove?.Invoke(evt);
    public void FireUp()
    {
        DraggableModel? droppedModel = null;
        OnDrop?.Invoke(out droppedModel);
        if (droppedModel is not null) DropToTarget?.Invoke(droppedModel);
    }
}
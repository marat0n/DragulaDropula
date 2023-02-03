using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public sealed class DraggingStateContainer<T>
{
    public DraggableModel<T>? ModelDraggingNow;
    
    public event Action<DraggableModel<T>>? OnStartDragging;
    
    public event Action<MouseEventArgs>? OnMove;

    public event Action<T?>? OnDrop;

    public void InvokeOnStartDragging(DraggableModel<T> draggable)
    {
        ModelDraggingNow = draggable;
        OnStartDragging?.Invoke(draggable);
    }

    public void InvokeOnMove(MouseEventArgs args)
    {
        OnMove?.Invoke(args);
    }

    public void InvokeOnDrop(T? data)
    {
        OnDrop?.Invoke(data);
        ModelDraggingNow = null;
    }
    
    public void InvokeOnDrop()
    {
        if (ModelDraggingNow is null) return;
        OnDrop?.Invoke(ModelDraggingNow.ItemToDrop);
        ModelDraggingNow = null;
    }
}
using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public sealed class DraggingStateContainer<T>
{
    public DraggableModel<T>? ModelDraggingNow;
    
    public event Action<DraggableModel<T>>? OnStartDragging;
    
    public event Action<MouseEventArgs>? OnMove;

    public event Action<T?, MouseEventArgs>? OnDrop;

    public void InvokeOnStartDragging(DraggableModel<T> draggable)
    {
        ModelDraggingNow = draggable;
        OnStartDragging?.Invoke(draggable);
    }

    public void InvokeOnMove(MouseEventArgs args)
    {
        if (ModelDraggingNow is not null)
        {
            OnMove?.Invoke(args);
        }
    }

    public void InvokeOnDrop(T? data, MouseEventArgs args)
    {
        OnDrop?.Invoke(data, args);
        ModelDraggingNow = null;
    }
    
    public void InvokeOnDrop(MouseEventArgs args)
    {
        if (ModelDraggingNow is null) return;
        OnDrop?.Invoke(ModelDraggingNow.ItemToDrop, args);
        ModelDraggingNow = null;
    }
}
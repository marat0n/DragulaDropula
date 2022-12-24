using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static DragulaDropula.DragNDropController;

namespace DragulaDropula;

public class DraggableModel : ComponentBase
{
    [Inject] protected DragNDropController MouseSrv { get; set; } = null!;
    
    /// <summary>
    /// Default child content.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    /// <summary>
    /// Child content will be rendered when user drags <c>Draggable</c> component.
    /// </summary>
    [Parameter] public RenderFragment? ContentWhenDragging { get; set; }
    
    /// <summary>
    /// Object will be passed to the <c>DropTarget</c> component.
    /// </summary>
    [Parameter] public object ItemToDrop { get; set; } = new();
    
    /// <summary>
    /// If <c>true</c> then this <c>Draggable</c> component will be returned to start position after dropping.
    /// <c>true</c> is set by default.
    /// </summary>
    [Parameter] public bool MustReturnBackOnDrop { get; set; } = true;

    /// <summary>
    /// Method will be invoked when user drops this component.
    /// </summary>
    [Parameter] public Action<object>? OnDrop { get; set; }

    /// <summary>
    /// Method will be invoked when user drops this component.
    /// </summary>
    [Parameter] public Action<object, int, int>? OnDropWithPosition { get; set; }

    /// <summary>
    /// X position. Set to 0 by default.
    /// </summary>
    [Parameter] public int X { get; set; }
    [Parameter] public EventCallback<int> XChanged { get; set; }

    /// <summary>
    /// Y position. Set to 0 by default.
    /// </summary>
    [Parameter] public int Y { get; set; }
    [Parameter] public EventCallback<int> YChanged { get; set; }

    protected bool IsDragging { get; private set; }

    private double CursorX { get; set; }
    private double CursorY { get; set; }

    private void TryReturnBack()
    {
        if (!MustReturnBackOnDrop) return;
        X = 0;
        Y = 0;
        XChanged.InvokeAsync(X);
        YChanged.InvokeAsync(Y);
    }

    protected void StartDragging(MouseEventArgs e)
    {
        IsDragging = true;
        CursorX = e.PageX;
        CursorY = e.PageY;

        MouseSrv.OnMove += MoveThis;
        MouseSrv.OnDrop += DropThis;
    }

    protected void StartDragging(MouseEventArgs e, DraggingEventHandler actionToMove, DroppingEventHandler actionToDrop)
    {
        IsDragging = true;
        CursorX = e.PageX;
        CursorY = e.PageY;

        MouseSrv.OnMove += actionToMove;
        MouseSrv.OnDrop += actionToDrop;
    }

    private void DropThis(out DraggableModel droppedItem)
    {
        IsDragging = false;
        droppedItem = this;
        
        MouseSrv.OnMove -= MoveThis;
        MouseSrv.OnDrop -= DropThis;

        OnDrop?.Invoke(droppedItem);
        OnDropWithPosition?.Invoke(droppedItem, X, Y);
        
        TryReturnBack();
    }

    protected void DropThis(out DraggableModel droppedItem, DraggingEventHandler actionToMove, DroppingEventHandler actionToDrop)
    {
        IsDragging = false;
        droppedItem = this;

        MouseSrv.OnMove -= actionToMove;
        MouseSrv.OnDrop -= actionToDrop;

        OnDrop?.Invoke(droppedItem);
        OnDropWithPosition?.Invoke(droppedItem, X, Y);

        TryReturnBack();
    }

    protected void MoveThis(MouseEventArgs e) {
        if (!IsDragging) return;

        X -= (int)(CursorX - e.PageX);
        Y -= (int)(CursorY - e.PageY);
        
        CursorX = e.PageX;
        CursorY = e.PageY;

        XChanged.InvokeAsync(X);
        YChanged.InvokeAsync(Y);
    }
}
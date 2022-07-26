using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
    [Parameter] public Action<object, double, double>? OnDropWithPosition { get; set; }

    /// <summary>
    /// X position. Set to 0 by default.
    /// </summary>
    [Parameter] public double X { get; set; }
    [Parameter] public EventCallback<double> XChanged { get; set; }

    /// <summary>
    /// Y position. Set to 0 by default.
    /// </summary>
    [Parameter] public double Y { get; set; }
    [Parameter] public EventCallback<double> YChanged { get; set; }

#region CursorHandling

    protected bool IsDragging { get; private set; }

    private double CursorX { get; set; }
    private double CursorY { get; set; }

    protected void StartDragging(MouseEventArgs e) {
        IsDragging = true;
        CursorX = e.PageX;
        CursorY = e.PageY;

        MouseSrv.OnMove += MoveThis;
        MouseSrv.OnDrop += DropThis;
    }

    private void DropThis(out DraggableModel droppedItem)
    {
        IsDragging = false;
        droppedItem = this;
        
        MouseSrv.OnMove -= MoveThis;
        MouseSrv.OnDrop -= DropThis;

        OnDrop?.Invoke(droppedItem);
        OnDropWithPosition?.Invoke(droppedItem, X, Y);
        
        if (MustReturnBackOnDrop)
        {
            X = 0;
            Y = 0;
            XChanged.InvokeAsync(X);
            YChanged.InvokeAsync(Y);
        }
    }

    private void MoveThis(MouseEventArgs e) {
        if (!IsDragging) return;

        X -= CursorX - e.PageX;
        Y -= CursorY - e.PageY;
        
        CursorX = e.PageX;
        CursorY = e.PageY;

        XChanged.InvokeAsync(X);
        YChanged.InvokeAsync(Y);
    }

#endregion
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public class DraggableModel : ComponentBase
{
    [Inject] protected DragNDropController MouseSrv { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public object ItemToDrop { get; set; } = new();
    [Parameter] public bool MustReturnBackOnDrop { get; set; } = true;

    [Parameter] public Action<object>? OnDrop { get; set; }
    
    // X coordinate handling
    protected double X;
    private readonly EventCallback<double> _xChanged = default;

    // Y coordinate handling
    protected double Y;
    private readonly EventCallback<double> _yChanged = default;

#region CursorHandling

    protected string CursorStyle = "grab";
    
    private bool _isDragging;
    protected bool IsDragging {
        get => _isDragging;
        set {
            _isDragging = value;
            CursorStyle = _isDragging ? "grabbing" : "grab";
        }
    }

    private double _cursorX;
    private double _cursorY;

    protected void StartDragging(MouseEventArgs e) {
        IsDragging = true;
        _cursorX = e.ClientX;
        _cursorY = e.ClientY;
        
        MouseSrv.OnMove += MoveThis;
        MouseSrv.OnDrop += DropThis;
    }

    private void DropThis(out DraggableModel droppedItem)
    {
        if (MustReturnBackOnDrop)
        {
            X = 0;
            Y = 0;
        }
        
        IsDragging = false;
        droppedItem = this;
        
        MouseSrv.OnMove -= MoveThis;
        MouseSrv.OnDrop -= DropThis;

        OnDrop?.Invoke(droppedItem);
    }

    private void MoveThis(MouseEventArgs e) {
        if (!IsDragging) return;

        X -= (_cursorX - e.ClientX);
        Y -= (_cursorY - e.ClientY);

        _cursorX = e.ClientX;
        _cursorY = e.ClientY;

        _xChanged.InvokeAsync(X);
        _yChanged.InvokeAsync(Y);
    }

#endregion
}
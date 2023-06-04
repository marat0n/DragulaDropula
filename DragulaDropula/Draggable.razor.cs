using DragulaDropula.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public class DraggableModel<T> : ComponentBase
{
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
    [Parameter] public T ItemToDrop { get; set; } = default!;
    
    /// <summary>
    /// If <c>true</c> then this <c>Draggable</c> component will be returned to start position after dropping.
    /// <c>true</c> is set by default.
    /// </summary>
    [Parameter] public bool MustReturnBackOnDrop { get; set; } = true;

    /// <summary>
    /// Method will be invoked when user drops this component.
    /// </summary>
    [Parameter] public Action<DraggableModel<T>>? OnDrop { get; set; }

    /// <summary>
    /// Method will be invoked when user drops this component.
    /// Also X and Y statements will be set as parameters.
    /// </summary>
    [Parameter] public Action<DraggableModel<T>, int, int>? OnDropWithPosition { get; set; }
    
    /// <summary>
    /// Method will be invoked when this Draggable move.
    /// </summary>
    [Parameter] public Action<int, int>? OnMove { get; set; }
    
    [Parameter] public Action<DraggableModel<T>, int, int>? OnMoveWithModel { get; set; }

    /// <summary>
    /// X position. Set to 0 by default.
    /// </summary>
    [Parameter] public int X { get; set; }
    [Parameter] public EventCallback<int> XChanged { get; set; }
    
    [Parameter] public int StartX { get; set; }

    /// <summary>
    /// Y position. Set to 0 by default.
    /// </summary>
    [Parameter] public int Y { get; set; }
    [Parameter] public EventCallback<int> YChanged { get; set; }
    
    [Parameter] public int StartY { get; set; }

    [Parameter] public string CssClass { get; set; } = string.Empty;
    
    [Parameter] public string Id { get; set; } = string.Empty;

    [CascadingParameter(Name = "DragulaDropula_DraggingStateContainer")]
    public DraggingStateContainer<T> DraggingStateContainer { get; set; } = null!;

    protected bool IsDragging { get; private set; }

    private double CursorX { get; set; }
    private double CursorY { get; set; }

    private readonly DraggingStateContainerIsNotSetException _draggingStateContainerIsNotSetException = new();

    private bool _firstParametersSet = true;
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!_firstParametersSet) return;
        
        X = StartX;
        Y = StartY;
        _firstParametersSet = false;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (!firstRender) return;
        if (DraggingStateContainer is null) throw _draggingStateContainerIsNotSetException;
    }

    private void TryReturnBack()
    {
        if (!MustReturnBackOnDrop) return;
        X = 0;
        Y = 0;
        XChanged.InvokeAsync(X);
        YChanged.InvokeAsync(Y);
        StateHasChanged();
    }

    protected void StartDragging(MouseEventArgs e)
    {
        IsDragging = true;
        CursorX = e.PageX;
        CursorY = e.PageY;

        DraggingStateContainer.InvokeOnStartDragging(this);
        DraggingStateContainer.OnMove += MoveThis;
        DraggingStateContainer.OnDrop += DropThis;
    }

    private void DropThis(T? data, MouseEventArgs _)
    {
        IsDragging = false;

        OnDrop?.Invoke(this);
        OnDropWithPosition?.Invoke(this, X, Y);
        
        TryReturnBack();
    }

    private void MoveThis(MouseEventArgs e) {
        if (!IsDragging) return;

        X -= (int)(CursorX - e.PageX);
        Y -= (int)(CursorY - e.PageY);
        
        CursorX = e.PageX;
        CursorY = e.PageY;

        XChanged.InvokeAsync(X);
        YChanged.InvokeAsync(Y);
        
        OnMove?.Invoke(X, Y);
        OnMoveWithModel?.Invoke(this, X, Y);
    }
}
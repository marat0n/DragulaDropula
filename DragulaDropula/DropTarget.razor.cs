using DragulaDropula.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DragulaDropula;

public class DropTargetModel<T> : ComponentBase
{
    private bool _isWaitDropping;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Method for validating dropped item. If validation is successful then <c>OnAccept</c> will be invoked.
    /// </summary>
    [Parameter] public Func<T?, bool>? ValidateItem { get; set; }

    /// <summary>
    /// Simple method for getting any dropped item.
    /// </summary>
    [Parameter] public Action<T?>? OnDrop { get; set; }
    
    [Parameter] public Action<T?, MouseEventArgs>? OnDropWithEventArgs { get; set; }
    
    /// <summary>
    /// <c>OnAccept</c> action will be invoked only if <c>ValidateItem</c> method returns <c>true</c>.
    /// </summary>
    [Parameter] public Action<T?>? OnAccept { get; set; }
    
    /// <summary>
    /// <c>OnReject</c> action will be invoked only if <c>ValidateItem</c> method returns <c>false</c>.
    /// </summary>
    [Parameter] public Action<T?>? OnReject { get; set; }
    
    [Parameter] public Action? OnStartHover { get; set; }
    
    [Parameter] public Action? OnEndHover { get; set; }

    /// <summary>
    /// The class name for CSS styles.
    /// </summary>
    [Parameter] public string CssClass { get; set; } = string.Empty;
    
    [Parameter] public string Id { get; set; } = string.Empty;
    
    [CascadingParameter(Name = "DragulaDropula_DraggingStateContainer")]
    public DraggingStateContainer<T> DraggingStateContainer { get; set; } = null!;
    
    private readonly DraggingStateContainerIsNotSetException _draggingStateContainerIsNotSetException = new();

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (!firstRender) return;
        if (DraggingStateContainer is null) throw _draggingStateContainerIsNotSetException;
    }

    private void InvokeOnDrop(T? data, MouseEventArgs args)
    {
        if (ValidateItem?.Invoke(data) ?? false)
            OnAccept?.Invoke(data);
        else OnReject?.Invoke(data);
        
        OnDrop?.Invoke(data);
        OnDropWithEventArgs?.Invoke(data, args);
        
        DraggingStateContainer.OnDrop -= InvokeOnDrop;
        _isWaitDropping = false;
    }

    protected void StartWaitDropping()
    {
        if (_isWaitDropping || DraggingStateContainer.ModelDraggingNow is null) return;
        
        DraggingStateContainer.OnDrop += InvokeOnDrop;
        OnStartHover?.Invoke();
        _isWaitDropping = true;
    }

    protected void EndWaitDropping()
    {
        DraggingStateContainer.OnDrop -= InvokeOnDrop;
        OnEndHover?.Invoke();
        _isWaitDropping = false;
    }
}
# DragulaDropula
Library for simple Drag-And-Drop functionality in Blazor.

## How to start use
1) Add DradNDropController to Scoped Services.
```c#
builder.Service.AddScoped<DragNDropController>();
```
2) Use DraggingZone component as an underlay for other draggable components.
3) Done ✅


## API
### Components
- **DraggingZone**
> Component you need to use as an underlay for Draggable components.

Parameters: <br>
`Width` - width of DraggingZone
`Height` - height of DraggingZone

- **Draggable**
> Component you can drag

Parameters: <br>
`ItemToDrop` - object you need to drop (Example below).
`MustReturnBackOnDrop` - boolean parameter means this component will return to start position when it dropped.
`OnDrop` - if you need run some logic when Draggable dropped then put your method here

- **DropTarget**
> Component for creating dropping area

Parameters: <br>
`OnDrop` - your method for getting dropped DraggableModel (and ItemToDrop in it) and somethind else you need.


## Example
Let's create some test page in blazor and use DragulaDropula here.
Page `Test.razor`:
```c#
@page "/Test"

<DraggingZone Width="100vw" Height="100vh">
    <h1>Some test here!</h1>
    
    <Draggable ItemToDrop="@("Banana!")">
        <div style="background-color: yellow; width: 100px; height: 100px; color: white;">
            <span style="background-color: darkorange">The banana</span>
        </div>
    </Draggable>
    
    <hr />
    
    <DropTarget OnDrop="@(model => _result = model.ItemToDrop)">
        <div style="background-color: sandybrown; width: 250px; height: 250px; color: black;">
            The banana box
        </div>
    </DropTarget>
    
    <hr />
    
    The result is @(_result).
</DraggingZone>

@code {
    private object _result = "nothing";
}
```

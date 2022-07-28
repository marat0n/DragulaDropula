# DragulaDropula
Library for simple Drag-And-Drop functionality in Blazor.

Nuget Package here 👉 https://www.nuget.org/packages/DragulaDropula

## How to start using
1) Add DradNDropController to Scoped Services.
```c#
builder.Service.AddScoped<DragNDropController>();
```
2) Add `DragulaDropula` namespace to _Imports.razor.
```c#
@using DragulaDropula
```
3) Done ✅


## API
### Components
<<<<<<< HEAD
**DraggingZone**
> Component you need to use as an underlay for Draggable components.

=======
**DraggingZone** — Component you need to use as an underlay for Draggable components.<br>
>>>>>>> 2185f0ddab09d532c4c821fbb39a56d5626d62c3
Parameters: <br>
`Width` - width of DraggingZone. <br>
`Height` - height of DraggingZone. <br>

<<<<<<< HEAD
**Draggable**
> Component you can drag

=======
**Draggable** — Component you can drag <br>
>>>>>>> 2185f0ddab09d532c4c821fbb39a56d5626d62c3
Parameters: <br>
`ItemToDrop` - object you need to drop (Example below). <br>
`MustReturnBackOnDrop` - boolean parameter means this component will return to start position when it's dropped. <br>
`OnDrop` - if you need to run some logic when Draggable is dropped then put your method here. <br>
`ChildContent` - default child content. <br>
`ContentWhenDragging` - child content will be rendered when user drags `Draggable` component. <br>

<<<<<<< HEAD
**DropTarget**
> Component for creating dropping area

=======
**DropTarget** — Component for creating dropping area <br>
>>>>>>> 2185f0ddab09d532c4c821fbb39a56d5626d62c3
Parameters: <br>
`OnDrop` - your method for getting dropped DraggableModel (and ItemToDrop inside) and something else you need. <br>
`ValidateItem` - method for validating dropped item. If validation is successful then `OnAccept` will be invoked. <br>
`OnAccept` - action will be invoked only if `ValidateItem` method returns true.<br>
`OnReject` - action will be invoked only if `ValidateItem` method returns false.<br>


## Example
Let's create a test page in blazor and use DragulaDropula here.
Page `Test.razor`:
```html
@page "/Test"

<DraggingZone Width="100vw" Height="100vh">
    <h1>Some test here!</h1>
    
    <Draggable
        OnDrop="@(o => Console.WriteLine("Drop banana!"))"
        ItemToDrop="@("banana")">
        
        <div style="background-color: yellow; width: 100px; height: 100px; color: white;">
            <span style="background-color: darkorange">The banana</span>
        </div>
    </Draggable>
    
    <Draggable
        OnDrop="@(o => Console.WriteLine("Drop apple!"))"
        ItemToDrop="@("apple")">
        
        <div style="background-color: red; width: 100px; height: 100px; color: white;">
            <span>The apple</span>
        </div>
    </Draggable>
    
    <hr />
    
    <DropTarget
        ValidateItem="@(o => o is "banana")"
        OnAccept="@(item => { _result = item; Console.WriteLine(_result); })"
        OnReject="@(o => _result = "absolutely nothing because YOU JUST CAN'T PUT APPLES IN THE BANANA BOX")">
        <div style="background-color: sandybrown; width: 250px; height: 250px; color: black;">
            The banana box
        </div>
    </DropTarget>
    
    <hr />
    
    The result is @(_result).
    
    <hr>
    
    
</DraggingZone>

@code {

    private object _result = "nothing";

}
```

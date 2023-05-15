# DragulaDropula
Library for simple Drag-And-Drop functionality in Blazor.

Nuget Package here 👉 https://www.nuget.org/packages/DragulaDropula

## How to start using
1) Add `DragulaDropula` namespace to `_Imports.razor`.
```c#
@using DragulaDropula
```
2) Done ✅


## API
### Components

**DraggingZone** — Component you need to use as an underlay for Draggable components.<br>
Parameters: <br>
`Width` - width of DraggingZone. <br>
`Height` - height of DraggingZone. <br>
`CssClass` - the css class which will be set to root html-component in DraggingZone. <br>

***
**Draggable** — Component you can drag <br>
Parameters: <br>
`ItemToDrop` - object you need to drop (Example below). <br>
`MustReturnBackOnDrop` - boolean parameter means this component will return to start position when it's dropped. <br>
`OnDrop` - if you need to run some logic when Draggable is dropped then put your method here. <br>
`OnDropWithPosition` - the same as `OnDrop` but also set X and Y parameters when component dropped. <br>
`ChildContent` - default child content. <br>
`ContentWhenDragging` - child content will be rendered when user drags `Draggable` component. <br>
`StartX` - if you need to set start `X` position for Draggable component. <br>
`StartY` - if you need to set start `Y` position for Draggable component. <br>
`CssClass` - the css class which will be set to root html-component in Draggable. <br>

Bindings: <br>
`X` - position on the X-axis. <br>
`Y` - position on the Y-axis. <br>

***
**DropTarget** — Component for creating dropping area <br>
Parameters: <br>
`OnDrop` - your method for getting dropped DraggableModel (and ItemToDrop inside) and something else you need. <br>
`ValidateItem` - method for validating dropped item. If validation is successful then `OnAccept` will be invoked. <br>
`OnAccept` - action will be invoked only if `ValidateItem` method returns true.<br>
`OnReject` - action will be invoked only if `ValidateItem` method returns false.<br>
`CssClass` - the css class which will be set to root html-component in DropTarget. <br>

### Classes
**DraggingStateContainer** — Class you need to create your own realization of DraggingZone or Draggable or DropTarget <br>
Fields: <br>
`ModelDraggingNow` - contains an Draggable component that moving right in the moment. <br>

Events: <br>
`OnStartDragging` - invoked when any Draggable start dragging. <br>
`OnMove` - invoked when any Draggable moving. <br>
`OnDrop` - invoked when user release the LMB (the mouseup html event) and drop any Draggable. <br>

### Exceptions
**DraggingStateContainerIsNotSetException** <br>
Can be throwed if you'll use `Draggable` or `DropTarget` components without wrapping of DraggingZone. <br>
<br>
Exception message:
> DraggingStateContainer must be set. <br>
> Try wrap your Draggable and DropTarget components with DraggingZone. <br>
> And be sure their type parameters `T` are the same. <br>


## Example
Let's create a test page in blazor and use DragulaDropula here.
Page `Test.razor`:
```html
@page "/Test"

<DraggingZone T="CelestialObject" Height="50%" Width="100%">
    <div style="height: 600px">
        <Draggable T="CelestialObject"
                   MustReturnBackOnDrop="false" StartX="500" StartY="200"
                   ItemToDrop="@(new CelestialObject(CelestialObjectType.Star, "Sun"))">
            
            <div style="width:200px;height:200px;border-radius:50%;text-align:center;line-height:200px;background:yellow">
                Sun
            </div>

            <Draggable T="CelestialObject"
                       MustReturnBackOnDrop="false" StartX="-120" StartY="-10"
                       ItemToDrop="@(new CelestialObject(CelestialObjectType.Planet, "Earth"))">

                <div style="width:95px;height:95px;border-radius:50%;text-align:center;line-height:95px;background:blue">
                    Earth
                </div>

                <Draggable T="CelestialObject"
                           MustReturnBackOnDrop="false" StartX="-80" StartY="20"
                           ItemToDrop="@(new CelestialObject(CelestialObjectType.Satellite, "Moon"))">

                    <div style="width:50px;height:50px;border-radius:50%;text-align:center;line-height:50px;background:purple">
                        Moon
                    </div>
                </Draggable>
            </Draggable>

            <Draggable T="CelestialObject"
                       MustReturnBackOnDrop="false" StartX="350" StartY="110"
                       ItemToDrop="@(new CelestialObject(CelestialObjectType.Planet, "Mars"))">

                <div style="width:80px;height:80px;border-radius:50%;text-align:center;line-height:80px;background:brown">
                    Mars
                </div>
            </Draggable>
        </Draggable>
    </div>
                
    <DropTarget T="CelestialObject"
                ValidateItem="@(co => co is not null && co.Type == CelestialObjectType.Planet)"
                OnAccept="OnAccept"
                OnReject="OnReject">
        
        <div style="width: 300px; height: 300px; background: red;">
            Please drop a planet
        </div>
    </DropTarget>
</DraggingZone>

Message: @_message


@code {

    private enum CelestialObjectType {Planet, Star, Satellite}

    private record CelestialObject(CelestialObjectType Type, string Name);

    private string _message = string.Empty;

    private void OnAccept(CelestialObject? accepted)
    {
        _message = $"You dropped planet {accepted?.Name}!";
        StateHasChanged();
    }

    private void OnReject(CelestialObject? rejected)
    {
        _message = "It's not a planet...";
        StateHasChanged();
    }

}
```

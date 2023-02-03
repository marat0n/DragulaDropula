namespace DragulaDropula.Exceptions;

public class DraggingStateContainerIsNotSetException : Exception
{
    public const string MessageConst =
        "\nDraggingStateContainer must be set.\n" +
        "Try wrap your Draggable and DropTarget components with DraggingZone.\n" +
        "And be sure their type parameters `T` are the same.\n" +
        "Documentation is here: https://github.com/marat0n/DragulaDropula.\n";
    
    public DraggingStateContainerIsNotSetException() : base(
        message: MessageConst
    ) { }
}
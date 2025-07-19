using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    private Stack<CanvasCommand> undoStack = new Stack<CanvasCommand>();
    private Stack<CanvasCommand> redoStack = new Stack<CanvasCommand>();

    public void ExecuteCommand(CanvasCommand command, PixelArtCanvas canvas)
    {
        command.Execute(canvas);
        undoStack.Push(command);
        redoStack.Clear();
    }

    public void Undo(PixelArtCanvas canvas)
    {
        if (undoStack.Count > 0)
        {
            CanvasCommand command = undoStack.Pop();
            command.Undo(canvas);
            redoStack.Push(command);
        }
    }

    public void Redo(PixelArtCanvas canvas)
    {
        if (redoStack.Count > 0)
        {
            CanvasCommand command = redoStack.Pop();
            command.Execute(canvas);
            undoStack.Push(command);
        }
    }

    public void Clear()
    {
        undoStack.Clear();
        redoStack.Clear();
    }
}
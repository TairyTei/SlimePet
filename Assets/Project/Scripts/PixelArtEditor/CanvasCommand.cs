public abstract class CanvasCommand
{
    public abstract void Execute(PixelArtCanvas canvas);
    public abstract void Undo(PixelArtCanvas canvas);
}
namespace ThirdPartyLogic
{
    public interface IMazeTraverser
    {
        Direction Orientation { get; }
        bool IsLegalMove(Direction direction);
        ThirdPartyPoint Location { get; }
        void Face(Direction direction);
        void Step();
        ThirdPartyPoint GetNext(Direction direction);
        void DumpState();
    }
}
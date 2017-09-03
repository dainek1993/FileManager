namespace FileManager
{
    class MoveEventArgs
    {
        public int Index { get; private set; }
        public Container ActiveContainer { get; private set; }
        public MoveEventArgs(int index, Container activeContainer)
        {
            Index = index;
            ActiveContainer = activeContainer;
        }
    }
}
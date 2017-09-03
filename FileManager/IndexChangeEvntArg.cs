namespace FileManager
{
    class IndexChangeEvntArg
    {
        public IndexChangeEvntArg(int index, Container container)
        {
            Container = container;
            Index = index;
        }
        public int Index { get; }
        internal Container Container { get; }
    }
}
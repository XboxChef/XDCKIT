namespace XDevkit
{
    public interface IXboxFile
    {
        string Name { get; set; }
        object CreationTime { get; set; }
        object ChangeTime  { get; set; }

        ulong Size { get; set; }

        bool IsReadOnly { get; set; }

        bool IsDirectory { get; }
    }
}

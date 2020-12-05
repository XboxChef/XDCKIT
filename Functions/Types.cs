namespace XDevkit
{
    public class SearchResults
    {
        public string ID { get; set; }
        public string Offset { get; set; }
        public string Value { get; set; }
    }

    public class trainers
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string TitleUpdate { get; set; }
        public CodeList CodeList { get; set; }
    }

    public class CodeList
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public uint Address { get; set; }
        public uint Code { get; set; }
    }
}

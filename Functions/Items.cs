namespace XDevkit
{
    public class Items
    {
        public string Class;
        public string DefaultV;
        public string Description;
        public string Name;
        public string Offset;
        public string RecommendedV;

        public Items(string Item_class, string offset, string name, string DValue, string RValue, string description)
        {
            Class = Item_class;
            Offset = offset;
            Name = name;
            DefaultV = DValue;
            RecommendedV = RValue;
            Description = description;
        }

        public override string ToString() =>
            $"Class = {Class} Offset = {Offset} Name = {Name} Default_Value = {DefaultV} Recommended_Value = {RecommendedV} Description = {Description}";
    }
}


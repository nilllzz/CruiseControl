namespace CruiseControl.Scenes.Configurator
{
    struct ConfigOption
    {
        public const string CATEGORY_BODY_COLORS = "Body Colors";
        public const string CATEGORY_ENGINES = "Engines";
        public const string CATEGORY_FUNCTIONALITY = "Functionality";
        public const string CATEGORY_TIRES = "Tires";
        public const string CATEGORY_EXTERIOR = "Exterior";
        public const string CATEGORY_TECHNOLOGY = "Technology";

        public string Name;
        public string Description;
        public string Category;
        public bool ToggleCategory;
    }
}

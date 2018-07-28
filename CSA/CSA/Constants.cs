namespace CSA
{
    public static class Constants
    {
        public const string MacroFilePatternXml = @"([^\\]*\.xml$)";
        public const string MacroFilePatternScript = @"([^\\]*\.CATScript$)";
        public const string MacroScriptPattern = @"Sub CATMain\((.*)\)";

        public static class ParameterNames
        {
            public const string Description = @"'Description:";
            public const string Image = @"'Image;";
        }

                public enum ScreenSize
        {
            L,
            M,
            S
        }
    }
}

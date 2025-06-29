namespace CodeSmithCore
{
    public static class StringUtils
    {
        public static string GetCommentFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");
            text = text.Replace("  ", " ");
            text = text.Trim();
            return text;
        }

        public static string GetCamelCaseVar(string text)
        {
            if(string.IsNullOrWhiteSpace(text)) return "_";
            return char.ToLower(text[0]) + text.Substring(1);
        }

        public static string GetCamelCaseMember(string text)
        {
            return "_"+GetCamelCaseVar(text);
        }
    }
}

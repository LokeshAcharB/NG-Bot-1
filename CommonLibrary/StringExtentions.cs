using System.Text.RegularExpressions;

namespace CommonLibrary
{
    public static class StringExtentions
    {
        private readonly static Regex rex = new Regex("^[a-zA-Z0-9-]*$");
        public static bool IsValidPUID(this string src) => src.Length == 4 && rex.IsMatch(src) ? true : false;
        public static bool IsAlphaNumeric(this string src) => rex.IsMatch(src) ? true : false;
    }
}

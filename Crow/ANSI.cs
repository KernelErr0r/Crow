namespace Crow
{
    public static class ANSI
    {
        public static string Underline(string input)
        {
            return $"\u001B[4m{input}\u001B[0m";
        }

        public static string Bold(string input)
        {
            return $"\u001B[1m{input}\u001B[0m";
        }

        public static string Reverse(string input)
        {
            return $"\u001B[7m{input}\u001B[0m";
        }
    }
}
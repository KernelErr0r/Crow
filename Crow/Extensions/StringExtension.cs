namespace Crow.Extensions
{
    public static class StringExtension
    {
        public static string Expand(this string input, int length)
        {
            int inputLength = ANSI.Strip(input).Length;

            while (inputLength < length)
            {
                input += ' ';
                inputLength++;
            }

            return input;
        }
    }
}
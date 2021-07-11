
namespace Crowbar
{
    public class StringClass
    {
        public static string ConvertFromNullTerminatedOrFullLengthString(string input)
        {
            string output;
            int positionOfFirstNullChar;
            positionOfFirstNullChar = input.IndexOf('\0');
            if (positionOfFirstNullChar == -1)
            {
                output = input;
            }
            else
            {
                output = input.Substring(0, positionOfFirstNullChar);
            }

            return output;
        }

        public static string RemoveUptoAndIncludingFirstDotCharacterFromString(string input)
        {
            string output;
            int positionOfFirstDotChar;
            positionOfFirstDotChar = input.IndexOf(".");
            if (positionOfFirstDotChar >= 0)
            {
                output = input.Substring(positionOfFirstDotChar + 1);
            }
            else
            {
                output = input;
            }

            return output;
        }
    }
}
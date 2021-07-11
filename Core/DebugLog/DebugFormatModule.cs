
namespace Crowbar
{
    static class DebugFormatModule
    {
        public static string FormatByteWithHexLine(string name, byte value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N0");
            line += " (0x";
            line += value.ToString("X2");
            line += ")";
            return line;
        }

        public static string FormatIntegerLine(string name, int value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N0");
            return line;
        }

        public static string FormatIntegerAsHexLine(string name, int value)
        {
            string line;
            line = name;
            line += ": ";
            line += "0x";
            line += value.ToString("X8");
            return line;
        }

        public static string FormatIntegerWithHexLine(string name, int value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N0");
            line += " (0x";
            line += value.ToString("X8");
            line += ")";
            return line;
        }

        public static string FormatLongWithHexLine(string name, long value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N0", Program.TheApp.InternalNumberFormat);
            line += " (0x";
            line += value.ToString("X16");
            line += ")";
            return line;
        }

        public static string FormatSingleFloatLine(string name, float value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N6", Program.TheApp.InternalNumberFormat);
            return line;
        }

        public static string FormatDoubleFloatLine(string name, double value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.ToString("N6", Program.TheApp.InternalNumberFormat);
            return line;
        }

        public static string FormatStringLine(string name, string value)
        {
            string line;
            line = name;
            line += ": ";
            line += value.TrimEnd('\0');
            return line;
        }

        public static string FormatIndexLine(string name, int value)
        {
            string line;
            line = "[";
            line += name;
            line += " index: ";
            line += value.ToString("N0");
            line += "]";
            return line;
        }

        public static string FormatVectorLine(string name, double x, double y, double z)
        {
            string line;
            line = name;
            line += "[x,y,z]: (";
            line += x.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += y.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += z.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ")";
            return line;
        }

        public static string FormatVectorLine(string name, SourceVector value)
        {
            string line;
            line = name;
            line += "[x,y,z]: (";
            line += value.x.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += value.y.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += value.z.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ")";
            return line;
        }

        public static string FormatQuaternionLine(string name, SourceQuaternion value)
        {
            string line;
            line = name;
            line += "[x,y,z,w]: (";
            line += value.x.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += value.y.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += value.z.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ", ";
            line += value.w.ToString("N6", Program.TheApp.InternalNumberFormat);
            line += ")";
            return line;
        }
    }
}
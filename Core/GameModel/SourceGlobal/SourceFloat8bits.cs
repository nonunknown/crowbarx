using System.Runtime.InteropServices;

namespace Crowbar
{

    // TODO: 
    public class SourceFloat8bits
    {
        public double TheFloatValue
        {
            get
            {
                double result;
                int sign;
                int floatSign;
                sign = GetSign(the8BitValue);
                if (sign == 1)
                {
                    floatSign = -1;
                }
                else
                {
                    floatSign = 1;
                }

                if (IsInfinity(the8BitValue))
                {
                    return maxfloat8bits * floatSign;
                }

                if (IsNaN(the8BitValue))
                {
                    return 0d;
                }

                int mantissa;
                int biased_exponent;
                mantissa = GetMantissa(the8BitValue);
                biased_exponent = GetBiasedExponent(the8BitValue);
                if (biased_exponent == 0 && mantissa != 0)
                {
                    float floatMantissa;
                    floatMantissa = mantissa / 8.0f;
                    result = floatSign * floatMantissa * half_denorm;
                }
                else
                {
                    result = GetSingle(the8BitValue);

                    // For debugging the conversion.
                    // result = CType(anInteger32, Single)
                }

                return result;
            }
        }

        // unsigned short mantissa : 3;
        // unsigned short biased_exponent : 4;
        // unsigned short sign : 1;

        private int GetMantissa(byte value)
        {
            return value & 0x7;
        }

        private int GetBiasedExponent(byte value)
        {
            return (value & 0x78) >> 3;
        }

        private int GetSign(byte value)
        {
            return (value & 0x80) >> 7;
        }

        private float GetSingle(byte value)
        {
            // FROM:
            // unsigned short mantissa : 3;
            // unsigned short biased_exponent : 4;
            // unsigned short sign : 1;
            // TO:
            // unsigned int mantissa : 23;
            // unsigned int biased_exponent : 8;
            // unsigned int sign : 1;
            var bitsResult = default(IntegerAndSingleUnion);
            int mantissa;
            int biased_exponent;
            int sign;
            int resultMantissa;
            int resultBiasedExponent;
            int resultSign;
            bitsResult.i = 0;
            mantissa = GetMantissa(the8BitValue);
            biased_exponent = GetBiasedExponent(the8BitValue);
            sign = GetSign(the8BitValue);
            resultMantissa = mantissa << 23 - 3;
            if (biased_exponent == 0)
            {
                resultBiasedExponent = 0;
            }
            else
            {
                resultBiasedExponent = biased_exponent - float8bias + float32bias << 23;
            }

            resultSign = sign << 31;

            // ' For debugging.
            // '------
            // ' TEST PASSED:
            // 'If (resultMantissa Or &H7FFFFF) <> &H7FFFFF Then
            // '	Dim i As Integer = 42
            // 'End If
            // '------
            // ' TEST PASSED:
            // 'If (resultBiasedExponent Or &H7F800000) <> &H7F800000 Then
            // '	Dim i As Integer = 42
            // 'End If
            // '------
            // ' TEST PASSED:
            // 'If resultSign <> &H80000000 AndAlso resultSign <> 0 Then
            // '	Dim i As Integer = 42
            // 'End If

            bitsResult.i = resultSign | resultBiasedExponent | resultMantissa;
            return bitsResult.s;
        }

        private bool IsInfinity(byte value)
        {
            int mantissa;
            int biased_exponent;
            mantissa = GetMantissa(value);
            biased_exponent = GetBiasedExponent(value);
            return biased_exponent == 15 & mantissa == 0;
        }

        private bool IsNaN(byte value)
        {
            int mantissa;
            int biased_exponent;
            mantissa = GetMantissa(value);
            biased_exponent = GetBiasedExponent(value);
            return biased_exponent == 15 & mantissa != 0;
        }

        private const int float32bias = 127;
        private const int float8bias = 7;
        private const float maxfloat8bits = 122880.0f;
        private const float half_denorm = 1.0f / 8.0f;
        public byte the8BitValue;

        [StructLayout(LayoutKind.Explicit)]
        public struct IntegerAndSingleUnion
        {
            [FieldOffset(0)]
            public int i;
            [FieldOffset(0)]
            public float s;
        }
    }
}
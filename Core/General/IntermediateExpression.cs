
namespace Crowbar
{
    public class IntermediateExpression
    {
        public IntermediateExpression(string iExpression, int iPrecedence)
        {
            theExpression = iExpression;
            thePrecedence = iPrecedence;
        }

        public string theExpression;
        public int thePrecedence;
    }
}
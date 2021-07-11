
namespace Crowbar
{
    public class SourcePhyVertex
    {
        public SourcePhyVertex()
        {
            theNormal = new SourceVector();
            theNormalIsNormalized = false;
        }

        public SourceVector vertex = new SourceVector();

        public SourceVector Normal
        {
            get
            {
                if (!theNormalIsNormalized)
                {
                    MathModule.VectorNormalize(ref theNormal);
                    theNormalIsNormalized = true;
                }

                return theNormal;
            }
        }

        public SourceVector UnnormalizedNormal
        {
            get
            {
                return theNormal;
            }
        }

        private SourceVector theNormal;
        private bool theNormalIsNormalized;
    }
}

namespace Crowbar
{
    public class SourceMdlFace04
    {
        public SourceMdlFace04()
        {
            for (int i = 0, loopTo = vertexInfo.Length - 1; i <= loopTo; i++)
                vertexInfo[i] = new SourceMdlFaceVertexInfo04();
        }

        // Public vertexIndex(11) As Integer
        // ------
        public SourceMdlFaceVertexInfo04[] vertexInfo = new SourceMdlFaceVertexInfo04[3];
    }
}
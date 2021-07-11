
namespace Crowbar
{
    public class SourceMdlAxisInterpBone2531
    {
        public SourceMdlAxisInterpBone2531()
        {
            for (int i = 0, loopTo = pos.Length - 1; i <= loopTo; i++)
                pos[i] = new SourceVector();
            for (int i = 0, loopTo1 = quat.Length - 1; i <= loopTo1; i++)
                quat[i] = new SourceQuaternion();
        }

        // FROM: Bloodlines SDK source 2015-06-16\sdk-src (16.06.2015)\src\public\studio.h
        // struct mstudioaxisinterpbone_t
        // {
        // int				control;// local transformation of this bone used to calc 3 point blend
        // int				axis;	// axis to check
        // Vector			pos[6];	// X+, X-, Y+, Y-, Z+, Z-
        // Quaternion		quat[6];// X+, X-, Y+, Y-, Z+, Z-
        // };

        public int controlBoneIndex;
        public int axis;
        public SourceVector[] pos = new SourceVector[6];
        public SourceQuaternion[] quat = new SourceQuaternion[6];
    }
}
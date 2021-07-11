﻿using System.IO;

namespace Crowbar
{
    public class SourceVrdFile49
    {

        #region Creation and Destruction

        public SourceVrdFile49(StreamWriter outputFileStream, SourceMdlFileData49 mdlFileData)
        {
            theOutputFileStreamWriter = outputFileStream;
            theMdlFileData = mdlFileData;
        }

        #endregion

        #region Methods

        public void WriteHeaderComment()
        {
            Common.WriteHeaderComment(theOutputFileStreamWriter);
        }

        public void WriteCommands()
        {
            if (theMdlFileData.theBones is object)
            {
                string line = "";
                SourceMdlBone aBone;
                SourceMdlBone aParentBone;
                SourceMdlBone aControlBone;
                SourceMdlBone aParentControlBone;
                SourceMdlQuatInterpBoneInfo aTrigger;
                SourceVector aTriggerTrigger;
                SourceVector aTriggerQuat;
                string aBoneName;
                string aParentBoneName;
                string aParentControlBoneName;
                string aControlBoneName;
                for (int i = 0, loopTo = theMdlFileData.theBones.Count - 1; i <= loopTo; i++)
                {
                    aBone = theMdlFileData.theBones[i];
                    if (aBone.proceduralRuleOffset != 0)
                    {
                        if (aBone.proceduralRuleType == SourceMdlBone.STUDIO_PROC_AXISINTERP)
                        {
                        }
                        else if (aBone.proceduralRuleType == SourceMdlBone.STUDIO_PROC_QUATINTERP)
                        {
                            // <helper> Bip01_L_Elbow Bip01_L_UpperArm Bip01_L_UpperArm Bip01_L_Forearm
                            // <display> 1.5 3 3 100
                            // <basepos> 0 0 0
                            // <trigger> 90 0 0 0 0 0 0 0 0 0
                            // <trigger> 90 0 0 -90 0 0 -45 0 0 0

                            // int i = sscanf( g_szLine, "%s %s %s %s %s", cmd, pBone->bonename, pBone->parentname, pBone->controlparentname, pBone->controlname );
                            aParentBone = theMdlFileData.theBones[aBone.parentBoneIndex];
                            aControlBone = theMdlFileData.theBones[aBone.theQuatInterpBone.controlBoneIndex];
                            aParentControlBone = theMdlFileData.theBones[aControlBone.parentBoneIndex];

                            // NOTE: A bone name in a VRD file must have its characters up to and including the first dot removed.
                            // aBoneName = aBone.theName.Replace("ValveBiped.", "")
                            // aParentBoneName = aParentBone.theName.Replace("ValveBiped.", "")
                            // aParentControlBoneName = aParentControlBone.theName.Replace("ValveBiped.", "")
                            // aControlBoneName = aControlBone.theName.Replace("ValveBiped.", "")
                            aBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aBone.theName);
                            aParentBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aParentBone.theName);
                            aParentControlBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aParentControlBone.theName);
                            aControlBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aControlBone.theName);
                            theOutputFileStreamWriter.WriteLine();
                            line = "<helper>";
                            line += " ";
                            line += aBoneName;
                            line += " ";
                            line += aParentBoneName;
                            line += " ";
                            line += aParentControlBoneName;
                            line += " ";
                            line += aControlBoneName;
                            theOutputFileStreamWriter.WriteLine(line);

                            // 'NOTE: Use "1" for the 3 size values because it looks like they are not used in compile.
                            // line = "<display>"
                            // line += " "
                            // line += "1"
                            // line += " "
                            // line += "1"
                            // line += " "
                            // line += "1"
                            // line += " "
                            // 'TODO: Reverse this to decompile.
                            // 'pAxis->percentage = distance / 100.0;
                            // 'tmp = pInterp->pos[k] + pInterp->basepos + g_bonetable[pInterp->control].pos * pInterp->percentage;
                            // line += "100"
                            // Me.theOutputFileStreamWriter.WriteLine(line)

                            line = "<basepos>";
                            line += " ";
                            line += "0";
                            line += " ";
                            line += "0";
                            line += " ";
                            line += "0";
                            theOutputFileStreamWriter.WriteLine(line);
                            for (int triggerIndex = 0, loopTo1 = aBone.theQuatInterpBone.theTriggers.Count - 1; triggerIndex <= loopTo1; triggerIndex++)
                            {
                                aTrigger = aBone.theQuatInterpBone.theTriggers[triggerIndex];
                                aTriggerTrigger = MathModule.ToEulerAngles(aTrigger.trigger);
                                aTriggerQuat = MathModule.ToEulerAngles(aTrigger.quat);
                                line = "<trigger>";
                                line += " ";
                                // pAxis->tolerance[j] = DEG2RAD( tolerance );
                                line += MathModule.RadiansToDegrees(1d / aTrigger.inverseToleranceAngle).ToString("0.######", Program.TheApp.InternalNumberFormat);

                                // trigger.x = DEG2RAD( trigger.x );
                                // trigger.y = DEG2RAD( trigger.y );
                                // trigger.z = DEG2RAD( trigger.z );
                                // AngleQuaternion( trigger, pAxis->trigger[j] );
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerTrigger.x).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerTrigger.y).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerTrigger.z).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerTrigger.z).ToString("0.######", TheApp.InternalNumberFormat)
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerTrigger.y).ToString("0.######", TheApp.InternalNumberFormat)
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerTrigger.x).ToString("0.######", TheApp.InternalNumberFormat)

                                // ang.x = DEG2RAD( ang.x );
                                // ang.y = DEG2RAD( ang.y );
                                // ang.z = DEG2RAD( ang.z );
                                // AngleQuaternion( ang, pAxis->quat[j] );
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerQuat.x).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerQuat.y).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += MathModule.RadiansToDegrees(aTriggerQuat.z).ToString("0.######", Program.TheApp.InternalNumberFormat);
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerQuat.z).ToString("0.######", TheApp.InternalNumberFormat)
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerQuat.y).ToString("0.######", TheApp.InternalNumberFormat)
                                // line += " "
                                // line += MathModule.RadiansToDegrees(aTriggerQuat.x).ToString("0.######", TheApp.InternalNumberFormat)

                                // VectorAdd( basepos, pos, pAxis->pos[j] );
                                line += " ";
                                line += aTrigger.pos.x.ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += aTrigger.pos.y.ToString("0.######", Program.TheApp.InternalNumberFormat);
                                line += " ";
                                line += aTrigger.pos.z.ToString("0.######", Program.TheApp.InternalNumberFormat);
                                theOutputFileStreamWriter.WriteLine(line);
                            }
                        }
                        else if (aBone.proceduralRuleType == SourceMdlBone.STUDIO_PROC_AIMATBONE || aBone.proceduralRuleType == SourceMdlBone.STUDIO_PROC_AIMATATTACH)
                        {
                            aBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aBone.theName);
                            aParentBone = theMdlFileData.theBones[aBone.theAimAtBone.parentBoneIndex];
                            aParentBoneName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(aParentBone.theName);
                            string anAimName;
                            if (aBone.proceduralRuleType == SourceMdlBone.STUDIO_PROC_AIMATBONE)
                            {
                                SourceMdlBone anAimBone;
                                anAimBone = theMdlFileData.theBones[aBone.theAimAtBone.aimBoneOrAttachmentIndex];
                                anAimName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(anAimBone.theName);
                            }
                            else
                            {
                                SourceMdlAttachment anAimAttachment;
                                anAimAttachment = theMdlFileData.theAttachments[aBone.theAimAtBone.aimBoneOrAttachmentIndex];
                                anAimName = StringClass.RemoveUptoAndIncludingFirstDotCharacterFromString(anAimAttachment.theName);
                            }

                            line = "<aimconstraint>";
                            line += " ";
                            line += aBoneName;
                            line += " ";
                            line += aParentBoneName;
                            line += " ";
                            line += anAimName;
                            theOutputFileStreamWriter.WriteLine(line);
                            line = "<aimvector>";
                            line += " ";
                            line += aBone.theAimAtBone.aim.x.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.aim.y.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.aim.z.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            theOutputFileStreamWriter.WriteLine(line);
                            line = "<upvector>";
                            line += " ";
                            line += aBone.theAimAtBone.up.x.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.up.y.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.up.z.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            theOutputFileStreamWriter.WriteLine(line);
                            line = "<basepos>";
                            line += " ";
                            line += aBone.theAimAtBone.basePos.x.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.basePos.y.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            line += " ";
                            line += aBone.theAimAtBone.basePos.z.ToString("0.######", Program.TheApp.InternalNumberFormat);
                            theOutputFileStreamWriter.WriteLine(line);
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;
        private SourceMdlFileData49 theMdlFileData;

        #endregion

    }
}
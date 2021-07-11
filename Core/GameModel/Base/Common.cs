using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    static class Common
    {
        public static string ReadPhyCollisionTextSection(BinaryReader theInputFileReader, long endOffset)
        {
            string result = "";
            long streamLastPosition;
            try
            {
                // streamLastPosition = theInputFileReader.BaseStream.Length() - 1
                streamLastPosition = endOffset;
                if (streamLastPosition > theInputFileReader.BaseStream.Position)
                {
                    // NOTE: Use -1 to avoid including the null terminator character.
                    result = Conversions.ToString(theInputFileReader.ReadChars((int)(streamLastPosition - theInputFileReader.BaseStream.Position - 1L)));
                    // Read the NULL byte to help with debug logging.
                    theInputFileReader.ReadChar();
                    // Only grab text to the first NULL byte. (Needed for PHY data stored within Titanfall 2 MDL file.)
                    result = result.Substring(0, result.IndexOf('\0'));
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return result;
        }

        public static string GetFlexRule(List<SourceMdlFlexDesc> flexDescs, List<SourceMdlFlexController> flexControllers, SourceMdlFlexRule flexRule)
        {
            string flexRuleEquation;
            flexRuleEquation = Constants.vbTab;
            flexRuleEquation += "%";
            flexRuleEquation += flexDescs[flexRule.flexIndex].theName;
            flexRuleEquation += " = ";
            if (flexRule.theFlexOps is object && flexRule.theFlexOps.Count > 0)
            {
                SourceMdlFlexOp aFlexOp;
                bool dmxFlexOpWasUsed;

                // Convert to infix notation.

                var stack = new Stack<IntermediateExpression>();
                string rightExpr;
                string leftExpr;
                dmxFlexOpWasUsed = false;
                for (int i = 0, loopTo = flexRule.theFlexOps.Count - 1; i <= loopTo; i++)
                {
                    aFlexOp = flexRule.theFlexOps[i];
                    if (aFlexOp.op == SourceMdlFlexOp.STUDIO_CONST)
                    {
                        stack.Push(new IntermediateExpression(Math.Round(aFlexOp.value, 6).ToString("0.######", Program.TheApp.InternalNumberFormat), 10));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_FETCH1)
                    {
                        // int m = pFlexcontroller( (LocalFlexController_t)pops->d.index)->localToGlobal;
                        // stack[k] = src[m];
                        // k++; 
                        stack.Push(new IntermediateExpression(flexControllers[aFlexOp.index].theName, 10));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_FETCH2)
                    {
                        stack.Push(new IntermediateExpression("%" + flexDescs[aFlexOp.index].theName, 10));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_ADD)
                    {
                        var rightIntermediate = stack.Pop();
                        var leftIntermediate = stack.Pop();
                        string newExpr = Convert.ToString(leftIntermediate.theExpression) + " + " + Convert.ToString(rightIntermediate.theExpression);
                        stack.Push(new IntermediateExpression(newExpr, 1));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_SUB)
                    {
                        var rightIntermediate = stack.Pop();
                        var leftIntermediate = stack.Pop();
                        string newExpr = Convert.ToString(leftIntermediate.theExpression) + " - " + Convert.ToString(rightIntermediate.theExpression);
                        stack.Push(new IntermediateExpression(newExpr, 1));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_MUL)
                    {
                        var rightIntermediate = stack.Pop();
                        if (rightIntermediate.thePrecedence < 2)
                        {
                            rightExpr = "(" + Convert.ToString(rightIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            rightExpr = rightIntermediate.theExpression;
                        }

                        var leftIntermediate = stack.Pop();
                        if (leftIntermediate.thePrecedence < 2)
                        {
                            leftExpr = "(" + Convert.ToString(leftIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            leftExpr = leftIntermediate.theExpression;
                        }

                        string newExpr = leftExpr + " * " + rightExpr;
                        stack.Push(new IntermediateExpression(newExpr, 2));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_DIV)
                    {
                        var rightIntermediate = stack.Pop();
                        if (rightIntermediate.thePrecedence < 2)
                        {
                            rightExpr = "(" + Convert.ToString(rightIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            rightExpr = rightIntermediate.theExpression;
                        }

                        var leftIntermediate = stack.Pop();
                        if (leftIntermediate.thePrecedence < 2)
                        {
                            leftExpr = "(" + Convert.ToString(leftIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            leftExpr = leftIntermediate.theExpression;
                        }

                        string newExpr = leftExpr + " / " + rightExpr;
                        stack.Push(new IntermediateExpression(newExpr, 2));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_NEG)
                    {
                        var rightIntermediate = stack.Pop();
                        string newExpr = "-" + rightIntermediate.theExpression;
                        stack.Push(new IntermediateExpression(newExpr, 10));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_EXP)
                    {
                        int ignoreThisOpBecauseItIsMistakeToBeHere = 4242;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_OPEN)
                    {
                        int ignoreThisOpBecauseItIsMistakeToBeHere = 4242;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_CLOSE)
                    {
                        int ignoreThisOpBecauseItIsMistakeToBeHere = 4242;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_COMMA)
                    {
                        int ignoreThisOpBecauseItIsMistakeToBeHere = 4242;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_MAX)
                    {
                        var rightIntermediate = stack.Pop();
                        if (rightIntermediate.thePrecedence < 5)
                        {
                            rightExpr = "(" + Convert.ToString(rightIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            rightExpr = rightIntermediate.theExpression;
                        }

                        var leftIntermediate = stack.Pop();
                        if (leftIntermediate.thePrecedence < 5)
                        {
                            leftExpr = "(" + Convert.ToString(leftIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            leftExpr = leftIntermediate.theExpression;
                        }

                        string newExpr = "max(" + leftExpr + ", " + rightExpr + ")";
                        stack.Push(new IntermediateExpression(newExpr, 5));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_MIN)
                    {
                        var rightIntermediate = stack.Pop();
                        if (rightIntermediate.thePrecedence < 5)
                        {
                            rightExpr = "(" + Convert.ToString(rightIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            rightExpr = rightIntermediate.theExpression;
                        }

                        var leftIntermediate = stack.Pop();
                        if (leftIntermediate.thePrecedence < 5)
                        {
                            leftExpr = "(" + Convert.ToString(leftIntermediate.theExpression) + ")";
                        }
                        else
                        {
                            leftExpr = leftIntermediate.theExpression;
                        }

                        string newExpr = "min(" + leftExpr + ", " + rightExpr + ")";
                        stack.Push(new IntermediateExpression(newExpr, 5));
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_2WAY_0)
                    {
                        // TODO: SourceMdlFlexOp.STUDIO_2WAY_0
                        // '#define STUDIO_2WAY_0	15	// Fetch a value from a 2 Way slider for the 1st value RemapVal( 0.0, 0.5, 0.0, 1.0 )
                        // 'int m = pFlexcontroller( (LocalFlexController_t)pops->d.index )->localToGlobal;
                        // 'stack[ k ] = RemapValClamped( src[m], -1.0f, 0.0f, 1.0f, 0.0f );
                        // 'k++; 
                        string newExpression;
                        // = C + (D - C) * (min(max((val - A) / (B - A), 0.0f), 1.0f))
                        // "1 - (min(max(" + flexControllers(aFlexOp.index).theName + " + 1, 0), 1))"
                        newExpression = "(1 - (min(max(" + flexControllers[aFlexOp.index].theName + " + 1, 0), 1)))";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_2WAY_1)
                    {
                        // TODO:			   SourceMdlFlexOp.STUDIO_2WAY_1()
                        // #define STUDIO_2WAY_1	16	// Fetch a value from a 2 Way slider for the 2nd value RemapVal( 0.5, 1.0, 0.0, 1.0 )
                        // int m = pFlexcontroller( (LocalFlexController_t)pops->d.index )->localToGlobal;
                        // stack[ k ] = RemapValClamped( src[m], 0.0f, 1.0f, 0.0f, 1.0f );
                        // k++; 
                        string newExpression;
                        // = C + (D - C) * (min(max((val - A) / (B - A), 0.0f), 1.0f))
                        // "(min(max(" + flexControllers(aFlexOp.index).theName + ", 0), 1))"
                        newExpression = "(min(max(" + flexControllers[aFlexOp.index].theName + ", 0), 1))";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_NWAY)
                    {
                        // TODO:			   SourceMdlFlexOp.STUDIO_NWAY()
                        SourceMdlFlexController v;
                        v = flexControllers[aFlexOp.index];
                        IntermediateExpression valueControllerIndex;
                        string flValue;
                        valueControllerIndex = stack.Pop();
                        flValue = flexControllers[Conversions.ToInteger(valueControllerIndex.theExpression)].theName;
                        IntermediateExpression filterRampW;
                        IntermediateExpression filterRampZ;
                        IntermediateExpression filterRampY;
                        IntermediateExpression filterRampX;
                        filterRampW = stack.Pop();
                        filterRampZ = stack.Pop();
                        filterRampY = stack.Pop();
                        filterRampX = stack.Pop();
                        string greaterThanX;
                        string lessThanY;
                        string remapX;
                        string greaterThanEqualY;
                        string lessThanEqualZ;
                        string greaterThanZ;
                        string lessThanW;
                        string remapZ;
                        greaterThanX = "min(1, (-min(0, (" + filterRampX.theExpression + " - " + flValue + "))))";
                        lessThanY = "min(1, (-min(0, (" + flValue + " - " + filterRampY.theExpression + "))))";
                        remapX = "min(max((" + flValue + " - " + filterRampX.theExpression + ") / (" + filterRampY.theExpression + " - " + filterRampX.theExpression + "), 0), 1)";
                        greaterThanEqualY = "-(min(1, (-min(0, (" + flValue + " - " + filterRampY.theExpression + ")))) - 1)";
                        lessThanEqualZ = "-(min(1, (-min(0, (" + filterRampZ.theExpression + " - " + flValue + ")))) - 1)";
                        greaterThanZ = "min(1, (-min(0, (" + filterRampZ.theExpression + " - " + flValue + "))))";
                        lessThanW = "min(1, (-min(0, (" + flValue + " - " + filterRampW.theExpression + "))))";
                        remapZ = "(1 - (min(max((" + flValue + " - " + filterRampZ.theExpression + ") / (" + filterRampW.theExpression + " - " + filterRampZ.theExpression + "), 0), 1)))";
                        flValue = "((" + greaterThanX + " * " + lessThanY + ") * " + remapX + ") + (" + greaterThanEqualY + " * " + lessThanEqualZ + ") + ((" + greaterThanZ + " * " + lessThanW + ") * " + remapZ + ")";
                        string newExpression;
                        newExpression = "((" + flValue + ") * (" + v.theName + "))";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_COMBO)
                    {
                        // #define STUDIO_COMBO	18	// Perform a combo operation (essentially multiply the last N values on the stack)
                        // int m = pops->d.index;
                        // int km = k - m;
                        // for ( int i = km + 1; i < k; ++i )
                        // {
                        // stack[ km ] *= stack[ i ];
                        // }
                        // k = k - m + 1;
                        int count;
                        string newExpression;
                        IntermediateExpression intermediateExp;
                        count = aFlexOp.index;
                        newExpression = "";
                        intermediateExp = stack.Pop();
                        newExpression += intermediateExp.theExpression;
                        for (int j = 2, loopTo1 = count; j <= loopTo1; j++)
                        {
                            intermediateExp = stack.Pop();
                            newExpression += " * " + intermediateExp.theExpression;
                        }

                        newExpression = "(" + newExpression + ")";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_DOMINATE)
                    {
                        // int m = pops->d.index;
                        // int km = k - m;
                        // float dv = stack[ km ];
                        // for ( int i = km + 1; i < k; ++i )
                        // {
                        // dv *= stack[ i ];
                        // }
                        // stack[ km - 1 ] *= 1.0f - dv;
                        // k -= m;
                        int count;
                        string newExpression;
                        IntermediateExpression intermediateExp;
                        count = aFlexOp.index;
                        newExpression = "";
                        intermediateExp = stack.Pop();
                        newExpression += intermediateExp.theExpression;
                        for (int j = 2, loopTo2 = count; j <= loopTo2; j++)
                        {
                            intermediateExp = stack.Pop();
                            newExpression += " * " + intermediateExp.theExpression;
                        }

                        intermediateExp = stack.Pop();
                        newExpression = intermediateExp.theExpression + " * (1 - " + newExpression + ")";
                        newExpression = "(" + newExpression + ")";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_DME_LOWER_EYELID)
                    {
                        SourceMdlFlexController pCloseLidV;
                        pCloseLidV = flexControllers[aFlexOp.index];
                        string flCloseLidV;
                        string flCloseLidVMin = Math.Round(pCloseLidV.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flCloseLidVMax = Math.Round(pCloseLidV.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flCloseLidV = "(min(max((" + pCloseLidV.theName + " - " + flCloseLidVMin + ") / (" + flCloseLidVMax + " - " + flCloseLidVMin + "), 0), 1))";
                        var closeLidIndex = stack.Pop();
                        SourceMdlFlexController pCloseLid;
                        pCloseLid = flexControllers[Conversions.ToInteger(closeLidIndex.theExpression)];
                        string flCloseLid;
                        string flCloseLidMin = Math.Round(pCloseLid.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flCloseLidMax = Math.Round(pCloseLid.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flCloseLid = "(min(max((" + pCloseLid.theName + " - " + flCloseLidMin + ") / (" + flCloseLidMax + " - " + flCloseLidMin + "), 0), 1))";

                        // Unused, but need to pop it off the stack.
                        var blinkIndex = stack.Pop();
                        var eyeUpDownIndex = stack.Pop();
                        SourceMdlFlexController pEyeUpDown;
                        pEyeUpDown = flexControllers[Conversions.ToInteger(eyeUpDownIndex.theExpression)];
                        string flEyeUpDown;
                        string flEyeUpDownMin = Math.Round(pEyeUpDown.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flEyeUpDownMax = Math.Round(pEyeUpDown.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flEyeUpDown = "(-1 + 2 * (min(max((" + pEyeUpDown.theName + " - " + flEyeUpDownMin + ") / (" + flEyeUpDownMax + " - " + flEyeUpDownMin + "), 0), 1)))";
                        string newExpression;
                        newExpression = "(min(1, (1 - " + flEyeUpDown + ")) * (1 - " + flCloseLidV + ") * " + flCloseLid + ")";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else if (aFlexOp.op == SourceMdlFlexOp.STUDIO_DME_UPPER_EYELID)
                    {
                        SourceMdlFlexController pCloseLidV;
                        pCloseLidV = flexControllers[aFlexOp.index];
                        string flCloseLidV;
                        string flCloseLidVMin = Math.Round(pCloseLidV.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flCloseLidVMax = Math.Round(pCloseLidV.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flCloseLidV = "(min(max((" + pCloseLidV.theName + " - " + flCloseLidVMin + ") / (" + flCloseLidVMax + " - " + flCloseLidVMin + "), 0), 1))";
                        var closeLidIndex = stack.Pop();
                        SourceMdlFlexController pCloseLid;
                        pCloseLid = flexControllers[Conversions.ToInteger(closeLidIndex.theExpression)];
                        string flCloseLid;
                        string flCloseLidMin = Math.Round(pCloseLid.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flCloseLidMax = Math.Round(pCloseLid.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flCloseLid = "(min(max((" + pCloseLid.theName + " - " + flCloseLidMin + ") / (" + flCloseLidMax + " - " + flCloseLidMin + "), 0), 1))";

                        // Unused, but need to pop it off the stack.
                        var blinkIndex = stack.Pop();
                        var eyeUpDownIndex = stack.Pop();
                        SourceMdlFlexController pEyeUpDown;
                        pEyeUpDown = flexControllers[Conversions.ToInteger(eyeUpDownIndex.theExpression)];
                        string flEyeUpDown;
                        string flEyeUpDownMin = Math.Round(pEyeUpDown.min, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        string flEyeUpDownMax = Math.Round(pEyeUpDown.max, 6).ToString("0.######", Program.TheApp.InternalNumberFormat);
                        flEyeUpDown = "(-1 + 2 * (min(max((" + pEyeUpDown.theName + " - " + flEyeUpDownMin + ") / (" + flEyeUpDownMax + " - " + flEyeUpDownMin + "), 0), 1)))";
                        string newExpression;
                        newExpression = "(min(1, (1 + " + flEyeUpDown + ")) * " + flCloseLidV + " * " + flCloseLid + ")";
                        stack.Push(new IntermediateExpression(newExpression, 5));
                        dmxFlexOpWasUsed = true;
                    }
                    else
                    {
                        stack.Clear();
                        break;
                    }
                }

                // The loop above leaves the final expression on the top of the stack.
                if (dmxFlexOpWasUsed)
                {
                    flexRuleEquation += stack.Peek().theExpression + " // WARNING: Expression is an approximation of what can only be done via DMX file.";
                }
                else if (stack.Count == 1)
                {
                    flexRuleEquation += stack.Peek().theExpression;
                }
                else if (stack.Count == 0 || stack.Count > 1)
                {
                    flexRuleEquation = "// " + flexRuleEquation + stack.Peek().theExpression + " // ERROR: Unknown flex operation.";
                }
                else
                {
                    flexRuleEquation = "// [Empty flex rule found and ignored.]";
                }
            }

            return flexRuleEquation;
        }

        public static void ProcessTexturePaths(List<string> theTexturePaths, List<SourceMdlTexture> theTextures, List<string> theModifiedTexturePaths, List<string> theModifiedTextureFileNames)
        {
            if (theTexturePaths is object)
            {
                foreach (string aTexturePath in theTexturePaths)
                    theModifiedTexturePaths.Add(aTexturePath);
            }

            if (theTextures is object)
            {
                foreach (SourceMdlTexture aTexture in theTextures)
                    theModifiedTextureFileNames.Add(aTexture.thePathFileName);
            }

            //if (Program.TheApp.Settings.DecompileRemovePathFromSmdMaterialFileNamesIsChecked)
            //TODO: 
            if (false)
            {
                // SourceFileNamesModule.CopyPathsFromTextureFileNamesToTexturePaths(theModifiedTexturePaths, theModifiedTextureFileNames)
                SourceFileNamesModule.MovePathsFromTextureFileNamesToTexturePaths(ref theModifiedTexturePaths, ref theModifiedTextureFileNames);
            }
        }

        public static void WriteHeaderComment(StreamWriter outputFileStreamWriter)
        {
            //if (!Program.TheApp.Settings.DecompileStricterFormatIsChecked)
            //TODO: IMPORTANT MAYBE FALSE???
            if (true)
            {
                string line = "";
                line = "// ";
                line += Program.TheApp.GetHeaderComment();
                outputFileStreamWriter.WriteLine(line);
            }
        }
    }
}
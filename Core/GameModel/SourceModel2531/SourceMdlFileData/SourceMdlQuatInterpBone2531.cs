﻿using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlQuatInterpBone2531
    {

        // FROM: Bloodlines SDK source 2015-06-16\sdk-src (16.06.2015)\src\public\studio.h
        // struct mstudioquatinterpbone_t
        // {
        // int				control;// local transformation to check
        // int				numtriggers;
        // int				triggerindex;
        // inline mstudioquatinterpinfo_t *pTrigger( int i ) const { return  (mstudioquatinterpinfo_t *)(((byte *)this) + triggerindex) + i; };
        // };

        public int controlBoneIndex;
        public int triggerCount;
        public int triggerOffset;
        public List<SourceMdlQuatInterpInfo2531> theTriggers;
    }
}
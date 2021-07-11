﻿using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlSequenceDesc36
    {
        public SourceMdlSequenceDesc36()
        {
            // short				anim[MAXSTUDIOBLENDS][MAXSTUDIOBLENDS];	// f64: 16x16x2 = 512 bytes each anim a short
            anim = new List<List<short>>(SourceModule2531.MAXSTUDIOBLENDS);
            for (int rowIndex = 0, loopTo = SourceModule2531.MAXSTUDIOBLENDS - 1; rowIndex <= loopTo; rowIndex++)
            {
                var animRow = new List<short>(SourceModule2531.MAXSTUDIOBLENDS);
                for (int columnIndex = 0, loopTo1 = SourceModule2531.MAXSTUDIOBLENDS - 1; columnIndex <= loopTo1; columnIndex++)
                    animRow.Add(0);
                anim.Add(animRow);
            }
        }

        // // sequence descriptions
        // struct mstudioseqdesc_t
        // {
        // int					szlabelindex;
        // inline char * const pszLabel( void ) const { return ((char *)this) + szlabelindex; }
        // 
        // int					szactivitynameindex;
        // inline char * const pszActivityName( void ) const { return ((char *)this) + szactivitynameindex; }
        // 
        // int					flags;		// looping/non-looping flags
        // 
        // int					activity;	// initialized at loadtime to game DLL values
        // int					actweight;
        // 
        // int					numevents;
        // int					eventindex;
        // inline mstudioevent_t *pEvent( int i ) const { return (mstudioevent_t *)(((byte *)this) + eventindex) + i; };
        // 
        // Vector				bbmin;		// per sequence bounding box
        // Vector				bbmax;		
        // 
        // int					numblends;
        // 
        // int					anim[MAXSTUDIOBLENDS][MAXSTUDIOBLENDS];	// animation number
        // 
        // int					movementindex;	// [blend] float array for blended movement
        // int					groupsize[2];
        // int					paramindex[2];	// X, Y, Z, XR, YR, ZR
        // float				paramstart[2];	// local (0..1) starting value
        // float				paramend[2];	// local (0..1) ending value
        // int					paramparent;
        // 
        // int					seqgroup;		// sequence group for demand loading
        // 
        // float				fadeintime;		// ideal cross fate in time (0.2 default)
        // float				fadeouttime;	// ideal cross fade out time (0.2 default)
        // 
        // int					entrynode;		// transition node at entry
        // int					exitnode;		// transition node at exit
        // int					nodeflags;		// transition rules
        // 
        // float				entryphase;		// used to match entry gait
        // float				exitphase;		// used to match exit gait
        // 
        // float				lastframe;		// frame that should generation EndOfSequence
        // 
        // int					nextseq;		// auto advancing sequences
        // int					pose;			// index of delta animation between end and nextseq
        // 
        // int					numikrules;
        // 
        // int					numautolayers;	//
        // int					autolayerindex;
        // inline mstudioautolayer_t *pAutolayer( int i ) const { return (mstudioautolayer_t *)(((byte *)this) + autolayerindex) + i; };
        // 
        // int					weightlistindex;
        // float				*pBoneweight( int i ) const { return ((float *)(((byte *)this) + weightlistindex) + i); };
        // float				weight( int i ) const { return *(pBoneweight( i)); };
        // 
        // int					posekeyindex;
        // float				*pPoseKey( int iParam, int iAnim ) const { return (float *)(((byte *)this) + posekeyindex) + iParam * groupsize[0] + iAnim; }
        // float				poseKey( int iParam, int iAnim ) const { return *(pPoseKey( iParam, iAnim )); }
        // 
        // int					numiklocks;
        // int					iklockindex;
        // inline mstudioiklock_t *pIKLock( int i ) const { return (mstudioiklock_t *)(((byte *)this) + iklockindex) + i; };
        // 
        // // Key values
        // int					keyvalueindex;
        // int					keyvaluesize;
        // inline const char * KeyValueText( void ) const { return keyvaluesize != 0 ? ((char *)this) + keyvalueindex : NULL; }
        // 
        // int					unused[3];		// remove/add as appropriate (grow back to 8 ints on version change!)
        // };

        public int nameOffset;
        public int activityNameOffset;
        public int flags;
        public int activity;
        public int activityWeight;
        public int eventCount;
        public int eventOffset;
        public SourceVector bbMin = new SourceVector();
        public SourceVector bbMax = new SourceVector();
        public int blendCount;
        public List<List<short>> anim;
        public int movementIndex;
        public int[] groupSize = new int[2];
        public int[] paramIndex = new int[2];
        public float[] paramStart = new float[2];
        public float[] paramEnd = new float[2];
        public int paramParent;
        public int sequenceGroup;
        public float fadeInTime;
        public float fadeOutTime;
        public int entryNodeIndex;
        public int exitNodeIndex;
        public int nodeFlags;
        public float entryPhase;
        public float exitPhase;
        public float lastFrame;
        public int nextSeq;
        public int pose;
        public int ikRuleCount;
        public int autoLayerCount;
        public int autoLayerOffset;
        public int weightOffset;
        public int poseKeyOffset;
        public int ikLockCount;
        public int ikLockOffset;
        public int keyValueOffset;
        public int keyValueSize;
        public int[] unused = new int[3];
        public string theActivityName;
        public List<short> theAnimDescIndexes;
        public List<SourceMdlAutoLayer37> theAutoLayers;
        public List<double> theBoneWeights;
        public List<SourceMdlEvent37> theEvents;
        public List<SourceMdlIkLock37> theIkLocks;
        public string theKeyValues;
        public string theName;
        public List<double> thePoseKeys;
        public int theWeightListIndex;
        public bool theBoneWeightsAreDefault;
    }
}
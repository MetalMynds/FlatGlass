using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities.Interop.Converters
{
    // Warning: <unknown> 3: Could not locate include file converr.h
    // Error: Failed to resolve name 'FCE'
    // Error: Failed to resolve name 'FCE'
    // Error: Failed to resolve name 'FCE'

    internal partial class Constants
    {

        /// CONVAPI_H -> 
        /// Error generating expression: Value cannot be null.
        ///Parameter name: node
        public const string CONVAPI_H = "";

        /// fOptPicture -> 0x0001
        public const int fOptPicture = 1;

        /// fOptLayoutInfo -> 0x0002
        public const int fOptLayoutInfo = 2;

        /// fOptPctComplete -> 0x0004
        public const int fOptPctComplete = 4;

        /// fRegAppPctComp -> 0x00000001
        public const int fRegAppPctComp = 1;

        /// fRegAppNoBinary -> 0x00000002
        public const int fRegAppNoBinary = 2;

        /// fRegAppPreview -> 0x00000004
        public const int fRegAppPreview = 4;

        /// fRegAppSupportNonOem -> 0x00000008
        public const int fRegAppSupportNonOem = 8;

        /// fRegAppIndexing -> 0x00000010
        public const int fRegAppIndexing = 16;

        /// RegAppOpcodeFilename -> 0x80
        public const int RegAppOpcodeFilename = 128;

        /// RegAppOpcodeInterimPath -> 0x81
        public const int RegAppOpcodeInterimPath = 129;

        /// RegAppOpcodeVer -> 0x01
        public const int RegAppOpcodeVer = 1;

        /// RegAppOpcodeDocfile -> 0x02
        public const int RegAppOpcodeDocfile = 2;

        /// RegAppOpcodeCharset -> 0x03
        public const int RegAppOpcodeCharset = 3;

        /// RegAppOpcodeReloadOnSave -> 0x04
        public const int RegAppOpcodeReloadOnSave = 4;

        /// RegAppOpcodePicPlacehold -> 0x05
        public const int RegAppOpcodePicPlacehold = 5;

        /// RegAppOpcodeFavourUnicode -> 0x06
        public const int RegAppOpcodeFavourUnicode = 6;

        /// RegAppOpcodeNoClassifyChars -> 0x07
        public const int RegAppOpcodeNoClassifyChars = 7;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct PCVT
    {

        /// short
        public short cbpcvt;

        /// short
        public short wVersion;

        /// short
        public short wPctWord;

        /// short
        public short wPctConvtr;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct Anonymous_75de2bd4_1e37_4542_b72d_92b7a410dd65
    {

        /// fPicture : 1
        ///fLayoutInfo : 1
        ///fPctComplete : 1
        ///AnonymousMember1 : 13
        public uint bitvector1;

        public uint fPicture
        {
            get
            {
                return ((uint)((this.bitvector1 & 1u)));
            }
            set
            {
                this.bitvector1 = ((uint)((value | this.bitvector1)));
            }
        }

        public uint fLayoutInfo
        {
            get
            {
                return ((uint)(((this.bitvector1 & 2u)
                            / 2)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 2)
                            | this.bitvector1)));
            }
        }

        public uint fPctComplete
        {
            get
            {
                return ((uint)(((this.bitvector1 & 4u)
                            / 4)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 4)
                            | this.bitvector1)));
            }
        }

        public uint AnonymousMember1
        {
            get
            {
                return ((uint)(((this.bitvector1 & 65528u)
                            / 8)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 8)
                            | this.bitvector1)));
            }
        }
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct Anonymous_ff69448c_f117_4cf8_af77_71189496962c
    {

        /// short
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public short rgf;

        /// Anonymous_75de2bd4_1e37_4542_b72d_92b7a410dd65
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public Anonymous_75de2bd4_1e37_4542_b72d_92b7a410dd65 Struct1;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct RFUOPT
    {

        /// Anonymous_ff69448c_f117_4cf8_af77_71189496962c
        public Anonymous_ff69448c_f117_4cf8_af77_71189496962c Union1;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct Anonymous_a3fbd784_d4f9_4ba2_8095_f2624303b5fd
    {

        /// fAcceptPctComp : 1
        ///fNoBinary : 1
        ///fPreview : 1
        ///fDontNeedOemConvert : 1
        ///fIndexing : 1
        ///unused : 27
        public uint bitvector1;

        public uint fAcceptPctComp
        {
            get
            {
                return ((uint)((this.bitvector1 & 1u)));
            }
            set
            {
                this.bitvector1 = ((uint)((value | this.bitvector1)));
            }
        }

        public uint fNoBinary
        {
            get
            {
                return ((uint)(((this.bitvector1 & 2u)
                            / 2)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 2)
                            | this.bitvector1)));
            }
        }

        public uint fPreview
        {
            get
            {
                return ((uint)(((this.bitvector1 & 4u)
                            / 4)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 4)
                            | this.bitvector1)));
            }
        }

        public uint fDontNeedOemConvert
        {
            get
            {
                return ((uint)(((this.bitvector1 & 8u)
                            / 8)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 8)
                            | this.bitvector1)));
            }
        }

        public uint fIndexing
        {
            get
            {
                return ((uint)(((this.bitvector1 & 16u)
                            / 16)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 16)
                            | this.bitvector1)));
            }
        }

        public uint unused
        {
            get
            {
                return ((uint)(((this.bitvector1 & 4294967264u)
                            / 32)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 32)
                            | this.bitvector1)));
            }
        }
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct Anonymous_1ea77779_f3de_460d_8f1e_4c15425c9a8e
    {

        /// Anonymous_a3fbd784_d4f9_4ba2_8095_f2624303b5fd
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public Anonymous_a3fbd784_d4f9_4ba2_8095_f2624303b5fd Struct1;

        /// unsigned int
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public uint lfRegApp;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct LFREGAPP
    {

        /// Anonymous_1ea77779_f3de_460d_8f1e_4c15425c9a8e
        public Anonymous_1ea77779_f3de_460d_8f1e_4c15425c9a8e Union1;
    }

    //[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    //public struct REGAPP
    //{

    //    /// short
    //    public short cbStruct;

    //    /// char[]
    //    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst =-1)]
    //    public string rgbOpcodeData;
    //}

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    struct Anonymous_46c56a76_fcca_4cde_8d45_e3bd107bea05
    {

        /// fDocfile : 1
        ///fNonDocfile : 1
        ///AnonymousMember1 : 14
        public uint bitvector1;

        public uint fDocfile
        {
            get
            {
                return ((uint)((this.bitvector1 & 1u)));
            }
            set
            {
                this.bitvector1 = ((uint)((value | this.bitvector1)));
            }
        }

        public uint fNonDocfile
        {
            get
            {
                return ((uint)(((this.bitvector1 & 2u)
                            / 2)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 2)
                            | this.bitvector1)));
            }
        }

        public uint AnonymousMember1
        {
            get
            {
                return ((uint)(((this.bitvector1 & 65532u)
                            / 4)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 4)
                            | this.bitvector1)));
            }
        }
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct Anonymous_59892578_633f_4eb9_84d3_892eaa991e8b
    {

        /// Anonymous_46c56a76_fcca_4cde_8d45_e3bd107bea05
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public Anonymous_46c56a76_fcca_4cde_8d45_e3bd107bea05 Struct1;

        /// short
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public short grfType;
    }

    //[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    //public struct REGAPPRET
    //{

    //    /// short
    //    public short cbStruct;

    //    /// char
    //    public byte cbSizefDocfile;

    //    /// char
    //    public byte opcodefDocfile;

    //    /// Anonymous_59892578_633f_4eb9_84d3_892eaa991e8b
    //    public Anonymous_59892578_633f_4eb9_84d3_892eaa991e8b Union1;

    //    /// char
    //    public byte cbSizeVer;

    //    /// char
    //    public byte opcodeVer;

    //    /// short
    //    public short verMajor;

    //    /// short
    //    public short verMinor;

    //    /// char
    //    public byte cbSizeCharset;

    //    /// char
    //    public byte opcodeCharset;

    //    /// char
    //    public byte charset;

    //    /// char[]
    //    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = -1)]
    //    public string opcodesOptional;
    //}

    /// Return Type: int
    [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
    public delegate int PFN_RTF();

    internal partial class MSWRD832
    {

        /// Return Type: int
        ///hWnd: HANDLE->void*
        ///szModule: char*
        //[System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "InitConverter32", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        //public static extern int InitConverter32(System.IntPtr hWnd, System.IntPtr szModule);
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "InitConverter32", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int InitConverter32(System.IntPtr hWnd, String szModule);

        /// Return Type: void
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "UninitConverter", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void UninitConverter();

        /// Return Type: FCE->short
        ///ghszFile: HANDLE->void*
        ///pstgForeign: void*
        ///ghBuff: HANDLE->void*
        ///ghszClass: HANDLE->void*
        ///ghszSubset: HANDLE->void*
        ///lpfnOut: PFN_RTF
        [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "ForeignToRtf32", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short ForeignToRtf32(System.IntPtr ghszFile, System.IntPtr pstgForeign, System.IntPtr ghBuff, System.IntPtr ghszClass, System.IntPtr ghszSubset, PFN_RTF lpfnOut);

        /// Return Type: FCE->short
        ///ghszFile: HANDLE->void*
        ///pstgForeign: void*
        ///ghBuff: HANDLE->void*
        ///ghshClass: HANDLE->void*
        ///lpfnIn: PFN_RTF
        [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "RtfToForeign32", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short RtfToForeign32(System.IntPtr ghszFile, System.IntPtr pstgForeign, System.IntPtr ghBuff, System.IntPtr ghshClass, PFN_RTF lpfnIn);

        /// Return Type: void
        ///haszClass: HANDLE->void*
        ///haszDescrip: HANDLE->void*
        ///haszExt: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "GetReadNames", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void GetReadNames(System.IntPtr haszClass, System.IntPtr haszDescrip, System.IntPtr haszExt);


        /// Return Type: void
        ///haszClass: HANDLE->void*
        ///haszDescrip: HANDLE->void*
        ///haszExt: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "GetWriteNames", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void GetWriteNames(System.IntPtr haszClass, System.IntPtr haszDescrip, System.IntPtr haszExt);


        /// Return Type: HGLOBAL->HANDLE->void*
        ///lFlags: unsigned int
        ///lpFuture: void*
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "RegisterApp", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr RegisterApp(uint lFlags, System.IntPtr lpFuture);


        /// Return Type: int
        ///fce: int
        ///lpszError: char*
        ///cb: int
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "CchFetchLpszError", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CchFetchLpszError(int fce, System.IntPtr lpszError, int cb);


        /// Return Type: int
        ///hkeyRoot: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("MSWRD832.CNV", EntryPoint = "FRegisterConverter", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int FRegisterConverter(System.IntPtr hkeyRoot);

    }

}

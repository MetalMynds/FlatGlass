using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MetalMynds.Utilities.Interop.Converters;

namespace MetalMynds.Utilities
{

    //public class ConverterHelper : IDisposable 
    //{
        
    //    public ConverterHelper(IWin32Window Owner)
    //    {
    //        if (MSWRD832.InitConverter32(Owner.Handle, "ConverterHelper") == 0)
    //        {
    //            throw new Exception(String.Format("ConverterHelper\nInitialise Converter Failed!"));
    //        }
    //    }

    //    public void RTFToWordDoc(String SourceFilename,String TargetFilename) 
    //    {

    //        //MSWRD832.RtfToForeign32(

    //    }

    //    // Dispose() calls Dispose(true)
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }
    //    // NOTE: Leave out the finalizer altogether if this class doesn't 
    //    // own unmanaged resources itself, but leave the other methods
    //    // exactly as they are. 
    //    ~ConverterHelper()
    //    {
    //        // Finalizer calls Dispose(false)
    //        Dispose(false);
    //    }
    //    // The bulk of the clean-up code is implemented in Dispose(bool)
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            //// free managed resources
    //            //if (managedResource != null)
    //            //{
    //            //    managedResource.Dispose();
    //            //    managedResource = null;
    //            //}

    //            MSWRD832.UninitConverter();

    //        }

    //        // free native resources if there are any.
    //        //if (nativeResource != IntPtr.Zero)
    //        //{
    //        //    Marshal.FreeHGlobal(nativeResource);
    //        //    nativeResource = IntPtr.Zero;
    //        //}

    //    }

    //}
}

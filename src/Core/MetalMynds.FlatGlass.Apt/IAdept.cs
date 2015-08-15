using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MetalMynds.FlatGlass.Apt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHavelock" in both code and config file together.
    [ServiceContract]
    public interface IAdept
    {
        [OperationContract]
        String GetWindow(String Name);

        [OperationContract]
        String[] ListWindows();

    }
}

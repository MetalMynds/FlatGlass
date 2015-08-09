using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public class WorkerParameters
    {
        protected String BaseID;
        protected Object BaseValue;

        public WorkerParameters(String WorkerID,Object Value)
        {
            BaseID = WorkerID;
            BaseValue = Value;
        }

        public virtual String WorkerID { get { return BaseID; } }

        public virtual Object Value { get { return BaseValue; } set { BaseValue = value; } }

    }
}

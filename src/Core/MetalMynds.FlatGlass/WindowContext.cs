using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.FlatGlass
{
    public class WindowContext
    {
        private readonly List<IHoldPlace> _placeHolders;

        private WindowContext()
        {
            _placeHolders = new List<IHoldPlace>();
        }

        public List<IHoldPlace> PlaceHolders { get { return _placeHolders; } }

        public virtual void Close()
        {
            _placeHolders.Clear();
        }

        public static WindowContext Create()
        {
            return new WindowContext();
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="IManageState.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IManageState
    {
        bool IsReady { get; }
        bool WaitForReady(TimeSpan Timeout);
    }
}

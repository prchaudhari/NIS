﻿
namespace nIS
{
    #region References
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public enum BatchStatus
    {
        New,
        Running,
        Completed,
        Failed,
        BatchDataNotAvailable,
        Approved,
        Archived,
        ApprovalInProgress
    }
}

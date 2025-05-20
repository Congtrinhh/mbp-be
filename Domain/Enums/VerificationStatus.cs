using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// current verification status
    /// </summary>
    public enum VerificationStatus
    {
        /// <summary>
        /// waiting for verification
        /// </summary>
        Pending = 0,
        /// <summary>
        /// verified
        /// </summary>
        Verified = 1,
        /// <summary>
        /// rejected
        /// </summary>
        Rejected = 2
    }
}

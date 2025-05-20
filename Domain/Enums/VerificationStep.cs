using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// current verification step
    /// </summary>
    public enum VerificationStep
    {
        /// <summary>
        /// not started
        /// </summary>
        None = 0,
        /// <summary>
        /// face image
        /// </summary>
        Face = 1,
        /// <summary>
        /// front side of the ID
        /// </summary>
        IdFront = 2,
        /// <summary>
        /// back side of the ID
        /// </summary>
        IdBack = 3,
    }
}

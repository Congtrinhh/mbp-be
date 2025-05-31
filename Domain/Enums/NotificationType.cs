using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum NotificationType
    {
        /// <summary>
        /// a user send an offer to an MC
        /// </summary>
        SendOffer = 1,
        /// <summary>
        /// an offer is rejected by an MC
        /// </summary>
        OfferRejected = 2,
        /// <summary>
        /// an offer is approved by an MC
        /// </summary>
        OfferApproved = 3,
        /// <summary>
        /// remind to review mc or review client
        /// </summary>
        ReviewReminder = 4,
        /// <summary>
        /// a contract is canceled
        /// </summary>
        ContractCanceled = 5,
        /// <summary>
        /// get one star review because of canceling an event that are about to start (within 1 day)
        /// </summary>
        GetOneStarReviewCancelContract = 6
    }
}

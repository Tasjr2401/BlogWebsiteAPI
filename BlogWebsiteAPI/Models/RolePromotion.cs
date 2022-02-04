using System;

namespace BlogWebsiteAPI.Models
{
    public class RolePromotion
    {
        public RolePromotion(int promoteeId, int promoterId, string previousRole, string newRole, DateTime dateOfChange)
        {
            PromoteeId = promoteeId;
            PromoterId = promoterId;
            PreviousRole = previousRole;
            NewRole = newRole;
            DateOfChange = dateOfChange;
        }
        public int PromoteeId { get; set; }
        public int PromoterId { get; set; }
        public string PreviousRole { get; set; }
        public string NewRole { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}

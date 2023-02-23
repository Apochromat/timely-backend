using System.ComponentModel;

namespace timely_backend.Models.DTO; 

public class GroupChangeDTO {
    [DisplayName("groupId")]
    public Guid GroupId { get; set; }
}
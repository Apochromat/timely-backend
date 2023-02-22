using System.ComponentModel;

namespace timely_backend.Models.DTO; 

public class EmailComfirmationToken {
    [DisplayName("token")]
    public string Token { set; get; }
}
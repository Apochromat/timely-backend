using System.ComponentModel;

namespace timely_backend.Models.DTO; 

public class Response {
    [DisplayName("status")]public string Status { get; set; }
    [DisplayName("message")]public string Message { get; set; }
}
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.OpenApi.Attributes;

namespace timely_backend.Models.Enum
{
    public enum TypeSchedulleEnum
    {
        [Display("Classroom")]
        Classroom,
        [Display("Teacher")]
        Teacher ,
        [Display("Group")]
        Group 
    }
}

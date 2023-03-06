using System.ComponentModel.DataAnnotations;
using timely_backend.Models.Enum;

namespace timely_backend.Models.DTO
{
    public class DuplicateDTO
    {
        [Required]
        public  DateTime DateToCopy { get; set; }
        public int AmountOfWeeks { get; set; }
        /// <summary>
        /// Classroom , Teacher , Group
        /// </summary>
        public TypeSchedulleEnum SchedulleType { get; set; }
        /// <summary>
        /// id group or teacher or classroom
        /// </summary>
        public Guid Id { get; set; }
    }
}

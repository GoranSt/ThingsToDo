using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.Models.Entity
{
    public class RemovedTasks
    {
        [Key]
        public int TaskId { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = " ")]
        public string Title { get; set; }

        [Required(ErrorMessage = " ")]
        public DateTime RemovedDate { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblPriority")]
        public int Priority { get; set; }
              
        [ForeignKey("CategoryId")]
        public virtual Categories Category { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;

namespace ThingsToDo.Models.Entity
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblTitle")]
        public string Title { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblDescription")]
        public string Description { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblPriority")]
        public int Priority { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblFromDate")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblUntilDate")]
        public DateTime ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblFinishedDate")]
        public DateTime? FinishedDate { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Categories Category { get; set; }
    }
}

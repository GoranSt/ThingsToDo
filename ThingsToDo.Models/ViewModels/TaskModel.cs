using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.Models.ViewModels
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblTitle")]
        public string Title { get; set; }

       
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblDescription")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblCategoryName")]
        public string CategoryName { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblPriority")]
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public bool isFinished { get; set; }
        public bool isExpired { get; set; }

        [Required(ErrorMessage = " ")]
        public int CategoryId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{dd MMMM, yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblFromDate")]
        public DateTime FromDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{dd MMMM, yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblUntilDate")]
        public DateTime ToDate { get; set; }

        public DateTime RemovedDate { get; set; }

        public string ToDataTableToDateFormat { get; set; }

        public DateTime? FinishedDate { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public int DT_RowId { get; set; }
        public TaskModel()
        {
            Categories = new List<CategoryModel>();
        }

    }

}

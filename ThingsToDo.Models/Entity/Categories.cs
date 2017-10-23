using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.Models.Entity
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(ResourceType = typeof(Resources.ThingsToDo), Name = "lblTitle")]
        public string Title { get; set; }
               
        public int UserId { get; set; }
    
    }
}

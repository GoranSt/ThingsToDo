using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.Models.Datatables
{
    public class DataTableResultModel<T> where T : class
    {
        public DataTableResultModel()
        {
            data = new List<T>();
        }

        public int sEcho { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}

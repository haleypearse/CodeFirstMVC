using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeFirstMVC.Models
{
    public class PagingParameterModel
    {
        public string Query { get; set; }

        public string SortBy { get; set; }

        public bool SortOrder { get; set; }

        const int maxPageSize = 100;

        public int pageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 10;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Common
{
    public class GetOptions
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int? RowsCount { get; set; }

        public IEnumerable<SortOption> Sort { get; set; }

        public IEnumerable<Filter> Filters { get; set; }
    }
}

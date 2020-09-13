using System.Collections.Generic;

namespace Movie.Demo.Test.Entity
{
    public class Search
    {
        public int page { get; set; }
        public List<SearchDetail> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
    
}

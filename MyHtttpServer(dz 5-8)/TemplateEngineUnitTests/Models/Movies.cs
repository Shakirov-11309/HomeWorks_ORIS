using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngineUnitTests.Models
{
    public class Movies
    {
        public int Id { get; set; }

        public string title { get; set; }

        public string description_card { get; set; }

        public string description { get; set; }

        public int release_year { get; set; }

        public double rating { get; set; }

        public double Imdb_rating { get; set; }

        public double amediateka_rating { get; set; }

        public string poster_url { get; set; }

        public string bg_url { get; set; }

        public string card_url { get; set; }
    }
}

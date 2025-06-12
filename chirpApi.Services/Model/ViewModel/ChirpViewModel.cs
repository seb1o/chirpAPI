using chirpAPI.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Model.ViewModel
{
    public class ChirpViewModel
    {

        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public string? ExtUrl { get; set; }


        public double? Lat { get; set; }

        public double? Lng { get; set; }

        public DateTime CreationTime { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaEveryday.Models
{
    public class Global
    {
        [DisplayName("Số ca mới hôm nay")]
        [DisplayFormat(DataFormatString = "{0:#,00}",ApplyFormatInEditMode = true)]
        public int NewConfirmed { get; set; }
        [DisplayName("Tổng số ca nhiễm")]
        [DisplayFormat(DataFormatString = "{0:#,00}", ApplyFormatInEditMode = true)]
        public int TotalConfirmed { get; set; }
        [DisplayName("Số ca chết hôm nay")]
        [DisplayFormat(DataFormatString = "{0:#,00}", ApplyFormatInEditMode = true)]
        public int NewDeaths { get; set; }
        [DisplayName("Tổng số ca chết")]
        [DisplayFormat(DataFormatString = "{0:#,00}", ApplyFormatInEditMode = true)]
        public int TotalDeaths { get; set; }
        [DisplayName("Số ca phục hồi hôm nay")]
        [DisplayFormat(DataFormatString = "{0:#,00}", ApplyFormatInEditMode = true)]
        public int NewRecovered { get; set; }
        [DisplayName("Tổng số ca phục hồi")]
        [DisplayFormat(DataFormatString = "{0:#,00}", ApplyFormatInEditMode = true)]
        public int TotalRecovered { get; set; }

        
    }
}

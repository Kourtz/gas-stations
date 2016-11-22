using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gas_Station.Models
{
    public class GasStation
    {
        public int Id { get; set; }
        public string County { get; set; }
        public string CountyID { get; set; }
        public string Municipality { get; set; }
        public string DepId { get; set; }
        public string Dep { get; set; }
        public string Address { get; set; }
        public string Brand { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public double Price { get; set; }

        public DateTime PriceUpdate { get; set; }
        public string FuelType { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public virtual ApplicationUser Manager { get; set; }

    }



}
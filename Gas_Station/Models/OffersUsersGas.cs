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
    public class OffersUsersGas
    {
        public int Id { get; set; }
        public GasStation GasStation { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orkidea.PollaExpress.WebFront.Models
{
    public class VmCustomer:Customer
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}
using PersonRegistryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonRegistryLibrary.Models
{
    public class AddressModel : IRegistryItem
    {
        public virtual int Id { get; set; }

        public virtual int PersonId { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        public virtual string Street { get; set; }

        [Required]
        public virtual string PostalCode { get; set; }

    }
}

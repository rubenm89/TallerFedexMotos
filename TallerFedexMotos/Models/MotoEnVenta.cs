using System;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class MotoEnVenta
    {
        public int Id { get; set; }

        [Display(Name = "Moto")]
        public int motoId { get; set; }

        public Moto moto { get; set; }

        [Display(Name = "Precio")]
        [Required]
        [Range(0,int.MaxValue)]
        public int precio { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class MotoEnReparacion
    {
        public int Id { get; set; }

        [Display(Name = "Moto")]
        public int motoId { get; set; }

        public Moto moto { get; set; }

        [Display(Name = "Trabajo realizado")]
        [Required]
        public string trabajoRealizado { get; set; }
    }
}
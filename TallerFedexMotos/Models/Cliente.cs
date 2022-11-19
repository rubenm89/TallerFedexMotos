using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required]
        public string apellido { get; set; }


        [Display(Name = "Moto")]
        public int motoId { get; set; }

        public Moto moto { get; set; }

    }
}

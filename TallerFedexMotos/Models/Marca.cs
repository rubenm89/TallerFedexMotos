using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class Marca
    {
        public int Id { get; set; }

        [Display(Name = "Marca")]
        [Required]
        public string nombreMarca { get; set; }
        public List<Modelo> ListaModelos { get; set; }
        public List<Moto> ListaMotos { get; set; }

    }
}
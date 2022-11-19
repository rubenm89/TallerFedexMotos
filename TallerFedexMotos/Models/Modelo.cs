using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class Modelo
    {
        public int Id { get; set; }

        [Display(Name = "Modelo")]
        [Required]
        public string nombreModelo { get; set; }

        [Display(Name = "Marca")]
        public int marcaId { get; set; }
        public Marca marca { get; set; }

        public List<Moto> ListaMotos { get; set; }

    }
}
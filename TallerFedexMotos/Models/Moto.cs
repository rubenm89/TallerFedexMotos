using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallerFedexMotos.Models
{
    public class Moto 
    {
        public int Id { get; set; }

        [Display(Name = "Modelo")]
        public int modeloId { get; set; }

        public Modelo modelo { get; set; }

        [Display(Name = "Año")]
        [Range(1900,2022)]
        public int año { get; set; }

        [Display(Name = "Foto moto")]
        public string imagenMoto { get; set; }
        public List<Cliente> ListaClientes { get; set; }
        public List<MotoEnVenta> ListaMotosEnVenta { get; set; }
        public List<MotoEnReparacion> ListaMotosEnReparacion { get; set; }

    }
}
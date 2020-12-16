using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppDistribuidorLucrosEntidades
{
    public class Funcionario
    {
        [Required]
        public string matricula { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]
        public string area { get; set; }
        public string cargo { get; set; }
        [Required]
        public string salario_bruto { get; set; }
        [Required]
        public DateTime data_de_admissao { get; set; }

        public decimal bonus_calculado { get; set; }
    }
}

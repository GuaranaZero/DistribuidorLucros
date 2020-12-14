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
        private const decimal salarioMinimo = 1045.00m;

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

        public decimal bonus_calculado { get { return CalculaBonusFuncionario(); } }

        private int peso_area_atuacao { get { return ObtemPesoAreaAtuacao(); } }

        private int peso_data_admissao { get { return CalculaPesoDataAdmissao(); } }

        private int peso_faixa_salarial { get { return CalculaPesoFaixaSalarial(); } }

        private decimal CalculaBonusFuncionario()
        {
            Decimal salarioBruno = decimal.Parse(salario_bruto, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            decimal bonus = (((salarioBruno * peso_data_admissao) + (salarioBruno * peso_area_atuacao)) / peso_faixa_salarial) * 12;
            return bonus;
        }

        private int ObtemPesoAreaAtuacao()
        {
            AreaList areaList = new AreaList();

            return areaList.areas.Where(c => c.nome.ToUpper() == area.ToUpper())
                                 .DefaultIfEmpty(new Area() { peso = 1 })
                                 .FirstOrDefault()
                                 .peso;
        }

        private int CalculaPesoDataAdmissao()
        {
            int anoDiff = (data_de_admissao.Year - DateTime.Now.Year - 1) +
                          (((data_de_admissao.Month > DateTime.Now.Month) ||
                          ((data_de_admissao.Month == DateTime.Now.Month) && (data_de_admissao.Day >= DateTime.Now.Day))) ? 1 : 0);

            if (anoDiff <= 1)
                return 1;
            else if (anoDiff > 1 && anoDiff <= 3)
                return 2;
            else if (anoDiff > 3 && anoDiff <= 8)
                return 3;
            else //> 8
                return 5;
        }

        private int CalculaPesoFaixaSalarial()
        {
            Decimal salarioBruno = decimal.Parse(salario_bruto, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            int quantidadeSalarioMinimos = (int)decimal.Round(salarioBruno / salarioMinimo, 0, MidpointRounding.ToZero);

            if (quantidadeSalarioMinimos <= 3 || cargo.ToUpper().Trim() == "ESTÁGIARIO")
                return 1;
            else if (quantidadeSalarioMinimos > 3 && quantidadeSalarioMinimos <= 5)
                return 2;
            else if (quantidadeSalarioMinimos > 5 && quantidadeSalarioMinimos <= 8)
                return 3;
            else // >8
                return 5;
        }
    }
}

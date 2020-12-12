using System.Collections.Generic;

namespace AppDistribuidorLucrosEntidades
{
    public class PagamentosConsolidados
    {
        public List<PagamentosConsolidados> participacoes { get; set; }
        public int total_de_funcionarios { get; set; }
        public decimal total_distribuido { get; set; }
        public decimal total_disponibilizado { get; set; }
        public decimal saldo_total_disponibilizado { get; set; }
    }
}
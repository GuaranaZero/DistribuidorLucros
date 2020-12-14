using System.Collections.Generic;

namespace AppDistribuidorLucrosEntidades
{
    public class PagamentosConsolidados
    {
        public List<Participacao> participacoes { get; set; }
        public int total_de_funcionarios { get; set; }
        public string total_distribuido { get; set; }
        public string total_disponibilizado { get; set; }
        public string saldo_total_disponibilizado { get; set; } 
    }
}
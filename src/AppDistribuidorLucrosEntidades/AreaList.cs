using System;
using System.Collections.Generic;

namespace AppDistribuidorLucrosEntidades
{
    public class AreaList
    {
        public List<Area> areas { get; set; }

        public AreaList()
        {
            InicializaAreaList();
        }

        private void InicializaAreaList()
        {
            areas = new List<Area>();

            areas.Add(new Area() { nome = "Diretoria", peso = 1 });
            areas.Add(new Area() { nome = "Contabilidade", peso = 2 });
            areas.Add(new Area() { nome = "Financeiro", peso = 2 });
            areas.Add(new Area() { nome = "Tecnologia", peso = 2 });
            areas.Add(new Area() { nome = "Serviços Gerais", peso = 3 });
            areas.Add(new Area() { nome = "Relacionamento com o Cliente", peso = 5 });
        }
    }
}
using AppDistribuidorLucrosEntidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDistribuidorLucrosService.Interfaces
{
    public interface IDistribuidorLucrosService
    {
        IEnumerable<Funcionario> GetAllItems();
        void Add(Funcionario novoFuncionario);
        Funcionario GetById(string chave);
        void Remove(Funcionario funcionario);
        PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado);
    }
}

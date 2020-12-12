using AppDistribuidorLucrosEntidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDistribuidorLucrosService.Interfaces
{
    public interface IDistribuidorLucrosService
    {
        IEnumerable<Funcionario> GetAllItems();
        Funcionario Add(Funcionario novoFuncionario);
        Funcionario GetById(string chave);
        void Remove(string chave);
    }
}

using AppDistribuidorLucrosEntidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDistribuidorLucrosService.Interfaces
{
    public interface IFuncionariosRepository
    {
        IEnumerable<Funcionario> GetAllItems();
        void Add(Funcionario novoFuncionario);
        Funcionario GetById(string chave);
        void Remove(Funcionario funcionario);
    }
}

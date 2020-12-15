using AppDistribuidorLucrosEntidades;

namespace AppDistribuidorLucrosService.Interfaces
{
    public interface IDistribuidorLucrosService
    {
        void AdicionaFuncionario(Funcionario funcionario);
        PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado);
        void RemoveFuncionario(Funcionario funcionario);
    }
}
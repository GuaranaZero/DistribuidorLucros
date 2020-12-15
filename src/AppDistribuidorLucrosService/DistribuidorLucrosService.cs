using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AppDistribuidorLucrosService
{
    public class DistribuidorLucrosService : IDistribuidorLucrosService
    {
        public DistribuidorLucrosService(IFuncionariosRepository funcionariosRepository)
        {
            _funcionariosRepository = funcionariosRepository;
        }

        private readonly IFuncionariosRepository _funcionariosRepository;

        public PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado)
        {
            List<Funcionario> funcionarios = (List<Funcionario>)_funcionariosRepository.GetAllItems();
            List<Participacao> participacoes = new List<Participacao>();

            decimal totalDistribuido = 0.00m;

            foreach (var funcionario in funcionarios)
            {
                participacoes.Add(new Participacao() { matricula = funcionario.matricula, nome = funcionario.nome, valor_da_participacao = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", funcionario.bonus_calculado.ToString()) });
                totalDistribuido += funcionario.bonus_calculado;
            }

            //Caso nao seja possivel distribuir todo o montande disponivel, atribui zero
            if (totalDistribuido > totalDisponibilizado)
                totalDistribuido = 0.00m;

            PagamentosConsolidados pagamentosConsolidados = new PagamentosConsolidados()
            {
                participacoes = participacoes,
                total_de_funcionarios = funcionarios.Count,
                total_distribuido = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", totalDistribuido.ToString()),
                total_disponibilizado = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", totalDisponibilizado.ToString()),
                saldo_total_disponibilizado = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", (totalDisponibilizado - totalDistribuido).ToString())
            };

            return pagamentosConsolidados;
        }

        public void RemoveFuncionario(Funcionario funcionario)
        {
            _funcionariosRepository.Remove(funcionario);
        }

        public void AdicionaFuncionario(Funcionario funcionario)
        {
            _funcionariosRepository.Add(funcionario);
        }
    }
}

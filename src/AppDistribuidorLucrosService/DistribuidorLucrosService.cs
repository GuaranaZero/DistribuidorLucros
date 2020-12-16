using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AppDistribuidorLucrosService
{
    public class DistribuidorLucrosService : IDistribuidorLucrosService
    {
        public DistribuidorLucrosService(IFuncionariosRepository funcionariosRepository)
        {
            _funcionariosRepository = funcionariosRepository;
        }

        private readonly IFuncionariosRepository _funcionariosRepository;
        private const decimal _salarioMinimo = 1045.00m;

        public PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado)
        {
            List<Funcionario> funcionarios = (List<Funcionario>)_funcionariosRepository.GetAllItems();
            List<Participacao> participacoes = new List<Participacao>();

            decimal totalDistribuido = 0.00m;

            foreach (var funcionario in funcionarios)
            {
                int pesoArea = ObtemPesoAreaAtuacao(funcionario.area);
                int pesoDataAdmissao = CalculaPesoDataAdmissao(funcionario.data_de_admissao);
                int pesoFaixaSalarial = CalculaPesoFaixaSalarial(funcionario.salario_bruto, funcionario.cargo);

                funcionario.bonus_calculado = CalculaBonusFuncionario(funcionario.salario_bruto, pesoDataAdmissao, pesoArea, pesoFaixaSalarial);

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

        private decimal CalculaBonusFuncionario(string salarioBruto, int pesoDataAdmissao, int pesoAreaAtuacao, int pesoFaixaSalarial)
        {
            Decimal salarioBruno = decimal.Parse(salarioBruto, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            decimal bonus = (((salarioBruno * pesoDataAdmissao) + (salarioBruno * pesoAreaAtuacao)) / pesoFaixaSalarial) * 12;
            return bonus;
        }

        private int ObtemPesoAreaAtuacao(string area)
        {
            AreaList areaList = new AreaList();

            return areaList.areas.Where(c => c.nome.ToUpper() == area.ToUpper())
                                 .DefaultIfEmpty(new Area() { peso = 1 })
                                 .FirstOrDefault()
                                 .peso;
        }

        private int CalculaPesoDataAdmissao(DateTime dataAdmissao)
        {
            int anoDiff = (dataAdmissao.Year - DateTime.Now.Year - 1) +
                          (((dataAdmissao.Month > DateTime.Now.Month) ||
                          ((dataAdmissao.Month == DateTime.Now.Month) && (dataAdmissao.Day >= DateTime.Now.Day))) ? 1 : 0);

            if (anoDiff <= 1)
                return 1;
            else if (anoDiff > 1 && anoDiff <= 3)
                return 2;
            else if (anoDiff > 3 && anoDiff <= 8)
                return 3;
            else //> 8
                return 5;
        }

        private int CalculaPesoFaixaSalarial(string salarioBruto, string cargo)
        {
            Decimal salarioBruno = decimal.Parse(salarioBruto, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            int quantidadeSalarioMinimos = (int)decimal.Round(salarioBruno / _salarioMinimo, 0, MidpointRounding.ToZero);

            if (quantidadeSalarioMinimos <= 3 || cargo.ToUpper().Trim() == "ESTÁGIARIO")
                return 1;
            else if (quantidadeSalarioMinimos > 3 && quantidadeSalarioMinimos <= 5)
                return 2;
            else if (quantidadeSalarioMinimos > 5 && quantidadeSalarioMinimos <= 8)
                return 3;
            else // >8
                return 5;
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

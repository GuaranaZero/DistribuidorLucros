using AppDistribuidorLucrosCore.Data;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AppDistribuidorLucrosAPITeste
{
    internal class DistribuidorLucrosServiceFake : IDistribuidorLucrosService
    {
        private readonly List<Funcionario> _funcionarios;
        IFuncionariosRepository _repository;
        private const decimal _salarioMinimo = 1045.00m;

        public DistribuidorLucrosServiceFake(IFuncionariosRepository funcionariosRepository)
        {
            var mock = new Mock<IFuncionariosRepository>();
            _repository = mock.Object;

            _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0009968", nome = "Victor Wilson", area = "Diretoria", cargo = "Diretor Financeiro", salario_bruto = "R$ 12.696,20", data_de_admissao = Convert.ToDateTime("2012-01-05") },
                new Funcionario() { matricula = "0004468", nome = "Flossie Wilson", area = "Contabilidade", cargo = "Auxiliar de Contabilidade", salario_bruto = "R$ 1.396,52", data_de_admissao = Convert.ToDateTime("2015-01-05") },
                new Funcionario() { matricula = "0008174", nome = "Sherman Hodges", area = "Relacionamento com o Cliente", cargo = "Líder de Relacionamento", salario_bruto = "R$ 3.899,74", data_de_admissao = Convert.ToDateTime("2015-06-07") },
                new Funcionario() { matricula = "0007463", nome = "Charlotte Romero", area = "Financeiro", cargo = "Contador Pleno", salario_bruto = "R$ 3.199,82", data_de_admissao = Convert.ToDateTime("2018-01-03") }
            };
        }

        public void AdicionaFuncionario(Funcionario novoFuncionario)
        {
            _funcionarios.Add(novoFuncionario);
        }

        public void RemoveFuncionario(Funcionario funcionario)
        {
            var item = _funcionarios.First(a => a.matricula == funcionario.matricula);
            _funcionarios.Remove(item);
        }

        public PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado)
        {
            List<Participacao> participacoes = new List<Participacao>();
            decimal totalDistribuido = 0.0m;

            foreach (var funcionario in _funcionarios)
            {
                int pesoArea = ObtemPesoAreaAtuacao(funcionario.area);
                int pesoDataAdmissao = CalculaPesoDataAdmissao(funcionario.data_de_admissao);
                int pesoFaixaSalarial = CalculaPesoFaixaSalarial(funcionario.salario_bruto, funcionario.cargo);

                funcionario.bonus_calculado = CalculaBonusFuncionario(funcionario.salario_bruto, pesoDataAdmissao, pesoArea, pesoFaixaSalarial);

                participacoes.Add(new Participacao() { matricula = funcionario.matricula, nome = funcionario.nome, valor_da_participacao = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", funcionario.bonus_calculado.ToString()) });
                totalDistribuido += funcionario.bonus_calculado;
            }

            PagamentosConsolidados pagamentosConsolidados = new PagamentosConsolidados()
            {
                participacoes = participacoes,
                total_de_funcionarios = _funcionarios.Count,
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
    }
}
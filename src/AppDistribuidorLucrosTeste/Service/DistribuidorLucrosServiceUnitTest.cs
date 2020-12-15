using AppDistribuidorLucrosAPI.Controllers;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService;
using AppDistribuidorLucrosService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace DistribuidorLucrosServiceTeste
{
    public class DistribuidorLucrosServiceUnitTest
    {
        private const decimal valorDisponibilizado = 5222330.19M;

        #region Fields

        DistribuidorLucrosService _service;
        private readonly List<Funcionario> _funcionarios;
        private readonly List<Participacao> _participacoes;
        private readonly PagamentosConsolidados _pagamentosConsolidados;
        private readonly Mock<IFuncionariosRepository> _mockRepositorio;

        #endregion

        public DistribuidorLucrosServiceUnitTest()
        {
            _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0009968", nome = "Victor Wilson", area = "Diretoria", cargo = "Diretor Financeiro", salario_bruto = "R$ 12.696,20", data_de_admissao = Convert.ToDateTime("2012-01-05") },
                new Funcionario() { matricula = "0004468", nome = "Flossie Wilson", area = "Contabilidade", cargo = "Auxiliar de Contabilidade", salario_bruto = "R$ 1.396,52", data_de_admissao = Convert.ToDateTime("2015-01-05") },
                new Funcionario() { matricula = "0008174", nome = "Sherman Hodges", area = "Relacionamento com o Cliente", cargo = "Líder de Relacionamento", salario_bruto = "R$ 3.899,74", data_de_admissao = Convert.ToDateTime("2015-06-07") },
                new Funcionario() { matricula = "0007463", nome = "Charlotte Romero", area = "Financeiro", cargo = "Contador Pleno", salario_bruto = "R$ 3.199,82", data_de_admissao = Convert.ToDateTime("2018-01-03") }
            };

            _participacoes = new List<Participacao>()
            {
                new Participacao() { matricula = "0009968", nome = "Victor Wilson", valor_da_participacao = "R$ 60941,76" },
                new Participacao() { matricula = "0004468", nome = "Flossie Wilson", valor_da_participacao = "R$ 50274,72" },
                new Participacao() { matricula = "0008174", nome = "Sherman Hodges", valor_da_participacao = "R$ 280781,28" },
                new Participacao() { matricula = "0007463", nome = "Charlotte Romero", valor_da_participacao = "R$ 115193,52" }
            };

            _pagamentosConsolidados = new PagamentosConsolidados()
            {
                participacoes = _participacoes,
                total_de_funcionarios = 4,
                total_distribuido = "R$ 507191,28",
                total_disponibilizado = "R$ 5222330,19",
                saldo_total_disponibilizado = "R$ 4715138,91"
            };

            _mockRepositorio = new Mock<IFuncionariosRepository>();
            _mockRepositorio
                .Setup(a => a.GetAllItems())
                .Returns(_funcionarios);

            IFuncionariosRepository _repository = _mockRepositorio.Object;

            _service = new DistribuidorLucrosService(_repository);
        }

        [Fact]
        public void AdicionaFuncionarioOkResult()
        {
            var funcionario = new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") };
            // Act
            _service.AdicionaFuncionario(funcionario);

            // Assert
            _mockRepositorio.Verify(x => x.Add(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact]
        public void RemoveFuncionarioOkResult()
        {
            var funcionario = new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") };
            // Act
            _service.RemoveFuncionario(funcionario);

            // Assert
            _mockRepositorio.Verify(x => x.Remove(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact]
        public void DistribuiParticipacaoOkResult()
        {
            decimal valorDisponibilizado = DistribuidorLucrosServiceUnitTest.valorDisponibilizado;
            var retorno = _service.CalculaPagamentos(valorDisponibilizado);

            //Converte objeto para string para comparacao no Assert
            string jsonRetornoEsperado = JsonConvert.SerializeObject(_pagamentosConsolidados);
            string jsonRetornoAtual = JsonConvert.SerializeObject(retorno);

            Assert.Equal(jsonRetornoEsperado, jsonRetornoAtual);
        }

        [Fact]
        public void DistribuiParticipacaoMontanteNaoSuficienteOkResult()
        {
            decimal valorDisponibilizado = 5000.00M;
            var retorno = _service.CalculaPagamentos(valorDisponibilizado);

            var pagamentosConsolidados = new PagamentosConsolidados()
            {
                participacoes = _participacoes,
                total_de_funcionarios = 4,
                total_distribuido = "R$ 0,00",
                total_disponibilizado = "R$ 5000,00",
                saldo_total_disponibilizado = "R$ 5000,00"
            };

            //Converte objeto para string para comparacao no Assert
            string jsonRetornoEsperado = JsonConvert.SerializeObject(pagamentosConsolidados);
            string jsonRetornoAtual = JsonConvert.SerializeObject(retorno);

            Assert.Equal(jsonRetornoEsperado, jsonRetornoAtual);
        }
    }
}

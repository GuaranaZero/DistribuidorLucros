using AppDistribuidorLucrosAPI.Controllers;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AppDistribuidorLucrosAPITeste
{
    public class DistribuidorLucrosAPIUnitTest
    {
        IDistribuidorLucrosService _service;
        DistribuidorLucrosController _controller;
        ILogger<DistribuidorLucrosController> _logger;

        public DistribuidorLucrosAPIUnitTest()
        {
            var mock = new Mock<ILogger<DistribuidorLucrosController>>();
            ILogger<DistribuidorLucrosController> logger = mock.Object;

            _service = new DistribuidorLucrosServiceFake();
            _controller = new DistribuidorLucrosController(logger, _service);
        }

        [Fact]
        public void CadastraFuncionariosOkResult()
        {
            var _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") },
                new Funcionario() { matricula = "0008601", nome = "Taylor Mccarthy", area = "Relacionamento com o Cliente", cargo = "Auxiliar de Ouvidoria", salario_bruto = "R$ 1.800,16", data_de_admissao = Convert.ToDateTime("2017-03-31") },
            };

            // Act
            var okResult = _controller.CadastraFuncionarios(_funcionarios);
            // Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public void DescadastraFuncionariosOkResult()
        {
            var _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") },
                new Funcionario() { matricula = "0008601", nome = "Taylor Mccarthy", area = "Relacionamento com o Cliente", cargo = "Auxiliar de Ouvidoria", salario_bruto = "R$ 1.800,16", data_de_admissao = Convert.ToDateTime("2017-03-31") },
            };

            _controller.CadastraFuncionarios(_funcionarios);

            // Act
            var okResult = _controller.DescadastraFuncionarios(_funcionarios);
            // Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public void CadastraFuncionariosBadRequest()
        {
            var _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0006877", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") }
            };

            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.CadastraFuncionarios(_funcionarios);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void DistribuiParticipacaoOkResult()
        {
            // Act
            var okResult = _controller.DistribuiParticipacao("R$ 5.812.891,20");
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
    }
}

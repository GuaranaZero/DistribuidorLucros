using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using System.Globalization;
using AppDistribuidorLucrosService;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace AppDistribuidorLucrosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistribuidorLucrosController : ControllerBase
    {
        private readonly ILogger<DistribuidorLucrosController> _logger;
        private readonly IDistribuidorLucrosService _distribuidorLucrosService;

        public DistribuidorLucrosController(ILogger<DistribuidorLucrosController> logger, IDistribuidorLucrosService distribuidorLucrosService)
        {
            _logger = logger;
            _distribuidorLucrosService = distribuidorLucrosService;
        }

        [HttpPost]
        [Route("api/CadastraFuncionarios")]
        public ActionResult CadastraFuncionarios([BindRequired, FromBody] List<Funcionario> funcionarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var funcionario in funcionarios)
            {
                _distribuidorLucrosService.AdicionaFuncionario(funcionario);
                _logger.LogInformation($"Adicionado ao cache : {funcionario.matricula + " " + funcionario.nome}");
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/DescadastraFuncionarios")]
        public ActionResult DescadastraFuncionarios([BindRequired, FromBody] List<Funcionario> funcionarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var funcionario in funcionarios)
            {
                _distribuidorLucrosService.RemoveFuncionario(funcionario);
                _logger.LogInformation($"Apagado do cache : {funcionario.matricula + " " + funcionario.nome}");
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/DistribuiParticipacao")]
        public ActionResult<PagamentosConsolidados> DistribuiParticipacao([BindRequired, FromQuery]string totalDisponibilizado)
        {
            decimal valor;

            if (!decimal.TryParse(totalDisponibilizado, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, null, out valor))
            {
                return BadRequest();
            }

            var pagamentosCalculados = _distribuidorLucrosService.CalculaPagamentos(valor);

            return Ok(pagamentosCalculados);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using System.Text.RegularExpressions;
using System.Globalization;

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
        public ActionResult CadastraFuncionarios([FromBody] List<Funcionario> funcionarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var funcionario in funcionarios)
            {
                _distribuidorLucrosService.Add(funcionario);
                _logger.LogInformation($"Adicionado ao cache : {funcionario.matricula + " " + funcionario.nome}");
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/DescadastraFuncionarios")]
        public ActionResult DescadastraFuncionarios([FromBody] List<Funcionario> funcionarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var funcionario in funcionarios)
            {
                _distribuidorLucrosService.Remove(funcionario);
                _logger.LogInformation($"Apagado do cache : {funcionario.matricula + " " + funcionario.nome}");
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/DistribuiParticipacao")]
        public ActionResult<PagamentosConsolidados> DistribuiParticipacao(string totalDisponibilizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal valor = decimal.Parse(totalDisponibilizado, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            var pagamentosCalculados = _distribuidorLucrosService.CalculaPagamentos(valor);

            return Ok(pagamentosCalculados);
        }
    }
}

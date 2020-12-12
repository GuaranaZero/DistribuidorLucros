using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService;
using AppDistribuidorLucrosService.Interfaces;

namespace AppDistribuidorLucrosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistribuidorLucrosController : ControllerBase
    {
        private readonly ILogger<DistribuidorLucrosController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDistribuidorLucrosService _distribuidorLucrosService;

        public DistribuidorLucrosController(ILogger<DistribuidorLucrosController> logger, IConfiguration configuration, IDistribuidorLucrosService distribuidorLucrosService)
        {
            _logger = logger;
            _configuration = configuration;
            _distribuidorLucrosService = distribuidorLucrosService;
        }

        [HttpPost]
        [Route("api/CadastraFuncionarios")]
        public void CadastraFuncionarios([FromBody] List<Funcionario> funcionarios)
        {
            //#if DEBUG
            //    _redisHelper.ClearCache();
            //#endif

            //var cache = _redisHelper.Connection.GetDatabase();

            //foreach (var funcionario in funcionarios)
            //{
            //    if (string.IsNullOrEmpty(cache.StringGet("Funcionarios" + funcionario.matricula)))
            //    {
            //        cache.StringSet("Funcionarios" + funcionario.matricula, JsonConvert.SerializeObject(funcionario));
            //        _logger.LogInformation($"Added to cache : {funcionario.matricula + " " + funcionario.nome}");
            //    }
            //}
        }
    }
}

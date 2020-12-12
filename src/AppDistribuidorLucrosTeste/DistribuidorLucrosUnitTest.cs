using AppDistribuidorLucrosAPI.Controllers;
using AppDistribuidorLucrosService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace AppDistribuidorLucrosTeste
{
    public class DistribuidorLucrosUnitTest
    {
        DistribuidorLucrosController _distribuidorLucrosController;

        public DistribuidorLucrosUnitTest(ILogger<DistribuidorLucrosController> logger, IConfiguration configuration, IDistribuidorLucrosService distribuidorLucrosService)
        {
            _distribuidorLucrosController = new DistribuidorLucrosController(logger, configuration, distribuidorLucrosService);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}

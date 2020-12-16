
using AppDistribuidorLucrosCore.Data;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosRepositorio;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using Xunit;

namespace DistribuidorLucrosRepositorioTeste
{
    public class DistribuidorLucrosRepositorioUnitTest
    {
        #region Fields

        private readonly FuncionariosRepository _repositorio;
        private readonly List<Funcionario> _funcionarios;
        private readonly List<string> _stringFuncionarios;

        private readonly Mock<IDatabase> _mockCache;
        private readonly Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
        private readonly Mock<IRedisConexao> _mockConexao;

        #endregion

        public DistribuidorLucrosRepositorioUnitTest()
        {
            _funcionarios = new List<Funcionario>()
            {
                new Funcionario() { matricula = "0009968", nome = "Victor Wilson", area = "Diretoria", cargo = "Diretor Financeiro", salario_bruto = "R$ 12.696,20", data_de_admissao = Convert.ToDateTime("2012-01-05") },
                new Funcionario() { matricula = "0004468", nome = "Flossie Wilson", area = "Contabilidade", cargo = "Auxiliar de Contabilidade", salario_bruto = "R$ 1.396,52", data_de_admissao = Convert.ToDateTime("2015-01-05") },
                new Funcionario() { matricula = "0008174", nome = "Sherman Hodges", area = "Relacionamento com o Cliente", cargo = "Líder de Relacionamento", salario_bruto = "R$ 3.899,74", data_de_admissao = Convert.ToDateTime("2015-06-07") },
                new Funcionario() { matricula = "0007463", nome = "Charlotte Romero", area = "Financeiro", cargo = "Contador Pleno", salario_bruto = "R$ 3.199,82", data_de_admissao = Convert.ToDateTime("2018-01-03") }
            };

            _stringFuncionarios = new List<string>()
            {
                JsonConvert.SerializeObject(_funcionarios[0]),
                JsonConvert.SerializeObject(_funcionarios[1]),
                JsonConvert.SerializeObject(_funcionarios[2]),
                JsonConvert.SerializeObject(_funcionarios[3])
            };

            _mockCache = new Mock<IDatabase>();
            _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();

            _mockConexao = new Mock<IRedisConexao>();
            _mockConexao
                .Setup(a => a.Connect())
                .Returns(_mockConnectionMultiplexer.Object);

            _mockConexao
                .Setup(a => a.Connection.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockCache.Object);

            _repositorio = new FuncionariosRepository(_mockConexao.Object);
        }

        [Fact]
        public void AdicionaNovoFuncionarioOkResult()
        {
            var funcionario = new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") };

            //Mock do retorno do Redis indicando que a chave nao existe no repositorio
            _mockCache
                .Setup(a => a.KeyExists(It.IsAny<RedisKey>(), default))
                .Returns(false);
            _mockCache
                .Setup(a => a.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), default, default, default))
                .Returns(true);

            _repositorio.Add(funcionario);

            // Assert
            _mockCache.Verify(x => x.KeyExists(It.IsAny<RedisKey>(), default), Times.Once);

            // Assert
            _mockCache.Verify(x => x.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), default, default, default), Times.Once);
        }

        [Fact]
        public void AdicionaFuncionarioExistenteOkResult()
        {
            var funcionario = new Funcionario() { matricula = "0006877", nome = "Cross Perkins", area = "Relacionamento com o Cliente", cargo = "Líder de Ouvidoria", salario_bruto = "R$ 3.371,47", data_de_admissao = Convert.ToDateTime("2016-12-06") };

            //Mock do retorno do Redis indicando que a chave existe no repositorio
            _mockCache
                .Setup(a => a.KeyExists(It.IsAny<RedisKey>(), default))
                .Returns(true);
            _mockCache
                .Setup(a => a.KeyDelete(It.IsAny<RedisKey>(), default))
                .Returns(true);
            _mockCache
                .Setup(a => a.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), default, default, default))
                .Returns(true);

            _repositorio.Add(funcionario);

            // Assert
            _mockCache.Verify(x => x.KeyExists(It.IsAny<RedisKey>(), default), Times.Once);

            // Assert
            _mockCache.Verify(x => x.KeyDelete(It.IsAny<RedisKey>(), default), Times.Once);

            // Assert
            _mockCache.Verify(x => x.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), default, default, default), Times.Once);
        }

        [Fact]
        public void ObtemTodosOsFuncionariosOkResult()
        {
            List<RedisKey> keys = new List<RedisKey>()
            {
                new RedisKey("Funcionario.0009968"),
                new RedisKey("Funcionario.0004468"),
                new RedisKey("Funcionario.0008174"),
                new RedisKey("Funcionario.0007463")
            };

            //Mock do retorno do Redis com as chaves encontradas a partir do filtro
            _mockConexao
                .Setup(a => a.Server.Keys(-1, "Funcionario.*", It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
                .Returns(keys);
            _mockCache
                .Setup(a => a.KeyExists(It.IsAny<RedisKey>(), default))
                .Returns(true);

            _mockCache
                .Setup(a => a.StringGet(keys[0], default))
                .Returns(_stringFuncionarios[0]);
            _mockCache
                .Setup(a => a.StringGet(keys[1], default))
                .Returns(_stringFuncionarios[1]);
            _mockCache
                .Setup(a => a.StringGet(keys[2], default))
                .Returns(_stringFuncionarios[2]);
            _mockCache
                .Setup(a => a.StringGet(keys[3], default))
                .Returns(_stringFuncionarios[3]);

            List<Funcionario> funcionarios = (List<Funcionario>)_repositorio.GetAllItems();

            // Assert
            _mockConexao.Verify(x => x.Server.Keys(-1, "Funcionario.*", It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()), Times.Once);

            // Assert
            _mockCache.Verify(x => x.KeyExists(It.IsAny<RedisKey>(), default), Times.Exactly(4));

            // Assert
            _mockCache.Verify(x => x.StringGet(It.IsAny<RedisKey>(), default), Times.Exactly(4));

            // Assert
            Assert.Equal(4, funcionarios.Count);
        }

        [Fact]
        public void ObtemFuncionarioEspecificoOkResult()
        {
            string key = "Funcionario.0009968";

            _mockCache
                .Setup(a => a.KeyExists(It.IsAny<RedisKey>(), default))
                .Returns(true);

            _mockCache
                .Setup(a => a.StringGet(key, default))
                .Returns(_stringFuncionarios[0]);

            Funcionario funcionario = _repositorio.GetById(key);
            
            // Assert
            _mockCache.Verify(x => x.KeyExists(It.IsAny<RedisKey>(), default), Times.Once);

            // Assert
            _mockCache.Verify(x => x.StringGet(It.IsAny<RedisKey>(), default), Times.Once);

            //Converte objeto para string para comparacao no Assert
            string jsonRetornoEsperado = _stringFuncionarios[0];
            string jsonRetornoAtual = JsonConvert.SerializeObject(funcionario);

            // Assert
            Assert.Equal(jsonRetornoEsperado, jsonRetornoAtual);
        }

        [Fact]
        public void ObtemFuncionarioEspecificoNaoOkResult()
        {
            string key = "Funcionario.0009968";

            _mockCache
                .Setup(a => a.KeyExists(It.IsAny<RedisKey>(), default))
                .Returns(false);

            Funcionario funcionario = _repositorio.GetById(key);

            // Assert
            _mockCache.Verify(x => x.KeyExists(It.IsAny<RedisKey>(), default), Times.Once);

            // Assert
            Assert.Null(funcionario);
        }
    }
}

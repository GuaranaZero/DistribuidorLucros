using AppDistribuidorLucrosCore.Data;
using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosService.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace AppDistribuidorLucrosRepositorio
{
    public class FuncionariosRepository : IFuncionariosRepository
    {
        public FuncionariosRepository(IRedisConexao conexao)
        {
            _conexao = conexao;
            _cache = _conexao.Connection.GetDatabase();
        }

        private readonly IRedisConexao _conexao;
        private readonly IDatabase _cache;

        public void Add(Funcionario novoFuncionario)
        {
            string chave = $"Funcionario.{novoFuncionario.matricula}";

            if (_cache.KeyExists(chave))
                Remove(novoFuncionario);

            _cache.StringSet(chave, JsonConvert.SerializeObject(novoFuncionario));

        }

        public IEnumerable<Funcionario> GetAllItems()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            foreach (var chave in _conexao.Server.Keys(-1, "Funcionario.*"))
            {
                Funcionario funcionario = GetById(chave);
                funcionarios.Add(funcionario);
            }

            return funcionarios;
        }

        public Funcionario GetById(string chave)
        {
            string registro = String.Empty;

            if (_cache.KeyExists(chave))
            {
                registro = _cache.StringGet(chave);
                Funcionario funcionario = JsonConvert.DeserializeObject<Funcionario>(registro);

                return funcionario;
            }
            else
            {
                return null;
            }
        }

        public void Remove(Funcionario funcionario)
        {
            _cache.KeyDelete($"Funcionario.{funcionario.matricula}");
        }
    }
}

using AppDistribuidorLucrosEntidades;
using AppDistribuidorLucrosRepositorio;
using AppDistribuidorLucrosService.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AppDistribuidorLucrosService
{
    public class DistribuidorLucrosService : IDistribuidorLucrosService
    {
        public DistribuidorLucrosService(RedisConexao redisConexao)
        {
            _connection = redisConexao;
            _cache = _connection.Connection.GetDatabase();
        }

        private readonly RedisConexao _connection;
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

            foreach (var chave in _connection.Server.Keys(-1, "Funcionario.*"))
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

        public PagamentosConsolidados CalculaPagamentos(decimal totalDisponibilizado)
        {
            List<Funcionario> funcionarios = (List<Funcionario>)GetAllItems();
            List<Participacao> participacoes = new List<Participacao>();

            decimal totalDistribuido = 0.0m;

            foreach (var funcionario in funcionarios)
            {
                participacoes.Add(new Participacao() { matricula = funcionario.matricula, nome = funcionario.nome, valor_da_participação = string.Format(CultureInfo.CurrentCulture, "R$ {0:#.###,##}", funcionario.bonus_calculado.ToString()) });
                totalDistribuido += funcionario.bonus_calculado;
            }

            //Caso nao seja possivel distribuir todo o montande disponivel, atribui zero
            if (totalDistribuido > totalDisponibilizado)
                totalDistribuido = 0.0m;

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
    }
}

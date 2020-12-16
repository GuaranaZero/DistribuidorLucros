using StackExchange.Redis;
using System;

namespace AppDistribuidorLucrosCore.Data
{
    public class RedisConexao : IRedisConexao
    {
        public RedisConexao(string connectionString, string endPoint)
        {
            _connectionString = connectionString;
            _endPoint = endPoint;

            _lazyConnection = new Lazy<IConnectionMultiplexer>(() =>
            {
                return Connect();
            });
        }

        public virtual IConnectionMultiplexer Connect()
        {
            return ConnectionMultiplexer.Connect(_connectionString);
        }

        private Lazy<IConnectionMultiplexer> _lazyConnection;
        private string _connectionString { get; }
        private string _endPoint { get; }

        public IConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }

        public IServer Server
        {
            get
            {
                return Connection.GetServer(_endPoint);
            }
        }
    }
}

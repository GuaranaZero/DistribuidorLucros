using StackExchange.Redis;
using System;

namespace AppDistribuidorLucrosRepositorio
{
    public class RedisConexao
    {
        public RedisConexao(string connectionString)
        {
            _connectionString = connectionString;

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(_connectionString);
            });
        }

        private Lazy<ConnectionMultiplexer> _lazyConnection;
        private string _connectionString { get; }

        public ConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }

        public void ClearCache()
        {
            var server = Connection.GetServer("redis-14280.c9.us-east-1-2.ec2.cloud.redislabs.com:14280");
            server.FlushDatabase();
        }
    }
}

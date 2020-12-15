using StackExchange.Redis;

namespace AppDistribuidorLucrosCore.Data
{
    public interface IRedisConexao
    {
        IConnectionMultiplexer Connection { get; }
        IServer Server { get; }

        IConnectionMultiplexer Connect();
    }
}
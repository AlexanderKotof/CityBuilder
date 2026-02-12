using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;

namespace com.example
{
    public interface INetworkClient
    {
        public IReadOnlyReactiveProperty<bool> IsConnected { get; }
		
        public UniTask<string> InvokeFunction(string function, Dictionary<string, string> header = null, Dictionary<string, object> body = null);
		
        public UniTask<string> InvokeFunction(string function, params (string, object)[] body);
		
        public UniTask<TResponse> InvokeFunction<TResponse>(string function, params (string, object)[] body) where TResponse : Response;
    }
}
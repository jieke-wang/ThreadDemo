using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkConsoleAppSample
{
    public class ProxySample
    {
        public class DynamicProxy<T> : RealProxy
        {
            private T _Instance;

            public DynamicProxy(T instance)
                : base(typeof(T))
            {
                _Instance = instance;
            }

            public override IMessage Invoke(IMessage msg)
            {
                // before
                Console.WriteLine("before");

                IMethodCallMessage callMessage = msg as IMethodCallMessage;
                object returnValue = callMessage.MethodBase.Invoke(_Instance, callMessage.Args);

                // after
                Console.WriteLine("after");

                return new ReturnMessage(returnValue, Array.Empty<object>(), 0, null, callMessage);
            }
        }

        public static class TransparentProxy
        {
            public static TInterface Create<TInterface, TImplemention>()
                where TImplemention : TInterface
            {
                TImplemention implemention = Activator.CreateInstance<TImplemention>();
                DynamicProxy<TInterface> dynamicProxy = new DynamicProxy<TInterface>(implemention);
                var instance = dynamicProxy.GetTransparentProxy();
                return (TInterface)instance;
            }
        }

        public interface ISayHello
        {
            void SayHello();
            void SayHello(string person);

            int Counter { get; set; }
        }

        public class SayHelloClass
            : MarshalByRefObject, ISayHello
        {
            public double Salary;

            private int _Counter { get; set; }
            public int Counter { get => _Counter; set => _Counter = value; }

            public void SayHello()
            {
                Console.WriteLine("Hello");
            }

            public void SayHello(string person)
            {
                Console.WriteLine($"Hello, {person}");
            }
        }

        public static void Test()
        {
            //ISayHello sayHello = TransparentProxy.Create<ISayHello, SayHelloClass>();
            SayHelloClass sayHello = TransparentProxy.Create<SayHelloClass, SayHelloClass>();

            //sayHello.SayHello();
            //sayHello.SayHello("jieke");
            //sayHello.Counter++;

            sayHello.Salary++;
        }
    }
}

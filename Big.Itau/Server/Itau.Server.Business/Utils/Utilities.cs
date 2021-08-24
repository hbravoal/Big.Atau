using System;
using System.Reflection;

namespace Itau.Server.Business.Utils
{
    internal class Utilities
    {
        public static object InvokeMethod(string typeName, string methodName, object[] objectParam)
        {
            // Get the Type for the class
            var objInstance = Activator.CreateInstance(Type.GetType(typeName));
            Type calledType = objInstance.GetType();

            // Invoke the method itself.
            MethodInfo method = calledType.GetMethod(methodName);
            return method.Invoke(objInstance, objectParam);
        }
    }
}
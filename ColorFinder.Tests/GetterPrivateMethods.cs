using System.Reflection;
using NUnit.Framework;

namespace ColorFinderTests
{
    public static class GetterPrivateMethods
    {
        public static MethodInfo GetMethod<T>(T testObject, string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or white space");
            }

            if (testObject == null)
            {
                Assert.Fail($"{nameof(testObject)} is null");
            }

            var method = testObject.GetType().GetMethod(
                methodName, BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (method == null)
            {
                Assert.Fail($"{methodName} method not found");
            }

            return method;
        }
    }
}
using System;
using Xunit;
using Chameleon.Common.Context;

namespace Test.Chameleon.Common
{
    public class ChameleonContextTest
    {
        [Fact]
        public void GetSet()
        {
            string str = "TestString";

            ChameleonContext.Current.Put("test", str);

            var result = Convert.ToString(ChameleonContext.Current.Get("test"));

            Assert.Equal(str, result);
        }
    }
}

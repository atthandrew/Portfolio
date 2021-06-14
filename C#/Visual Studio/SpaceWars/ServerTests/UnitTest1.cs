///<author>
/// Andrew Thompson & William Meldrum
/// </author>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerNS;
using SpaceWars;

namespace ServerTests
{
    [TestClass]
    public class UnitTest1
    {
        //Time and network-related code in Server did not permit for much unit testing, but we got it set up :) 
     
        [TestMethod]
        public void TestGenerateNormalizedVector()
        {
            Server server = new Server();
            PrivateObject priv = new PrivateObject(server);
            Vector2D test = new Vector2D((Vector2D)priv.Invoke("GenerateNormalizedVector"));

            Assert.AreEqual(1.0, test.Length());
        }

    }
}

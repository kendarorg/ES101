using System;
using System.Collections.Generic;
using Bus;
using InMemory.Es;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
// ReSharper disable SuggestBaseTypeForParameter

// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Bus
{
    [TestClass]
    public class ESTest
    {

     
        [TestInitialize]
        public void Setup()
        {
            
        }

        [TestMethod]
        public void ShouldNotRegisterForQueueAndTopic()
        {
            var ag = new TestAggregate();
        }
    }
}
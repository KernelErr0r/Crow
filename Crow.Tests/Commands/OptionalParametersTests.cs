using System;
using System.Reflection;
using Crow.Commands;
using Crow.Commands.Parsers;
using Xunit;

namespace Crow.Tests.Commands
{
    public class OptionalParametersTests
    {
        private ArgumentParser argumentParser = new ArgumentParser();
        private MethodInfo method1;
        private MethodInfo method2;

        public OptionalParametersTests()
        {
            method1 = GetType().GetMethod("Method1", BindingFlags.NonPublic | BindingFlags.Instance);
            method2 = GetType().GetMethod("Method2", BindingFlags.NonPublic | BindingFlags.Instance);
        
            argumentParser.RegisterTypeParser(new NumberParser());
            argumentParser.RegisterTypeParser(new StringParser());
        }
    
        [Fact]
        public void TestOptionalParameters1()
        {
            var parsedArguments = argumentParser.Parse(method1, "test1",  "test2");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal("test2", parsedArguments[1]);
            
            method1.Invoke(this, parsedArguments.ToArray());
        }
        
        [Fact]
        public void TestOptionalParameters2()
        {
            var parsedArguments = argumentParser.Parse(method1, "test1");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal(Type.Missing, parsedArguments[1]);

            method1.Invoke(this, parsedArguments.ToArray());
        }
        
        [Fact]
        public void TestOptionalParameters3()
        {
            var parsedArguments = argumentParser.Parse(method2, "test1", "3", "test2");
            
            Assert.Equal(3, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal(3, parsedArguments[1]);
            Assert.Equal("test2", parsedArguments[2]);

            method2.Invoke(this, parsedArguments.ToArray());
        }
        
        private void Method1(string arg1, string arg2 = "") {}
        private void Method2(string arg1, int arg2 = 5, string arg3 = "") {}
    }
}
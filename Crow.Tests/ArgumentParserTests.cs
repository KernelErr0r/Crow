using System;
using System.Reflection;
using Crow.Commands;
using Crow.Commands.Parsers;
using Xunit;

namespace Crow.Tests
{
    public class ArgumentParserTests
    {
        [Fact]
        public void TestStringParser()
        {
            var argumentParser = new ArgumentParser();
            
            argumentParser.RegisterTypeParser(new StringParser());

            var parsedArguments = argumentParser.Parse(GetType().GetMethod("ExampleMethod1", BindingFlags.NonPublic | BindingFlags.Instance), "test1", "test2");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal("test2", parsedArguments[1]);
        }

        [Fact]
        public void TestNumberParser()
        {
            var argumentParser = new ArgumentParser();
            
            argumentParser.RegisterTypeParser(new NumberParser());

            var parsedArguments = argumentParser.Parse(GetType().GetMethod("ExampleMethod2", BindingFlags.NonPublic | BindingFlags.Instance), "15");
            
            Assert.Single(parsedArguments);
            Assert.Equal(15, parsedArguments[0]);
        }

        [Fact]
        public void TestOptionalParameters1()
        {
            var argumentParser = new ArgumentParser();
            var method = GetType().GetMethod("ExampleMethod3", BindingFlags.NonPublic | BindingFlags.Instance);

            argumentParser.RegisterTypeParser(new StringParser());

            var parsedArguments = argumentParser.Parse(method, "test1",  "test2");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal("test2", parsedArguments[1]);
            
            method.Invoke(this, parsedArguments.ToArray());
        }
        
        [Fact]
        public void TestOptionalParameters2()
        {
            var argumentParser = new ArgumentParser();
            var method = GetType().GetMethod("ExampleMethod3", BindingFlags.NonPublic | BindingFlags.Instance);

            argumentParser.RegisterTypeParser(new StringParser());

            var parsedArguments = argumentParser.Parse(method, "test1");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal(Type.Missing, parsedArguments[1]);

            method.Invoke(this, parsedArguments.ToArray());
        }
        
        private void ExampleMethod1(string arg1, string arg2) {}
        private void ExampleMethod2(int arg1) {}
        private void ExampleMethod3(string arg1, string arg2 = "") {}
    }
}
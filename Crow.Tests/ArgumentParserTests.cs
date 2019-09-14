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
            
            Assert.Equal(parsedArguments.Count, 2);
            Assert.Equal(parsedArguments[0], "test1");
            Assert.Equal(parsedArguments[1], "test2");
        }

        [Fact]
        public void TestNumberParser()
        {
            var argumentParser = new ArgumentParser();
            
            argumentParser.RegisterTypeParser(new NumberParser());

            var parsedArguments = argumentParser.Parse(GetType().GetMethod("ExampleMethod2", BindingFlags.NonPublic | BindingFlags.Instance), "15");
            
            Assert.Equal(parsedArguments.Count, 1);
            Assert.Equal(parsedArguments[0], 15);
        }
        
        private void ExampleMethod1(string arg1, string arg2) {}
        private void ExampleMethod2(int arg1) {}
    }
}
using System.Reflection;
using Crow.Commands;
using Crow.Commands.Parsers;
using Xunit;

namespace Crow.Tests.Commands.Parsers
{
    public class StringParserTests
    {
        private ArgumentParser argumentParser = new ArgumentParser();
        private MethodInfo method1;

        public StringParserTests()
        {
            method1 = GetType().GetMethod("Method1", BindingFlags.NonPublic | BindingFlags.Instance);

            argumentParser.RegisterTypeParser(new StringParser());
        }
    
        [Fact]
        public void TestStringParser()
        {
            var parsedArguments = argumentParser.Parse(method1, "test1", "test2");
            
            Assert.Equal(2, parsedArguments.Count);
            Assert.Equal("test1", parsedArguments[0]);
            Assert.Equal("test2", parsedArguments[1]);

            method1.Invoke(this, parsedArguments.ToArray());
        }
        
        private void Method1(string arg1, string arg2) {}
    }
}
using System.Reflection;
using Crow.Commands;
using Crow.Commands.Parsers;
using Xunit;

namespace Crow.Tests.Commands.Parsers
{
    public class NumberParserTests
    {
        private ArgumentParser argumentParser = new ArgumentParser();
        private MethodInfo method1;

        public NumberParserTests()
        {
            method1 = GetType().GetMethod("Method1", BindingFlags.NonPublic | BindingFlags.Instance);

            argumentParser.RegisterTypeParser(new NumberParser());
            argumentParser.RegisterTypeParser(new StringParser());
        }
    
        [Fact]
        public void TestNumberParser()
        {
            var parsedArguments = argumentParser.Parse(GetType().GetMethod("Method1", BindingFlags.NonPublic | BindingFlags.Instance), "15");
            
            Assert.Single(parsedArguments);
            Assert.Equal(15, parsedArguments[0]);
            
            method1.Invoke(this, parsedArguments.ToArray());
        }
        
        private void Method1(int arg1) {}
    }
}
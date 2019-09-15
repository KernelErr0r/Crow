using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crow.Api.Commands;

namespace Crow.Commands
{
    public class ArgumentParser : IArgumentParser
    {
        private List<Tuple<TypeParser, Type, object>> parsers = new List<Tuple<TypeParser, Type, object>>(); //Attribute || Type of Parser || Instance
        
        public void RegisterTypeParser(object parser)
        {
            var type = parser.GetType();
            var attributes = type.GetCustomAttributes(typeof(TypeParser)) as TypeParser[];
            
            if (attributes != null && type.GetMethod("CanParse") != null)
            {
                foreach (var attribute in attributes)
                {
                    parsers.Add(new Tuple<TypeParser, Type, object>(attribute, type, parser));
                }
            }
            else
            {
                throw new ArgumentException("Invalid parser");
            }
        }

        public void UnregisterTypeParser(object parser)
        {
            var type = parser.GetType();
            var attributes = type.GetCustomAttributes(typeof(TypeParser)) as TypeParser[];

            if (attributes != null && type.GetMethod("CanParse") != null)
            {
                for (int i = 0; i < parsers.Count; i++)
                {
                    foreach (var attribute in attributes)
                    {
                        if (parsers[i].Item1 == attribute)
                        {
                            parsers.RemoveAt(i);
                        }   
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid parser");
            }
           
        }
    
        public List<object> Parse(MethodInfo methodInfo, params string[] arguments)
        {
            var result = new List<object>();
            var parameters = methodInfo.GetParameters();
            var typeCandidates = new Dictionary<ParameterInfo, List<Tuple<TypeParser, Type, object>>>();
            var argumentIndex = 0;
            var optionalParameters = 0;
            
            arguments = arguments ?? new string[0];
            
            foreach(var parameter in parameters)
                if(parameter.IsOptional)
                    optionalParameters++;

            if (arguments.Length >= parameters.Length - optionalParameters)
            {
                foreach (var parameter in parameters)
                {
                    foreach (var parser in parsers)
                    {
                        var status = parser.Item2.GetMethod("CanParse")
                            .Invoke(parser.Item3, new [] { parameter.ParameterType }) as bool?;

                        if (status ?? false)
                        {
                            if (!typeCandidates.ContainsKey(parameter))
                                typeCandidates.Add(parameter, new List<Tuple<TypeParser, Type, object>>());

                            typeCandidates[parameter].Add(parser);
                        }
                    }

                    typeCandidates[parameter] = typeCandidates[parameter].OrderBy(x => x.Item1.Priority).ToList();

                    if (!parameter.ParameterType.IsArray)
                    {
                        foreach (var type in typeCandidates[parameter])
                        {
                            if (parameter.ParameterType == type.Item1.Type && argumentIndex < arguments.Length)
                            {
                                foreach (var method in type.Item2.GetMethods())
                                {
                                    if (method.ReturnType == parameter.ParameterType)
                                    {
                                        result.Add(method.Invoke(type.Item3, new object[] { arguments[argumentIndex++] }));
                                        
                                        break;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        //TODO
                    }
                }
                
                for (int i = 0; i < Math.Max(parameters.Length - result.Count, 0); i++)
                    result.Add(Type.Missing);
            }
            else
            {
                throw new ArgumentException();
            }

            return result;
        }
    }
}
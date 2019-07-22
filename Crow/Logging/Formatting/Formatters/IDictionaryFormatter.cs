using System;
using System.Collections;
using System.Collections.Generic;

namespace Crow.Logging.Formatting.Formatters
{
    public class IDictionaryFormatter : IFormatter<IDictionary>
    {
        private ILogger _logger;

        public IDictionaryFormatter(ILogger logger)
        {
            _logger = logger;;
        }

        public void Format(string loglevel, object input)
        {
            var dictionary = (IDictionary)input;

            var keys = new List<object>();
            var values = new List<object>();

            foreach (var key in dictionary.Keys)
                keys.Add(key);

            foreach (var value in dictionary.Values)
                values.Add(value);

            for (int i = 0; i < dictionary.Count; i++)
            {
                if (i > 0)
                    _logger.Log("", "  " + $"{keys[i]} = {values[i]}", _logger.Scope);
                else
                    _logger.Log(loglevel, $"{keys[i]} = {values[i]}", _logger.Scope);
            };
        }
    }
}
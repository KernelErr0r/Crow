using System;
using Crow.Api.Commands;

namespace Crow.Commands.Parsers
{
    [TypeParser(typeof(short))]
    [TypeParser(typeof(ushort))]
    [TypeParser(typeof(int))]
    [TypeParser(typeof(uint))]
    [TypeParser(typeof(long))]
    [TypeParser(typeof(ulong))]
    public class NumberParser
    {
        public bool CanParse(Type type)
        {
            return type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong);
        }
        
        public short ParseInt16(string str) => Int16.Parse(str);
        public ushort ParseUInt16(string str) => UInt16.Parse(str);
        public int ParseInt32(string str) => Int32.Parse(str);
        public uint ParseUInt32(string str) => UInt32.Parse(str);
        public long ParseInt64(string str) => Int64.Parse(str);
        public ulong ParseUInt64(string str) => UInt64.Parse(str);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace httpServCsharp
{
    class Program
    {
 
 
        static void Main(string[] args)
        {
            var prefixes = new List<string>() { "http://*:8080/" };
            MyListener.startListener(prefixes);
        }
    }
}

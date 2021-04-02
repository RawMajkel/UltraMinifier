using System;
using System.IO;

namespace UltraMinifier.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var stylesheet = new Stylesheet(@"C:\Users\mdroz\OneDrive\MSI GS66\Desktop\bootstrap.min.css");

            Console.WriteLine($"CSS before: {stylesheet.MinifiedValue}\n");
            Console.WriteLine($"CSS after: {stylesheet.UltraMinifiedValue}");

            File.WriteAllText(@"C:\Users\mdroz\OneDrive\MSI GS66\Desktop\bootstrap.ultra.css", stylesheet.UltraMinifiedValue);
        }
    }
}

// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .BuildServiceProvider();

Console.WriteLine("Hello, World!");

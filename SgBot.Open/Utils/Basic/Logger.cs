using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace SgBot.Open.Utils.Basic
{
    internal static class Logger
    {
        public static void Log(string what, LogLevel level)
        {
            try
            {
                AnsiConsole.Markup($"[green][[SgBot.Open]][/] > {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                switch (level)
                {
                    case LogLevel.Simple:
                        AnsiConsole.Markup(" [white]" +
                                           "  [[INFO]]    [/]");
                        break;
                    case LogLevel.Important:
                        AnsiConsole.Markup(" [yellow]" +
                                           "[[IMPORTANT]] [/]");
                        break;
                    case LogLevel.Warning:
                        AnsiConsole.Markup(" [red]" +
                                           " [[WARNING]]  [/]");
                        break;
                    case LogLevel.Error:
                        AnsiConsole.Markup(" [red]" +
                                           "  [[ERROR]]   [/]");
                        break;
                    case LogLevel.Fatal:
                        AnsiConsole.Markup(" [red]" +
                                           "  [[FATAL]]   [/]");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
                // AnsiConsole.Markup($"[white]{what}[/]\n");
                Console.WriteLine(what);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e}");
            }
        }
    }
    public enum LogLevel
    {
        Simple,
        Important,
        Warning,
        Error,
        Fatal
    }
}

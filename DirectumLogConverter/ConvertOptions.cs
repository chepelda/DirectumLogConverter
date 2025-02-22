﻿using System;
using CommandLine;
using DirectumLogConverter.Properties;

namespace DirectumLogConverter
{
  /// <summary>
  /// Опции конвертации.
  /// </summary>
  internal sealed class ConvertOptions
  {
    #region Поля и свойства

    [Value(0, MetaName = "Source", Required = true)]
    public string InputPath { get; set; }

    [Value(1, MetaName = "Destination")]
    public string OutputPath { get; set; }

    [Option('c', "csv", Default = false)]
    public bool CsvFormat { get; set; }

    #endregion

    #region Методы

    /// <summary>
    /// Показать справку и выйти.
    /// </summary>
    private static void ShowUsageAndExit()
    {
      Console.WriteLine(Resources.Usage, AppDomain.CurrentDomain.FriendlyName, Program.ConvertedFilenamePostfix);
      Environment.Exit((int)Program.ExitCode.Success);
    }

    /// <summary>
    /// Получить опции конвертации из аргументов командной строки.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <returns>Опции конвертации.</returns>
    public static ConvertOptions GetFromArgs(string[] args)
    {
      var parser = new Parser(settings =>
      {
        settings.AutoHelp = false;
        settings.AutoVersion = false;
        settings.CaseSensitive = false;
        settings.EnableDashDash = true;
      });

      var parsedArguments = parser.ParseArguments<ConvertOptions>(args);
      var result = default(ConvertOptions);
      parsedArguments.WithParsed(options => result = options)
        .WithNotParsed(errors =>
        {
          foreach (var error in errors)
          {
            if (error.Tag == ErrorType.UnknownOptionError)
            {
              Console.Error.WriteLine(Resources.ResourceManager.GetString(nameof(ErrorType.UnknownOptionError)), ((UnknownOptionError)error).Token);
              Environment.Exit((int)Program.ExitCode.Error);
            }
          }

          ShowUsageAndExit();
        });
      return result;
    }

    #endregion
  }
}
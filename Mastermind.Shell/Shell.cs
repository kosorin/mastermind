using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    public static class Shell
    {
        public static void Initialize(ShellOptions options)
        {
            if (IsInitialized)
            {
                return;
            }
            IsInitialized = true;

            Options = options;

            Console.OutputEncoding = Options.Encoding;

            ShellColorizer.Initialize(Options.DefaultForegroundColor, Options.DefaultBackgroundColor);
            ShellTextStyler.Initialize();
        }

        public static ShellOptions Options { get; private set; }

        private static bool IsInitialized { get; set; }


        public static void Clear()
        {
            ShellColorizer.Clear();
        }


        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void Write(object value)
        {
            Console.Write(value);
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public static void WriteLine(object value)
        {
            Console.WriteLine(value);
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }


        public static bool PromptBool(string question, bool? defaultValue = null)
        {
            var options = new[] { "y", "n", };
            var defaultOption = defaultValue.HasValue
                ? (defaultValue.Value ? 0 : 1)
                : (int?)null;

            return Prompt(question, options, defaultOption, line =>
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    return defaultValue.HasValue
                        ? defaultValue.Value
                        : (bool?)null;
                }
                var value = line.Trim().ToUpper();
                return value == "Y" ? true
                    : (value == "N" ? false
                    : (bool?)null);
            }, value => value.HasValue) ?? throw new Exception("Unexpected error.");
        }

        public static int PromptInt(string question, int? defaultValue = null, (int Min, int Max)? range = null)
        {
            var options = new List<string>();

            if (range.HasValue)
            {
                options.Add($"{range.Value.Min}..{range.Value.Max}");
            }

            var defaultOption = (int?)null;
            if (defaultValue.HasValue)
            {
                options.Add(defaultValue.Value.ToString());
                defaultOption = options.Count - 1;
            }

            return Prompt(question, options, defaultOption, line =>
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    return defaultValue.HasValue
                        ? defaultValue.Value
                        : (int?)null;
                }
                if (int.TryParse(line, out var value))
                {
                    return !range.HasValue || (range.Value.Min <= value && value <= range.Value.Max)
                        ? value
                        : (int?)null;
                }
                return null;
            }, value => value.HasValue) ?? throw new Exception("Unexpected error.");
        }

        public static string PromptString(string question, string defaultValue = null, bool allowEmpty = false)
        {
            if (defaultValue != null && string.IsNullOrWhiteSpace(defaultValue))
            {
                defaultValue = null;
                allowEmpty = true;
            }

            var options = allowEmpty || defaultValue != null
                ? new[] { defaultValue }
                : null;
            var defaultOption = defaultValue != null ? 0 : (int?)null;

            return Prompt(question, options, defaultOption, line =>
            {
                return string.IsNullOrWhiteSpace(line) && defaultValue != null
                    ? defaultValue
                    : line;
            }, value => allowEmpty || !string.IsNullOrWhiteSpace(value) || value == defaultValue);
        }

        public static T PromptEnum<T>(string question, T? defaultValue = null) where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException($"Type {type.Name} must be enum.");
            }

            var values = Enum.GetValues(type).Cast<T>().ToList();
            var options = values.Select(x => x.ToString().ToLower()).ToList();
            var defaultOption = defaultValue.HasValue ? values.IndexOf(defaultValue.Value) : (int?)null;
            if (defaultOption < 0)
            {
                defaultOption = null;
            }

            return Prompt(question, options, defaultOption, line =>
            {
                line = line?.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(line))
                {
                    return defaultValue;
                }

                var optionIndex = options.IndexOf(line);
                if (optionIndex >= 0)
                {
                    return values[optionIndex];
                }

                var matchedOptions = options.Where(x => x.StartsWith(line)).ToList();
                if (matchedOptions.Count == 1)
                {
                    return values[options.IndexOf(matchedOptions[0])];
                }

                return null;
            }, value => value.HasValue) ?? throw new Exception("Unexpected error.");
        }

        public static T Prompt<T>(string question, IList<string> options, int? defaultOption, Func<string, T> converter, Func<T, bool> validator)
        {
            T value;
            while (true)
            {
                using (ShellColorizer.SetForeground(Options.PromptQuestionColor))
                {
                    Console.Write("> ");

                    var showColon = false;
                    if (!string.IsNullOrWhiteSpace(question))
                    {
                        showColon = true;
                        Console.Write($"{question} ");
                    }
                    if (options?.Count > 0)
                    {
                        showColon = true;
                        using (ShellColorizer.SetForeground(Options.PromptOptionColor))
                        {
                            Console.Write("[");
                            for (int i = 0; i < options.Count; i++)
                            {
                                var option = options[i];

                                ShellTextStyler underlineStyler = null;
                                try
                                {
                                    if (defaultOption == i)
                                    {
                                        underlineStyler = ShellTextStyler.Set(ShellTextStyle.Underline);
                                    }
                                    Console.Write(option);
                                }
                                finally
                                {
                                    underlineStyler?.Dispose();
                                }

                                if (i + 1 < options.Count)
                                {
                                    Console.Write('/');
                                }
                            }
                            Console.Write("] ");
                        }
                    }

                    if (showColon)
                    {
                        Console.Write(": ");
                    }
                }

                try
                {
                    using (ShellColorizer.SetForeground(Options.PromptColor))
                    {
                        var line = Console.ReadLine();
                        try
                        {
                            value = converter(line);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                finally
                {
                    UndoLine();
                }

                if (validator?.Invoke(value) ?? true)
                {
                    return value;
                }
            }
        }


        public static void UndoLine(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                MoveUp();
                ClearLine();
            }
        }

        public static void ClearLine()
        {
            var currentRow = Console.CursorTop;
            Console.SetCursorPosition(0, currentRow);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentRow);
        }

        private static void MoveUp()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}

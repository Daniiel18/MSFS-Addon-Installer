namespace MSFS.AddonInstaller.Utils
{
    public static class ConsoleUI
    {
        public static void DrawInlineHeader(
            string text,
            ConsoleColor color)
        {
            int width = Console.WindowWidth - 1;

            Console.ForegroundColor = color;

            var line = $"── {text} ";

            if (line.Length < width)
                line += new string('─', width - line.Length);

            Console.WriteLine(line);

            Console.ResetColor();
        }

        public static void WriteEnumeratedColumns(
        IReadOnlyList<string> items,
        int preferredColumns = 2,
        int columnPadding = 4)
        {
            if (items.Count == 0)
                return;

            int consoleWidth = Console.WindowWidth - 1;

            var numbered = items
                .Select((item, index) => $"{index + 1}. {item}")
                .ToList();

            int columns = preferredColumns;
            int rows = (int)Math.Ceiling(numbered.Count / (double)columns);

            int columnWidth = (consoleWidth / columns) - columnPadding;
            if (columnWidth < 20)
            {
                columns = 1;
                rows = numbered.Count;
                columnWidth = consoleWidth;
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int index = r + (c * rows);
                    if (index >= numbered.Count)
                        continue;

                    var text = numbered[index];

                    if (text.Length > columnWidth)
                        text = text.Substring(0, columnWidth - 1) + "…";

                    Console.Write(text.PadRight(columnWidth + columnPadding));
                }
                Console.WriteLine();
            }
        }
    }
}

using PlaneGame.Domain.Tactics;
using PlaneGame.Extensions;

namespace PlaneGame;

public static class ConsoleInput
{
    public static int ReadTeamCount()
    {
        const int minTeams = 2;

        while (true)
        {
            Console.Write($"Введите количество команд (минимум {minTeams}): ");
            var input = Console.ReadLine();

            var isNumber = int.TryParse(input, out var value);

            if (!isNumber)
            {
                Console.WriteLine("Это не число.");
                continue;
            }

            if (value < minTeams)
            {
                Console.WriteLine($"Количество команд должно быть не меньше {minTeams}.");
                continue;
            }

            return value;
        }
    }

    public static TacticType ReadTactic()
    {
        var tactics = Enum.GetValues<TacticType>();

        Console.WriteLine("Выберите стратегию команды:");

        for (var i = 0; i < tactics.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {tactics[i].GetDisplayName()}");
        }

        while (true)
        {
            Console.Write("Введите номер стратегии: ");
            var input = Console.ReadLine();

            var isNumber = int.TryParse(input, out var index);

            if (!isNumber)
            {
                Console.WriteLine("Введите число.");
                continue;
            }

            var isInRange = index >= 1 && index <= tactics.Length;

            if (!isInRange)
            {
                Console.WriteLine("Такой стратегии не существует.");
                continue;
            }

            return tactics[index - 1];
        }
    }
}

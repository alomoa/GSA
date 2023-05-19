namespace GSA.utils
{
    public class ConsoleHelpers
    {

        public void ProcessCommands()
        {
            var command = Console.ReadLine();
            var commandParts = command.Split(' ');

            if (commandParts[0] == "Capital")
            {
                var strategies = commandParts[1].Split(" ");

                ProcessCapital(strategies);

            }
            else if (commandParts[0] == "cumulative-pnl")
            {
                var region = commandParts[1];

                ProcessCumulativePnL(region);
            }
            else
            {
                Console.WriteLine("Invalid command.");
            }

        }

        public void ProcessCapital(string[] strategies)
        {
            foreach (string strategy in strategies)
            {
                Console.WriteLine($"{strategy}");

            }
        }

        public void ProcessCumulativePnL(string region)
        {
           
                Console.WriteLine($"{region}");

        }
    }
}

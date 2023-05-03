
using System.Text;

namespace BuildProjectManager
{
    public static class ConsoleHelpers
    {
        public static int GetIntegerInRange(int pMin, int pMax, string pMessage)
        {
            if (pMin > pMax)
            {
                throw new Exception($"Minimum value {pMin} cannot be greater than maximum value {pMax}");
            }

            int result;

            do
            {
                Console.WriteLine(pMessage);
                Console.WriteLine($"Please enter a number between {pMin} and {pMax} inclusive.");

                string userInput = Console.ReadLine();

                try
                {
                    result = int.Parse(userInput);
                }
                catch
                {
                    Console.WriteLine($"{userInput} is not a number");
                    continue;
                }

                if (result >= pMin && result <= pMax)
                {
                    return result;
                }
                Console.WriteLine($"{result} is not between {pMin} and {pMax} inclusive.");
            } while (true);
        }
    }

    // Base class for all menu items to inherit from
    abstract class MenuItem
    {
        public abstract string MenuText();
        public abstract void Select();
    }

    // Lays out basic functionality for menu items
    abstract class ConsoleMenu : MenuItem
    {
        protected List<MenuItem> _menuItems = new List<MenuItem>();

        public bool IsActive { get; set; }

        public abstract void CreateMenu();

        public override void Select()
        {
            IsActive = true;
            do
            {
                CreateMenu(); // Re-initialises menu after returning from child menu item
                string output = $"{MenuText()}{Environment.NewLine}";
                int selection = ConsoleHelpers.GetIntegerInRange(1, _menuItems.Count, this.ToString()) - 1;
                _menuItems[selection].Select();
            } while (IsActive);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(MenuText());
            for (int i = 0; i < _menuItems.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_menuItems[i].MenuText()}");
            }
            return sb.ToString();
        }
    }

    // Returns to previous menu
    class ReturnItem : MenuItem
    {
        private ConsoleMenu _menu;

        public ReturnItem(ConsoleMenu parentItem)
        {
            _menu = parentItem;
        }

        public override string MenuText()
        {
            return "<-Return";
        }

        public override void Select()
        {
            _menu.IsActive = false; // Breaks out of do-while loop to return to the parent loop (Line 62)
        }
    }
}

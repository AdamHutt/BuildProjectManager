
using System.Text;

namespace BuildProjectManager
{
    #region Entry menu and items

    class EntryMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {
            _menuItems.Clear();
            //Populate list with menu items...
        }

        public override string MenuText()
        {
            return "Build Project Manager";
        }
    }

    class LoadBeigeFileMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Load Beige file from storage";
        }
    }

    class DisplayPortfolioSummaryMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Display a summary of entire portfolio transactions";
        }
    }

    class AddProjectMenuItem : MenuItem
    {
        private ProjectManager _projectManager;
        public AddProjectMenuItem(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override string MenuText()
        {
            return "Add new project";
        }

        public override void Select() // Add new project
        {
            int id;
            do // Input validation
            {
                Console.Write("Enter a project ID (Leave blank to return): ");
                string strId = Console.ReadLine();
                if (strId == "") { return; }
                try
                {
                    id = int.Parse(strId);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid integer.");
                    continue;
                }
                break;
            } while (true);
            bool isNewBuild;
            do // Input validation
            {
                Console.Write("Is this project a new build (VAT exempt)? Y/n or leave blank to return: ");
                string strIsNewBuild = Console.ReadLine();
                if (strIsNewBuild.ToUpper() == "Y") { isNewBuild = true; }
                else if (strIsNewBuild == "N") { isNewBuild = false; }
                else if (strIsNewBuild == "") { return; }
                else { Console.WriteLine("Please enter Y/n or leave blank to return."); continue; }
                break;
            } while (true);
            
            try
            {
                _projectManager.AddNewProject(id, isNewBuild);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine($"Successfully added a new project with id: {id}!");
        }
    }

    class RemoveProjectMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Remove existing project";
        }
    }

    class RemoveProjectMenuItem : MenuItem
    {
        public RemoveProjectMenuItem()
        {

        }

        public override string MenuText()
        {
            throw new NotImplementedException();
        }
    }

    class SelectProjectMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Select existing project";
        }
    }

    #endregion

    #region Project menu and items

    class AddTransactionMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {
            
        }

        public override string MenuText()
        {
            return "Add new transaction to project";
        }
    }

    class RemoveTransactionMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {
            
        }

        public override string MenuText()
        {
            return "Remove existing transaction from project";
        }
    }

    class DisplayProjectSummaryMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {
            
        }

        public override string MenuText()
        {
            return "Display a summary of this project's transactions";
        }
    }

    class DisplayPurchaseMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {
            
        }

        public override string MenuText()
        {
            return "Display purchases for this project";
        }
    }

    class DisplaySalesMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Display sales for this project";
        }
    }

    #endregion
}

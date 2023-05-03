
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

    class AddProjectMenu : ConsoleMenu
    {
        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Add new project";
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

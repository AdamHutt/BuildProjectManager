
using System.Text;

namespace BuildProjectManager
{
    #region Entry menu and items

    class EntryMenu : ConsoleMenu
    {
        ProjectManager _projectManager;
        public EntryMenu(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            _menuItems.Add(new AddProjectMenuItem(_projectManager));
            _menuItems.Add(new LoadBeigeFileMenu(_projectManager));
            if(_projectManager.Projects.Count > 0)
            {
                _menuItems.Add(new DisplayPortfolioSummaryMenuItem(_projectManager));
                _menuItems.Add(new SelectProjectMenu(_projectManager));
                _menuItems.Add(new RemoveProjectMenu(_projectManager));
            }
            _menuItems.Add(new ReturnItem(this));
        }

        public override string MenuText()
        {
            return "Build Project Manager";
        }
    }

    class LoadBeigeFileMenu : ConsoleMenu
    {
        private ProjectManager _projectManager;
        public LoadBeigeFileMenu(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override void CreateMenu()
        {

        }

        public override string MenuText()
        {
            return "Load Beige file from storage";
        }
    }

    class DisplayPortfolioSummaryMenuItem : MenuItem
    {
        private ProjectManager _projectManager;
        public DisplayPortfolioSummaryMenuItem(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override void Select()
        {
            float[,] summary = _projectManager.PortfolioSummary();
            
            Console.WriteLine("{0, 7} {1, 11} {2, 11} {3, 11} {4, 11}", "ID", "Sales", "Purchases", "Refund", "Profit");
            for (int i = 0; i < _projectManager.Projects.Count + 1; i++)
            {
                if (i < _projectManager.Projects.Count) 
                {
                    Console.WriteLine("{0, 7:N0} {1, 11:N} {2, 11:N} {3, 11:N} {4, 11:N}", summary[i, 0], summary[i, 1], summary[i, 2], summary[i, 3], summary[i, 4]);
                }
                else
                {
                    Console.WriteLine("{0, 7} {1, 11:N} {2, 11:N} {3, 11:N} {4, 11:N}", "Total", summary[i, 1], summary[i, 2], summary[i, 3], summary[i, 4]);
                }
            }
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
                Console.Write("Enter a project ID (leave blank to return): ");
                string strId = Console.ReadLine();
                if (string.IsNullOrEmpty(strId)) { return; }
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
                else if (string.IsNullOrEmpty(strIsNewBuild)) { return; }
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
        ProjectManager _projectManager;
        public RemoveProjectMenu(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            foreach(Project p in _projectManager.Projects)
            {
                _menuItems.Add(new RemoveProjectMenuItem(p._id, _projectManager));
            }
            _menuItems.Add(new ReturnItem(this));
        }

        public override string MenuText()
        {
            return "Remove existing project";
        }
    }

    class RemoveProjectMenuItem : MenuItem
    {
        private ProjectManager _projectManager;
        private int _id;
        public RemoveProjectMenuItem(int id, ProjectManager projectManager)
        {
            _id = id;
            _projectManager = projectManager;
        }

        public override string MenuText()
        {
            return $"Delete project {_id}";
        }

        public override void Select()
        {
            _projectManager.RemoveExistingProject(_id); // No user input therefore no validation needed
        }
    }

    class SelectProjectMenu : ConsoleMenu
    {
        private ProjectManager _projectManager;
        public SelectProjectMenu(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            foreach(Project p in _projectManager.Projects)
            {
                _menuItems.Add(new SelectedProjectMenu(p));
            }
            _menuItems.Add(new ReturnItem(this));
        }

        public override string MenuText()
        {
            return "Select existing project";
        }
    }

    class SelectedProjectMenu : ConsoleMenu
    {
        private Project _project;
        public SelectedProjectMenu(Project project)
        {
            _project = project;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            _menuItems.Add(new AddTransactionMenuItem(_project));
            if(_project.transactions.Count > 0) 
            { 
                _menuItems.Add(new DisplayProjectSummaryMenuItem(_project));
                _menuItems.Add(new DisplayPurchaseMenuItem(_project));
                _menuItems.Add(new DisplaySalesMenuItem(_project));
                _menuItems.Add(new RemoveTransactionMenu(_project));
            }
            _menuItems.Add(new ReturnItem(this));
        }

        public override string MenuText()
        {
            string s;
            if (_project._isNewBuild) { s = "New Build"; } else { s = "Renovation"; }
            return String.Format("ID: {0, -7:N0} Type: {1, 1}", _project._id, s);
        }
    }

    #endregion

    #region Project menu and items

    class AddTransactionMenuItem : MenuItem
    {
        private Project _project;
        public AddTransactionMenuItem(Project project)
        {
            _project = project;
        }

        public override string MenuText()
        {
            return "Add new transaction to project";
        }

        public override void Select()
        {
            float amount;
            string type;
            do // Input validation
            {
                Console.Write("Enter transaction amount (leave blank to return): ");
                string strAmount = Console.ReadLine();
                if (string.IsNullOrEmpty(strAmount)) { return; }
                try
                {
                    amount = float.Parse(strAmount);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid floating point number.");
                    continue;
                }
                type = "";
                Console.Write("Specify whether the transaction is Sale, Purchase, Land or Renovation; S/P/L/R or leave blank to return: ");
                type = Console.ReadLine();
                if (string.IsNullOrEmpty(type)) { return; }
                if (type != "S" && type != "P" && type != "L" && type != "R")
                {
                    Console.WriteLine("Please enter a valid type: \'S/P/L/R\' or leave blank to return.");
                    continue;
                }
                break;
            } while (true);
            _project.AddNewTransaction(amount, type[0]);
        }
    }

    class RemoveTransactionMenu : ConsoleMenu
    {
        private Project _project;
        public RemoveTransactionMenu(Project project)
        {
            _project = project;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            foreach(Transaction t in _project.transactions)
            {
                _menuItems.Add(new RemoveTransactionMenuItem(t, _project));
            }
            _menuItems.Add(new ReturnItem(this));
        }

        public override string MenuText()
        {
            return "Remove existing transaction from project";
        }
    }

    class RemoveTransactionMenuItem : MenuItem
    {
        private Project _project;
        private Transaction _transaction;
        public RemoveTransactionMenuItem(Transaction transaction, Project project)
        {
            _project = project;
            _transaction = transaction;
        }

        public override string MenuText()
        {
            return string.Format("Type: {0, 3} Amount: {1, 11:N}", _transaction._type, _transaction._amount);
        }

        public override void Select()
        {
            _project.RemoveExistingTransaction(_transaction);
        }
    }

    class DisplayProjectSummaryMenuItem : MenuItem
    {
        private Project _project;
        public DisplayProjectSummaryMenuItem(Project project)
        {
            _project = project;
        }

        public override string MenuText()
        {
            return "Display a summary of this project's transactions";
        }

        public override void Select()
        {
            float[] summary = _project.ProjectSummary();
            Console.WriteLine("{0, 7:N0} {1, 11:N} {2, 11:N} {3, 11:N} {4, 11:N}", "ID", "Sales", "Purchases", "Refund", "Profit");
            Console.WriteLine("{0, 7:N0} {1, 11:N} {2, 11:N} {3, 11:N} {4, 11:N}", summary[0], summary[1], summary[2], summary[3], summary[4]);
        }
    }

    class DisplayPurchaseMenuItem : MenuItem
    {
        private Project _project;
        public DisplayPurchaseMenuItem(Project project)
        {
            _project = project;
        }

        public override string MenuText()
        {
            return "Display purchases for this project";
        }

        public override void Select()
        {

            Console.WriteLine("{0, -7} {1, 11}", "Type", "Amount");
            foreach(Transaction t in _project.transactions)
            {
                if (t._type != 'S') { Console.WriteLine("{0, -7} {1, 11}", t._type, t._amount); }
            }
        }
    }

    class DisplaySalesMenuItem : MenuItem
    {
        private Project _project;
        public DisplaySalesMenuItem(Project project)
        {
            _project = project;
        }

        public override string MenuText()
        {
            return "Display sales for this project";
        }

        public override void Select()
        {
            Console.WriteLine("{0, -7} {1, 11}", "Type", "Amount");
            foreach (Transaction t in _project.transactions)
            {
                if (t._type == 'S') { Console.WriteLine("{0, -7} {1, 11}", t._type, t._amount); }
            }
        }
    }

    #endregion
}

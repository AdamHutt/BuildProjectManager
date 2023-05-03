
namespace BuildProjectManager
{
    class ProjectManager
    {
        public List<Project> Projects { get; private set; }
        public ProjectManager()
        {
            Projects = new List<Project>();
        }

        public void AddNewProject(int id, bool isNewBuild)
        {
            if(id.GetType() != typeof(int)) { throw new ArgumentException("Invalid Input: must be an integer"); }
            if(id < 1) { throw new ArgumentException("Invalid ID: cannot be less than 1"); }
            foreach(Project p in Projects)
            {
                if(id == p._id) { throw new ArgumentException("Invalid ID: already in use"); }
            }
            Projects.Add(new Project(id, isNewBuild));
        }

        public void RemoveExistingProject(int id)
        {
            foreach(Project p in Projects)
            {
                if(p._id == id)
                {
                    Projects.Remove(p);
                    return;
                }
                throw new ArgumentException("Invalid ID: no projects with this ID");
            }
        }

        public float[,] PortfolioSummary()
        {
            float[,] summary = new float[Projects.Count + 1, 5];
            int i = 0;
            foreach(Project p in Projects)
            {
                float[] s = p.ProjectSummary();
                int j = 0;
                foreach(float v in s)
                {
                    if(j != 0) { summary[Projects.Count + 1, j] += v; } // Add to total
                    summary[i, j] = v;
                    j++;
                }
                i++;
            }
            return summary;
        }
    }

    public class Project
    {
        const float VAT = 1.2f;

        protected List<Transaction> transactions = new List<Transaction>();
        public int _id { get; private set; }
        public bool _isNewBuild;

        public Project(int id, bool isNewBuild)
        {
            _id = id;
        }

        public void AddNewTransaction(Transaction t)
        {
            transactions.Add(t);
        }

        public void RemoveExistingTransaction(Transaction t)
        {
            transactions.Remove(t);
        }

        public float[] ProjectSummary()
        {
            float[] summary = new float[] { 0f, 0f, 0f, 0f, 0f };
            summary[0] = _id; // Id
            foreach(Transaction t in transactions)
            {
                if(t.type == Transaction.TRANSACTION_TYPE.Sale) { summary[1] += t.amount; } // Sales
                else { summary[2] += t.amount; } // Purchase
            }
            if(_isNewBuild) { summary[3] = summary[2] - (summary[2] / VAT); } // Refund
            summary[4] = (summary[2] - summary[3]) + summary[4]; // Profit
            return summary;
        }
    }

    public class Transaction
    {
        public enum TRANSACTION_TYPE
        {
            Sale = 'S',
            Purchase = 'P',
            Land = 'L',
            Renovation = 'R'
        }
        public TRANSACTION_TYPE type { get; private set; }
        public float amount { get; private set; }
    }
}

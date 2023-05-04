using System.IO;
using System.Runtime.CompilerServices;

namespace BuildProjectManager
{
    class ProjectManager
    {
        const string inputDir = "/BuildProjectInput";

        public List<Project> Projects { get; private set; }
        public ProjectManager()
        {
            Projects = new List<Project>();

            if(!Directory.Exists(Directory.GetCurrentDirectory() + inputDir)) // Create directory for user to place Beige files in, if one doesn't already exist.
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + inputDir);
            }
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
                if (p.transactions.Count < 1) { summary[i, 0] = p._id; continue; }
                float[] s = p.ProjectSummary();
                int j = 0;
                foreach(float v in s)
                {
                    if(j != 0) { summary[Projects.Count, j] += v; } // Add to total
                    summary[i, j] = v;
                    j++;
                }
                i++;
            }
            return summary;
        }

        public void BeigeFileParser(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + inputDir + "/" + fileName))
                {
                    List<Project> importedProjects = new List<Project>(); // temporary list of projects - separate from ProjectManager list so data isn't corrupted if file reading goes wrong
                    do
                    {
                        string line = sr.ReadLine();
                        if (char.IsDigit(line[0]))
                        {
                            line += ',';
                            string bufferStr = ""; // char buffer for each iteration of foreach loop
                            int valCounter = 0; // keeps track of current value being read
                            tempTransaction temp = new tempTransaction(); // struct to store temporary transaction data
                            int i = 0;
                            foreach (char c in line)
                            {
                                if(c == ',') // comma means new end of value
                                {
                                    if (valCounter == 0) // id
                                    {
                                        int id;
                                        try { id = int.Parse(bufferStr); }
                                        catch (FormatException e) { Console.WriteLine(e.Message); return; }
                                        temp.id = id;
                                        i++;
                                    }
                                    else if (valCounter == 1) // type
                                    {
                                        if (bufferStr[0] == 'S' || bufferStr[0] == 'P' || bufferStr[0] == 'L' || bufferStr[0] == 'R') { temp.type = bufferStr[0]; }
                                        else { Console.WriteLine($"Value \'{bufferStr[0]}\' is not a valid transaction type."); return; }
                                        i++;
                                    }
                                    else if (valCounter == 2) // amount
                                    {
                                        float amount;
                                        try { amount = float.Parse(bufferStr); }
                                        catch (FormatException e) { Console.WriteLine(e.Message); return; }
                                        temp.amount = amount;
                                    }
                                    else { Console.WriteLine("Error: Number of values is greater than 3."); return; }
                                    bufferStr = ""; // clear buffer
                                    valCounter++; 
                                    if (valCounter < 3) { continue; } // if line not finished reading don't attempt to generate project from temp
                                    AddTransactionD:
                                    foreach (Project p in importedProjects) // only reaches this code once all 3 values have been read
                                    {
                                        if (p._id == temp.id) { p.AddNewTransaction(temp.amount, temp.type); goto SkipD; }
                                    }
                                    bool isNewBuild = false; // only reaches here if there is currently no project with the id
                                    if (temp.type == 'L') { isNewBuild = true; }
                                    importedProjects.Add(new Project(temp.id, isNewBuild)); // create new project with the id
                                    goto AddTransactionD; // once this new project has been initialised we gan go back to adding the transaction from this line
                                }
                                bufferStr += c;
                                i++;
                            SkipD:;
                            }
                        }
                        else if (char.IsLetter(line[0]))
                        {
                            line = line.Replace(" ", "");
                            string bufferStr = ""; // char buffer for each iteration of foreach loop
                            tempTransaction temp = new tempTransaction(); // struct to store temporary transaction data
                            int i = 0;
                            foreach (char c in line)
                            {
                                if (c == '(') // type
                                {
                                    if (bufferStr != "Sale" && bufferStr != "Purchase" && bufferStr != "Land" && bufferStr != "Renovation") { Console.WriteLine("Error: type is not valid"); return; }
                                    temp.type = bufferStr[0];
                                    bufferStr = "";
                                    i++;
                                    continue;
                                }
                                if (c == '=') // id
                                {
                                    int id;
                                    bufferStr = bufferStr.Replace(")", ""); // remove the ')' from string buffer to leave only digit chars
                                    try { id = int.Parse(bufferStr); }
                                    catch(FormatException e) { Console.WriteLine(e.Message); return; }
                                    temp.id = id;
                                    bufferStr = "";
                                    i++;
                                    continue;
                                }
                                if (c == ';') // amount
                                {
                                    float amount;
                                    try { amount = float.Parse(bufferStr); }
                                    catch(FormatException e) { Console.WriteLine(e.Message); return; }
                                    temp.amount = amount;
                                    bufferStr = "";
                                    // end of line
                                    AddTransactionL:
                                    foreach (Project p in importedProjects) // only reaches this code once all 3 values have been read
                                    {
                                        if (p._id == temp.id) { p.AddNewTransaction(temp.amount, temp.type); goto SkipL; }
                                    }
                                    bool isNewBuild = false; // only reaches here if there is currently no project with the id
                                    if (temp.type == 'L') { isNewBuild = true; }
                                    importedProjects.Add(new Project(temp.id, isNewBuild)); // create new project with the id
                                    goto AddTransactionL; // once this new project has been initialised we gan go back to adding the transaction from this line
                                }
                                else { if (i >= line.Length - 1) { Console.WriteLine("Error: \';\' expected at line end."); return; } }
                                
                                bufferStr += c;
                                i++;
                            SkipL:;
                            }
                        }
                        Console.WriteLine("Doing while...");
                    } while (!sr.EndOfStream);
                    foreach (Project p in importedProjects)
                    {
                        Projects.Add(p);
                    }
                }
            } catch (FileNotFoundException e) { Console.WriteLine(e.Message); return; }
            Console.WriteLine("File data successfully added to Project Manager!");
        }
        struct tempTransaction
        {
            public int id;
            public char type;
            public float amount;
        }
    }

    public class Project
    {
        const float VAT = 1.2f;

        public List<Transaction> transactions = new List<Transaction>();
        public int _id { get; private set; }
        public bool _isNewBuild;

        public Project(int id, bool isNewBuild)
        {
            _id = id;
            _isNewBuild = isNewBuild;
        }

        public void AddNewTransaction(float amount, char type)
        {
            transactions.Add(new Transaction(amount, type));
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
                if(t._type == 'S') { summary[1] += t._amount; } // Sales
                else { summary[2] += t._amount; } // Purchase
            }
            if(_isNewBuild) { summary[3] = summary[2] - (summary[2] / VAT); } // Refund
            summary[4] = (summary[1] - summary[2]) + summary[3]; // Profit
            return summary;
        }
    }

    public class Transaction
    {
        public Transaction(float amount, char type) 
        { 
            _amount = amount;
            _type = type;
        }
        
        public char _type { get; private set; }
        public float _amount { get; private set; }
    }
}

using System.Text.Json;
using System.Xml;

namespace ConsoleApp74
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===PERSONAL FINANCE MANAGER===");

            bool running = true;
            BankAccount account = new BankAccount();

            while (running)
            {
                Console.WriteLine("1. Add transaction");
                Console.WriteLine("2. List of transactions");
                Console.WriteLine("3. Current balance");
                Console.WriteLine("4. Delete transaction");
                Console.WriteLine("5. Save / Load from file");
                Console.WriteLine("6. Exit");

                string choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        account.AddTransaction();
                        break;
                    case "2":
                        account.ShowTransactions();
                        break;
                    case "3":
                        account.ShowBalance();
                        break;
                    case "4":
                        account.DeleteTransaction();
                        break;
                    case "5":
                        Console.WriteLine("1. Save | 2. Load from file");
                        string secondChoice = Console.ReadLine();
                        
                        if(secondChoice == "1")
                        {
                            account.Save();
                        }
                        else if(secondChoice == "2")
                        {
                            account.Load();
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice!");
                        }
                            break;
                    case "6":
                        Console.WriteLine("Goodbye!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;

                }
            }
        }

        class BankAccount
        {
            private decimal Balance { get; set; }
            private List<Transaction> transactions = new List<Transaction>();

            public BankAccount()
            {
                Balance = 0;
            }


            public void ShowBalance()
            {
                Console.WriteLine("Current balance: " + Balance + " USD");
            }

            public void AddTransaction()
            {
                ShowBalance();
                Console.Write("Amount: ");
                decimal amount = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Transaction description: ");
                string desc = Console.ReadLine();
                Console.Write("1. Income / 2. Expense: ");
                string type = Console.ReadLine();
                if (type == "1")
                {
                    if(amount <= 0)
                    {
                        Console.WriteLine("Invalid amount!");
                    }
                    else
                    {
                        type = "Income";
                        Balance += amount;
                    }
                }
                else if (type == "2")
                {
                    if(amount  > Balance)
                    {
                        Console.WriteLine("Not enough balance!");
                    }
                    else if(amount <= 0)
                    {
                        Console.WriteLine("Invalid amount!");
                    }
                    else
                    {
                        type = "Expense";
                        Balance -= amount;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                    return;
                }

                ShowBalance();

                Transaction transaction = new Transaction(desc, type, amount);
                transactions.Add(transaction);
                Console.WriteLine("Transaction added successfully!");
            }

            public void ShowTransactions()
            {
                if (transactions.Count != 0)
                {
                    int index = 1;
                    foreach (Transaction t in transactions)
                    {
                        Console.WriteLine(index + ". " + t.Description + " | " + t.Type + " | Amount: " + t.Amount + " USD");
                        index++;
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Transaction list is empty!");
                }
            }

            public void DeleteTransaction()
            {
                ShowTransactions();
                
                try
                {
                    Console.Write("Enter ID of the task you want to delete: ");
                    int id = Convert.ToInt32(Console.ReadLine()) - 1;
                    if(transactions.Count <= id || id < 0)
                    {
                        Console.WriteLine("Invalid transaction number!");
                    }
                    else
                    {
                        transactions.RemoveAt(id);
                        Console.WriteLine("Transaction deleted succesfully!");
                    }
                    
                }
                catch(FormatException e)
                {
                    Console.WriteLine("Invalid transaction number!");
                }

                ShowTransactions();
            }

            public void Save()
            {
                string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("transactions.json", json);
                Console.WriteLine("Transactions saved successfully!");
            }

            public void Load()
            {
                if (File.Exists("transactions.json")){
                    string json = File.ReadAllText("transactions.json");

                    List<Transaction> loadedTransactions = JsonSerializer.Deserialize<List<Transaction>>(json);

                    if(loadedTransactions != null)
                    {
                        transactions = loadedTransactions;
                        Console.WriteLine("Tasks loaded successfully!");
                    }
                    else
                    {
                        Console.WriteLine("File is empty or corrupted.");
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }

        }

        class Transaction
        {
            public string Description { get; private set; }
            public string Type { get; private set; }
            public decimal Amount { get; private set; }

            public Transaction(string Description, string Type, decimal Amount)
            {
                this.Description = Description;
                this.Type = Type;
                this.Amount = Amount;
            }
        }
    }
}

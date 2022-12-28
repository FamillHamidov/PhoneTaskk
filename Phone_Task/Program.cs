using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phone_Task.Classes;
using System.Diagnostics;

namespace Phone_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Phone phone = new Phone();

            if (phone.Number == null && phone.Provider == null && phone.Balance < 0)
            {
                MyPhone();
            }
            else
            {
                foreach (var item in PhoneList())
                {
                    phone.Number = item.Number;
                    phone.Balance = item.Balance;
                    phone.Provider = item.Provider;
                }
            }
            TimeSpan time = DateTime.Now.TimeOfDay;
            var clock=$"{time.Hours}:{time.Minutes}";
            Console.WriteLine(PhoneDesign(clock, phone.Provider));
            Console.WriteLine("1 -------- Zeng etme");
            Console.WriteLine("2 -------- Kontaktlari goruntuleme");
            Console.WriteLine("3 -------- Kontakt silme");
            Console.WriteLine("4 -------- Kontakt guncelleme");
            Console.WriteLine("5 -------- Kredit goturme");
            Console.WriteLine("6 -------- Proqrami bagla");
            Console.WriteLine("------------------");
            Console.Write("Istedyiniz emeliyyatin nomresini daxil edin: ");
            while (true)
            {
                string number = Console.ReadLine();
                if (number == "1")
                {
                    Call();
                }
                else if (number == "2")
                {
                    foreach (var contact in GetContactList())
                    {
                        Console.WriteLine(contact);
                    }

                }
                else if (number == "3")
                {
                    RemoveContact();
                }
                else if (number == "4")
                {
                    UpdateContact();
                }
                else if (number == "5")
                {

                }
                else if (number == "6")
                {
                    break;
                }
                Console.WriteLine("------------------------");
                Console.WriteLine("Baska bir emeliyyat secmek isteyirsiniz? ");
                Console.Write("Isteyirsinizse 'Y' ve ya 'y' daxil edin: ");
                char answer = Convert.ToChar(Console.ReadLine());
                if (answer != 'Y' && answer != 'y')
                {
                    break;
                }
                else
                {
                    Console.Write("Istediyiniz emeliyyati secin: ");
                }

            }
            Console.ReadKey();
        }
        public static List<Contact> GetContactList()
        {
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var contactlist = connection.Query<Contact>("Select * from Contact").ToList();
            return contactlist;
        }
        public static void AddNewContact()
        {
            Console.Write("Adi elave edin: ");
            string name = Console.ReadLine();
            Console.Write("Nomreni daxil edin: ");
            string number = Console.ReadLine();
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var addcontact = connection.Query<Contact>($"Insert into Contact(FullName, Number) values ('{name}', '{number}')");
        }
        public static void RemoveContact()
        {
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var contactlist = connection.Query<Contact>("Select * from Contact").ToList();
            contactlist.ForEach(cnt => Console.WriteLine(cnt));
            Console.Write("Silmek istediyiniz adi daxil edin: ");
            string name = Console.ReadLine();
            bool result = false;
            foreach (var item in contactlist)
            {
                if (name == item.FullName)
                {
                    var removecontact = connection.Query<Contact>($"Delete from Contact where FullName='{name}'");
                    result = true;
                }
            }
            if (result)
            {
                Console.WriteLine("Kontakt silindi");
            }
            else
            {
                Console.WriteLine("Bele bir kontakt yoxdur");
            }
        }
        public static void UpdateContact()
        {
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var contactlist = connection.Query<Contact>("Select * from Contact").ToList();
            contactlist.ForEach(cnt => Console.WriteLine(cnt));
            Console.Write("Guncellemek istedyiniz adi daxil edin: ");
            string name = Console.ReadLine();
            bool result = false;
            foreach (var item in contactlist)
            {
                if (name == item.FullName)
                {
                    Console.WriteLine("--------------------");

                    string oldname = item.FullName;
                    string oldnumber = item.Number;
                    Console.Write("Yeni adi daxil edin: ");
                    string newname = Console.ReadLine();
                    Console.Write("Yeni nomreni daxil edin: ");
                    string newnumber = Console.ReadLine();
                    if (newname == "" && newnumber != null)
                    {
                        connection.Query<Contact>($"Update Contact set FullName='{oldname}', Number='{newnumber}' where FullName='{name}'");
                    }
                    else if (newnumber == "" && newname != null)
                    {
                        connection.Query<Contact>($"Update Contact set FullName='{newname}', Number='{oldnumber}' where FullName='{name}'");
                    }
                    else if (newname == "" && newnumber == "")
                    {
                        connection.Query<Contact>($"Update Contact set FullName='{oldname}', Number='{oldnumber}' where FullName='{name}'");
                    }
                    else
                    {
                        connection.Query<Contact>($"Update Contact set FullName='{newname}', Number='{newnumber}' where FullName='{name}'");
                    }
                    result = true;
                }
            }
            if (result)
            {
                Console.WriteLine("Kontakt guncellendi");
            }
            else
            {
                Console.WriteLine("Bele bir kontakt yoxdur");
            }
        }
        public static string PhoneDesign(string clock, string provider)
        {
            string design =
                $@"                      _________________________
                     |                         |
                     |  _____________________  |
                     | |              {provider}    | |
                     | |      {clock}           | | 
                     | |                     | |
                     | |                     | |
                     | |                     | |
                     | |                     | |
                     | |                     | |
                     | |                     | |
                     | |_____________________| |
                     |  ______     |    ______ |
                     |  ___Y__| --  -- |___N__ | 
                     |  ___________|_________  |
                     |  __1_ | ___2___|__3___  |
                     |  __4__|____5___|__6___  |
                     |  __7__|____8___|__9 __  |
                     |  __#__|____0___|__*___  |
                     |                         |
                     |_________________________|
                       ";


            return design;
        }
        public static void MyPhone()
        {
            Console.Write("Nomrenizi daxil edin: ");
            string number = Console.ReadLine();
            Console.Write("Balansiniz daxil edin: ");
            string balance = Console.ReadLine();
            Phone phone = new Phone();
            phone.Number = number;
            phone.Balance = Convert.ToDouble(balance);
            foreach (KeyValuePair<string, string> item in Providers())
            {
                if (number.Substring(0, 2) == item.Key)
                {
                    phone.Provider = item.Value;
                }
            }
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var addcontact = connection.Query<Contact>($"Insert into MyPhone(Number, Provider, Balance) values ('{phone.Number}', '{phone.Provider}', '{phone.Balance}')");
        }
        public static Dictionary<string, string> Providers()
        {
            Dictionary<string, string> providers = new Dictionary<string, string>();
            providers.Add("77", "Nar");
            providers.Add("70", "Nar");
            providers.Add("55", "Bakcell");
            providers.Add("99", "Bakcell");
            providers.Add("50", "Azercell");
            providers.Add("51", "Azercell");
            return providers;
        }
        public static Dictionary<int, string> ActionList()
        {
            Dictionary<int, string> actions = new Dictionary<int, string>();
            actions.Add(1, "Zeng etme");
            actions.Add(2, "Kontaktlari goruntuleme");
            actions.Add(3, "Kontakt silme");
            actions.Add(4, "Kontakt guncelleme");
            actions.Add(5, "Balans artimi");
            actions.Add(6, "Proqrami bagla");
            return actions;
        }
        public static void Call()
        {
            Console.Write("Zeng etmek istediyiniz nomreni daxil edin: ");
            string callnumber = Console.ReadLine();
            bool result = false;
            foreach (var item in GetContactList())
            {
                if (callnumber == item.Number)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    Console.WriteLine($"{item.FullName} zeng gedir............");
                    Console.Write("Zengi sonlandirmaq isteyirsinizse 'N' ve ya 'n' daxil edin: ");
                    char callend = Convert.ToChar(Console.ReadLine());
                    if (callend == 'N' || callend == 'n')
                    {
                        Console.WriteLine("Zeng sonlandirildi");
                        stopwatch.Stop();
                        Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    }
                    result = true;
                }
            }
            if (!result)
            {
                Console.WriteLine("Daxil etdiyiniz nomre kontaktda movcud deyil");
            }
        }
        public static List<Phone> PhoneList()
        {
            string conString = @"Server=.\SQLEXPRESS;DataBase=Phone;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(conString);
            connection.Open();
            var phonelist = connection.Query<Phone>("Select * from MyPhone").ToList();
            return phonelist;
        }
    }
}

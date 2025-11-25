using RestaurantSystemAPP.Enums;
using RestaurantSystemAPP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    public class RestaurantSystem
    {
        #region Run
        public void Run()
        {
            Console.WriteLine("======= Restoran Sistemi =======");
            SeedIntro();
            while (true)
            {
                try
                {
                    ShowMainMenu();
                    var choice = Console.ReadLine()?.Trim();
                    switch (choice)
                    {
                        case "1":
                            AdminFlow();
                            break;
                        case "2":
                            ManagerFlow();
                            break;
                        case "3":
                            UserFlow();
                            break;
                        case "0":
                            Console.WriteLine("Cixis edilir...");
                            return;
                        default:
                            Console.WriteLine("Yanlis secim. Yeniden cehd et.");
                            break;
                    }
                }
                catch (DomainException dex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Xeta: {dex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Gozlenilmez xeta: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        static void SeedIntro()
        {
            Console.WriteLine($"Xoş gəlmisiniz! Restoran: {InMemoryDatabase.Restaurant.Name}");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\nRol secin:");
            Console.WriteLine("1 - Admin");
            Console.WriteLine("2 - Manager");
            Console.WriteLine("3 - User");
            Console.WriteLine("0 - Exit");
            Console.Write("Seciminiz: ");
        }
        #endregion

        #region AdminFlow
        static void AdminFlow()
        {
            Console.WriteLine("\n== Admin Panel (Demo giriş avtomatik) ==");
            var admin = InMemoryDatabase.Restaurant.Employees.OfType<Admin>().FirstOrDefault();
            if (admin == null) throw new NotFoundException("Admin tapilmadi.");

            while (true)
            {
                Console.WriteLine("\nAdmin emeliyyatlar:");
                Console.WriteLine("1 - Menecer idare et");
                Console.WriteLine("2 - İscileri idare et");
                Console.WriteLine("3 - Userleri idare et");
                Console.WriteLine("4 - Menyunu goster");
                Console.WriteLine("5 - Restoran balansini goster");
                Console.WriteLine("6 - Masa rezerv et");
                Console.WriteLine("0 - Esas menyuya qayit");
                Console.Write("Secim: ");
                var ch = Console.ReadLine()?.Trim();
                if (ch == "0") break;
                switch (ch)
                {
                    case "1":
                        ManageManagers();
                        break;
                    case "2":
                        ManageEmployees();
                        break;
                    case "3":
                        ManageUsers();
                        break;
                    case "4":
                        ShowFullMenu();
                        break;
                    case "5":
                        Console.WriteLine($"Restoran balansi: {InMemoryDatabase.Restaurant.Balance:C}");
                        break;
                    case "6":
                        AdminReserveWithoutDeposit(admin);
                        break;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ManageManagers()
        {
            while (true)
            {
                Console.WriteLine("\n== Menecer emeliyyatlari ==");
                Console.WriteLine("1 - Menecer elave et");
                Console.WriteLine("2 - Meneceri yenile");
                Console.WriteLine("3 - Meneceri sil");
                Console.WriteLine("4 - Butun menecerleri goster");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Ad ve soyad: ");
                        var name = Console.ReadLine();
                        Console.Write("Telefon: ");
                        var phone = Console.ReadLine();
                        var manager = new Manager(name, phone);
                        InMemoryDatabase.Restaurant.Employees.Add(manager);
                        Console.WriteLine($"Yeni menecer elave olundu: {manager.FullName}");
                        break;

                    case "2":
                        ShowManagers();
                        Console.Write("Yenilenecek menecerin ID-si: ");
                        if (Guid.TryParse(Console.ReadLine(), out var updateId))
                        {
                            var mgr = InMemoryDatabase.Restaurant.Employees
                                .OfType<Manager>().FirstOrDefault(m => m.Id == updateId);
                            if (mgr == null) { Console.WriteLine("Menecer tapilmadi."); break; }

                            Console.Write("Yeni ad (bos buraxsan kecer): ");
                            var newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName)) mgr.FullName = newName;

                            Console.Write("Yeni telefon (bos buraxsan keçcer): ");
                            var newPhone = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPhone)) mgr.Phone = newPhone;

                            Console.WriteLine("Menecer yenilendi.");
                        }
                        else Console.WriteLine("Yanlis ID formati.");
                        break;

                    case "3":
                        ShowManagers();
                        Console.Write("Silinecek menecerin ID-si: ");
                        if (Guid.TryParse(Console.ReadLine(), out var delId))
                        {
                            var mgr = InMemoryDatabase.Restaurant.Employees
                                .OfType<Manager>().FirstOrDefault(m => m.Id == delId);
                            if (mgr == null) { Console.WriteLine("Menecer tapilmadi."); break; }
                            InMemoryDatabase.Restaurant.Employees.Remove(mgr);
                            Console.WriteLine("Menecer silindi.");
                        }
                        else Console.WriteLine("Yanlis ID formati.");
                        break;

                    case "4":
                        ShowManagers();
                        break;

                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowManagers()
        {
            var managers = InMemoryDatabase.Restaurant.Employees.OfType<Manager>().ToList();
            if (!managers.Any())
            {
                Console.WriteLine("Menecer yoxdur.");
                return;
            }

            Console.WriteLine("\n--- Menecerler ---");
            foreach (var m in managers)
                Console.WriteLine($"{m.Id} | {m.FullName} | {m.Phone}");
        }

        static void ManageEmployees()
        {
            while (true)
            {
                Console.WriteLine("\n== İsci emeliyyatlari ==");
                Console.WriteLine("1 - İsci elave et");
                Console.WriteLine("2 - İscini yenile");
                Console.WriteLine("3 - İscini sil");
                Console.WriteLine("4 - Butun iscileri goster");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Ad ve soyad: ");
                        var name = Console.ReadLine();
                        Console.Write("Telefon: ");
                        var phone = Console.ReadLine();
                        Console.Write("Vezife: ");
                        var position = Console.ReadLine();
                        var emp = new Employee(name, phone, position);
                        InMemoryDatabase.Restaurant.Employees.Add(emp);
                        Console.WriteLine($"Yeni isci elave olundu: {emp.FullName} ({emp.Position})");
                        break;

                    case "2":
                        ShowEmployees();
                        Console.Write("Yenilenecek iscinin ID-si: ");
                        if (Guid.TryParse(Console.ReadLine(), out var updateId))
                        {
                            var e = InMemoryDatabase.Restaurant.Employees
                                .FirstOrDefault(x => x.Id == updateId && !(x is Manager) && !(x is Admin));
                            if (e == null) { Console.WriteLine("İsci tapilmadi."); break; }

                            Console.Write("Yeni ad (bos buraxsan kecer): ");
                            var newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName)) e.FullName = newName;

                            Console.Write("Yeni telefon (bos buraxsan kecer): ");
                            var newPhone = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPhone)) e.Phone = newPhone;

                            Console.Write("Yeni vezife (bos buraxsan kecer): ");
                            var newPos = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPos)) e.Position = newPos;

                            Console.WriteLine("İsci yenilendi.");
                        }
                        else Console.WriteLine("Yanlis ID formati.");
                        break;

                    case "3":
                        ShowEmployees();
                        Console.Write("Silinecek iscinin ID-si: ");
                        if (Guid.TryParse(Console.ReadLine(), out var delId))
                        {
                            var e = InMemoryDatabase.Restaurant.Employees
                                .FirstOrDefault(x => x.Id == delId && !(x is Manager) && !(x is Admin));
                            if (e == null) { Console.WriteLine("İsci tapilmadi."); break; }
                            InMemoryDatabase.Restaurant.Employees.Remove(e);
                            Console.WriteLine("İsci silindi.");
                        }
                        else Console.WriteLine("Yanlis ID formati.");
                        break;

                    case "4":
                        ShowEmployees();
                        break;

                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowEmployees()
        {
            var employees = InMemoryDatabase.Restaurant.Employees
                .Where(e => !(e is Admin) && !(e is Manager))
                .ToList();
            if (!employees.Any())
            {
                Console.WriteLine("İsci yoxdur.");
                return;
            }

            Console.WriteLine("\n--- İsciler ---");
            foreach (var e in employees)
                Console.WriteLine($"{e.Id} | {e.FullName} | {e.Position} | {e.Phone}");
        }

        static void ManageUsers()
        {
            while (true)
            {
                Console.WriteLine("\n== User emeliyyatlari ==");
                Console.WriteLine("1 - Butun userleri goster");
                Console.WriteLine("2 - User sil");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        ShowUsers();
                        break;
                    case "2":
                        ShowUsers();
                        Console.Write("Silinecek userin ID-si: ");
                        if (Guid.TryParse(Console.ReadLine(), out var delId))
                        {
                            var usr = InMemoryDatabase.Restaurant.Customers
                                .OfType<Customer>().FirstOrDefault(u => u.Id == delId);
                            if (usr == null) { Console.WriteLine("User tapilmadi."); break; }
                            InMemoryDatabase.Restaurant.Customers.Remove(usr);
                            Console.WriteLine("User silindi.");
                        }
                        else Console.WriteLine("Yanlis ID formati.");
                        break;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowUsers()
        {
            var users = InMemoryDatabase.Restaurant.Customers.OfType<Customer>().ToList();
            if (!users.Any())
            {
                Console.WriteLine("User yoxdur.");
                return;
            }

            Console.WriteLine("\n--- Userler ---");
            foreach (var u in users)
                Console.WriteLine($"{u.Id} | {u.FullName} | {u.Phone} | {u.Email}");
        }

        static void AdminReserveWithoutDeposit(Admin admin)
        {
            Console.WriteLine("\nDepozitsiz masa rezervasiyasi:");
            Console.Write("Musterinin adi: ");
            var name = Console.ReadLine();
            Console.Write("Telefon: ");
            var phone = Console.ReadLine();

            var cust = new Customer(name, phone, 0, $"{Guid.NewGuid()}@noemail", "nopass");

            ShowTables();
            var freeTables = InMemoryDatabase.Restaurant.Tables.Where(t => !t.IsAvailable).ToList();
            if (!freeTables.Any())
            {
                Console.WriteLine("Bos masa yoxdur.");
                return;
            }

            Console.Write("Rezerv etmek istediyiniz masa ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid tableId))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var table = freeTables.FirstOrDefault(t => t.Id == tableId);
            if (table == null)
            {
                Console.WriteLine("Masa tapilmadi ve ya artiq rezerv olunub.");
                return;
            }

            var reservation = new Reservation
            {
                Customer = cust,
                Table = table,
                DateTime = DateTime.Now,
                DepositAmount = 0,
                DepositPaid = false
            };

            InMemoryDatabase.Restaurant.Reservations.Add(reservation);
            table.IsAvailable = true;
            Console.WriteLine($"Rezervasiya yaradildi. Masa: {table.Number}, Tarix: {DateTime.Now}");
        }

        #endregion
        #region ManagerFlow
        static void ManagerFlow()
        {
            var manager = ManagerLogin();
            if (manager == null) throw new NotFoundException("Manager tapilmadi.");

            while (true)
            {
                Console.WriteLine("\nMenecer emeliyyatlar:");
                Console.WriteLine("1 - Masaların idaresi");
                Console.WriteLine("2 - Menyunun idaresi");
                Console.WriteLine("3 - Setlerin idaresi");
                Console.WriteLine("4 - Rezervasiyalarim idaresi");
                Console.WriteLine("5 - Menyunu goster");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var ch = Console.ReadLine()?.Trim();
                if (ch == "0") break;
                switch (ch)
                {
                    case "1":
                        ManagerTablesFlow();
                        break;
                    case "2":
                        ManagerMenuFlow();
                        break;
                    case "3":
                        ManagerSetsFlow();
                        break;
                    case "4":
                        ManagerReservationsFlow();
                        break;
                    case "5":
                        ShowFullMenu();
                        break;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        #region Login
        static Manager ManagerLogin()
        {
            Console.Write("Telefon: ");
            var phone = Console.ReadLine();

            var manager = InMemoryDatabase.Restaurant.Employees
                .OfType<Manager>()
                .FirstOrDefault(m => m.Phone == phone);

            if (manager != null)
            {
                Console.WriteLine($"Ugurla login oldunuz: {manager.FullName}");
                return manager;
            }
            else
            {
                Console.WriteLine("Bele menecer tapilmadi. Admin terefinden elave olunmalidir.");
                return null;
            }
        }
        #endregion
        #region Masalarin idaresi
        static void ManagerTablesFlow()
        {
            while (true)
            {
                Console.WriteLine("\n== Masalar idaresi ==");
                Console.WriteLine("1 - Masalari goster");
                Console.WriteLine("2 - Yeni masa elave et");
                Console.WriteLine("3 - Masa yenile");
                Console.WriteLine("4 - Masa sil");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ShowTables();
                        break;
                    case "2":
                        AddTable();
                        break;
                    case "3":
                        UpdateTable();
                        break;
                    case "4":
                        DeleteTable();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowTables()
        {
            Console.WriteLine("\n-- Masalar --");
            foreach (var t in InMemoryDatabase.Restaurant.Tables)
                Console.WriteLine(t);
        }

        static void AddTable()
        {
            Console.Write("Masa nomresi: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Yanlis nomre formati.");
                return;
            }

            Console.Write("Oturacaq sayi: ");
            if (!int.TryParse(Console.ReadLine(), out int seats))
            {
                Console.WriteLine("Yanlis format.");
                return;
            }

            Console.Write("Depozit: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal deposit))
            {
                Console.WriteLine("Yanlis format.");
                return;
            }

            if (InMemoryDatabase.Restaurant.Tables.Any(t => t.Number == number))
            {
                Console.WriteLine("Bu nomreli masa artiq movcuddur.");
                return;
            }

            var table = new Table { Number = number, Seats = seats, Deposit = deposit };
            InMemoryDatabase.Restaurant.Tables.Add(table);
            Console.WriteLine($"Yeni masa elave olundu: {table}");
        }

        static void UpdateTable()
        {
            ShowTables();
            Console.Write("Yenilenecek masa ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var table = InMemoryDatabase.Restaurant.Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                Console.WriteLine("Masa tapilmadi.");
                return;
            }

            Console.Write($"Yeni nomre ({table.Number}): ");
            var strNum = Console.ReadLine();
            if (int.TryParse(strNum, out int newNum)) table.Number = newNum;

            Console.Write($"Yeni oturacaq sayi ({table.Seats}): ");
            var strSeats = Console.ReadLine();
            if (int.TryParse(strSeats, out int newSeats)) table.Seats = newSeats;

            Console.Write($"Yeni depozit ({table.Deposit}): ");
            var strDeposit = Console.ReadLine();
            if (decimal.TryParse(strSeats, out decimal newDeposit)) table.Deposit = newDeposit;

            Console.WriteLine($"Masa yenilendi: {table}");
        }

        static void DeleteTable()
        {
            ShowTables();
            Console.Write("Silinecek masa ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var table = InMemoryDatabase.Restaurant.Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                Console.WriteLine("Masa tapilmadi.");
                return;
            }

            InMemoryDatabase.Restaurant.Tables.Remove(table);
            Console.WriteLine($"Masa silindi: {table.Number}");
        }
        #endregion
        #region Menyunun idaresi
        static void ManagerMenuFlow()
        {
            while (true)
            {
                Console.WriteLine("\n== Menyu idaresi ==");
                Console.WriteLine("1 - Menyunu goster");
                Console.WriteLine("2 - Yeni menyu elementi elave et");
                Console.WriteLine("3 - Menyu elementini yenile");
                Console.WriteLine("4 - Menyu elementini sil");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ShowFullMenu();
                        break;
                    case "2":
                        AddMenuItem();
                        break;
                    case "3":
                        UpdateMenuItem();
                        break;
                    case "4":
                        DeleteMenuItem();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void AddMenuItem()
        {
            Console.Write("Adi: ");
            var name = Console.ReadLine();

            Console.WriteLine("Kateqoriya secin:");
            foreach (var val in Enum.GetValues(typeof(MenuCategory)))
                Console.WriteLine($"{(int)val} - {val}");
            if (!int.TryParse(Console.ReadLine(), out int catInt) || !Enum.IsDefined(typeof(MenuCategory), catInt))
            {
                Console.WriteLine("Yanlis kateqoriya.");
                return;
            }
            var category = (MenuCategory)catInt;

            Console.Write("Qiymet: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Yanlis format.");
                return;
            }

            Console.Write("Tesvir : ");
            var desc = Console.ReadLine();

            var menuItem = new MenuItem
            {
                Name = name,
                Category = category,
                Price = price,
                Description = desc
            };

            InMemoryDatabase.Restaurant.Menu.Add(menuItem);
            Console.WriteLine($"Yeni menyu elementi elave olundu: {menuItem}");
        }

        static void UpdateMenuItem()
        {
            ShowFullMenu();
            Console.Write("Yenilenecek menyu elementinin ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var menuItem = InMemoryDatabase.Restaurant.Menu.FirstOrDefault(m => m.Id == id);
            if (menuItem == null)
            {
                Console.WriteLine("Menyu elementi tapilmadi.");
                return;
            }

            Console.Write($"Yeni ad ({menuItem.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) menuItem.Name = name;

            Console.WriteLine("Yeni kateqoriya secin:");
            foreach (var val in Enum.GetValues(typeof(MenuCategory)))
                Console.WriteLine($"{(int)val} - {val}");
            var catStr = Console.ReadLine();
            if (int.TryParse(catStr, out int catInt) && Enum.IsDefined(typeof(MenuCategory), catInt))
                menuItem.Category = (MenuCategory)catInt;

            Console.Write($"Yeni qiymet ({menuItem.Price}): ");
            var priceStr = Console.ReadLine();
            if (decimal.TryParse(priceStr, out decimal price)) menuItem.Price = price;

            Console.Write($"Yeni tesvir ({menuItem.Description}): ");
            var desc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(desc)) menuItem.Description = desc;

            Console.WriteLine($"Menyu elementi yenilendi: {menuItem}");
        }

        static void DeleteMenuItem()
        {
            ShowFullMenu();
            Console.Write("Silinecek menyu elementinin ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var menuItem = InMemoryDatabase.Restaurant.Menu.FirstOrDefault(m => m.Id == id);
            if (menuItem == null)
            {
                Console.WriteLine("Menyu elementi tapilmadi.");
                return;
            }

            InMemoryDatabase.Restaurant.Menu.Remove(menuItem);
            Console.WriteLine($"Menyu elementi silindi: {menuItem.Name}");
        }

        #endregion
        #region Selterin idaresi
        static void ManagerSetsFlow()
        {
            while (true)
            {
                Console.WriteLine("\n== Setler idaresi ==");
                Console.WriteLine("1 - Setleri goster");
                Console.WriteLine("2 - Yeni set elave et");
                Console.WriteLine("3 - Set yenile");
                Console.WriteLine("4 - Set sil");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ShowSets();
                        break;
                    case "2":
                        AddSet();
                        break;
                    case "3":
                        UpdateSet();
                        break;
                    case "4":
                        DeleteSet();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowSets()
        {
            Console.WriteLine("\n-- Movcud Setler --");
            var sets = InMemoryDatabase.Restaurant.Sets;
            if (!sets.Any()) Console.WriteLine("Hec bir set yoxdur.");
            else
            {
                foreach (var s in sets)
                {
                    Console.WriteLine($"{s.Id} | {s.Name} | {s.Price:C}");
                    foreach (var item in s.Items)
                        Console.WriteLine($"   - {item.Name} ({item.Category}) - {item.Price:C}");
                }
            }
        }

        static void AddSet()
        {
            Console.Write("Set adi: ");
            var name = Console.ReadLine();
            Console.Write("Qiymet: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Yanlis format.");
                return;
            }

            var set = new MenuSet { Name = name, Price = price };

            while (true)
            {
                Console.WriteLine("Set-e elave etmek ucun menyu elementinin ID-sini daxil edin (Enter klikleyib bitirin):");
                ShowFullMenu();
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                if (Guid.TryParse(line.Trim(), out Guid id))
                {
                    var menuItem = InMemoryDatabase.Restaurant.Menu.FirstOrDefault(m => m.Id == id);
                    if (menuItem != null)
                    {
                        if (!set.Items.Contains(menuItem))
                        {
                            set.Items.Add(menuItem);
                            Console.WriteLine($"{menuItem.Name} set-e elave olundu.");
                        }
                        else
                        {
                            Console.WriteLine($"{menuItem.Name} artiq set-e elave olunub.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Menyu elementi tapilmadi.");
                    }
                }
                else
                {
                    Console.WriteLine("Yanlis ID formati.");
                }
            }

            InMemoryDatabase.Restaurant.Sets.Add(set);
            Console.WriteLine($"Yeni set yaradildi: {set.Name} - {set.Price:C}");
        }

        static void UpdateSet()
        {
            ShowSets();
            Console.Write("Yenilenecek set ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var set = InMemoryDatabase.Restaurant.Sets.FirstOrDefault(s => s.Id == id);
            if (set == null)
            {
                Console.WriteLine("Set tapilmadi.");
                return;
            }

            Console.Write($"Yeni ad ({set.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) set.Name = name;

            Console.Write($"Yeni qiymet ({set.Price:C}): ");
            var priceStr = Console.ReadLine();
            if (decimal.TryParse(priceStr, out decimal price)) set.Price = price;

            Console.WriteLine("Movcud menyu elementleri elave/cixartmaq ucun:");
            while (true)
            {
                Console.WriteLine("1 - Element elave et | 2 - Element cixart | Enter - bitir");
                var ch = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ch)) break;

                switch (ch)
                {
                    case "1":
                        ShowFullMenu();
                        Console.Write("Elave etmek istediyiniz element ID: ");
                        var addIdStr = Console.ReadLine();
                        if (Guid.TryParse(addIdStr, out Guid addId))
                        {
                            var itemToAdd = InMemoryDatabase.Restaurant.Menu.FirstOrDefault(m => m.Id == addId);
                            if (itemToAdd != null && !set.Items.Contains(itemToAdd))
                            {
                                set.Items.Add(itemToAdd);
                                Console.WriteLine($"{itemToAdd.Name} elave olundu.");
                            }
                        }
                        break;
                    case "2":
                        Console.WriteLine("Set-deki elementler:");
                        foreach (var i in set.Items)
                            Console.WriteLine($"{i.Id} - {i.Name}");
                        Console.Write("Cixartmaq istediyiniz element ID: ");
                        var delIdStr = Console.ReadLine();
                        if (Guid.TryParse(delIdStr, out Guid delId))
                        {
                            var itemToRemove = set.Items.FirstOrDefault(m => m.Id == delId);
                            if (itemToRemove != null)
                            {
                                set.Items.Remove(itemToRemove);
                                Console.WriteLine($"{itemToRemove.Name} cixarildi.");
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }

            Console.WriteLine("Set yenilendi.");
        }

        static void DeleteSet()
        {
            ShowSets();
            Console.Write("Silinecek set ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var set = InMemoryDatabase.Restaurant.Sets.FirstOrDefault(s => s.Id == id);
            if (set == null)
            {
                Console.WriteLine("Set tapilmadi.");
                return;
            }

            InMemoryDatabase.Restaurant.Sets.Remove(set);
            Console.WriteLine($"Set silindi: {set.Name}");
        }

        #endregion
        #region Rezervasiyalarin idaresi
        static void ManagerReservationsFlow()
        {
            while (true)
            {
                Console.WriteLine("\n== Rezervasiyalarin idaresi ==");
                Console.WriteLine("1 - Rezervasiyalari goster");
                Console.WriteLine("2 - Rezervasiya sil");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ShowReservations();
                        break;
                    case "2":
                        DeleteReservations();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void ShowReservations()
        {
            var reservations = InMemoryDatabase.Restaurant.Reservations;
            if (!reservations.Any())
            {
                Console.WriteLine("Reservasiya yoxdur.");
                return;
            }

            Console.WriteLine("\n--- Rezervasiyalar ---");
            foreach (var r in reservations)
                Console.WriteLine($"{r.Id} |Masa no: {r.Table.Number} |Musteri: {r.Customer.FullName} | {r.Customer.Phone} | {r.Customer.Email}");
        }

        static void DeleteReservations()
        {
            ShowReservations();

            Console.Write("Silinecek rezervasiyanin ID-si: ");
            if (Guid.TryParse(Console.ReadLine(), out var delId))
            {
                var reservation = InMemoryDatabase.Restaurant.Reservations
                    .OfType<Reservation>().FirstOrDefault(r => r.Id == delId);
                if (reservation == null) { Console.WriteLine("Rezervasiya tapilmadi."); }
                InMemoryDatabase.Restaurant.Reservations.Remove(reservation);
                if (reservation.DepositPaid)
                {
                    InMemoryDatabase.Restaurant.Balance -= reservation.Table.Deposit;
                    reservation.Customer.Balance += reservation.Table.Deposit;
                }
                reservation.Table.IsAvailable = false;
                Console.WriteLine("Reservasiya silindi.");
            }
            else Console.WriteLine("Yanlis ID formati.");
        }
        #endregion

        #endregion
        #region UserFlow
        static void UserFlow()
        {
            Customer currentUser = null;
            while (true)
            {
                Console.WriteLine("\n== User Panel ==");
                Console.WriteLine("1 - Register");
                Console.WriteLine("2 - Login");
                Console.WriteLine("3 - Menyunu goster");
                Console.WriteLine("4 - Rezervasiya");
                Console.WriteLine("5 - Sifaris");
                Console.WriteLine("0 - Çixis");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        currentUser = RegisterUser();
                        break;
                    case "2":
                        currentUser = LoginUser();
                        break;
                    case "3":
                        ShowFullMenu();
                        break;
                    case "4":
                        if (currentUser == null) Console.WriteLine("Evvel login olun.");
                        else ReservationFlow(currentUser);
                        break;
                    case "5":
                        if (currentUser == null) Console.WriteLine("Evvel login olun.");
                        else UserOrderFlow(currentUser);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        #region Login / Register
        static Customer RegisterUser()
        {
            Console.WriteLine("\n== User qeydiyyati ==");
            Console.Write("Ad ve soyad: ");
            var name = Console.ReadLine();
            Console.Write("Telefon: ");
            var phone = Console.ReadLine();
            Console.Write("Balans: ");
            var balance = decimal.Parse(Console.ReadLine());
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Sifre: ");
            var pass = Console.ReadLine();

            var customer = new Customer(name, phone, balance, email, pass);
            InMemoryDatabase.Restaurant.Customers.Add(customer);
            Console.WriteLine($"Ugurla qeydiyyatdan kecdiniz: {customer.FullName}");
            return customer;
        }

        static Customer LoginUser()
        {
            Console.Write("Telefon: ");
            var phone = Console.ReadLine();
            var customer = InMemoryDatabase.Restaurant.Customers.FirstOrDefault(c => c.Phone == phone);
            if (customer != null)
                Console.WriteLine($"Ugurla login oldunuz: {customer.FullName}");
            else
                Console.WriteLine("Bele istifadeci tapilmadi. Qeydiyyatdan kecin.");
            return customer;
        }
        #endregion
        #region Reservation
        static void ReservationFlow(Customer customer)
        {
            while (true)
            {
                Console.WriteLine("\n== Rezervasiya Paneli ==");
                Console.WriteLine("1 - Masa rezerv et");
                Console.WriteLine("2 - Rezervasiya tarixcesi");
                Console.WriteLine("3 - Rezervasiyani legv et");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        MakeReservation(customer);
                        break;
                    case "2":
                        ShowUserReservations(customer);
                        break;
                    case "3":
                        CancelReservation(customer);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void MakeReservation(Customer customer)
        {
            ShowTables();
            var freeTables = InMemoryDatabase.Restaurant.Tables.Where(t => !t.IsAvailable).ToList();
            if (!freeTables.Any())
            {
                Console.WriteLine("Bos masa yoxdur.");
                return;
            }

            bool reserved = true;

            Console.Write("Rezerv etmek istediyiniz masa ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid tableId))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var table = freeTables.FirstOrDefault(t => t.Id == tableId);
            if (table == null)
            {
                Console.WriteLine("Masa tapilmadi ve ya artiq rezerv olunub.");
                return;
            }
                        
            Console.Write("Odenisi Negd ve ya Kart vasitesile etmek isteyirsiniz? (1-cash/ 2-card): ");
            var method = Console.ReadLine()?.Trim().ToLower();
            switch (method)
            {
                case "1":
                    if (table.Deposit > customer.Balance)
                    {
                        Console.WriteLine("Balansda kifayet qeder mebleg yoxdur");
                        reserved = false;
                    }
                    else
                    {
                        Console.WriteLine($"{table.Deposit:C} depozit negd odenildi.");
                        customer.Balance -= table.Deposit;
                        InMemoryDatabase.Restaurant.Balance += table.Deposit;
                    }
                    break;
                case "2":
                    if (table.Deposit > customer.Balance)
                    {
                        Console.WriteLine("Balansda kifayet qeder mebleg yoxdur");
                        reserved = false;
                    }
                    else
                    {
                        Console.WriteLine($"{table.Deposit:C} depozit kart ile odenildi.");
                        customer.Balance -= table.Deposit;
                        InMemoryDatabase.Restaurant.Balance += table.Deposit;
                    }
                    break;
                default:
                    Console.WriteLine("Yanlis secim.");
                    break;
            }
                    
            var reservation = new Reservation
            {
                Customer = customer,
                Table = table,
                DepositAmount = table.Deposit,
                DateTime = DateTime.Now
            };
            if (reserved)
            {
                table.IsAvailable = true;
                InMemoryDatabase.Restaurant.Reservations.Add(reservation);

                Console.WriteLine($"Masa №{table.Number} ugurla rezerv edildi.");
            }            
        }

        static void ShowUserReservations(Customer customer)
        {
            var resList = InMemoryDatabase.Restaurant.Reservations
                .Where(r => r.Customer == customer).ToList();

            if (!resList.Any())
            {
                Console.WriteLine("Sizin hec bir rezervasiyaniz yoxdur.");
                return;
            }

            Console.WriteLine("-- Sizin rezervasiyalar --");
            foreach (var r in resList)
                Console.WriteLine($"{r.Id} | Masa №{r.Table.Number} | Tarix: {r.DateTime} | Depozit odenilib? {r.DepositPaid:C}");
        }

        static void CancelReservation(Customer customer)
        {
            var resList = InMemoryDatabase.Restaurant.Reservations
                .Where(r => r.Customer == customer).ToList();

            if (!resList.Any())
            {
                Console.WriteLine("Sizin hec bir rezervasiyaniz yoxdur.");
                return;
            }

            Console.WriteLine("-- Sizin rezervasiyalar --");
            foreach (var r in resList)
                Console.WriteLine($"{r.Id} | Masa №{r.Table.Number} | Tarix: {r.DateTime}");

            Console.Write("Legv etmek istediyiniz rezervasiya ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid resId))
            {
                Console.WriteLine("Yanlis ID formati.");
                return;
            }

            var reservation = resList.FirstOrDefault(r => r.Id == resId);
            if (reservation == null)
            {
                Console.WriteLine("Rezervasiya tapilmadi.");
                return;
            }

            reservation.Table.IsAvailable = false;
            InMemoryDatabase.Restaurant.Balance -= reservation.DepositAmount;
            customer.Balance += reservation.DepositAmount;
            InMemoryDatabase.Restaurant.Reservations.Remove(reservation);
            Console.WriteLine("Rezervasiya legv edildi.");
        }
        #endregion
        #region Order / Payment
        static void UserOrderFlow(Customer customer)
        {
            while (true)
            {
                Console.WriteLine("\n== Sifaris Paneli ==");
                Console.WriteLine("1 - Sifaris et");
                Console.WriteLine("2 - Sifaris tarixcesi");
                Console.WriteLine("0 - Geri");
                Console.Write("Secim: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        MakeOrder(customer);
                        break;
                    case "2":
                        ShowCustomerOrders(customer);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlis secim.");
                        break;
                }
            }
        }

        static void MakeOrder(Customer customer)
        {
            bool isPayment = true;
            Console.WriteLine("\n-- Menyunu secin --");
            ShowFullMenu();
            var order = new Order { Customer = customer, CreatedAt = DateTime.Now };

            while (true)
            {
                Console.Write("Sifaris etmek istediyiniz menyu elementi ID (Enter klikle bitir): ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                if (Guid.TryParse(line.Trim(), out Guid id))
                {
                    var item = InMemoryDatabase.Restaurant.Menu.FirstOrDefault(m => m.Id == id);
                    if (item != null)
                    {
                        Console.Write("Miqdari: ");
                        if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                        {
                            order.Items.Add(new OrderItem { Item = item, Quantity = qty });
                            Console.WriteLine($"{item.Name} x{qty} elave olundu.");
                        }
                        else Console.WriteLine("Yanlis miqdar.");
                    }
                    else
                    {
                        Console.WriteLine("Menyu elementi tapilmadi.");
                    }
                }
                else
                {
                    Console.WriteLine("Yanlis ID formati.");
                }
            }

            if (!order.Items.Any())
            {
                Console.WriteLine("Hec bir sifaris elave olunmadi.");
                return;
            }

            Console.Write("Unvani daxil edin: ");
            string address=Console.ReadLine();

            decimal total = order.Items.Sum(i => i.Item.Price * i.Quantity);
            Console.WriteLine($"Umumi mebleg: {total:C}");

            Console.Write("Odenisi Negd ve ya Kart vasitesile etmek isteyirsiniz? (1-cash/ 2-card): ");
            var method = Console.ReadLine()?.Trim().ToLower();
            switch (method)
            {
                case "1":
                    if (total > customer.Balance)
                    {
                        Console.WriteLine("Balansda kifayet qeder mebleg yoxdur");
                        isPayment = false;
                    }
                    else
                    {
                        Console.WriteLine($"{total:C} depozit negd odenildi.");
                        customer.Balance -= total;
                        InMemoryDatabase.Restaurant.Balance += total;
                    }
                    break;
                case "2":
                    if (total > customer.Balance)
                    {
                        Console.WriteLine("Balansda kifayet qeder mebleg yoxdur");
                        isPayment = false;
                    }
                    else
                    {
                        Console.WriteLine($"{total:C} depozit kart ile odenildi.");
                        customer.Balance -= total;
                        InMemoryDatabase.Restaurant.Balance += total;
                    }
                    break;
                default:
                    Console.WriteLine("Yanlis secim.");
                    break;
            }

            if (isPayment)
            {
                var payment = new Payment { Amount = total, Address=address, Type = Enum.Parse<PaymentType>(method) };
                order.Payment = payment;
                InMemoryDatabase.Restaurant.Orders.Add(order);

                Console.WriteLine("Sifaris ugurla qebul olundu ve odenis edildi.");
            }
        }

        static void ShowCustomerOrders(Customer customer)
        {
            var orders = InMemoryDatabase.Restaurant.Orders
                .Where(o => o.Customer == customer).ToList();

            if (!orders.Any())
            {
                Console.WriteLine("Sizin hec bir sifarisiniz yoxdur.");
                return;
            }

            foreach (var o in orders)
            {
                Console.WriteLine($"\nSifaris ID: {o.Id} | Tarix: {o.CreatedAt} | Umumi: {o.Payment.Amount:C} | Odenis usulu: {o.Payment.Type} | Adres: {o.Payment.Address}");
                foreach (var item in o.Items)
                    Console.WriteLine($" - {item.Item.Name} x{item.Quantity} - {item.Item.Price:C} her biri");
            }
        }
        #endregion
        #endregion

        #region ShowFullMenu
        static void ShowFullMenu()
        {
            Console.WriteLine("\n=== Menyu ===");
            var groups = InMemoryDatabase.Restaurant.Menu
                .GroupBy(m => m.Category)
                .OrderBy(g => g.Key);
            foreach (var g in groups)
            {
                Console.WriteLine($"\n-- {g.Key} --");
                foreach (var item in g)
                    Console.WriteLine(item);
            }

            if (InMemoryDatabase.Restaurant.Sets.Any())
            {
                Console.WriteLine("\n=== Sets ===");
                foreach (var s in InMemoryDatabase.Restaurant.Sets)
                    Console.WriteLine(s);
            }
        }
        #endregion
    }
}

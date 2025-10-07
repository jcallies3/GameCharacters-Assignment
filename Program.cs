using NLog;
using System.Reflection;
using System.Text.Json;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

List<string> fileNames = ["mario.json", "dk.json", "sf2.json"];
// Deserialize files
List<Mario> marios = [];
List<DonkeyKong> dks = [];
List<StreetFighter> sfs = [];
if (File.Exists(fileNames[0]))
{
    marios = JsonSerializer.Deserialize<List<Mario>>(File.ReadAllText(fileNames[0]))!;
    logger.Info($"File deserialized {fileNames[0]}");
}
if (File.Exists(fileNames[1]))
{
    dks = JsonSerializer.Deserialize<List<DonkeyKong>>(File.ReadAllText(fileNames[1]))!;
    logger.Info($"File deserialized {fileNames[1]}");
}
if (File.Exists(fileNames[2]))
{
    sfs = JsonSerializer.Deserialize<List<StreetFighter>>(File.ReadAllText(fileNames[2]))!;
    logger.Info($"File deserialized {fileNames[2]}");
}


do
{
    // display choices to user
    Console.WriteLine("1) Display Characters");
    Console.WriteLine("2) Add Character");
    Console.WriteLine("3) Remove Character");
    Console.WriteLine("4) Edit Character");
    Console.WriteLine("Enter to quit");

    // input selection
    string? choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);

    if (choice == "1")
    {
        Console.WriteLine("Choose game.");
        Console.WriteLine("1) Super Mario Bros");
        Console.WriteLine("2) Donkey Kong");
        Console.WriteLine("3) Street Fighter 2");
        choice = Console.ReadLine();
        if (choice == "1")
        {
            // Display Mario Characters
            foreach (var c in marios)
            {
                Console.WriteLine(c.Display());
            }
        }
        if (choice == "2")
        {
            // Display DonkeyKong Characters
            foreach (var c in dks)
            {
                Console.WriteLine(c.Display());
            }
        }
        if (choice == "3")
        {
            // Display StreetFighter Characters
            foreach (var c in sfs)
            {
                Console.WriteLine(c.Display());
            }
        }
        
    }
    else if (choice == "2")
    {
        Console.WriteLine("Choose game.");
        Console.WriteLine("1) Super Mario Bros");
        Console.WriteLine("2) Donkey Kong");
        Console.WriteLine("3) Street Fighter 2");
        choice = Console.ReadLine();
        if(choice == "1")
        {
            // Add Mario Character
            // Generate unique ID
            Mario mario = new()
            {
                Id = marios.Count == 0 ? 1 : marios.Max(c => c.Id) + 1
            };
            // Input Name, Description
            InputCharacter(mario);
            // Add Character
            marios.Add(mario);
            File.WriteAllText(fileNames[0], JsonSerializer.Serialize(marios));
            logger.Info($"Character added: {mario.Name}");
        }
        if(choice == "2")
        {
            // Add DonkeyKong Character
            // Generate unique ID
            DonkeyKong dk = new()
            {
                Id = dks.Count == 0 ? 1 : dks.Max(c => c.Id) + 1
            };
            // Input Name, Description
            InputCharacter(dk);
            // Add Character
            dks.Add(dk);
            File.WriteAllText(fileNames[1], JsonSerializer.Serialize(dks));
            logger.Info($"Character added: {dk.Name}");
        }
        if(choice == "3")
        {
            // Add StreetFighter Character
            // Generate unique ID
            StreetFighter sf = new()
            {
                Id = sfs.Count == 0 ? 1 : sfs.Max(c => c.Id) + 1
            };
            // Input Name, Description
            InputCharacter(sf);
            // Add Character
            sfs.Add(sf);
            File.WriteAllText(fileNames[2], JsonSerializer.Serialize(sfs));
            logger.Info($"Character added: {sf.Name}");
        }
    }
    else if (choice == "3")
    {
        Console.WriteLine("Choose game.");
        Console.WriteLine("1) Super Mario Bros");
        Console.WriteLine("2) Donkey Kong");
        Console.WriteLine("3) Street Fighter 2");
        choice = Console.ReadLine();
        if (choice == "1")
        {
            // Remove Mario Character
            Console.WriteLine("Enter the Id of the character to remove:");
            if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
            {
                Mario? character = marios.FirstOrDefault(c => c.Id == Id);
                if (character == null)
                {
                    logger.Error($"Character Id {Id} not found");
                }
                else
                {
                    marios.Remove(character);
                    // serialize list<marioCharacter> into json file
                    File.WriteAllText(fileNames[0], JsonSerializer.Serialize(marios));
                    logger.Info($"Character Id {Id} removed");
                }
            }
            else
            {
                logger.Error("Invalid Id");
            }
        }
         if (choice == "2")
        {
            // Remove DonkeyKong Character
            Console.WriteLine("Enter the Id of the character to remove:");
            if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
            {
                DonkeyKong? character = dks.FirstOrDefault(c => c.Id == Id);
                if (character == null)
                {
                    logger.Error($"Character Id {Id} not found");
                }
                else
                {
                    dks.Remove(character);
                    // serialize list<marioCharacter> into json file
                    File.WriteAllText(fileNames[1], JsonSerializer.Serialize(dks));
                    logger.Info($"Character Id {Id} removed");
                }
            }
            else
            {
                logger.Error("Invalid Id");
            }
        }
         if (choice == "3")
        {
            // Remove StreetFighter Character
            Console.WriteLine("Enter the Id of the character to remove:");
            if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
            {
                StreetFighter? character = sfs.FirstOrDefault(c => c.Id == Id);
                if (character == null)
                {
                    logger.Error($"Character Id {Id} not found");
                }
                else
                {
                    sfs.Remove(character);
                    // serialize list<marioCharacter> into json file
                    File.WriteAllText(fileNames[2], JsonSerializer.Serialize(sfs));
                    logger.Info($"Character Id {Id} removed");
                }
            }
            else
            {
                logger.Error("Invalid Id");
            }
        }
        
    }
    else if (choice == "4")
    {
        Console.WriteLine("Choose game.");
        Console.WriteLine("1) Super Mario Bros");
        Console.WriteLine("2) Donkey Kong");
        Console.WriteLine("3) Street Fighter 2");
        choice = Console.ReadLine();
    }
    else if (string.IsNullOrEmpty(choice))
    {
        break;
    }
    else
    {
        logger.Info("Invalid choice");
    }
} while (true);

logger.Info("Program ended");

static void InputCharacter(Character character)
{
    Type type = character.GetType();
    PropertyInfo[] properties = type.GetProperties();
    var props = properties.Where(p => p.Name != "Id");
    foreach (PropertyInfo prop in props)
    {
        if (prop.PropertyType == typeof(string))
        {
            Console.WriteLine($"Enter {prop.Name}:");
            prop.SetValue(character, Console.ReadLine());
        }
        else if (prop.PropertyType == typeof(List<string>))
        {
            List<string> list = [];
            do
            {
                Console.WriteLine($"Enter {prop.Name} or (enter) to quit:");
                string response = Console.ReadLine()!;
                if (string.IsNullOrEmpty(response))
                {
                    break;
                }
                list.Add(response);
            } while (true);
            prop.SetValue(character, list);
        }
    }
}
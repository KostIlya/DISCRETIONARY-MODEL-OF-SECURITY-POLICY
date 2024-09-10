//using System;
//РЕАЛИЗАЦИЯ ПОЛИТИК ИНФОРМАЦИОННОЙ БЕЗОПАСНОСТИ ДИСКРЕЦИОННАЯ МОДЕЛЬ ПОЛИТИКИ БЕЗОПАСНОСТИ

//Дискреционная политика безопасности определяет отображение   (объектов на пользователей-субъектов).
//В соответствии с данным отображением, каждый объект   объявляется собственностью соответствующего пользователя,
//который может выполнять над ними определенную совокупность действий  , в которую могут входить несколько элементарных
//действий (чтение, запись, модификация и т.д.). Пользователь, являющийся собственником объекта, иногда имеет право передавать
//часть или все права другим пользователям (обладание администраторскими правами).
namespace LR2
{
    class Program
    {

        private static List<string> Users = new List<string> { "Admin", "Vasya", "Ilya", "Dima", "Katya","Petya", "Nastya", "Gosha", "Alex" };
        private static List<string> Files = new List<string> { "file1", "file2", "file3", "file4" };
        private static List<string> AccessFiles = new List<string> { "Полный запрет", "Передача прав", "Запись","Запись, Передача прав","Чтение","Чтение, Передача прав","Чтение, Запись","Полный доступ" };
        private static int usersCount = Users.Count;
        private static int filesCount = Files.Count;
        private static bool userFlag = false;
        private static string command = "";
        private static string user = "";
        private static int readAccess = AccessFiles.IndexOf("Чтение");
        private static int writeAccess = AccessFiles.IndexOf("Запись");
        private static int grantAccess = AccessFiles.IndexOf("Передача прав");
        private static int[][] AccessRules = new int[usersCount][];
        private static Random rand = new Random();
        /// <summary>
        /// Идентификация пользователей
        /// </summary>
        static void Identification()
        {
            while (!userFlag)
            {
                Console.Write("User: ");
                command = Console.ReadLine();
                for (int i = 0; i < usersCount; i++)
                {
                    if (command == Users[i])
                    {
                        userFlag = true;
                        user = command;
                    }
                }
                if (!userFlag)
                {
                    Console.WriteLine("Идентификация не прошла, попробуйте снова");
                }
                else
                {
                    Console.WriteLine("Идентификация прошла успешно, добро пожаловать в систему");
                    PrintTableAccess(user);
                }
            }
        }
        /// <summary>
        /// Заполнение перечня прав пользователей
        /// </summary>
        /// <param name="AccessRules"></param>
        static void FillAccess(ref int[][] AccessRules)
        {
            for (int i = 0; i < usersCount; i++)
            {
                AccessRules[i] = new int[filesCount];
                if (Users.IndexOf("Admin") == i)
                {
                    for (int j = 0; j < filesCount; j++)
                    {
                        AccessRules[i][j] = 7;
                    }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        AccessRules[i][j] = rand.Next(8);
                    }
                }
            }
        }
        /// <summary>
        /// Печать перечня доступных прав
        /// </summary>
        /// <param name="user"></param>
        static void PrintTableAccess(string user)
        {
            Console.WriteLine("Перечень Ваших прав:");
            for (int i = 0; i < filesCount; i++)
            {
                Console.WriteLine($"#{i}    {Files[i]}:     {AccessFiles[AccessRules[Users.IndexOf(user)][i]]}");
            }
        }
        static void FindFile(out string file, int mode = 0)
        {
            string com = "";
            file = "";
            bool isNumeric;
            bool fileFlag = false;
            int number;
            while (file == "" && !fileFlag)
            {
                if (mode == grantAccess)
                {
                    Console.Write("Право на какой объект передается? ");
                }
                else
                {
                    Console.Write("Над каким объектом производится операция? ");
                }
                com = Console.ReadLine();
                isNumeric = int.TryParse(com, out number);
                if (!isNumeric)
                {
                    for (int i = 0; i < filesCount; i++)
                    {
                        if (com == Files[i])
                        {
                            file = Files[i];
                            fileFlag = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (number < filesCount)
                    {
                        file = Files[number];
                        fileFlag = true;
                    }
                }
                if (!fileFlag)
                {
                    Console.WriteLine("Данный файл не существует");
                }
            }
        }

        /// <summary>
        /// Поиск пользователя получателя права доступа
        /// </summary>
        /// <param name="userRecipient"></param>
        static void FindUserRecipient(out string userRecipient)
        {
            string com = "";
            userRecipient = "";
            bool userRecipientFlag = false;
            while (userRecipient == "" && !userRecipientFlag)
            {
                Console.Write("Какому пользователю передается право? ");
                com = Console.ReadLine();
                for (int i = 0; i < usersCount; i++)
                {
                    if (com == Users[i])
                    {
                        userRecipient = Users[i];
                        userRecipientFlag = true;
                        break;
                    }
                }
                if (!userRecipientFlag)
                {
                    Console.WriteLine("Данный пользователь не существует");
                }
            }
        }

        /// <summary>
        /// operation: 1- Передача прав, 2- Запись, 4- Чтение
        /// </summary>
        static void CheckAccess(string user, string file, int operation)
        {
            if ((AccessRules[Users.IndexOf(user)][Files.IndexOf(file)] & operation) == operation)
            {
                if (operation != grantAccess)
                {
                    Console.WriteLine("Операция прошла успешно!");
                }
            }
            else
            {
                Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления!");
            }
        }
        /// <summary>
        /// CheckAccess с перегрузкой
        /// </summary>
        static void CheckAccess(string user, string file, int operation, out bool permit)
        {
            permit = false;
            if ((AccessRules[Users.IndexOf(user)][Files.IndexOf(file)] & operation) == operation)
            {
                if (operation != grantAccess)
                {
                    Console.WriteLine("Операция прошла успешно!");
                }
                permit = true;
            }
            else
            {
                Console.WriteLine("Отказ в выполнении операции. У Вас нет прав для ее осуществления!");
            }
        }
        /// <summary>
        /// Чтение
        /// </summary>
        static void Read(string user)
        {
            FindFile(out string file);
            CheckAccess(user, file, readAccess);
        }
        static void Write(string user)
        {
            FindFile(out string file);
            CheckAccess(user, file, writeAccess);
        }

        static bool TransferableAccessRight(string user, string file, int Access, out int transferableAccessRight, ref string com)
        {
            transferableAccessRight = 0;
            if ((AccessRules[Users.IndexOf(user)][Files.IndexOf(file)] & Access) == Access)
            {
                transferableAccessRight = Access;
                return true;
            }
            else
            {
                Console.WriteLine("У вас нет данного права доступа. Введите корректное право, чтобы повторить; Введите 'no', чтобы прекратить передачу права.");
                com = Console.ReadLine();
                return false;
            }
        }

        static void Grant(string user)
        {
            string com;
            bool transmitedSuccessful = false;
            bool permit = false;            // есть ли разрешение на передачу этого права
            string userRecipient;           // пользователь получатель права доступа
            int transferableAccessRight = 0;    // передаваемое право доступа
            FindFile(out string file, grantAccess);
            CheckAccess(user, file, grantAccess, out permit);
            if (permit)
            {
                Console.Write("Какое право передается? ");
                com = Console.ReadLine();
                while (!transmitedSuccessful && com != "no")
                {
                    if (com == "read")
                    {
                        transmitedSuccessful = TransferableAccessRight(user, file, readAccess, out transferableAccessRight, ref com);
                    }
                    else if (com == "write")
                    {
                        transmitedSuccessful = TransferableAccessRight(user, file, writeAccess, out transferableAccessRight, ref com);
                    }
                    else if (com == "grant")
                    {
                        transmitedSuccessful = TransferableAccessRight(user, file, grantAccess, out transferableAccessRight, ref com);
                    }
                    else
                    {
                        Console.WriteLine("У вас нет этого права доступа! Введите корректное право, чтобы повторить; Введите 'no', чтобы прекратить передачу права.");

                        com = Console.ReadLine();
                    }
                }
                if (com != "no")
                {
                    FindUserRecipient(out userRecipient);
                    AccessRules[Users.IndexOf(userRecipient)][Files.IndexOf(file)] |= transferableAccessRight;
                    Console.WriteLine("Операция прошла успешно");
                }
                else
                {
                    Console.WriteLine("Операция не выполнена");
                }
            }
        }
        static void Main()
        {
            Console.WriteLine("Костылев И.А., гр. 4401, вариант 7"); // 9 субъектов доступа и 4 объекта доступа
            FillAccess(ref AccessRules);

            while (command != "exit")
            {
                Identification();

                while ((command != "quit") && (command != "exit"))
                {
                    Console.Write("Жду ваших указаний > ");
                    command = Console.ReadLine();
                    if (command == "quit")
                    {
                        userFlag = false;
                    }
                    switch (command)
                    {
                        case "table":
                            Table();
                            break;
                        case "read":
                            Read(user);
                            break;
                        case "write":
                            Write(user);
                            break;
                        case "grant":
                            Grant(user);
                            break;
                        case "quit":
                            break;
                        case "exit":
                            Console.WriteLine("Вы точно хотите выйти? Если да, то напишите 'yes'");
                            command = Console.ReadLine();
                            if (command == "yes") { command = "exit"; } 
                            break;
                        case "":
                            break;
                        default:
                            Console.WriteLine("Неверная команда!");
                            break;
                    }
                }

            }
        }

        static void Table()
        {
            for (int i=0; i< usersCount; i++)
            {
                Console.WriteLine("\n\n" + Users[i]);
                PrintTableAccess(Users[i]);
            }
        }
    }
}
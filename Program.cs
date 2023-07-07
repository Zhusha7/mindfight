namespace mindfight
{
    internal class Program
    {
        private static void WriteLine(string text)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}", text));
        }
        private static void WriteRightLine(string text)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 4) + (text.Length)) + "}", text));
        }
        
        static void DisplayRules()
        {
            Console.Clear();
            WriteRightLine("Sveikiname prisijungus prie X protmūšio programos.");
            WriteRightLine("Šis protmūšis jums leidžia pasirinkti iš X klausimų kategorijų.");
            WriteRightLine("Pasirinkus kategoriją pradėsite žaidimą ir turėsite pasirinkti iš 4 galimų variantų,");
            WriteRightLine("kuris yra jūsų klausimui teisingas atsakymas.");
            Console.ReadKey();
        }

        static bool Menu(User currentUser)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.name} {currentUser.surname}\n");
                WriteLine("MENU\n");
                WriteRightLine("1: taisyklės");
                WriteRightLine("2: Pradėti žaidimą");
                WriteRightLine("0: išeiti iš programos");
                WriteRightLine("q: grįžti prie prisijungimo");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1 or ConsoleKey.NumPad1:
                        DisplayRules();
                        continue;
                    case ConsoleKey.D2 or ConsoleKey.NumPad2:
                        while (PickCathegory(currentUser)){}
                        continue;
                    case ConsoleKey.D0 or ConsoleKey.NumPad0:
                        return false;
                    case ConsoleKey.Q:
                        return true;
                    default:
                        WriteLine("");
                        WriteLine("Neteisingas ivedimas, paspauskite betką kad pabandyti dar karta.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static bool PickCathegory(User currentUser)
        {
            Console.Clear();
            Console.WriteLine($"Prisijunges vartotojas: {currentUser.name} {currentUser.surname}\n");
            WriteLine("Pasirinkite kategoriją!");
            WriteRightLine("1. Žaidimai");
            WriteRightLine("2. Programavimas");
            var key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    GamesTheme(currentUser);
                    return false;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    ProgrammingTheme(currentUser);
                    return false;
                default:
                    WriteLine("");
                    WriteLine("Neteisingas ivedimas, paspauskite bet ką, kad pabandyti dar karta.");
                    Console.ReadKey();
                    return true;
            }
        }

        static void CheckAnswers(User currentUser, Question[] questions, int[] attempts)
        {
            int i = 1;
            foreach (var question in questions)
            {
                Console.Clear();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.name} {currentUser.surname}\n");
                WriteLine(question.question);
                WriteRightLine(string.Join("\t", question.answers));
                WriteRightLine($"Atsakėte: {attempts[i - 1]} {question.answers[attempts[i - 1]-1]}");
                if (question.answer == attempts[i - 1])
                {
                    WriteRightLine("Atsakyta teisingai!");
                }
                else
                {
                    WriteRightLine("Atsakyta neteisingai :c");
                    WriteRightLine("Teisingas atsakymas: " + question.answers[question.answer-1]);
                }

                i++;
                Console.ReadKey();
            }
        }

        static void ProgrammingTheme(User currentUser)
        {
            Question[] questions = {
                new Question(
                    @"Kiek yra 'bit' viename 'byte'",
                    new String[] { "16", "4", "6", "8" },
                    4
                ),
                new Question(
                    @"Ką daro main metodas?",
                    new String[] { "Aprašo programos prad", "Fallout 5", "Fallout 4", "Fallout: New Vegas" },
                    1
                )
            };
            int[] attempts = PlayGame(currentUser, questions);
            CheckAnswers(currentUser,questions, attempts);

        }

        static void GamesTheme(User currentUser)
        {
            Question[] questions = new Question[]
            {
                new Question(
                    @"Kiek iš viso yra 'Assassin's Creed' žaidimų platformoje 'Steam'?",
                    new String[] { "16", "14", "10", "69" },
                    1
                ),
                new Question(
                    @"Kuris iš šių 'Fallout' serijos žaidimų neegzistuoja 2023 metais",
                    new String[] { "Fallout 76", "Fallout 5", "Fallout 4", "Fallout: New Vegas" },
                    2
                )
            };
            int[] attempts = PlayGame(currentUser, questions);
            CheckAnswers(currentUser, questions, attempts);
        }

        static int[] PlayGame(User currentUser, Question[] questions)
        {
            int[] attempts = new int[questions.Length];
            int i = 1;
            foreach (Question question in questions)
            {
                Console.Clear();
                Console.WriteLine($"Prisijunges vartotojas: {currentUser.name} {currentUser.surname}\nKlausimas: {i}/{questions.Length}");
                WriteLine(question.question);
                for (int j = 0; j < question.answers.Length; j++)
                {
                    WriteRightLine($"{j+1} {question.answers[j]}");
                }
                char temp = Console.ReadKey().KeyChar;
                while (temp is < '0' or > '4')
                {
                    WriteLine("");
                    WriteRightLine("Neteisingas ivedimas, pabandykite dar karta");
                    temp = Console.ReadKey().KeyChar;
                }
                attempts[i-1]=int.Parse(temp.ToString());
                i++;
            }
            return attempts;
        }

        internal struct Question
        {
            public string question;
            public string[] answers;
            public int answer;

            public Question(string question, string[] answers, int answer)
            {
                this.question = question;
                this.answers = answers;
                this.answer = answer;
            }
        }
        internal struct User
        {
            public string name, surname;
            public int score;
            
            public User(string name, string surname)
            {
                this.name = name;
                this.surname = surname;
                this.score = 0;
            }
        }

        static class Game
        {
            static List<User> users = new List<User>();

            public static User Login()
            {
                while (true)
                {
                    Console.Clear();
                    WriteLine("Iveskite varda ir pavarde:");
                    string? nameSurname = Console.ReadLine();
                    if(String.IsNullOrEmpty(nameSurname) || !nameSurname.Contains(' '))
                    {
                        WriteLine("Neteisingas įvedimas, pabandykite dar kartą.");
                        Console.ReadKey();
                        continue;
                    }
                    string[] nameSurnameArr = nameSurname.Split(' ', 2);
                    string name = nameSurnameArr[0];
                    string surname = nameSurnameArr[1];
                    bool check = false;
                    foreach (var it in users)
                    {
                        if(name == it.name && surname == it.surname)
                        {
                            WriteLine($"Vartotojas {name} {surname} jau egzistuoja!");
                            WriteLine("Paspauskite enter jeigu norite tęsti prisijungus prie šio vartotojo.");
                            if (Console.ReadKey().Key == ConsoleKey.Enter) return it;
                            check = true;
                        }
                        if ((name == it.name || surname == it.surname) && !check)
                        {
                            WriteLine("Vardas arba pavardė sutampa su kažkuo, bet ne abu.");
                            WriteLine("Pabandykite dar kartą!");
                            Console.ReadKey();
                            check = true;
                        }
                    }
                    if(check) continue;
                    User user = new User(name, surname);
                    users.Add(user);
                    WriteLine($"Sveikiname prisijungus {name} {surname}!");
                    Console.ReadKey();
                    return user;
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                var currentUser = Game.Login();
                if (Menu(currentUser))
                    continue;
                return;
            }
        }
    }
}
using System.CommandLine;

class Program
{
    private const string Path = "Users.csv";

    static int Main(string[] args)
    {
        var loginOption = new Option<string?>("--login");
        var pwdOption = new Option<string?>("--pwd");

        RootCommand rootCommand = new("Авторизация");

        var registrCommand = new Command("register", "Регитсрация нового пользователя");
        registrCommand.Options.Add(loginOption);
        registrCommand.Options.Add(pwdOption);

        rootCommand.Subcommands.Add(registrCommand);

        registrCommand.SetAction(res =>
        {
            try
            {
                string? loginInput = res.GetValue(loginOption);
                string? passwordInput = res.GetValue(pwdOption);

                if(!String.IsNullOrWhiteSpace(loginInput) && !String.IsNullOrWhiteSpace(passwordInput))
                {
                    var lines1 = File.ReadAllLines(Path);

                    foreach (var line in lines1)
                    {
                        var userInfo = line.Split(';');

                        if (userInfo[0].Trim() == loginInput)
                        {
                            Console.WriteLine("Пользователь уже существует");
                            return;
                        }

                    }

                    File.AppendAllLines(Path, [$"{loginInput};{passwordInput}"]);
                    Console.WriteLine("---Успех---");
                    return;
                }

                Console.Write("Логин: ");
                loginInput = Console.ReadLine();


                var lines = File.ReadAllLines(Path);

                foreach (var line in lines)
                {
                    var userInfo = line.Split(';');

                    if (userInfo[0].Trim() == loginInput)
                    {
                        Console.WriteLine("Пользователь уже существует");
                        return;
                    }

                }

                Console.Write("Пароль: ");
                passwordInput = Console.ReadLine();

                Console.Write("Подтвердите пароль");
                if (passwordInput == Console.ReadLine())
                {
                    File.AppendAllLines(Path, [$"{loginInput};{passwordInput}"]);
                }
                else
                {
                    Console.WriteLine("Не успех");
                    return;
                }

                Console.WriteLine("---Успех---");
            }
            catch
            {
                Console.WriteLine("Ужас");
            }
        });

        rootCommand.Options.Add(loginOption);
        rootCommand.Options.Add(pwdOption);

        rootCommand.SetAction(async res =>
        {
            int attempts = 3;

            string? loginInput = res.GetValue(loginOption);
            string? passwordInput = res.GetValue(pwdOption);

            if (String.IsNullOrWhiteSpace(loginInput) && String.IsNullOrWhiteSpace(passwordInput))
            {
                while (true)
                {
                    if (String.IsNullOrWhiteSpace(loginInput))
                    {
                        Console.Write("Логин: ");
                        loginInput = Console.ReadLine();
                        Console.Write("Пароль: ");
                        passwordInput = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine($"Логин (-r - для сброса({attempts})): {loginInput}");
                        Console.Write("Пароль: ");
                        passwordInput = Console.ReadLine();
                        if (passwordInput == "-r")
                        {
                            Retry(out attempts, out loginInput);
                            continue;
                        }
                    }


                    if (TryLogin(loginInput, passwordInput))
                    {
                        Console.WriteLine("---Успешно---");
                        break;
                    }
                    else
                    {
                        if (attempts == 0)
                        {
                            Retry(out attempts, out loginInput);
                        }

                        attempts--;
                    }
                }
                return 0;
            }
            if (!String.IsNullOrWhiteSpace(loginInput) && String.IsNullOrWhiteSpace(passwordInput))
            {
                Console.Write("Пароль: ");
                passwordInput = Console.ReadLine();

                TryLogin(loginInput, passwordInput);
                if (TryLogin(loginInput, passwordInput))
                {
                    Console.WriteLine("---Успешно---");
                    return 0;
                }
                Console.WriteLine("Не успех");
                return 0;
            }

            TryLogin(loginInput, passwordInput);
            if (TryLogin(loginInput, passwordInput))
            {
                Console.WriteLine("---Успешно---");
                return 0;
            }
            Console.WriteLine("Не успех");
            return 0;

            void Retry(out int attempts, out string loginInput)
            {
                loginInput = "";
                attempts = 3;
            }

            bool TryLogin(string login, string password)
            {
                var lines = File.ReadAllLines(Path);

                foreach (var line in lines)
                {
                    var userInfo = line.Split(';');

                    if (userInfo[0].Trim() == login && userInfo[1].Trim() == password)
                        return true;
                }

                return false;
            }


            return 0;

        });

        var result = rootCommand.Parse(args);
        result.Invoke();
        return 1;

    }
}



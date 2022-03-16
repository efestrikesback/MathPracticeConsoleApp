//********************************************************************************************************************************************************************
#region VARIABLES
string profileDirectory = @"D:\projects\MathPracticeApp\MathPracticeConsoleApp\savegames\"; //please enter your desired save directory ex "D:\projects\MathPracticeApp\MathPracticeConsoleApp\savegames\"
string profileFileExtension = ".dat";
double firstOperand = 0;
double secondOperand = 0;
int sessionPoints = 0;
int level = 1;
string userName = "playerName";
int operation = 0;
string operationAsString = "default";
double answer = 0;
int hearth = 5;
double userAnswer;
List<int> bonusPerQuestion = new List<int> { 0, 5, 10, 20 };
Random random = new Random();
#endregion 
//********************************************************************************************************************************************************************
#region PROGRAM BODY

Console.WriteLine("*** Welcome to the math quiz game! ***");

InitializeUserName();

SetLevel();

Console.WriteLine($"***Player name is {userName} , player's level is {level}***");

while (hearth != 0 && Console.ReadLine() != "exit")
{
    AskQuestion(level);

    GetUserAnswer();

    EvaluateAnswer(level);

    SetLevel();

    DecleareLevel(sessionPoints);

    Console.WriteLine($"current point is {sessionPoints} please press enter for the next question");
}

TellNoHearthLeft();

IsInTopThree(sessionPoints);

ShowResult();

WriteProfileData(userName, sessionPoints);
#endregion PROGRAM BODY

//********************************************************************************************************************************************************************
#region METHODS

void InitializeUserName()
{

    do
    {
        Console.WriteLine("Please enter your name");
        userName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("User name cannot be empty .");
        }
    }
    while (string.IsNullOrWhiteSpace(userName));

    WriteProfileData(userName, ReadProfileHighScore(userName));

}

void GetUserAnswer()
{
    bool isCorrect;
    do
    {
        isCorrect = Double.TryParse(Console.ReadLine(), out userAnswer);

        if (!isCorrect) Console.WriteLine("Please type in correct format.");
    }
    while (!isCorrect);

}

void SetLevel()
{

    if (sessionPoints >= 30 && sessionPoints < 80)
    {
        level = 2;
    }
    else if (sessionPoints >= 80)
    {
        level = 3;
    }
    else
    {
        level = 1;
    }
}

void GenerateOperands(int level)
{

    switch (level)
    {
        case 1:
            firstOperand = random.Next(0, 99);
            secondOperand = random.Next(1, 99);
            break;
        case 2:
            firstOperand = random.Next(0, 999);
            secondOperand = random.Next(1, 999);
            break;
        case 3:
            firstOperand = random.Next(0, 9999);
            secondOperand = random.Next(1, 9999);
            break;
    }
}

void GenerateOperator(int level)
{
    switch (level)
    {
        case 1:
            operation = random.Next(1, 3);
            break;

        case 2:
            operation = random.Next(1, 5);
            break;
        case 3:
            operation = random.Next(1, 6);
            break;
    }

    switch (operation)
    {
        case 1:
            answer = firstOperand + secondOperand;
            operationAsString = "+";
            break;

        case 2:
            answer = firstOperand - secondOperand;
            operationAsString = "-";
            break;
        case 3:
            answer = firstOperand * secondOperand;
            operationAsString = "*";
            break;
        case 4:
            answer = firstOperand / secondOperand;
            operationAsString = "/";
            break;
        case 5:
            answer = firstOperand % secondOperand;
            operationAsString = "%";
            break;
    }

}

void EvaluateAnswer(int level)
{
    if (userAnswer == answer)
    {
        sessionPoints = sessionPoints + bonusPerQuestion[level];
    }

    else if (userAnswer == 777)
    {
        AddPoints();

    }

    else
    {
        hearth--;
    }
}

void AskQuestion(int level)
{
    GenerateOperands(level);

    GenerateOperator(level);

    Console.ForegroundColor = ConsoleColor.Blue;

    Console.WriteLine($"*** QUESTION *** \n difficulty: level {level}\n please solve the question and write your answer");

    Console.WriteLine($" solve {firstOperand}{operationAsString}{secondOperand}");

    Console.ResetColor();
}

void ShowResult()
{

    if (sessionPoints > ReadProfileHighScore(userName))
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"New personal record with {sessionPoints} points!!!");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"Points earned: {sessionPoints}, personal best : {ReadProfileHighScore(userName)}");
        Console.ResetColor();

    }

    if (IsInTopThree(sessionPoints))
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"You are in top 3 with {sessionPoints} points");
        Console.ResetColor();
    }
}

int ReadProfileHighScore(string userName)
{
    int points = 0;
    try
    {
        string fileName = profileDirectory + userName + profileFileExtension;
        if (!File.Exists(fileName))
        {
            Console.WriteLine("No profile found for user " + userName + ".");
        }

        FileStream fileStream = File.OpenRead(fileName);
        using (StreamReader streamReader = new StreamReader(fileStream))
        {
            string line = streamReader.ReadLine();

            if (line != null)
            {

                points = int.Parse(line);
            }
            streamReader.Close();
        }
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Cannot open profile file, " + e.Message);
        Console.ResetColor();
    }

    return points;
}

void WriteProfileData(string userName, int points)
{
    string fileName = profileDirectory + userName + profileFileExtension;
    StreamWriter streamWriter = new StreamWriter(fileName, false);
    streamWriter.WriteLine(points);
    streamWriter.Close();
}

bool IsInTopThree(int point)
{
    List<int> allScores = new List<int>();
    foreach (string fileName in Directory.GetFiles(profileDirectory))
    {
        string userName = Path.GetFileNameWithoutExtension(fileName);
        int score = ReadProfileHighScore(userName);
        allScores.Add(score);
    }

    if (allScores.Count < 3)
    {
        return true;
    }

    return point > allScores[2];
}

void AddPoints()
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    sessionPoints = sessionPoints + 20;
    Console.WriteLine("This is going to be a terrible game...");
    Console.Beep(659, 125); Console.Beep(659, 125); Thread.Sleep(125);
    Console.ResetColor();
}

void DecleareLevel(int point)
{
    switch (point)
    {
        case 30:
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("You are now level 2");

            Console.ResetColor();
            break;
        case 80:
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("You are now level 3");

            Console.ResetColor();
            break;
    }

}

void TellNoHearthLeft()
{
    if (hearth == 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(" You ran out of hearths, please pay 1 USD to buy one health. ");
        Console.ResetColor();
    }
}

#endregion METHODS

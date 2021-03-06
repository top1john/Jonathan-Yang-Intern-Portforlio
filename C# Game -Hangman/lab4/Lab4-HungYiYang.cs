using System;
using System.Drawing;
using System.IO;
using GDIDrawer;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            //create CDrawer Canvas 
            CDrawer Canvas = new CDrawer();

            do
            {
                string UsedChar = "";
                int life = 6;
                int Matches = 0;
                int TotalMatches = 0;
                string PickedWord;

                //clear window
                Console.Clear();

                Console.WriteLine("\t\t\tLab4-Hangman-HungYiYang");
                Console.WriteLine();

                //Console.WriteLine(CountWords());      //use to count total words

                PickedWord = PickWord(CountWords());

                Console.WriteLine(PickedWord);        //use to show picked word

                //creat a character array to display each of the letter from the chosen word
                char[] LetterDispay = new char[PickWord(CountWords()).Length];

                //turn the letters in picked word in to '-'
                for (int index = 0; index < LetterDispay.Length; index++)
                {
                    LetterDispay[index] = '-';
                }

                do
                {
                    //create graphic screen to show letter, used letters, and life left
                    DrawScreen(Canvas, LetterDispay, UsedChar, life);

                    //show how many letters in the chosenword matches the user input letter
                    Matches = CheckGuess(GetGuess(ref UsedChar), ref LetterDispay, PickedWord);

                    //if 0 match was found
                    if (Matches < 1)
                    {
                        //life -1
                        life--;
                    }

                    //if match was found
                    else
                    {
                        //add number of matches to the total matches
                        TotalMatches += Matches;
                    }

                    //loop until life=0 or TotalMatches=number of letters in the chosen word
                } while (life > 0 && TotalMatches < PickedWord.Length);

                //if no life is left
                if (life == 0)
                {
                    //save each letter in the PickedWord into each index in LetterDispay[]
                    for (int index = 0; index < LetterDispay.Length; index++)
                    {
                        char.TryParse(PickedWord.Substring(index, 1), out LetterDispay[index]);
                    }

                    DrawScreen(Canvas, LetterDispay, UsedChar, life);

                    //display lose message
                    Canvas.AddText("You Lose!", 68, Color.Gray);
                }
                //if still some life left 
                else
                {
                    DrawScreen(Canvas, LetterDispay, UsedChar, life);
                    //display win message
                    Canvas.AddText("You Win!", 68, Color.Gray);
                }

                Console.WriteLine();

            } while ((YesNo("Play again (yes or no)? ") == "yes"));
        }

        static private int CountWords()
        {
            StreamReader srInFile;          //create srInFile as reference variable to StreamReader class                 

            int TotalWords = 0;             //total words

            try
            {
                //create name and path of text feie for srInFile to open
                srInFile = new StreamReader("Hangman.txt");

                try
                {
                    //read lines of characters and returns the data as string until the end of the stream
                    while ((srInFile.ReadLine()) != null)
                    {
                        //+1 to total words
                        TotalWords++;
                    }
                }
                //if error occurs,display error message
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading file: {e.Message}");
                }
                //when finish reading
                finally
                {
                    //Closes the current StreamWriter and releases the file
                    srInFile.Close();
                }
            }
            //if fails to find text file
            catch (Exception e)
            {
                Console.WriteLine($"Error opening file: {e.Message}");
            }

            //return int value back to calling program
            return TotalWords;
        }

        static private string PickWord(int TotalWords)
        {
            Random randomNumbers = new Random();            //random nmber generator
            StreamReader srInFile;                          //create srInFile as reference variable to StreamReader class  

            string sInput;                                  //temporary storage for string
            string[] TextBank = new string[TotalWords];     //create an array to save strings in each of its index

            srInFile = new StreamReader("Hangman.txt");     //create name and path of text feie for srInFile to open

            uint index = 0;                                 //counter

            //save each string in TextBank[index]
            while ((sInput = srInFile.ReadLine()) != null)
            {
                TextBank[index] = sInput;
                index++;
            }

            //choose a random string by picking a random index from TextBank[]
            sInput = TextBank[randomNumbers.Next(TotalWords)];

            //return string back to the calling program
            return sInput;
        }

        static private void DrawScreen(CDrawer Canvas, char[] characters, string UsedChar, int life)
        {
            string Letters = "";

            //clear Canvas window
            Canvas.Clear();

            //display used words
            Canvas.AddText($"Letters used: {UsedChar}", 18, 15, 10, 500, 80, Color.Lavender);

            //draw hangman stage and rope
            Canvas.AddLine(320, 100, 320, 450, Color.Aqua, 3);
            Canvas.AddLine(320, 120, 440, 120, Color.Aqua, 3);
            Canvas.AddLine(320, 140, 340, 120, Color.Aqua, 3);
            Canvas.AddLine(320, 400, 500, 400, Color.Aqua, 3);
            Canvas.AddLine(480, 400, 480, 450, Color.Aqua, 3);
            Canvas.AddLine(320, 400, 480, 440, Color.Aqua, 3);
            Canvas.AddLine(320, 440, 480, 400, Color.Aqua, 3);
            Canvas.AddLine(410, 120, 410, 170, Color.Red, 2);

            //display hangman base on how many lives left
            switch (life)
            {
                case 5:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    break;
                case 4:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    Canvas.AddEllipse(395, 200, 30, 80, Color.Tan);
                    break;
                case 3:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    Canvas.AddEllipse(395, 200, 30, 80, Color.Tan);
                    Canvas.AddLine(410, 200, 375, 220, Color.Tan, 3);
                    break;
                case 2:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    Canvas.AddEllipse(395, 200, 30, 80, Color.Tan);
                    Canvas.AddLine(410, 200, 375, 220, Color.Tan, 3);
                    Canvas.AddLine(410, 200, 445, 220, Color.Tan, 3);
                    break;
                case 1:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    Canvas.AddEllipse(395, 200, 30, 80, Color.Tan);
                    Canvas.AddLine(410, 200, 375, 220, Color.Tan, 3);
                    Canvas.AddLine(410, 200, 445, 220, Color.Tan, 3);
                    Canvas.AddLine(415, 270, 430, 330, Color.Tan, 3);
                    break;
                case 0:
                    Canvas.AddEllipse(395, 170, 30, 30, Color.Tan);
                    Canvas.AddEllipse(395, 200, 30, 80, Color.Tan);
                    Canvas.AddLine(410, 200, 375, 220, Color.Tan, 3);
                    Canvas.AddLine(410, 200, 445, 220, Color.Tan, 3);
                    Canvas.AddLine(415, 270, 430, 330, Color.Tan, 3);
                    Canvas.AddLine(405, 270, 390, 330, Color.Tan, 3);
                    break;
            }


            for (int index = 0; index < characters.Length; index++)
            {
                Letters += characters[index] + " ";
            }


            Canvas.AddText(Letters, 40, 0, 480, 800, 100, Color.Orange);

        }

        static private char GetGuess(ref string UsedChar)
        {
            string input;               //temporary storage for input
            char enteredchar;           //temporary storage to store a character
            bool SameLetter;            //bool to indicate if the input character matches a letter in the word


            do
            {
                //set default = false
                SameLetter = false;

                Console.Write("Please enter a letter: ");

                //convert user input into small letter
                input = Console.ReadLine().ToLower();

                //either more than letter or non-letter is entered, display an error message
                if (!char.TryParse(input, out enteredchar) || !char.IsLetter(enteredchar))
                {
                    Console.WriteLine("Invalid input. ONLY single letter will be accepted");
                    Console.WriteLine();
                }

                //if the entered letter is found in the "letter used".display an error message
                foreach (char letter in UsedChar)
                {
                    if (enteredchar == letter)
                    {
                        Console.WriteLine($"Invalid input. Letter '{enteredchar}' was previously used.");
                        Console.WriteLine();

                        //SameLetter = true
                        SameLetter = true;
                    }
                }
                //loop if any of the above error is found
            } while (!char.TryParse(input, out enteredchar) || !char.IsLetter(enteredchar) || SameLetter);

            //add the input letter into the string "UsedChar"
            UsedChar += enteredchar.ToString();

            //return input as char back to calling program
            return enteredchar;
        }

        static private int CheckGuess(char input, ref char[] characters, string answer)
        {
            int Match = 0;              //number of matched letter

            //loop through every letter in the "word"
            for (int index = 0; index < answer.Length; index++)
            {
                //if the letter matched the input character
                if (answer.Substring(index, 1) == input.ToString())
                {
                    //turn the value in that index from '-' to the input character
                    characters[index] = input;

                    //match +1
                    Match++;
                }
            }

            Console.WriteLine($"{Match} match(s) of letter '{input}' were found.");
            Console.WriteLine();

            //return the total number of match as integer back to the calling program
            return Match;
        }



        static private string YesNo(string Question)
        {
            bool valid = false;             //valid input checker, set default = false
            string input = "";                //temporary storage for input


            while (valid == false)
            {
                Console.Write(Question);

                input = Console.ReadLine().ToLower();

                //if input = yes (lowercase), turn valid = true, and also make string Question = "yes"
                if (input == "yes")
                {
                    Question = "yes";

                    valid = true;
                }

                //if input = no (lowercase), turn valid = true, and also make string Question = "no"
                else if (input == "no")
                {
                    Question = "no";

                    valid = true;
                }

                //if any input, display following message
                else
                {
                    Console.WriteLine("You must answer yes or no.");
                    Console.WriteLine();
                }
            }

            //return string Question back to main()
            return Question;
        }

    }
}


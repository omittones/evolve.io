using System;

namespace core
{
    public class CreatureName
    {
        public const int MAX_NAME_LENGTH = 10;
        public const int MIN_NAME_LENGTH = 3;

        public string Name { get; private set; }

        public string Parents { get; private set; }

        public CreatureName()
        {
            this.Name = "";
            var chosenLength = Rnd.nextFloat(MIN_NAME_LENGTH, MAX_NAME_LENGTH);
            for (var i = 0; i < chosenLength; i++)
                this.Name += getRandomChar();
            this.sanitizeName();
            this.Parents = "[PRIMORDIAL]";
        }

        private CreatureName(string name, string parents)
        {
            Name = name;
            Parents = parents;
        }

        public static CreatureName SpawnFrom(params CreatureName[] parts)
        {
            var result = new CreatureName("", "");
            for (var i = 0; i < parts.Length; i++)
            {
                var portion = ((float) parts[i].Name.Length)/parts.Length;
                var start = (int) Math.Min(Math.Max((float) Math.Round(portion*i), 0), parts[i].Name.Length);
                var end = (int) Math.Min(Math.Max((float) Math.Round(portion*(i + 1)), 0), parts[i].Name.Length);
                result.Name = result.Name + parts[i].Name.Substr(start, end);
            }

            if (result.Name.Length >= 1)
            {
                result.mutateName();
                result.sanitizeName();
            }
            else
            {
                result = new CreatureName();
            }

            result.Parents = andifyParents(parts);

            return result;
        }

        private static string andifyParents(params CreatureName[] parts)
        {
            var result = "";
            for (var i = 0; i < parts.Length; i++)
            {
                if (i >= 1)
                    result = result + " & ";
                result = result + parts[i].Name;
            }
            return result;
        }

        private readonly double[] letterFrequencies =
        {
            8.167, 1.492, 2.782, 4.253, 12.702, 2.228, 2.015, 6.094, 6.966, 0.153, 0.772,
            4.025, 2.406, 6.749, 7.507, 1.929, 0.095, 5.987, 6.327, 9.056, 2.758, 0.978, 2.361, 0.150, 1.974, 10000.0
        };

        private static bool isVowel(char a)
        {
            return (a == 'a' || a == 'e' || a == 'i' || a == 'o' || a == 'u' || a == 'y');
        }

        private void sanitizeName()
        {
            var output = "";
            var vowelsSoFar = 0;
            var consonantsSoFar = 0;
            foreach (var character in Name)
            {
                if (isVowel(character))
                {
                    consonantsSoFar = 0;
                    vowelsSoFar++;
                }
                else
                {
                    vowelsSoFar = 0;
                    consonantsSoFar++;
                }
                if (vowelsSoFar <= 2 && consonantsSoFar <= 2)
                {
                    output = output + character;
                }
                else
                {
                    var chanceOfAddingChar = 0.5;
                    if (Name.Length <= MIN_NAME_LENGTH)
                    {
                        chanceOfAddingChar = 1.0;
                    }
                    else if (Name.Length >= MAX_NAME_LENGTH)
                    {
                        chanceOfAddingChar = 0.0;
                    }
                    if (Rnd.nextFloat(0, 1) < chanceOfAddingChar)
                    {
                        var extraChar = ' ';
                        while (extraChar == ' ' || (isVowel(character) == isVowel(extraChar)))
                        {
                            extraChar = getRandomChar();
                        }
                        output = output + extraChar + character;
                        if (isVowel(character))
                        {
                            consonantsSoFar = 0;
                            vowelsSoFar = 1;
                        }
                        else
                        {
                            consonantsSoFar = 1;
                            vowelsSoFar = 0;
                        }
                    }
                }
            }

            this.Name = output.ToLower().Capitalize();
        }

        private char getRandomChar()
        {
            var letterFactor = Rnd.nextFloat(0, 100);
            var letterChoice = 0;
            while (letterFactor > 0)
            {
                letterFactor -= (float) letterFrequencies[letterChoice];
                letterChoice++;
            }
            return (char) (letterChoice + 96);
        }

        private void mutateName()
        {
            var input = this.Name;

            if (input.Length >= 3)
            {
                if (Rnd.nextFloat(0, 1) < 0.2)
                {
                    var removeIndex = Rnd.nextInt(0, input.Length);
                    input = input.Substr(0, removeIndex) + input.Substr(removeIndex + 1, input.Length);
                }
            }
            if (input.Length <= 9)
            {
                if (Rnd.nextFloat(0, 1) < 0.2)
                {
                    var insertIndex = Rnd.nextInt(0, input.Length + 1);
                    input = input.Substr(0, insertIndex) + getRandomChar() + input.Substr(insertIndex, input.Length);
                }
            }
            var changeIndex = Rnd.nextInt(0, input.Length);
            input = input.Substr(0, changeIndex) + getRandomChar() + input.Substr(changeIndex + 1, input.Length);

            this.Name = input;
        }
    }
}
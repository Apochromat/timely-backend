using System.Text;
using System.Text.RegularExpressions;

namespace timely_backend {
    /// <summary>
    /// Static class for useful functions
    /// </summary>
    public static class Misc {
        /// <summary>
        /// Transliterate russian or english name and email to special string 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns> String looks like ""</returns>
        public static string TransliterateNameAndEmail(string name, string email) {
            // Delete redundant spaces and split name
            string[] nameParts = Regex.Replace(name, @"\s+", " ").Split(' ');

            if (nameParts.Length < 2) {
                throw new InvalidOperationException("fullName must contain firstname and lastname divided by space");
            }
            
            // Transliterate email name
            string emailTransliterated = Transliterate(email);

            // Get transliterated initials
            string firstNameInitialTransliterated = "_" + Transliterate(nameParts[1].Substring(0, 1));
            string patronymicInitialTransliterated = "";
            if (nameParts.Length > 2) {
                patronymicInitialTransliterated = "_" + Transliterate(nameParts[2].Substring(0, 1));
            }

            // Transliterate lastname
            string lastNameTransliterated = Transliterate(nameParts[0]);

            // Generate string with lastname, initials
            string result =
                $"{lastNameTransliterated}{firstNameInitialTransliterated}{patronymicInitialTransliterated}_{emailTransliterated}";

            return result;
        }

        /// <summary>
        /// Transliterate russian or english string 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Transliterated string</returns>
        private static string Transliterate(string input) {
            Dictionary<char, string> transliterateTable = new Dictionary<char, string> {
                { 'а', "a" }, { 'б', "b" }, { 'в', "v" }, { 'г', "g" }, { 'д', "d" }, { 'е', "e" },
                { 'ё', "yo" }, { 'ж', "zh" }, { 'з', "z" }, { 'и', "i" }, { 'й', "y" }, { 'к', "k" },
                { 'л', "l" }, { 'м', "m" }, { 'н', "n" }, { 'о', "o" }, { 'п', "p" }, { 'р', "r" },
                { 'с', "s" }, { 'т', "t" }, { 'у', "u" }, { 'ф', "f" }, { 'х', "h" }, { 'ц', "ts" },
                { 'ч', "ch" }, { 'ш', "sh" }, { 'щ', "sch" }, { 'ъ', "" }, { 'ы', "y" }, { 'ь', "" },
                { 'э', "e" }, { 'ю', "yu" }, { 'я', "ya" },
                { 'a', "a" }, { 'b', "b" }, { 'c', "c" }, { 'd', "d" }, { 'e', "e" }, { 'f', "f" },
                { 'g', "g" }, { 'h', "h" }, { 'i', "i" }, { 'j', "j" }, { 'k', "k" }, { 'l', "l" },
                { 'm', "m" }, { 'n', "n" }, { 'o', "o" }, { 'p', "p" }, { 'q', "q" }, { 'r', "r" },
                { 's', "s" }, { 't', "t" }, { 'u', "u" }, { 'v', "v" }, { 'w', "w" }, { 'x', "x" },
                { 'y', "y" }, { 'z', "z" }
            };

            StringBuilder output = new StringBuilder();

            foreach (char c in input.ToLower()) {
                if (transliterateTable.TryGetValue(c, out string transliterate)) {
                    output.Append(transliterate);
                }
                else {
                    output.Append(c);
                }
            }

            return output.ToString();
        }
    }
}
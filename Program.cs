/*
Курс: Платформа Microsoft. NET і мова програмування C# 
Зустріч № 
Тема: Інтерфейс. 
ЗАВДАННЯ 
Уявімо, ви робите систему фільтрації коментарів на якомусь веб-порталі, будь то  новини, відео-хостинг,… 
Ви хочете фільтрувати коментарі за різними критеріями, вміти легко додавати  нові фільтри і модифікувати старі. 
Припустимо, ми будемо фільтрувати спам, коментарі з негативним змістом і  занадто довгі коментарі. 
Спам будемо фільтрувати за наявністю зазначених ключових слів в тексті. Негативний зміст визначатимемо за наявністю одного з трьох смайликів: 
:( =( :| 
Занадто довгі коментарі будемо визначати виходячи з даного числа - максимальної довжини коментаря. 
Ви вирішили абстрагувати фільтр у вигляді наступного інтерфейсу: 
interface TextAnalyzer{ 
Label processText(String text); 
} 
Label - тип-перерахування, які містить мітки, якими будемо позначати текст: enum Label{ SPAM, NEGATIVE_TEXT, TOO_LONG, OK } 
Далі, вам необхідно реалізувати три класи, які реалізують даний інтерфейс:  SpamAnalyzer, NegativeTextAnalyzer і TooLongTextAnalyzer. 
SpamAnalyzer повинен конструюватися від масиву рядків з ключовими словами.  Об'єкт цього класу повинен зберігати в своєму стані цей масив рядків в приватному  поле keywords. 
NegativeTextAnalyzer повинен конструюватися конструктором за  замовчуванням. 
TooLongTextAnalyzer повинен конструюватися від int'а з максимальною  допустимою довжиною коментаря. Об'єкт цього класу повинен зберігати в своєму  стані це число в приватному поле maxLength.

Напевно ви вже помітили, що SpamAnalyzer і NegativeTextAnalyzer багато  в чому схожі - вони обидва перевіряють текст на наявність будь-яких ключових слів  (в разі спаму ми отримуємо їх з конструктора, в разі негативного тексту ми  заздалегідь знаємо набір сумних смайликів) і в разі знаходження одного з ключових  слів повертають Label (SPAM і NEGATIVE_TEXT відповідно), а якщо нічого не  знайшлося - повертають OK. 
Давайте цю логіку абстрагуємося в абстрактний клас KeywordAnalyzer наступним чином: 
Виділимо два абстрактних методу getKeywords і getLabel, один з яких буде  повертати набір ключових слів, а другий мітку, якої необхідно позначити позитивні  спрацьовування. Нам нема чого показувати ці методи споживачам класів, тому  залишимо доступ до них тільки для спадкоємців. 
Реалізуємо processText таким чином, щоб він залежав тільки від getKeywords і getLabel. 
Зробимо SpamAnalyzer і NegativeTextAnalyzer спадкоємцями  KeywordAnalyzer і реалізуємо абстрактні методи. 
Останній штрих - написати метод checkLabels, який буде повертати мітку для  коментаря по набору аналізаторів тексту. checkLabels повинен повертати першим  не-OK мітку в порядку даного набору аналізаторів, і OK, якщо все аналізатори  повернули OK. 
Використовуйте, будь ласка, модифікатор доступу за замовчуванням для всіх  класів. 
У підсумку, реалізуйте класи KeywordAnalyzer, SpamAnalyzer,  NegativeTextAnalyzer і TooLongTextAnalyzer і метод checkLabels.  TextAnalyzer і Label вже підключені, зайві обсяги імпорту вам не будуть потрібні.

Для тестування скористайтесь наступною функцією 
public static void Main(string[] args)
{ 
    // ініціалізація аналізаторів для перевірки в порядку даного набору аналізаторів 
    String[] spamKeywords = { "spam", "bad" };
    int commentMaxLength = 40;
    TextAnalyzer[] textAnalyzers1 = {
        new SpamAnalyzer(spamKeywords),
        new NegativeTextAnalyzer(),
        new TooLongTextAnalyzer(commentMaxLength) };
    TextAnalyzer[] textAnalyzers2 = {
        new SpamAnalyzer (spamKeywords),
        new TooLongTextAnalyzer (commentMaxLength),
        new NegativeTextAnalyzer ()
    };
    TextAnalyzer[] textAnalyzers3 = {
        new TooLongTextAnalyzer (commentMaxLength),
        new SpamAnalyzer (spamKeywords),
        new NegativeTextAnalyzer ()
    };
    TextAnalyzer[] textAnalyzers4 = {
        new TooLongTextAnalyzer (commentMaxLength),
        new NegativeTextAnalyzer (),
        new SpamAnalyzer (spamKeywords)
    };
    TextAnalyzer[] textAnalyzers5 = {
        new NegativeTextAnalyzer (),
        new SpamAnalyzer (spamKeywords),
        new TooLongTextAnalyzer (commentMaxLength)
    };
    TextAnalyzer[] textAnalyzers6 = {
        new NegativeTextAnalyzer (),
        new TooLongTextAnalyzer (commentMaxLength),
        new SpamAnalyzer (spamKeywords)
    };
    // тестові коментарі 
    String[] tests = new String[8];
    tests[0] = "This comment is so good."; // OK 
    tests[1] = "This comment is so Loooooooooooooooooooooooooooong."; //  TOO_LONG 
    tests[2] = "Very negative comment !!!! = (!!!!;"; // NEGATIVE_TEXT 
    tests[3] = "Very BAAAAAAAAAAAAAAAAAAAAAAAAD comment with: |;"; //  NEGATIVE_TEXT or TOO_LONG 
    tests[4] = "This comment is so bad ...."; // SPAM 
    tests[5] = "The comment is a spam, maybeeeeeeeeeeeeeeeeeeeeee!"; // SPAM or  TOO_LONG 
    tests[6] = "Negative bad :( spam."; // SPAM or NEGATIVE_TEXT 
    tests[7] = "Very bad, very neg = (, very .................."; // SPAM or  NEGATIVE_TEXT or TOO_LONG
    TextAnalyzer[][] textAnalyzers = { textAnalyzers1, textAnalyzers2, textAnalyzers3, textAnalyzers4, textAnalyzers5, textAnalyzers6 };
    int numberOfAnalyzer; // номер аналізатора, зазначений в ідентифікатор textAnalyzers {№} 
    int numberOfTest = 0; // номер тесту, який відповідає індексу тестових коментарів 
    foreach (String test in tests)
    {
        numberOfAnalyzer = 1;
        Console.Write("test #" + numberOfTest + ":");
        Console.WriteLine(test);
        foreach (TextAnalyzer[] analyzers in textAnalyzers)
        {
            Console.Write(numberOfAnalyzer + ":");
            Console.WriteLine(CheckLabels(analyzers, test));
            numberOfAnalyzer++;
        }
        numberOfTest++;
    }

}
*/

using System;

namespace _05112020dz
{
    public interface TextAnalyzer
    {
        Label processText(String text);
    }
    public enum Label { SPAM, NEGATIVE_TEXT, TOO_LONG, OK }

    public abstract class KeywordAnalyzer : TextAnalyzer
    {
        protected abstract String[] getKeywords();

        protected abstract Label getLabel();

        public Label processText(String text) // public override Label processText(String text)
        {
            String[] keywords = getKeywords();
            foreach (String keyword in keywords)
            {
                if (text.Contains(keyword))
                {
                    return getLabel();
                }
            }
            return Label.OK;
        }
    }

    public class SpamAnalyzer : KeywordAnalyzer
    {
        private String[] keywords;

        public SpamAnalyzer(String[] keywords)
        {
            this.keywords = keywords;
        }

        protected override String[] getKeywords()
        {
            return keywords;
        }

        protected override Label getLabel()
        {
            return Label.SPAM;
        }
    }

    public class NegativeTextAnalyzer : KeywordAnalyzer
    {
        private String[] KEYWORDS = { ":(", "=(", ":|" };

        protected override String[] getKeywords()
        {
            return KEYWORDS;
        }

        protected override Label getLabel()
        {
            return Label.NEGATIVE_TEXT;
        }
    }

    public class TooLongTextAnalyzer : TextAnalyzer
    {
        private int maxLength;

        public TooLongTextAnalyzer(int limit)
        {
            this.maxLength = limit;
        }

        public Label processText(String text)
        {
            if (text.Length > maxLength)
                return Label.TOO_LONG;
            else
                return Label.OK;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // ініціалізація аналізаторів для перевірки в порядку даного набору аналізаторів 
            String[] spamKeywords = { "spam", "bad" };
            int commentMaxLength = 40;
            TextAnalyzer[] textAnalyzers1 = {
                new SpamAnalyzer(spamKeywords),
                new NegativeTextAnalyzer(),
                new TooLongTextAnalyzer(commentMaxLength) };
            TextAnalyzer[] textAnalyzers2 = {
                new SpamAnalyzer (spamKeywords),
                new TooLongTextAnalyzer (commentMaxLength),
                new NegativeTextAnalyzer ()
            };
            TextAnalyzer[] textAnalyzers3 = {
                new TooLongTextAnalyzer (commentMaxLength),
                new SpamAnalyzer (spamKeywords),
                new NegativeTextAnalyzer ()
            };
            TextAnalyzer[] textAnalyzers4 = {
                new TooLongTextAnalyzer (commentMaxLength),
                new NegativeTextAnalyzer (),
                new SpamAnalyzer (spamKeywords)
            };
            TextAnalyzer[] textAnalyzers5 = {
                new NegativeTextAnalyzer (),
                new SpamAnalyzer (spamKeywords),
                new TooLongTextAnalyzer (commentMaxLength)
            };
            TextAnalyzer[] textAnalyzers6 = {
                new NegativeTextAnalyzer (),
                new TooLongTextAnalyzer (commentMaxLength),
                new SpamAnalyzer (spamKeywords)
            };
            // тестові коментарі 
            String[] tests = new String[8];
            tests[0] = "This comment is so good."; // OK 
            tests[1] = "This comment is so Loooooooooooooooooooooooooooong."; //  TOO_LONG 
            tests[2] = "Very negative comment !!!! = (!!!!;"; // NEGATIVE_TEXT 
            tests[3] = "Very BAAAAAAAAAAAAAAAAAAAAAAAAD comment with: |;"; //  NEGATIVE_TEXT or TOO_LONG 
            tests[4] = "This comment is so bad ...."; // SPAM 
            tests[5] = "The comment is a spam, maybeeeeeeeeeeeeeeeeeeeeee!"; // SPAM or  TOO_LONG 
            tests[6] = "Negative bad :( spam."; // SPAM or NEGATIVE_TEXT 
            tests[7] = "Very bad, very neg = (, very .................."; // SPAM or  NEGATIVE_TEXT or TOO_LONG
            TextAnalyzer[][] textAnalyzers = { textAnalyzers1, textAnalyzers2, textAnalyzers3, textAnalyzers4, textAnalyzers5, textAnalyzers6 };
            int numberOfAnalyzer; // номер аналізатора, зазначений в ідентифікатор textAnalyzers {№} 
            int numberOfTest = 0; // номер тесту, який відповідає індексу тестових коментарів 
            foreach (String test in tests)
            {
                numberOfAnalyzer = 1;
                Console.Write("test #" + numberOfTest + ":");
                Console.WriteLine(test);
                foreach (TextAnalyzer[] analyzers in textAnalyzers)
                {
                    Console.Write(numberOfAnalyzer + ":");
                    Console.WriteLine(CheckLabels(analyzers, test));
                    numberOfAnalyzer++;
                }
                numberOfTest++;
            }
        }

        private static Label CheckLabels(TextAnalyzer[] analyzers, string test)
        {
            if (true)
            {
                foreach (TextAnalyzer obj_txt_an in analyzers)
                {
                    Label label_check = obj_txt_an.processText(test);
                    if (label_check != Label.OK)
                        return label_check;
                }
                return Label.OK;
            }
        }
    }
}
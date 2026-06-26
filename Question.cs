namespace PART2_POE_
{
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        Keyword
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public QuestionType Type { get; set; }
    }
}
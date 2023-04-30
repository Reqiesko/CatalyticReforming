using System.Collections.Generic;

namespace DAL;

public class Question : EntityBase
{
    public string Text { get; set; }

    public List<Answer> Answers { get; set; }
}
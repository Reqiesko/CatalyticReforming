using System.Collections.Generic;


namespace DAL.Models.test;

public class Question : EntityBase
{
    public string Text { get; set; }

    public virtual List<Answer> Answers { get; set; }
}




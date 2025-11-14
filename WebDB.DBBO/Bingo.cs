using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDB.DBBO
{
    [Serializable]
    public class Bingo
    {
        public virtual int ID { get; set; }
        public virtual string Sponsor { get; set; }
        public virtual string Question { get; set; }
        public virtual string Choices { get; set; }
        public virtual string Answer { get; set; }
        public virtual string SponsorType { get; set; }
        public virtual string[] ChoiceAry { get => Choices?.Split('|') ?? Array.Empty<string>(); }
        public virtual int[] AnswerAry { get => (Answer?.Split('|') ?? Array.Empty<string>()).Select(x => int.Parse(x)).ToArray(); }
    }
}

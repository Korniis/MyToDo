﻿namespace MyToDo.Api.Context
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

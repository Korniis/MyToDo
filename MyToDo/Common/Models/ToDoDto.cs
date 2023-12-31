﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class ToDoDto:BaseDto
    {/// <summary>
    /// 待办实体
    /// </summary>
        private int id;
        private string content;
        private string title;
        private int status;
        /// <summary>
        /// 标题
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 内容
        /// </summary>

        public string Content
        {
            get { return content; }
            set { content = value; }

        }
    /// <summary>
    /// 标题
    /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>

        public int Status
        {
            get { return status; }
            set { status = value; }
        }


    }
}

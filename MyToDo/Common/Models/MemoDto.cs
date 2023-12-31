﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{

    /// <summary>
    ///备忘录
    /// </summary>
    public class MemoDto:BaseDto
    { 
        private string content;
        private string title;
        private int status;
        
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
